local protoHelper = require("Framework.Network.ProtoHelper")
local NetReceiver = BaseClass("NetReceiver")

-- -- protobuf用法
-- local fishingRoomStatus = protoHelper.GetClass("FishingRoomStatus")
-- -- repeated字段赋值
-- local room1 = fishingRoomStatus.singleRoomStatus:add()
-- local room1.roomID = 1
-- local room1.status = 2
-- local room1.roomMoneyLimit = 3

-- -- 打包成bytes
-- local encode = protoHelper.Encode(fishingRoomStatus)

-- -- 解包bytes
-- local decode = protoHelper.Dncode("FishingRoomStatus", encode)

-- 私有的 静态

local LuaCall = CS.Hr.LuaCallStatic
local LuaEventID = CS.Hr.LuaEventID
local PlayerData = LuaCall.GetPlayerData()
local TollgateData = LuaCall.GetTollgateData()
local RuntimeDataCache = require("GameData.RuntimeDataCache")
local GameSetting = LuaCall.GetGameSetting()
local ServerData = LuaCall.GetServerData()
-- local ServerCacheData = LuaCall.GetServerCacheData()
local ItemIDTransform = require "Common.ItemIDTransform"
-- 各个经常用到的全局变量都缓存一下，提升效率
local ShowUITips = LuaCall.LuaShowUITips
local GetDictionary = LuaCall.GetDictionary
local HideMask = LuaCall.HideMask
local ShowMask = LuaCall.ShowMask

local tbCommandFunc = {}
local function RegisterFunc(nCommand, func)
    if tbCommandFunc[nCommand] == nil then
        tbCommandFunc[nCommand] = func
    else
        Logger.Log("NetReceiver a command does not allow multi handler id " .. nCommand)
    end
end
local function UnRegister(nCommand)
    tbCommandFunc[nCommand] = nil
end

-- 获得物品
-- Param1 类似Message里面定义的ItemPair的结构
-- Param2 是否展示reward界面
local function GetItem(arrayChangeItem, bIsShowRewardForm, nNewPlayerIndex)

    if arrayChangeItem == nil or #arrayChangeItem == 0 then
        return
    end

    local rewardItemsInfo = {}
    for i = 1, #arrayChangeItem do
        local nItemID = ItemIDTransform.PlatformItemTypeMappingFishingItemType(math.ceil(arrayChangeItem[i].itemType))-- 服务器的金币id是0，红包id是15我们的是1和7，转换一下
        PlayerData:AddItemCount(nItemID, arrayChangeItem[i].itemCount)

        rewardItemsInfo[i] = {}
        rewardItemsInfo[i].itemID = nItemID
        rewardItemsInfo[i].itemCount = math.ceil(arrayChangeItem[i].itemCount)
    end

    --获得界面
    if bIsShowRewardForm then
        if nNewPlayerIndex then
            rewardItemsInfo.nNewPlayerIndex = nNewPlayerIndex
        end
        LuaCall.LuaOpenForm(UIFormID.UIObtainForm, rewardItemsInfo)
    end
end

-- 类方法
-- 被c#调用的
NetReceiver.OnReceive = function(Sender, GameEventArg)
    NetReceiver.Process(GameEventArg.MsgCommandID, GameEventArg.ContentData, GameEventArg.InnerPacket)
end

function NetReceiver.Process(nCommand, bytes, innerPacket)
    local func = tbCommandFunc[nCommand]
    if func ~= nil then
        func(bytes, innerPacket, nCommand)
    else
        Logger.Log("this command not regist handler. commandId " .. nCommand)
    end
end

local function Delete()
    LuaCall.RemoveEvent(CS.Hr.NetMsgReceiveBytesEventArgs.EventId, NetReceiver.OnReceive)
end

local function Init()
    LuaCall.AddEvent(CS.Hr.NetMsgReceiveBytesEventArgs.EventId, NetReceiver.OnReceive)
end

NetReceiver.__delete = Delete
NetReceiver.__init = Init
-- 消息处理函数 私有

-- 登录前更新公告说明回应
local function ProcessGetStateServerNoticeInfoResponse(bytes)
    LuaCall.HideMask("kGameServerStateServerGetNoticeInfo")
    local msg = assert(protoHelper.Decode("GetStateServerNoticeInfoResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! GetStateServerNoticeInfoResponse")
        return
    end

    local nResult = msg.result
    local nVersion = msg.versionNumber
    local nNoticeContext = msg.noticeContext

    if nResult == 0 then
        LuaCall.LuaOpenForm(UIFormID.UIMessageBoxForm, {nNoticeContext})
    end
end
RegisterFunc(HrConstantMSG.kGameServerStateServerGetNoticeInfo, ProcessGetStateServerNoticeInfoResponse)

-- 登陆回应
local function ServerLoginResponseHandler(bytes)
    local msg = assert(protoHelper.Decode("LoginResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! ServerLoginResponseHandler")
        return
    end
end
RegisterFunc(HrConstantMSG.kGameServerLoginResponse, ServerLoginResponseHandler)

-- 房间状态回应
local function FishingRoomStatusResponseHandler(bytes)
    local msg = assert(protoHelper.Decode("FishingRoomStatus", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingRoomStatusResponseHandler")
        return
    end

    -- for i = 1, #msg.singleRoomStatus do
    --     Logger.Log(msg.singleRoomStatus[i])
    -- end

    -- Logger.Log("MyTest Lua:" .. tostring(#msg.singleRoomStatus))
    -- Logger.Log_r(msg.singleRoomStatus)
    for key, value in pairs(msg.singleRoomStatus) do
        Logger.Log(
            "FishingRoomStatusResponseHandler roomID:" ..
                tostring(value.roomID) ..
                    " status:" .. tostring(value.status) .. "moneyLimit:" .. tostring(value.roomMoneyLimit)
        )
        -- todo 房间状态设置
        if value.roomID then
            GameDataManager.ServerDataCache[tonumber(value.roomID)] = tonumber(value.status)
        end
    end
    GameLuaEvent.UIEventRoomStatusChange()
end
RegisterFunc(HrConstantMSG.kFishServerRoomStatus, FishingRoomStatusResponseHandler)

-- 手机注册获取验证码回应
local function GetVerificationCodeNewMobileResponse(bytes)
    local msg = assert(protoHelper.Decode("GetVerificationCodeResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingEnterGameResponse")
        return
    end

    local nResult = msg.result
    if nResult == 1 then
        ShowUITips(GetDictionary("Login.NetReceiver.RegisterIDNotExists"))
    elseif nResult == 2 then
        ShowUITips(GetDictionary("Login.NetReceiver.BindPhoneError"))
    elseif nResult == 3 then
        ShowUITips(GetDictionary("Login.NetReceiver.IDUnbindedPhone"))
    end
end

RegisterFunc(HrConstantMSG.kGameServerGetVerificationCodeNewMobile, GetVerificationCodeNewMobileResponse)
-- 处理函数完

-- 大厅升级飞机
local function ProcessAircraftUpgradeResponse(bytes)
    LuaCall.HideMask("kFishAircraftUpgradeReqRes")
    local msg = assert(protoHelper.Decode("FishingCommonReqRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingCommonReqRes")
        return
    end

    local nResult = msg.result
    local nAircraftID = tonumber(msg.paramArr[1])
    local nType = tonumber(msg.paramArr[2])
    local nLevel = tonumber(msg.paramArr[3])
    local nItemInfoArr = msg.itemInfoArr
    Logger.Log("ProcessAircraftUpgradeResponse result:" .. nResult)

    if nResult == 0 then
        -- 剩余的物品数量，不是消耗的物品数量
        for i = 1, #nItemInfoArr do
            local itemID = nItemInfoArr[i].itemType
            local itemCount = nItemInfoArr[i].itemCount
            PlayerData:SetItemCount(itemID, itemCount)
        end
        --存储C# PlayerDataMode中字典的飞机数据
        PlayerData:UpgradePlayerAircraftAttribute(nAircraftID, nType, nLevel)

        GameLuaEvent.UIEventUpgradeAttributeSuccess(nAircraftID, nType, nLevel)
    else
        --升级失败
        ShowUITips(GetDictionary("Upgrade.UpgradeDefeated"))
    end
end
RegisterFunc(HrConstantMSG.kFishAircraftUpgradeReqRes, ProcessAircraftUpgradeResponse)

-- 兑换物品
local function ProcessExchangeItemResponse(bytes)
    LuaCall.HideMask("kFishLogicClientExchangeItemReqRes")
    local msg = assert(protoHelper.Decode("FishingExchangeItemReqRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingExchangeItemReqRes")
        return
    end

    local nResult = msg.nResult
    local nExchangeType = msg.nExchangeType
    local nGroup = msg.nExchangeGroup
    local fromItems = msg.fromItems
    local toItems = msg.toItems
    Logger.Log("ProcessExchangeItemResponse result:" .. nResult)

    if nResult == 0 then
        local costItemsInfo = {}
        local gainItemsInfo = {}
        --消耗的物品
        for i = 1, #fromItems do
            local nCostType = fromItems[i].itemType
            local nCostCount = fromItems[i].itemCount
            -- * nGroup
            Logger.Log("ProcessExchangeItemResponse CostType:" .. nCostType .. " CostCount:" .. nCostCount)
            PlayerData:AddItemCount(nCostType, -nCostCount)
            costItemsInfo[i] = {}
            costItemsInfo[i].itemID = math.ceil(nCostType)
            costItemsInfo[i].itemCount = math.ceil(nCostCount)
        end
        --获取物品
        for i = 1, #toItems do
            local nGainType = toItems[i].itemType
            local nGainCount = toItems[i].itemCount
            -- * nGroup
            Logger.Log("ProcessExchangeItemResponse toItems:" .. nGainType .. " toItems:" .. nGainCount)
            PlayerData:AddItemCount(nGainType, nGainCount)
            gainItemsInfo[i] = {}
            gainItemsInfo[i].itemID = math.ceil(nGainType)
            gainItemsInfo[i].itemCount = math.ceil(nGainCount)
        end
        --[11-17][18-33]是商城兑换
        if nExchangeType >= EnumExchangeType.StoreGlodMin and nExchangeType <= EnumExchangeType.StoreGlodMax then
            --首充双倍
            GameLuaEvent.UIEventStoreDoubleBuySuccess(nExchangeType)
        end
        --[5-10]是兑换商城道具兑换
        -- if nExchangeType >= EnumExchangeType.ExchangeStoreItemMin and nExchangeType <= EnumExchangeType.ExchangeStoreItemMax then
        -- end
        GameLuaEvent.UIEventExchangeSuccess(nExchangeType, gainItemsInfo, costItemsInfo)
    elseif nResult == 100 then
        ShowUITips(GetDictionary("Exchange.ItemNotEnough"))
    elseif nResult == 101 then
        ShowUITips(GetDictionary("Exchange.DataAllocationError"))
    else
        local infoString = string.format(GetDictionary("Exchange.ExchangeError"), nResult)
        ShowUITips(infoString)
    end
end
RegisterFunc(HrConstantMSG.kFishLogicClientExchangeItemReqRes, ProcessExchangeItemResponse)
RegisterFunc(HrConstantMSG.kFishLogicClientRoomExchangeItemReqRes, ProcessExchangeItemResponse)
-- 闯关模式回应
local function ProcessAircraftPassTollgateRes(bytes)
    LuaCall.HideMask("kFishAircraftSurvivePassReqRes")
    Logger.Log("ProcessAircraftPassTollgateRes")
    local msg = assert(protoHelper.Decode("FishingCommonReqRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingCommonReqRes")
        return
    end

    local nResult = msg.result
    local nCurTollgateIndex = msg.paramArr[1]
    local nPassFlag = msg.paramArr[2]
    local rewardItems = msg.itemInfoArr

    TollgateData:SetTollgateIndex(nCurTollgateIndex)

    Logger.Log(
        string.format(
            "ProcessAircraftPassTollgateRes Result:%d TollgateIndex:%d PassFlag:%d",
            nResult,
            nCurTollgateIndex,
            nPassFlag
        )
    )

    nPassFlag = tonumber(nPassFlag)

    if nResult == 0 then
        if nPassFlag == 0 then --未成功那么直接按失败处理
            GameLuaEvent.UIEventSurvivePassResponse(nPassFlag)
        elseif nPassFlag == 1 then
            --奖励的物品
            local itemInfoTable = {}
            for i = 1, #rewardItems do
                Logger.Log("rewardItems", rewardItems[i]["itemType"], rewardItems[i]["itemCount"])
                local itemType = math.ceil(rewardItems[i].itemType)
                local itemCount = rewardItems[i].itemCount

                --更改金币
                if itemType == ItemID.Gold then
                    PlayerData:AddItemCount(ItemID.Gold, itemCount)
                end
                --更改枪管
                if itemType == ItemID.AttackBarrelID then
                    PlayerData:AddItemCount(ItemID.AttackBarrelID, itemCount)
                end
                --更改装甲
                if itemType == ItemID.HPArmorID then
                    PlayerData:AddItemCount(ItemID.HPArmorID, itemCount)
                end
                --更改核心
                if itemType == ItemID.FireSpeedCoreID then
                    PlayerData:AddItemCount(ItemID.FireSpeedCoreID, itemCount)
                end

                itemInfoTable[i] = {}
                itemInfoTable[i].itemID = itemType
                itemInfoTable[i].itemCount = itemCount
                PlayerData:AddItemCount(nCostType, nCostCount)
            end
            GameLuaEvent.UIEventCloseWinMessage()
            GameLuaEvent.UIEventSurvivePassResponse(nPassFlag, itemInfoTable)
        end
    else
        ShowUITips("Error! ProcessAircraftPassTollgateRes Error!! Result:" .. nResult)
        GameLuaEvent.UIEventSurvivePassResponse(0)
    end
end

RegisterFunc(HrConstantMSG.kFishAircraftSurvivePassReqRes, ProcessAircraftPassTollgateRes)

-- apk升级信息，广告墙信息
local function ApkUpdateInfoHandler(bytes)
    local msg = assert(protoHelper.Decode("GetStateUpdateInfoResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingCommonReqRes")
        return
    end

    if msg.result ~= 0 then
        Logger.Log("get apkUpdateInfo error")
    end

    -- Logger.Log("result " .. msg.result)

    local adWall = msg.adWallState
    -- Logger.Log("广告墙" .. adWall)
    -- Logger.Log("--------------")

    -- Logger.Log(msg.moduleUpdateInfoResponse)
    -- Logger.Log(#msg.moduleUpdateInfoResponse)
    for k, v in pairs(msg.moduleUpdateInfoResponse) do
        -- Logger.Log(k)
        -- Logger.Log("___")
        -- Logger.Log(v)
        -- Logger.Log("__")
    end

    if msg.packageUpdateInfoResponse ~= nil then
    -- Logger.Log(msg.packageUpdateInfoResponse.latestversion)
    -- Logger.Log(msg.packageUpdateInfoResponse.expiredversion)
    -- Logger.Log(msg.packageUpdateInfoResponse.url)
    end
end

RegisterFunc(HrConstantMSG.kGameServerStateServerUpdateInfo, ApkUpdateInfoHandler)

-- 充值购买钻石
local function ProcessItemChangeNotify(bytes)
    LuaCall.HideMask("kFishingClientBuyVIPReqRes")
    ---Logger.Log("ProcessItemChangeNotify")

    local msg = assert(protoHelper.Decode("FishingItemChangeNotify", bytes), 0)
    if msg == 0 then
        Logger.LogError("decode error !!!!!!!!!!! FishingItemChangeNotify")
        return
    end

    local notifyType = tonumber(msg.notifyType)

    if type(notifyType) == "string" or notifyType == 0 then
        Logger.LogError("Error notifyType in HandleItemChangeNotify")
        return
    end

    local arrayChangeItem = msg.changeItem
    if arrayChangeItem ~= nil then
        GetItem(arrayChangeItem, true)
    end

    NetSender.SendQueryUserExtraInfoRequest()
end
RegisterFunc(HrConstantMSG.kFishServerItemChangeNotify, ProcessItemChangeNotify)

--充值成功用户数据
local function ProcessDBProxyScorePaySuccessNotify(bytes, innerPacket, nCommand)
    Logger.Log("开始解析充值成功消息")
    local msg = assert(protoHelper.Decode("FishingPaySuccessNotify", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingPaySuccessNotify")
        return
    end

    local userid = tonumber(msg.userid)
    local newVipLevel = tonumber(msg.newVipLevel)
    local newVipExp = tonumber(msg.newVipExp)
    local newVipLeftEffectiveDays = tonumber(msg.newVipLeftEffectiveDays)
    local newFirstBuyStatus = msg.newFirstBuyStatus
    local expireDate = msg.expireDate
    local lastRewardDate = msg.lastRewardDate
    local vipLoginReward = msg.vipLoginReward
    local rewardSecondMoney = msg.rewardSecondMoney
    local vipLoginRewardItem = msg.vipLoginRewardItem
    local totalBuy = msg.totalBuy -- 总共充值了多少钱

    if userid ~= PlayerData.UserID then
        Logger.Log("HandleDBProxyScorePaySuccess : _userId != PlayerData.UserID")
        return
    end

    if newVipLevel < 0 or newVipExp < 0 or newVipLeftEffectiveDays < 0 then
        Logger.LogError("HandleDBProxyScorePaySuccess : server data error!")
        return
    end

    PlayerData.VIPLevel = newVipLevel
    PlayerData.VIPExp = newVipExp
    PlayerData.VIPLeftEffectiveDays = newVipLeftEffectiveDays

    if nCommand == HrConstantMSG.kFishBuyVipWithSecondMoney then
        GameLuaEvent.UIEventVIPBuySuccess() --vip购买成功
        -- Hr.HrGameWorld.EventComponent().SendEvent(Fish.HrFishingEvent.EVENT_ROOM_VIPBUYSUCCESS)
        PlayerData:AddItemCount(ItemID.Diamond, -30)
    end
    --GameLuaEvent.UIEventPaySuccess() --充值成功消息
    GameLuaEvent.UIEventVIPInfoChange() --玩家个人信息发生变化
end
RegisterFunc(HrConstantMSG.kServerDBProxyScorePaySuccessNotify, ProcessDBProxyScorePaySuccessNotify)
RegisterFunc(HrConstantMSG.kFishBuyVipWithSecondMoney, ProcessDBProxyScorePaySuccessNotify)

-- 用户额外信息
local function ProcessUserExtraInfoResponse(bytes)
    Logger.Log("ProcessUserExtraInfoResponse kGameServerQueryUserExtraInfo")
    -- LuaCall.HideMask("kGameServerQueryUserExtraInfo")
    local msg = assert(protoHelper.Decode("QueryUserExtraInfoResponse", bytes), 0)
    if msg == 0 then
        Logger.LogError("decode error !!!!!!!!!!! QueryUserExtraInfoResponse")
        return
    end

    local nResult = msg.result
    if nResult == 0 then
        GameDataManager.ServerDataCache.VipRewardStatusList = LuaList.New()
        for i, v in ipairs(msg.vipRewardStatus) do
            local vipRewardData = {}
            vipRewardData.vipLevel = v.vipLevel
            vipRewardData.rewardStatus = v.rewardStatus --vip等级和领取状态
            GameDataManager.ServerDataCache.VipRewardStatusList:Add(vipRewardData)
        end
        GameLuaEvent.UIEventUserExtraInfo()
    end
end
RegisterFunc(HrConstantMSG.kGameServerQueryUserExtraInfo, ProcessUserExtraInfoResponse)

-- 升级炮台回应
local function UpgradeBetIndexHandler(bytes)
    LuaCall.HideMask("kFishServerUpgradeBetIndexReqRes")

    local msg = assert(protoHelper.Decode("FishingUpgradeGunBetIndex", bytes), 0)
    if msg == 0 then
        Logger.LogError("decode error !!!!!!!!!!! UpgradeBetIndexHandler")
        return
    end
    Logger.Log("upgrade bet response")

    local nResult = msg.result

    if nResult ~= 0 then
        ShowUITips(GetDictionary("FireLevelUpgrade.UnlockBetFailed"))
        return
    end

    -- 升级到了多少倍index
    local nUpgradeBetIndex = msg.toUpgradeBetIndex

    -- 如果有奖励，就弹框
    local rewardItems = msg.rewardItems
    local nSecondMoney = msg.nSecondMoney

    PlayerData.SecondMoney = PlayerData.SecondMoney - RuntimeDataCache.nUpgradeBetCostSecondMoney
    PlayerData.MaxGunIndex = nUpgradeBetIndex

    local nRewradSecondNum = 0

    local dicRewardInfo = {}
    dicRewardInfo.itemID = ItemID.Gold
    dicRewardInfo.itemCount = 0

    for i = 1, #rewardItems do
        if rewardItems[i].itemType == ItemID.Gold then
            dicRewardInfo.itemCount = dicRewardInfo.itemCount + rewardItems[i].itemCount
        elseif ItemID.m_sc_nSecondMoneyID == rewardItems[i].itemType then
            nRewradSecondNum = nRewradSecondNum + rewardItems[i].itemCount
        end
    end

    Logger.Log("drop gold " .. dicRewardInfo.itemCount)

    LuaCall.FireEvent(LuaEventID.EVENT_UI_GOLDDROP, "UpgradeBetResponse", dicRewardInfo.itemCount, 8)

    Logger.Log("upgrade bet nSecondMoney " .. nSecondMoney)

    LuaCall.FireEvent(LuaEventID.EVENT_UI_UPGRADE_BET_SUCCESS, "", nUpgradeBetIndex)
end
RegisterFunc(HrConstantMSG.kFishServerUpgradeBetIndexReqRes, UpgradeBetIndexHandler)

-- 领取vip大礼包
local function ProcessGetVipRewardResponse(bytes)
    LuaCall.HideMask("kGameServerGetVipReward")
    local msg = assert(protoHelper.Decode("GetVipRewardResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! GetVipRewardResponse")
        return
    end

    local nResult = msg.result
    local nUserID = msg.userID
    local nVipLevel = msg.vipLevel
    local rewardItem = msg.rewardItem
    Logger.Log("ProcessGetVipRewardResponse result:" .. nResult)

    if nResult == 0 then
        local rewardItemsInfo = {}
        --获取物品
        GetItem(rewardItem, true)

        --设置vip大礼包为不可领取 nVipLevel
        local rewardStatusList = GameDataManager.ServerDataCache.VipRewardStatusList
        for i = 1, rewardStatusList:Count() do
            if rewardStatusList.items[i].vipLevel == nVipLevel then
                rewardStatusList:Remove(rewardStatusList.items[i])
                break
            end
        end
        GameLuaEvent.UIEventGetVipReward(nVipLevel)
    else
        --领取失败
        ShowUITips(string.format(GetDictionary("VIPNoble.GetVIPRewardDefeated"), nResult))
    end
end
RegisterFunc(HrConstantMSG.kGameServerGetVipReward, ProcessGetVipRewardResponse)

--查找好友回应
local function ProcessGameServerSearchFriendResponse(bytes)
    LuaCall.HideMask("kGameServerSearchFriend")
    local msg = assert(protoHelper.Decode("SearchFriendResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! SearchFriendResponse")
        return
    end

    local nResult = msg.result
    local nFriendInfo = msg.friend
    Logger.Log("ProcessGameServerSearchFriendResponse result:" .. nResult)

    if nResult == 0 then
        GameLuaEvent.UIEventGetUserIDSuccess(nFriendInfo)
    else
        --您输入的ID号不存在
        ShowUITips(GetDictionary("Backpack.InputUserIDNotExist"))
    end
end
RegisterFunc(HrConstantMSG.kGameServerSearchFriend, ProcessGameServerSearchFriendResponse)

--赠送宝箱回应
local function ProcessGivePresentItemResponse(bytes)
    local msg = assert(protoHelper.Decode("PresentItemResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! PresentItemResponse")
        return
    end

    local nResult = msg.result
    local subItemType = msg.subItemType
    local subItemCount = msg.subItemCount
    Logger.Log("ProcessGivePresentItemResponse result:" .. nResult)

    if nResult == 0 then
        Logger.Log("subItems:" .. subItemType .. " toItems:" .. subItemCount)
        PlayerData:AddItemCount(subItemType, -subItemCount)
        --赠送成功
        ShowUITips(GetDictionary("Backpack.PresentedSuccess"))
        GameLuaEvent.UIEventPresentItemSuccess()
    elseif nResult == 1 then
        --赠送未知错误
        ShowUITips(string.format(GetDictionary("Backpack.PresentedUnknownError"), nResult))
    elseif nResult == 2 then
        --赠送未知错误
        if PlayerData.VIPLevel < 3 then
            --VIP2及以上才能赠送!
            ShowUITips(GetDictionary("Backpack.VIP2OpenPresented"))
        elseif PlayerData.VIPLeftEffectiveDays <= 0 then
            --VIP已过期!
            ShowUITips(GetDictionary("Backpack.VIPOutData"))
        end
        local vipLevel = PlayerData.VIPLevel > 2 and PlayerData.VIPLeftEffectiveDays
    elseif nResult == 3 then
        --不同渠道不能赠送
        ShowUITips(GetDictionary("Backpack.PresentedDifferentChannel"))
    end
end
RegisterFunc(HrConstantMSG.kGameServerPresentItem, ProcessGivePresentItemResponse)

-- 更改用户信息回应
local function ProcessModifyUserInfoResponse(bytes)
    LuaCall.HideMask("kGameServerModifyUserInfoRequest")
    local msg = assert(protoHelper.Decode("GameServerModifyUserInfoResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! GameServerModifyUserInfoResponse")
        return
    end

    local result = msg.result
    -- 成功:0，失败:1
    -- if result ~= 0 then
    --     Logger.Log("ModifyUserInfo fail")
    --     -- ShowUITips(GetDictionary("Backpack.PresentedDifferentChannel"))
    --     return
    -- end

    local userid = tonumber(msg.userid)
    local kv = msg.kv
    local failReason = tonumber(msg.failReason)

    if result == 0 then
        for i = 1, #kv do
            if kv[i].fieldName == UserInfoFieldName.Nick then -- nickname
                -- ServerCacheData.PlayerBaiscData.NicName = kv[i].fieldValue
                -- 设置游戏内玩家昵称, 设置本地存储昵称
                PlayerData.NickName = kv[i].fieldValue
                ShowUITips(GetDictionary("ModifyUserInfo.ChangeNickNameSuccess"))
            elseif kv[i].fieldName == UserInfoFieldName.Gender then
                PlayerData.AvatarIndex = tonumber(kv[i].fieldValue) --man:1,woman:0
            elseif kv[i].fieldName == UserInfoFieldName.RoleIndex then
                local index = tonumber(kv[i].fieldValue)
                if index == 0 then
                    index = 0
                else
                    index = 1
                end
                PlayerData.AvatarIndex = tonumber(index)
                -- ServerCacheData.PlayerBaiscData.RoleIndex = tonumber(index)
                ShowUITips(GetDictionary("ModifyUserInfo.ChangeRoleIndexSuccess"))
            end
        end
        GameLuaEvent.UIEventVIPInfoChange()
    elseif result ~= 0 then
        for i = 1, #kv do
            if kv[i].fieldName == UserInfoFieldName.Nick then -- nickname
                ShowUITips(GetDictionary("ModifyUserInfo.NickNameWrong"))
            elseif kv[i].fieldName == UserInfoFieldName.RoleIndex then
                ShowUITips(GetDictionary("ModifyUserInfo.RoleIndexWrong"))
            end
        end
    end
end
RegisterFunc(HrConstantMSG.kGameServerModifyUserInfoResponse, ProcessModifyUserInfoResponse)

--在线时收到礼物
local function ProcessSinglePresentItemResponse(bytes)
    local msg = assert(protoHelper.Decode("FishingSinglePresentItemRequest", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingSinglePresentItemRequest")
        return
    end

    local presentItemInfo = msg
    Logger.Log("ProcessSinglePresentItemResponse ")
    local presentItemList = GameDataManager.ServerDataCache.ServerPresentItemList
    presentItemList:Add(presentItemInfo)

    LuaCall.AddPopupUIToSequence(UIFormID.UIReceivedGiftForm, presentItemInfo)
end
RegisterFunc(HrConstantMSG.kGameServerPresentItemFromOther, ProcessSinglePresentItemResponse)

--接收其他玩家的赠送回应
local function ProcessAcceptPresentItemResponse(bytes)
    LuaCall.HideMask("kGameServerAcceptPresentItem")
    local msg = assert(protoHelper.Decode("FishingAcceptPresentItemResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingAcceptPresentItemResponse")
        return
    end

    local nResult = msg.result
    local nPresentType = msg.itemType
    local nPresentNum = msg.itemCount
    Logger.Log("ProcessAcceptPresentItemResponse result:" .. nResult)

    if nResult == 0 then
        --获取物品
        PlayerData:AddItemCount(nPresentType, nPresentNum)
        GameLuaEvent.UIEventPresentAcceptSuccess()
    end
end
RegisterFunc(HrConstantMSG.kGameServerAcceptPresentItem, ProcessAcceptPresentItemResponse)

--登录时礼物列表
local function ProcessListPresentItemResponse(bytes)
    local msg = assert(protoHelper.Decode("FishingPresentItemRequestList", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingPresentItemRequestList")
        return
    end

    for i, v in ipairs(msg.singleRequest) do
        GameDataManager.ServerDataCache.ServerPresentItemList:Add(v)
        LuaCall.AddPopupUIToSequence(UIFormID.UIReceivedGiftForm, v)
    end
end
RegisterFunc(HrConstantMSG.kGameServerQueryPresentItemRequestList, ProcessListPresentItemResponse)

--商城列表回应
local function ProcessGetShopInfoResponse(bytes)
    Logger.Log("ProcessGetShopInfoResponse")
    LuaCall.HideMask("kGameServerGetShopInfo")
    local msg = assert(protoHelper.Decode("GetShopInfoResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! GetShopInfoResponse")
        return
    end

    --print("-----------------------------------productCount",#msg.product)
    for i, v in ipairs(msg.product) do
        local storeData = {}
        storeData.ProductType = v.productType
        storeData.ProductID = v.productID
        storeData.ProductName = v.productName
        storeData.ProductPrice = string.format("%d", v.productPrice)
        storeData.ItemID = v.rewardInfo[1].rewardType
        storeData.ItemCount = v.rewardInfo[1].rewardCount
        --print(string.format( "%s||productType:%s||productID:%s||productPrice:%s||productName:%s",i,v.productType,v.productID,v.productPrice,v.productName))
        storeData.PayType = {}
        for j = 1, #v.payInfo do
            local payType = v.payInfo[j].payType
            table.insert(storeData.PayType, payType)
            --print("payInfo",v.payInfo[j].payType,v.payInfo[j].goodID)
        end
        -- for j=1,#v.rewardInfo do
        --     print("rewardInfo",v.rewardInfo[j].rewardType,v.rewardInfo[j].rewardCount)
        -- end
        GameDataManager.ServerDataCache.ServerStoreInfoList:Add(storeData)
    end

    GameLuaEvent.UIEventGetShopInfo()
end
RegisterFunc(HrConstantMSG.kGameServerGetShopInfo, ProcessGetShopInfoResponse)

-- 绑定手机回应
local function BindPhoneNumResponse(bytes)
    Logger.Log("BindPhoneNumResponse")
    LuaCall.HideMask("kGameServerBindMobileNumber")
    local msg = assert(protoHelper.Decode("BindMobileNumberResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! BindMobileNumberResponse")
        return
    end

    local nResult = msg.result
    Logger.Log("BindPhoneNumResponse result: " .. nResult)

    local strTips = ""

    if nResult == 1 or nResult == 3 or nResult == 5 then
        strTips = "Safe.VerifyCodeError"
    elseif nResult == 2 then
        strTips = "Safe.PhoneNumberError"
    elseif nResult == 4 then
        strTips = "Safe.AccountBondPhone"
    elseif nResult == 6 then
        strTips = "Safe.PhoneBondAccount"
    elseif nResult == 102 then
        strTips = "Safe.PhoneNotMatchAccount"
    elseif nResult == 103 then
        strTips = "Safe.GoldNotEnough"
    elseif nResult == 104 then
        strTips = "Safe.FrequentOperation"
    elseif nResult == 0 then
        ServerData.BindPhoneNumber = msg.moblieNumber
        -- 记录下绑定时间7天之内无法解绑
        GameSetting:SetInt("BindPhoneTimestamp" .. PlayerData.UserID, os.time())
        GameLuaEvent.UIEventBindPhoneSuccess()
        strTips = "Safe.BindPhoneSuccess"
    else
        strTips = "Safe.ErrorUnknowr"
    end

    ShowUITips(GetDictionary(strTips))
end
RegisterFunc(HrConstantMSG.kGameServerBindMobileNumber, BindPhoneNumResponse)

-- 解绑手机
local function UnBindPhoneNumResponse(bytes)
    Logger.Log("UnBindPhoneNumResponse")
    LuaCall.HideMask("kGameServerUnbindMobileNumber")
    local msg = assert(protoHelper.Decode("UnbindMobileNumberResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! UnbindMobileNumberResponse")
        return
    end

    local nResult = msg.result
    local strTips = ""

    if nResult == 1 or nResult == 3 then
        strTips = "Safe.VerifyCodeError"
    elseif nResult == 2 then
        strTips = "Safe.PhoneNumberError"
    elseif nResult == 4 then
        strTips = "Safe.AccountBondPhone"
    elseif nResult == 5 then
        strTips = "Safe.VerifyCodeOutOfTime"
    elseif nResult == 6 then
        strTips = "Safe.PhoneBondAccount"
    elseif nResult == 102 then
        strTips = "Safe.PhoneNotMatchAccount"
    elseif nResult == 103 then
        strTips = "Safe.GoldNotEnough"
    elseif nResult == 104 then
        strTips = "Safe.FrequentOperation"
    elseif nResult == 0 then
        ServerData.BindPhoneNumber = ""
        GameLuaEvent.UIEventUnbindPhoneSuccess()
        strTips = "Safe.UnbindPhoneSuccess"
    else
        strTips = "Safe.ErrorUnknowr"
    end

    ShowUITips(GetDictionary(strTips))
end
RegisterFunc(HrConstantMSG.kGameServerUnbindMobileNumber, UnBindPhoneNumResponse)

-- 通过手机绑定进行密码修改
local function ResetPasswordByPhoneResponse(bytes)
    Logger.Log("ResetPasswordByPhoneResponse")
    LuaCall.HideMask("kGameServerResetPasswordNew")
    local msg = assert(protoHelper.Decode("ResetPasswordResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! ResetPasswordResponse")
        return
    end

    local nResult = msg.result

    local strTips = ""

    if nResult == 1 then
        strTips = "Safe.PleaseReLogin"
    elseif nResult == 2 then
        strTips = "Safe.PleaseEnterCorrectOldPassword"
    elseif nResult == 3 then
        strTips = "Safe.NewPasswordError"
    elseif nResult == 4 then
        strTips = "Safe.NewPasswordEquelsOldPassword"
    elseif nResult == 5 then
        strTips = "Safe.VerifyCodeError"
    elseif nResult == 6 then
        strTips = "Safe.VerifyCodeErrorTooManyTimes"
    elseif nResult == 0 then
        strTips = "Safe.ModifyPasswordSuccess"
        LuaCall.LuaOpenForm(
            UIFormID.UIMessageBoxForm,
            {GetDictionary(strTips), GameLuaEvent.UIEventModifyPasswordSuccess, nil, 1}
        )
        return
    else
        strTips = "Safe.ErrorUnknow"
    end

    ShowUITips(GetDictionary(strTips))
end
RegisterFunc(HrConstantMSG.kGameServerResetPasswordNew, ResetPasswordByPhoneResponse)

-- 每日任务完成奖励
local function HandleFishingDailyMissionCompleteReward(bytes)
    Logger.Log("HandleFishingDailyMissionCompleteReward")
    LuaCall.HideMask("kFishServerLobbyDailyMissionCompleteReqRes")
    local msg = assert(protoHelper.Decode("FishingClientServerGetActivityReqRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingClientServerGetActivityReqRes")
        return
    end

    local nResult = msg.nResult
    if nResult == 0 then
        GameLuaEvent.UIEventDMReceiverReward(
            tonumber(msg.nMissionType),
            tonumber(msg.nDailyActivity),
            tonumber(msg.nWeekActivity)
        )
    else
        ShowUITips(GetDictionary("DailyMission.ReceiveRewardError"))
    end
end
RegisterFunc(HrConstantMSG.kFishServerLobbyDailyMissionCompleteReqRes, HandleFishingDailyMissionCompleteReward)
RegisterFunc(HrConstantMSG.kFishServerRoomDailyMissionCompleteReqRes, HandleFishingDailyMissionCompleteReward)
RegisterFunc(HrConstantMSG.kFishingClientLogicGetActivityReqRes, HandleFishingDailyMissionCompleteReward)
RegisterFunc(HrConstantMSG.kFishingClientLobbyGetActivityReqRes, HandleFishingDailyMissionCompleteReward)

local function HandleFishingActivityMissionCompleteReward(bytes)
    Logger.Log("HandleFishingActivityMissionCompleteReward")
    LuaCall.HideMask("kFishingClientLobbyGetActivityRewardReqRes")
    local msg = assert(protoHelper.Decode("FishingClientServerGetActivityRewardReqRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingClientServerGetActivityRewardReqRes")
        return
    end

    local nResult = msg.nResult

    if nResult == 0 then
        GetItem(msg.rewardItem, true)
        GameLuaEvent.UIEventDMReceiverStageReward(math.ceil(msg.nActivityType), math.ceil(msg.nIndex))
    else
        ShowUITips(GetDictionary("DailyMission.ReceiveRewardError"))
    end
end
RegisterFunc(HrConstantMSG.kFishingClientLogicGetActivityRewardReqRes, HandleFishingActivityMissionCompleteReward)
RegisterFunc(HrConstantMSG.kFishingClientLobbyGetActivityRewardReqRes, HandleFishingActivityMissionCompleteReward)

-- 收到服务器用户table信息
local function HandleServerQueryUserTableData(bytes)
    Logger.Log("HandleServerQueryUserTableData")
    local msg = assert(protoHelper.Decode("QueryUserTableDataResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! QueryUserTableDataResponse")
        return
    end

    local nCommand = msg.command
    local row = msg.row
    local userData = {}
    local tonumber = tonumber

    -- 每日任务完成信息
    if nCommand == HrConstantMSG.kFishLogicServerQueryUserNewDailyMission then
        if #row < 0 then
            return
        end

        for i = 1, #row do
            local rowData = {}
            local arrFieldIndex = row[i].index
            if arrFieldIndex ~= nil then
                for j = 1, #arrFieldIndex do
                    local name = arrFieldIndex[j].name
                    if name == "userid" then
                    elseif name == "missionid" then
                        rowData.missionID = tonumber(arrFieldIndex[j].value.intValue) + 1
                    end
                end
            end

            arrFieldIndex = row[i].value
            for j = 1, #arrFieldIndex do
                local name = arrFieldIndex[j].name
                if name == "progress" then
                    rowData.progress = tonumber(arrFieldIndex[j].value.intValue)
                elseif name == "status" then
                    rowData.status = tonumber(arrFieldIndex[j].value.intValue)
                end
            end

            table.insert(userData, rowData)
        end
        GameLuaEvent.LogicEventDailyMissionServerData(userData)
    elseif nCommand == HrConstantMSG.kFishLogicServerQueryUserActivity then
        if #row > 0 then
            for i = 1, #row do
                local rowData = {}
                local fieldIndexArr = row[i].value
                for j = 1, #fieldIndexArr do
                    local name = fieldIndexArr[j].name
                    if name == "dailyActivity" then
                        rowData.dailyActivity = tonumber(fieldIndexArr[j].value.intValue)
                    elseif name == "dailyRewardStatus" then
                        rowData.dailyRewardStatus = tonumber(fieldIndexArr[j].value.ulongValue)
                    elseif name == "monthActivity" then
                        rowData.monthActivity = tonumber(fieldIndexArr[j].value.intValue)
                    elseif name == "monthRewardStatus" then
                        rowData.monthRewardStatus = tonumber(fieldIndexArr[j].value.ulongValue)
                    elseif name == "dailyActivity" then
                        rowData.dailyActivity = tonumber(fieldIndexArr[j].value.intValue)
                    end
                end

                table.insert(userData, rowData)
            end
        end

        GameLuaEvent.LogicEventDailyMissionStageData(userData)
    elseif nCommand == HrConstantMSG.kFishLogicServerNewUserTask2 then
        Logger.Log("kFishLogicServerNewUserTask2 " .. #row)
        LuaCall.HideMask("kFishLogicServerNewUserTask2")
        if #row > 0 then
            for i = 1, #row do
                local rowData = {}
                local fieldIndexArr = row[i].index
                if fieldIndexArr ~= nil then
                    for j = 1, #fieldIndexArr do
                        local name = fieldIndexArr[j].name
                        if name == "taskid" then
                            rowData.taskID = math.ceil(fieldIndexArr[j].value.intValue)
                        end
                    end
                end

                local fieldValueArr = row[i].value
                for j = 1, #fieldValueArr do
                    local name = fieldValueArr[j].name
                    if name == "progress" then
                        rowData.progress = math.ceil(fieldValueArr[j].value.intValue)
                    elseif name == "rewardstatus" then
                        rowData.rewardstatus = math.ceil(fieldValueArr[j].value.intValue)
                    end
                end

                table.insert(userData, rowData)
            end
        end

        GameLuaEvent.LogicEventNewPlayerMissonServerData(userData)
    end
end

RegisterFunc(HrConstantMSG.kGameServerQueryUserTableData, HandleServerQueryUserTableData)

-- 没有绑定手机的修改密码
local function ResetPasswordByOldPwdResponse(bytes)
    Logger.Log("ResetPasswordByOldPwdResponse")
    LuaCall.HideMask("kGameServerModifyPassword")
    local msg = assert(protoHelper.Decode("ModifyPasswordResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! ModifyPasswordResponse")
        return
    end

    local strTips = ""
    local nResult = msg.result

    if nResult == 1 then
        strTips = "Safe.PleaseReLogin"
    elseif nResult == 2 then
        strTips = "Safe.PleaseEnterCorrectOldPassword"
    elseif nResult == 3 then
        strTips = "Safe.NewPasswordError"
    elseif nResult == 4 then
        strTips = "Safe.NewPasswordEquelsOldPassword"
    elseif nResult == 5 then
        strTips = "Safe.VerifyCodeError"
    elseif nResult == 6 then
        strTips = "Safe.VerifyCodeErrorTooManyTimes"
    elseif nResult == 0 then
        strTips = "Safe.ModifyPasswordSuccess"
        LuaCall.LuaOpenForm(
            UIFormID.UIMessageBoxForm,
            {GetDictionary(strTips), GameLuaEvent.UIEventModifyPasswordSuccess, nil, 1}
        )
        return
    else
        strTips = "Safe.ErrorUnknow"
    end

    ShowUITips(GetDictionary(strTips))
end

RegisterFunc(HrConstantMSG.kGameServerModifyPassword, ResetPasswordByOldPwdResponse)

--兑换京东卡回应
local function HandleClientJingDongGiftCardResponse(bytes)
    LuaCall.HideMask("kGameServerExchangeJingDongGiftCard")
    local msg = assert(protoHelper.Decode("ExchangeJingDongGiftCardResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! ExchangeJingDongGiftCardResponse")
        return
    end

    local nResult = msg.result
    local nSubNum = msg.subNum
    if nResult == 0 then
        GameLuaEvent.UIEventExchangeJDCardSuccess(nSubNum)
        --NetSender.SendGetUserFishInfo()
        PlayerData:AddItemCount(ItemID.RedPacket, -nSubNum)
    elseif nResult == 4 then
        ShowUITips(GetDictionary("Exchange.TodayIsBeenExchanged"))
    else
        ShowUITips(string.format(GetDictionary("Exchange.ExchangeErrorGoService"), nResult))
    end
end
RegisterFunc(HrConstantMSG.kGameServerExchangeJingDongGiftCard, HandleClientJingDongGiftCardResponse)

--兑换话费回应
local function HandleRedPacketExchangeMobileBillResponse(bytes)
    LuaCall.HideMask("kClientSendMobileBillRequestNew")
    local msg = assert(protoHelper.Decode("ServerSendExchangeMobileBillResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! ServerSendExchangeMobileBillResponse")
        return
    end

    local nResult = msg.result
    local reqhongbaomoney = msg.reqhongbaomoney
    local realhongbaomoney = msg.realhongbaomoney
    if nResult == 0 then
        PlayerData:SetItemCount(ItemID.RedPacket, realhongbaomoney)
        GameLuaEvent.UIEventExchangeHFSuccess(reqhongbaomoney)
    elseif nResult == 8 then
        ShowUITips(GetDictionary("Exchange.TodayIsBeenExchanged"))
    else
        ShowUITips(string.format(GetDictionary("Exchange.ExchangeErrorGoService"), nResult))
    end
end
RegisterFunc(HrConstantMSG.kClientSendMobileBillRequestNew, HandleRedPacketExchangeMobileBillResponse)

-- 实名认证
local function HandleRealNameVerifyResponse(bytes)
    LuaCall.HideMask("kGameServerRealNameVerify")

    local msg = assert(protoHelper.Decode("RealNameVerifyResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! RealNameVerifyResponse")
        return
    end

    local nResult = msg.result
    if msg.realNameVerifyInfo ~= nil then
        local realNameVerifyInfo = msg.realNameVerifyInfo
        local realNameData = ServerData.RealNameData
        realNameData:Reset()

        local canclose = nil
        if realNameVerifyInfo.popupInfo ~= nil then
            canclose = realNameVerifyInfo.popupInfo.canClose
        end

        realNameData:SetRealNameInfo(realNameVerifyInfo.age, canclose, realNameVerifyInfo.isVerified)

        local timeInfo = realNameVerifyInfo.timeInfo
        if timeInfo ~= nil then
            realNameData:SetRealNameTimeInfo(
                timeInfo.forbidLoginBeginTime,
                timeInfo.forbidLoginEndTime,
                timeInfo.dailyGameTimeUpperLimit,
                timeInfo.dailyGameTime
            )
        end

        local payInfo = realNameVerifyInfo.payInfo
        if payInfo ~= nil then
            realNameData:SetRealNamePayInfo(
                payInfo.singlePayMoneyUpperLimit,
                payInfo.monthPayMoneyUpperLimit,
                payInfo.monthPayMoney
            )
        end
    end
    if nResult == 0 then
        GetItem(msg.reward, true)
        ShowUITips(GetDictionary("RealNameForm.VerifySucceed"))
        GameLuaEvent.UIEventVerifyRealNameSuccess()
    elseif nResult == 1 then
        ShowUITips(GetDictionary("RealNameForm.VerifySucceedWithoutReward"))
        GameLuaEvent.UIEventVerifyRealNameSuccess()
    else
        ShowUITips(GetDictionary("RealNameForm.VerifyFailed"))
    end
end
RegisterFunc(HrConstantMSG.kGameServerRealNameVerify, HandleRealNameVerifyResponse)

-- 邮件信息回应
local function HandleAllMailResponse(bytes)
    Logger.Log("HandleAllMailResponse QueryAllMailResponse")
    local msg = assert(protoHelper.Decode("QueryAllMailResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! QueryAllMailResponse")
        return
    end

    -- todo 在邮件datasystem里添加收到的信息
    GameDataManager.ServerDataCache.Mail:UpdataSystemMail(msg.systemMail)
    GameDataManager.ServerDataCache.Mail:UpdataGameNotice(msg.systemNotice)

    -- 刷新邮件界面
    GameLuaEvent.UIEventReadMailSuccess()
end
RegisterFunc(HrConstantMSG.kGameServerQueryAllMail, HandleAllMailResponse)

-- 领取邮件内奖励回应
local function HandleMailRewardResponse(bytes, innerPacket)
    local msg = assert(protoHelper.Decode("GetMailRewardResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! GetMailRewardResponse")
        return
    end
    Logger.Log("HandleMailRewardResponse")
    local userID = msg.userID
    local mailID = msg.mailID
    local mailType = msg.mailType
    local result = msg.result -- 0成功 1不存在 2过期

    local command = 0
    local arrReward = {}

    if result == 1 then
        ShowUITips(GetDictionary("Mail.RewardNotExists"))
        return
    elseif result == 2 then
        ShowUITips(GetDictionary("Mail.RewardOutTime"))
        return
    end

    for i = 0, (innerPacket.Count - 1) do
        command = innerPacket[i].command
        if command == HrConstantMSG.kFishLogicClientMailRewardInnerPackRes then
            local msgs = protoHelper.Decode("RewardItemInfo", innerPacket[i].serialized)

            if nResult == 0 then
                GetItem(msgs.item, true)
            end
        end
    end

    -- todo发消息什么的
    GameLuaEvent.UIEventMailRewardSuccess(mailID)
end
RegisterFunc(HrConstantMSG.kGameServerGetMailReward, HandleMailRewardResponse)

-- 好友系统
-- 修改好友备注
-- 服务器此处给我返回
local EnumFriendOperationResult = require("Data.EnumFriendOperationResult")
local EnumFriendOperationType = require("Data.EnumFriendOperationType")
local function HandleFishingServerModifyFriendRemarkNickRes(bytes)
    local msg = assert(protoHelper.Decode("ModifyFriendRemarkNickResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! ModifyFriendRemarkNickResponse")
        return
    end
    Logger.Log("HandleFishingServerModifyFriendRemarkNickRes")

    if msg.result == 0 then
    -- todo消息 msg.friendUserID msg.remarkNick
    end
end
RegisterFunc(HrConstantMSG.kGameServerModifyFriendRemarkNick, HandleFishingServerModifyFriendRemarkNickRes)

-- 名人堂排行榜
local function HandleGlobalRankingListRes(bytes)
    --LuaCall.HideMask("kGameServerQueryGlobalRankingList")
    local msg = assert(protoHelper.Decode("QueryGlobalRankingListResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! QueryGlobalRankingListResponse")
        return
    end

    local nRankType = tonumber(msg.request.rankID)
    local arrRankingData = msg.topRanking

    -- arrRankingData
    -- userID: 197220
    -- ranking: 19.0
    -- value: 5224966178
    -- nick: 0I3
    -- vipLevel: 0
    -- extraValue: 0
    -- roleIndex: 0
    if nRankType == 1 then
        if #arrRankingData > 50 then
            for i = 1, 50 do
                GameDataManager.ServerDataCache.GlobalRanking.RicherRankWealthData[i] = arrRankingData[i]
            end
        else
            GameDataManager.ServerDataCache.GlobalRanking.RicherRankWealthData = arrRankingData
        end
    else
        if #arrRankingData > 50 then
            for i = 1, 50 do
                GameDataManager.ServerDataCache.GlobalRanking.RicherRankBulletData[i] = arrRankingData[i]
            end
        else
            GameDataManager.ServerDataCache.GlobalRanking.RicherRankBulletData = arrRankingData
        end
    end

    GameLuaEvent.UIEventRicherRankRefresh()
end
RegisterFunc(HrConstantMSG.kGameServerQueryGlobalRankingList, HandleGlobalRankingListRes)

-- 登录成功之后的额外消息
local function HandleFishingWorldExtraInfoNotice(bytes)
    local msg = assert(protoHelper.Decode("FishingLobbyExtraInfoResponse", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingLobbyExtraInfoResponse")
        return
    end

    local nServerTime = tonumber(msg.serverTime)

    CS.Hr.TimeUtils.SetStartServerTime(nServerTime, 0)
end
RegisterFunc(HrConstantMSG.kFishLobbyServerExtraInfoRes, HandleFishingWorldExtraInfoNotice)

--获取破产补助
local function HandleGetPovertyRewardRes(bytes)
    LuaCall.HideMask("kFishingClientLogicGetPovertyReward")
    local msg = assert(protoHelper.Decode("FishingCommonReqRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingCommonReqRes")
        return
    end

    local nResult = msg.result
    if nResult == 0 then
        local nDailyMaxCount = tonumber(msg.paramArr[1])
        local nRewardMoney = tonumber(msg.paramArr[2])
        local nCurrentTimes = tonumber(msg.paramArr[3])

        --重新设置破产补助次数
        GameDataManager.ServerDataCache.PovertyRewardAvailableCount = nDailyMaxCount - nCurrentTimes
        GameDataManager.ServerDataCache.PovertyRewardDailyMaxCount = nDailyMaxCount
        PlayerData:AddItemCount(ItemID.Gold, nRewardMoney)
        --通知领取成功
        GameLuaEvent.UIEventGetPovertyRewardSuccess(nRewardMoney)
    else
        --破产补助领取失败
        ShowUITips(GetDictionary("Break.GetPovertyRewardDefeated"))
    end
end
RegisterFunc(HrConstantMSG.kFishingClientLogicGetPovertyReward, HandleGetPovertyRewardRes)

--使用道具后的回应
local function HandleFishingUsePropRes(bytes, innerPacket, nCommand)
    local msg = assert(protoHelper.Decode("FishingUseFishingProp", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingUseFishingProp")
        return
    end

    local nResult = msg.result
    local nPropID = msg.propID
    local nCostItemID = msg.costItemID
    local nPropNum = msg.propNum --消耗的道具剩余数量
    local nSeatID = msg.seatID --座位号，应该不需要
    local nAddMoney = msg.addFishMoney

    if nCostItemID == nil then
        nCostItemID = nPropID
    end

    if nCommand == HrConstantMSG.kFishServerLobbyUsePropReqRes then
        LuaCall.HideMask("kFishServerLobbyUsePropReqRes")
    end
    if nCommand == HrConstantMSG.kFishServerRoomUsePropReqRes then
        LuaCall.HideMask("kFishServerRoomUsePropReqRes")
    end

    --print("HandleFishingUsePropRes", nPropID, nCostItemID, nPropNum)

    if nResult == 0 then
        PlayerData:SetItemCount(nCostItemID, nPropNum)
        GameLuaEvent.UIEventRoomMainUseSkillSuccess(nPropID)
    else
        ShowUITips(string.format(GetDictionary("MoneyBattle.PropsUseDefeated"), nResult))
    end
end
RegisterFunc(HrConstantMSG.kFishServerLobbyUsePropReqRes, HandleFishingUsePropRes)
RegisterFunc(HrConstantMSG.kFishServerRoomUsePropReqRes, HandleFishingUsePropRes)

-- 新手任务奖励领取
local function HandleNewPlayerMissionRewardRes(bytes)
    LuaCall.HideMask("kFishLogicClientGetNewPlayerMissionRewardReqRes")
    local msg = assert(protoHelper.Decode("FishingDailyMissionCompleteRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingDailyMissionCompleteRes")
        return
    end

    local nResult = msg.nResult
    if nResult == 0 then
        -- 展示奖励
        local reward = {}
        table.insert(
            reward,
            {
                itemType = msg.nRewardID,
                itemCount = msg.nRewardNum
            }
        )

        GetItem(reward, true, msg.nMissionIndex)
        GameLuaEvent.LogicEventNewPlayerMissionGetRewardSuccess(math.ceil(msg.nMissionIndex))
    else
        ShowUITips(GetDictionary("DailyMission.ReceiveRewardError"))
    end
end
RegisterFunc(HrConstantMSG.kFishLogicClientGetNewPlayerMissionRewardReqRes, HandleNewPlayerMissionRewardRes)

--冰冻状态通知
local function HandleFishingRoomFreezeTollgateResponse(bytes)
    local msg = assert(protoHelper.Decode("FishingScriptFreezeStatusAppearRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingScriptFreezeStatusAppearRes")
        return
    end

    local nSeatID = msg.seatID
    local nAppearType = msg.appearType --1.出现 2.消失
    local nLastTime = msg.lastTime
    Logger.Log(string.format("FishingScriptFreezeStatusAppearRes %d %d %d", nSeatID, nAppearType, nLastTime))

    GameLuaEvent.UIEventRoomMainFreezeEffect(nAppearType, nLastTime)
end
RegisterFunc(HrConstantMSG.kFishServerFreezeTollgateRes, HandleFishingRoomFreezeTollgateResponse)

local function HandleFishLogicClientGetNewPlayerGiftBagReqRes(bytes)
    LuaCall.HideMask("kFishLogicClientGetNewPlayerGiftBagReqRes")
    local msg = assert(protoHelper.Decode("FishingBuyGiftBagReqRes", bytes), 0)
    if msg == 0 then
        Logger.Log("decode error !!!!!!!!!!! FishingBuyGiftBagReqRes")
    end

    nResult = msg.nResult
    nRewardItems = msg.giftRewardItems
    if nResult == 0 then
        local rewardItemsInfo = {}
        for i = 1, #nRewardItems do
            rewardItemsInfo[i] = {}
            rewardItemsInfo[i].itemType = math.ceil(nRewardItems[i].itemType)
            rewardItemsInfo[i].itemCount = math.ceil(nRewardItems[i].itemCount)
        end
        GetItem(rewardItemsInfo, false)

        GameLuaEvent.UIEventGetNewPlayerGiftSuccess(nResult, rewardItemsInfo)
    else
        GameLuaEvent.UIEventGetNewPlayerGiftSuccess(nResult)
        ShowUITips(GetDictionary("NewPlayerGift.RewardTip_3") .. nResult)
    end
end
RegisterFunc(HrConstantMSG.kFishLogicClientGetNewPlayerGiftBagReqRes, HandleFishLogicClientGetNewPlayerGiftBagReqRes)

-- return 模块
netReceiver = NetReceiver.New()

return netReceiver
