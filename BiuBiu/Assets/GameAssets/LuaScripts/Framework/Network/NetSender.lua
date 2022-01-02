local protoHelper = require("Framework.Network.ProtoHelper")
local MSGCommand = HrConstantMSG
local LuaSendPacket = CS.Hr.LuaCallStatic.LuaSendPacket
local LuaCall = CS.Hr.LuaCallStatic
local PlayerData = LuaCall.GetPlayerData()
local ServerData = LuaCall.GetServerData()
local TollgateData = LuaCall.GetTollgateData()
-- -- protobuf用法
-- local fishingRoomStatus = protoHelper.GetClass("FishingRoomStatus")
-- -- repeated 引用类型赋值
-- local room1 = fishingRoomStatus.singleRoomStatus:add()
-- local room1.roomID = 1
-- local room1.status = 2
-- local room1.roomMoneyLimit = 3

-- repeated 值类型字段赋值
-- valueType.append(1)
-- valueType.append(2)

-- -- 打包成bytes
-- local encode = protoHelper.Encode(fishingRoomStatus)

-- -- 解包bytes
-- local decode = protoHelper.Dncode("FishingRoomStatus", encode)

-- -- 发送消息
-- LuaSend(MSGCommand.kGameServerEnterGameRequest, encode)
-- -- 发送http标记位为1的消息
-- LuaSend(MSGCommand.kGameServerEnterGameRequest, encode, 1)

local NetSender = {}

-- todo 对于一些比较频繁的消息进行缓存

-- 登录前请求更新公告说明
function NetSender.SendGetStateServerNoticeInfoReq(gameCode)
    LuaCall.ShowMask("kGameServerStateServerGetNoticeInfo")
    local req = protoHelper.GetClass("GetStateServerNoticeInfoRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! GetStateServerNoticeInfoRequest")
    end
    req.gameCode = gameCode
    LuaSendPacket(MSGCommand.kGameServerStateServerGetNoticeInfo, protoHelper.Encode(req), 1)
end

-- 登出在C#
-- function NetSender.SendLogOut()
--     local req = protoHelper.GetClass("FishingLogoutRequest")
--     LuaSendPacket(MSGCommand.kFishServerLogout, protoHelper.Encode(req))
-- end

-- 请求服务器发送注册手机的验证码
function NetSender.GameServerRequestPhoneRegisterVerifyCode(strPhoneNumber, userID)
    Logger.Log("send request server phone register verifycode " .. strPhoneNumber)
    -- show mask kGameServerGetVerificationCodeNewMobile
    local reqVerifyCode = protoHelper.GetClass("GetVerificationCodeNewMobileRequest")
    reqVerifyCode.mobileNumber = strPhoneNumber

    LuaSendPacket(MSGCommand.kGameServerGetVerificationCodeNewMobile, protoHelper.Encode(reqVerifyCode))
end
-- 函数完

--飞机属性升级
function NetSender.SendUpgradeAircraftReq(nAircraftID, nType, nLevel)
    LuaCall.ShowMask("kFishAircraftUpgradeReqRes")
    local req = protoHelper.GetClass("FishingCommonReqRes")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! FishingCommonReqRes")
    end
    req.paramArr:append(nAircraftID)
    req.paramArr:append(nType)
    req.paramArr:append(nLevel)
    Logger.Log("SendUpgradeAircraftReq")
    LuaSendPacket(MSGCommand.kFishAircraftUpgradeReqRes, protoHelper.Encode(req))
end

--兑换物品
function NetSender.SendExchangeItemReq(exchangeType, exchangeNum)
    LuaCall.ShowMask("kFishLogicClientExchangeItemReqRes")
    local req = protoHelper.GetClass("FishingExchangeItemReqRes")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! FishingExchangeItemReqRes")
    end
    req.nResult = 0
    req.nExchangeType = exchangeType
    req.nExchangeGroup = exchangeNum

    if TollgateData.RoomID == HrConstantGame.Lobby then
        LuaSendPacket(MSGCommand.kFishLogicClientExchangeItemReqRes, protoHelper.Encode(req))
    else
        LuaSendPacket(MSGCommand.kFishLogicClientRoomExchangeItemReqRes, protoHelper.Encode(req))
    end
end

-- 闯关模式 关卡结束请求 0 失败 1 成功
function NetSender.SendSurvivePassTollgateReq(nTollgateIndex, nPassFlag)
    LuaCall.ShowMask("kFishAircraftSurvivePassReqRes")
    Logger.Log("SendSurvivePassTollgateReq!")
    local req = protoHelper.GetClass("FishingCommonReqRes")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! FishingCommonReqRes")
    end
    req.paramArr:append(nTollgateIndex)
    req.paramArr:append(nPassFlag)
    LuaSendPacket(MSGCommand.kFishAircraftSurvivePassReqRes, protoHelper.Encode(req))
end

-- 获取广告墙，整包更新信息，模块更新信息

function NetSender.SendGetUpgradeInfoBeforeLogin()
    local req = protoHelper.GetClass("GetStateUpdateInfoRequest")

    req.adWallRequest.deviceType = 1
    req.adWallRequest.gameCode = 58
    req.adWallRequest.version = 30000898

    req.moduleRequest.td = "huawei"
    req.moduleRequest.tag = "test1000"

    req.packageRequest.channelname = "gwdlby"
    req.packageRequest.packagename = "com_bohaoo_dlby"

    LuaSendPacket(MSGCommand.kGameServerStateServerUpdateInfo, protoHelper.Encode(req), 1)
end

-- 购买vip
function NetSender.SendClientBuyVIPReq()
    LuaCall.ShowMask("kFishingClientBuyVIPReqRes")
    Logger.Log("SendClientBuyVIPReq!")
    local nRoomType = TollgateData.RoomID
    if nRoomType == HrConstantGame.Lobby then
        LuaSendPacket(MSGCommand.kFishingClientBuyVIPReqRes, nil)
    else
        LuaSendPacket(MSGCommand.kFishingClientBuyVIP2ReqRes, nil)
    end
end

-- 升级炮台
function NetSender.SendUpgradeBetReq(nToBetIndex)
    Logger.Log("SendUpgradeBetReq : " .. nToBetIndex)
    LuaCall.ShowMask("kFishServerUpgradeBetIndexReqRes")

    local req = protoHelper.GetClass("FishingUpgradeGunBetIndex")

    req.result = 0
    req.seatID = 0
    req.toUpgradeBetIndex = nToBetIndex
    req.nSecondMoney = 0
    LuaSendPacket(MSGCommand.kFishServerUpgradeBetIndexReqRes, protoHelper.Encode(req))
end

-- 领取vip大礼包
function NetSender.SendGetVipRewardReq(vipLevel)
    LuaCall.ShowMask("kGameServerGetVipReward")
    local req = protoHelper.GetClass("GetVipRewardRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! GetVipRewardRequest")
    end

    req.userID = PlayerData.UserID
    req.vipLevel = vipLevel

    LuaSendPacket(MSGCommand.kGameServerGetVipReward, protoHelper.Encode(req))
end

-- 查找好友返回
function NetSender.SendSearchFriendReq(userID)
    LuaCall.ShowMask("kGameServerSearchFriend")
    local req = protoHelper.GetClass("SearchFriendRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! SearchFriendRequest")
    end

    local nUserID = tonumber(userID)
    req.userID = nUserID

    LuaSendPacket(MSGCommand.kGameServerSearchFriend, protoHelper.Encode(req))
end

-- 赠送好友物品
function NetSender.SendGivePresentItemReq(fromUserID, toUserID, itemType, itemCount, fromUserNick)
    local req = protoHelper.GetClass("PresentItemRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! PresentItemRequest")
    end

    req.fromUserID = fromUserID
    req.toUserID = toUserID
    req.itemType = itemType
    req.itemCount = itemCount
    req.fromUserNick = fromUserNick
    LuaSendPacket(MSGCommand.kGameServerPresentItem, protoHelper.Encode(req))
end

-- 更改用户信息请求
--  enumUserInfoFieldNameNick = 1;
-- 	enumUserInfoFieldNameAvatar = 2;//only used for query
-- 	enumUserInfoFieldNameGender = 3;
-- 	enumUserInfoFieldNameAvatarForModify = 4;//only used for modify
-- 	enumUserInfoFieldNameVIPLevel = 5;
-- 	enumUserInfoFieldNameMobileNumber = 6;
-- 	enumUserInfoFieldNameGameTurnSum = 7;
-- 	enumUserInfoFieldNameContinuousWinTime = 8;
-- 	enumUserInfoFieldNameCoupon = 9;
-- 	enumUserInfoFieldNameBuySpecialGoodsFirst = 10;
-- 	enumUserInfoFieldNameFishWeaponLevel = 11;
-- 	enumUserInfoFieldNameAvatarIndex = 12;
-- 	enumUserInfoFieldNameServiceFee = 13;
-- 	enumUserInfoFieldNameScoreWinFromRobot = 14;

-- 	enumUserInfoFieldNamePassword = 15;
-- 	enumUserInfoFieldNameSpeakerNum = 16;
-- 	enumUserInfoFieldNameTotalExchangeValue = 17;
-- 	enumUserInfoFieldNameTexasPokerBestCards = 18;
-- 	enumUserInfoFieldNameEmail = 19;
-- 	enumUserInfoFieldNameTexasPokerAvatarIndex = 20;
-- 	enumUserInfoFieldNameNewGiftIndex = 21;
-- 	enumUserInfoFieldNameNewGiftID = 22;
-- 	enumUserInfoFieldNameRoleIndex = 23;
-- 	enumUserInfoFieldNameUserIDCard = 24;
-- 	enumUserInfoFieldNameUserIDCardName = 25;
-- 	enumUserInfoFieldNameUseLoginProtection = 26;
-- 	enumUserInfoFieldNameNewNick = 27;
function NetSender.SendModifyUserInfoReq(userID, kv)
    LuaCall.ShowMask("kGameServerModifyUserInfoRequest")
    Logger.Log("ModifyUserInfoReq")
    local req = protoHelper.GetClass("GameServerModifyUserInfoRequest")
    req.userid = userID
    for i = 1, #kv do
        local newkv = req.kv:add()
        newkv.fieldName = kv[i].fieldName
        newkv.fieldValue = tostring(kv[i].fieldValue)
    end

    LuaSendPacket(MSGCommand.kGameServerModifyUserInfoRequest, protoHelper.Encode(req))
end

-- 接收礼物的请求
function NetSender.SendAcceptPresentItemReq(nPresentInfoID)
    LuaCall.ShowMask("kGameServerAcceptPresentItem")
    local req = protoHelper.GetClass("FishingAcceptPresentItemRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! FishingAcceptPresentItemRequest")
    end

    req.userID = PlayerData.UserID
    req.presentRequestId = nPresentInfoID

    LuaSendPacket(MSGCommand.kGameServerAcceptPresentItem, protoHelper.Encode(req))
end

-- 请求商城列表
function NetSender.SendGetShopInfoRequest(td)
    LuaCall.ShowMask("kGameServerGetShopInfo")
    local req = protoHelper.GetClass("GetShopInfoRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! SendGetShopInfoRequest")
    end

    req.td = td
    LuaSendPacket(MSGCommand.kGameServerGetShopInfo, protoHelper.Encode(req))
end

-- 请求用户特别的信息
function NetSender.SendQueryUserExtraInfoRequest()
    Logger.Log("SendQueryUserExtraInfoRequest kGameServerQueryUserExtraInfo")
    -- LuaCall.ShowMask("kGameServerQueryUserExtraInfo")

    local req = protoHelper.GetClass("QueryUserExtraInfoRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! SendQueryUserExtraInfoRequest")
    end

    req.userID = PlayerData.UserID

    LuaSendPacket(MSGCommand.kGameServerQueryUserExtraInfo, protoHelper.Encode(req))
end

-- 获取手机验证码
function NetSender.SendRequestBindPhoneVerifyCode(phone)
    local req = protoHelper.GetClass("GetVerificationCodeRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! GetVerificationCodeRequest")
        return
    end

    req.userID = "" .. PlayerData.UserID
    req.type = 0

    if phone == nil then
        req.mobileNumber = ServerData.BindPhoneNumber
    else
        req.mobileNumber = phone
    end

    Logger.Log("get verifycode userID : " .. req.userID .. " mobileNumber : " .. phone)

    LuaSendPacket(MSGCommand.kGameServerGetVerificationCode, protoHelper.Encode(req))
end

-- 绑定手机
function NetSender.SendBindPhoneNumRequest(strPhoneNumber, strVerifyCode)
    local req = protoHelper.GetClass("BindMobileNumberRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! BindMobileNumberRequest")
        return
    end
    LuaCall.ShowMask("kGameServerBindMobileNumber")

    Logger.Log("SendBindPhoneNumRequest " .. strPhoneNumber .. "  code " .. strVerifyCode)
    req.userID = PlayerData.UserID
    req.mobileNumber = strPhoneNumber
    req.verificationCode = strVerifyCode

    LuaSendPacket(MSGCommand.kGameServerBindMobileNumber, protoHelper.Encode(req))
end

-- 解绑手机
function NetSender.SendUnBindPhoneNumRequest(strVerifyCode)
    local req = protoHelper.GetClass("UnbindMobileNumberRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! UnbindMobileNumberRequest")
        return
    end
    LuaCall.ShowMask("kGameServerUnbindMobileNumber")

    Logger.Log("SendUnBindPhoneNumRequest " .. "  code " .. strVerifyCode)
    req.userID = PlayerData.UserID
    req.verificationCode = strVerifyCode

    LuaSendPacket(MSGCommand.kGameServerUnbindMobileNumber, protoHelper.Encode(req))
end

-- 绑定手机下修改密码
function NetSender.SendResetPasswordByPhoneRequest(strVerifyCode, strNewPassword)
    Logger.Log("SendResetPasswordByPhoneRequest")

    local req = protoHelper.GetClass("ResetPasswordRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! ResetPasswordRequest")
        return
    end
    LuaCall.ShowMask("kGameServerResetPasswordNew")

    req.userID = PlayerData.UserID
    req.verificationCode = strVerifyCode
    req.newPassword = strNewPassword

    LuaSendPacket(MSGCommand.kGameServerResetPasswordNew, protoHelper.Encode(req))
end

-- 未绑定手机下修改密码
function NetSender.SendResetPasswordByOldPwdRequest(strOldPassword, strNewPassword)
    local req = protoHelper.GetClass("ModifyPasswordRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! ModifyPasswordRequest")
        return
    end

    Logger.Log("SendResetPasswordByOldPwdRequest")
    LuaCall.ShowMask("kGameServerModifyPassword")

    req.userID = PlayerData.UserID
    req.oldPassword = strOldPassword
    req.newPassword = strNewPassword

    LuaSendPacket(MSGCommand.kGameServerModifyPassword, protoHelper.Encode(req))
end
--兑换京东卡
function NetSender.SendClientLogicJingDongGiftCardReq(num, phoneNum)
    LuaCall.ShowMask("kGameServerExchangeJingDongGiftCard")
    local req = protoHelper.GetClass("ExchangeJingDongGiftCardRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! SendClientLogicJingDongGiftCardReq")
        return
    end

    req.num = num
    req.mobile = phoneNum

    LuaSendPacket(MSGCommand.kGameServerExchangeJingDongGiftCard, protoHelper.Encode(req))
end

-- 重新查询玩家基本数据 调用这个消息的时候注意
-- function NetSender.SendGetUserFishInfo()
--     local req = protoHelper.GetClass("QueryUserFishInfo")
--     if req == nil then
--         Logger.LogError("error!!! invalid!!!! SendGetUserFishInfo")
--         return
--     end

--     req.userID = PlayerData.UserID
--     --req.roomID
--     --req.sessionID
--     LuaSendPacket(MSGCommand.kFishServerQueryUserBasicFishInfo, protoHelper.Encode(req))
-- end

--兑换话费
function NetSender.SendRedPackExchangeBillReq(money, phoneNum)
    LuaCall.ShowMask("kClientSendMobileBillRequestNew")
    local req = protoHelper.GetClass("ClientSendPhoneBillRequest")
    if req == nil then
        Logger.LogError("error!!! invalid!!!! SendRedPackExchangeBillReq")
        return
    end

    req.userid = PlayerData.UserID
    req.hongbaomoney = money
    req.mobileNumber = phoneNum
    LuaSendPacket(MSGCommand.kClientSendMobileBillRequestNew, protoHelper.Encode(req))
end
-- 好友操作

local serverFriendOperationReq = protoHelper.GetClass("FriendOperation")
serverFriendOperationReq.type = 0
serverFriendOperationReq.msg = ""
serverFriendOperationReq.timestamp = 0
serverFriendOperationReq.result = 0
local fromUserInfo = serverFriendOperationReq.fromUser
local toUserInfo = serverFriendOperationReq.toUser

local EnumFriendOperationType = require("Data.EnumFriendOperationType")

function NetSender.SendFriendOperationReq(
    nType,
    nFromUserID,
    strFromNick,
    nFromRoleIndex,
    nToUserID,
    strToNick,
    nToRoleIndex,
    strReason,
    nTimestamp,
    nResult)
    if nType == EnumFriendOperationType.enumRejectFriendRequest then
        LuaCall.ShowMask("kFishServerFriendOperation")
    end

    Logger.Log("SendFriendOperationReq type: " .. nType)
    fromUserInfo.userID = nFromUserID
    fromUserInfo.nick = strFromNick
    fromUserInfo.roleIndex = nFromRoleIndex

    toUserInfo.userID = nToUserID
    toUserInfo.nick = strToNick
    toUserInfo.roleIndex = nToRoleIndex

    serverFriendOperationReq.type = nType

    if strReason == nil then
        serverFriendOperationReq.msg = ""
    else
        serverFriendOperationReq.msg = strReason
    end

    if nTimestamp == nil then
        serverFriendOperationReq.timestamp = -1
    else
        serverFriendOperationReq.timestamp = nTimestamp
    end

    if nResult == nil then
        serverFriendOperationReq.result = 0
    else
        serverFriendOperationReq.result = nResult
    end

    LuaSendPacket(MSGCommand.kFishServerFriendOperation, protoHelper.Encode(serverFriendOperationReq))
end

-- 我收到服务器聊天返回后, 主动发送已收到确认消息
function NetSender.SendFriendOperationReqACK(nType, fromUser, toUser, strReason, nTimestamp, nResult)
    Logger.Log("SendFriendOperationReqACK type : " .. nType)
    fromUserInfo.userID = tonumber(fromUser.userID)
    fromUserInfo.nick = fromUser.nick
    fromUserInfo.roleIndex = tonumber(fromUser.roleIndex)

    fromUserInfo.userID = tonumber(toUser.userID)
    fromUserInfo.nick = fromUser.nick
    fromUserInfo.roleIndex = tonumber(toUser.roleIndex)

    if nTimestamp == nil then
        serverFriendOperationReq.timestamp = -1
    else
        serverFriendOperationReq = nTimestamp
    end

    serverFriendOperationReq.type = nType
    serverFriendOperationReq.msg = strReason
    serverFriendOperationReq.result = nResult

    LuaSendPacket(MSGCommand.kGameServerFriendOperationAck, protoHelper.Encode(serverFriendOperationReq))
end

-- 主动请求好友列表, 返回好友列表和最新别人发给我的消息
function NetSender.SendGetFriendOperationListReq(nVersion)
    local req = protoHelper.GetClass("GetFriendInfoRequest")
    if req == nil then
        Logger.LogError("error! GetFriendInfoRequest")
        return
    end

    Logger.Log("SendGetFriendOperationListReq")

    req.version = nVersion
    req.userID = PlayerData.UserID
    LuaSendPacket(MSGCommand.kGameServerGetFriendOperationList, protoHelper.Encode(req))
end

-- 修改好友备注
function NetSender.SendModifyFriendRemarkNickReq(nUserID, strName)
    local req = protoHelper.GetClass("ModifyFriendRemarkNickRequest")
    if req == nil then
        Logger.LogError("error! ModifyFriendRemarkNickRequest")
        return
    end

    Logger.Log("ModifyFriendRemarkNickReq " .. nUserID .. "  " .. strName)

    req.userID = PlayerData.UserID
    req.friendUserID = tonumber(nUserID)
    req.remarkNick = strName

    LuaSendPacket(MSGCommand.kGameServerModifyFriendRemarkNick, protoHelper.Encode(req))
end

-- 请求所有邮箱信息
function NetSender.SendAllMailReq()
    Logger.Log("SendAllMailReq QueryAllMailRequest")
    local req = protoHelper.GetClass("QueryAllMailRequest")
    if req == nil then
        Logger.LogError("error! QueryAllMailRequest")
        return
    end

    req.userID = PlayerData.UserID

    -- LuaCall.ShowMask("kGameServerQueryAllMail")
    LuaSendPacket(MSGCommand.kGameServerQueryAllMail, protoHelper.Encode(req))
end

-- 请求单个邮件内奖励
function NetSender.SendMailRewardReq(nMailID, nMailType)
    local req = protoHelper.GetClass("GetMailRewardRequest")
    if req == nil then
        Logger.LogError("error! GetMailRewardRequest")
        return
    end

    Logger.Log("SendMailRewardReq")
    -- mailType 1系统邮件 2游戏公告
    req.userID = PlayerData.UserID
    req.mailID = nMailID
    req.mailType = nMailType
    LuaSendPacket(MSGCommand.kGameServerGetMailReward, protoHelper.Encode(req))
end

-- 实名认证
function NetSender.SendRealNameVerifyReq(strName, strCode)
    local req = protoHelper.GetClass("RealNameVerifyRequest")
    if req == nil then
        Logger.LogError("error! RealNameVerifyRequest")
        return
    end

    LuaCall.ShowMask("kGameServerRealNameVerify")
    req.realName = strName
    req.idCardNum = strCode

    LuaSendPacket(MSGCommand.kGameServerRealNameVerify, protoHelper.Encode(req))
end

-- 捕鱼里的功能排行榜
function NetSender.SendFishingRankRequest(uniqueID, dataType, rankType)
    local req = protoHelper.GetClass("QueryRankRequest")
    if req == nil then
        Logger.LogError("error! QueryRankRequest")
        return
    end

    LuaCall.ShowMask("kFishRankQueryRankRequest")
    req.userID = PlayerData.UserID
    req.uniqueID = uniqueID
    req.dataType = dataType
    req.rankType = rankType
    req.needRanks = {}

    LuaSendPacket(MSGCommand.kFishRankQueryRankRequest, protoHelper.Encode(req))
end

-- 请求名人堂排行榜数据 todo回应回来是100名的数据,客户端只要前50名
function NetSender.SendGlobalRankingListReq(nTypeIndex)
    local req = protoHelper.GetClass("QueryGlobalRankingListRequest")
    if req == nil then
        Logger.LogError("error! QueryGlobalRankingListRequest")
        return
    end

    --LuaCall.ShowMask("kGameServerQueryGlobalRankingList")
    req.rankID = nTypeIndex

    LuaSendPacket(MSGCommand.kGameServerQueryGlobalRankingList, protoHelper.Encode(req))
end

-- 请求刷新今日任务
function NetSender.SendFishingDailyMissionRefreshReq(nMissionIndex)
    LuaCall.ShowMask("kFishServerLobbyRefreshDailyMissionReqRes")
    local req = protoHelper.GetClass("FishingRefreshDailyMissionReq")
    if req == nil then
        Logger.LogError("error! FishingRefreshDailyMissionReq")
        return
    end

    req.nMissionIndex = nMissionIndex

    if TollgateData.RoomID > 0 then
        LuaSendPacket(MSGCommand.kFishServerRoomRefreshDailyMissionReqRes, protoHelper.Encode(req))
    else
        LuaSendPacket(MSGCommand.kFishServerLobbyRefreshDailyMissionReqRes, protoHelper.Encode(req))
    end
end

-- 今日任务领奖
function NetSender.SendFishingDailyMissionCompleteRewardReq(nMissionIndex)
    LuaCall.ShowMask("kFishServerLobbyDailyMissionCompleteReqRes")
    local req = protoHelper.GetClass("FishingClientServerGetActivityReqRes")
    if req == nil then
        Logger.LogError("error! FishingClientServerGetActivityReqRes")
        return
    end

    req.nResult = 0
    req.nMissionType = nMissionIndex

    if TollgateData.RoomID > 0 then
        LuaSendPacket(MSGCommand.kFishingClientLogicGetActivityReqRes, protoHelper.Encode(req))
    else
        LuaSendPacket(MSGCommand.kFishingClientLobbyGetActivityReqRes, protoHelper.Encode(req))
    end
end

-- 活跃度完成请求奖励
function NetSender.SendFishingActivityRewardReq(nActivityType, nMissionIndex)
    LuaCall.ShowMask("kFishingClientLobbyGetActivityRewardReqRes")
    local req = protoHelper.GetClass("FishingClientServerGetActivityRewardReqRes")
    if req == nil then
        Logger.LogError("error! FishingClientServerGetActivityRewardReqRes")
        return
    end

    req.nResult = 0
    req.nActivityType = nActivityType
    req.nIndex = nMissionIndex

    if TollgateData.RoomID <= 0 then
        LuaSendPacket(MSGCommand.kFishingClientLobbyGetActivityRewardReqRes, protoHelper.Encode(req))
    else
        LuaSendPacket(MSGCommand.kFishingClientLogicGetActivityRewardReqRes, protoHelper.Encode(req))
    end
end

-- 请求破产补助
function NetSender.SendGetPovertyRewardReq()
    LuaCall.ShowMask("kFishingClientLogicGetPovertyReward")
    local req = protoHelper.GetClass("FishingGetPovertyRewardRequest")
    if req == nil then
        Logger.LogError("error! FishingGetPovertyRewardRequest")
        return
    end

    req.userID = PlayerData.UserID

    LuaSendPacket(MSGCommand.kFishingClientLogicGetPovertyReward, protoHelper.Encode(req))
end

-- 使用道具
-- 大厅内使用道具
function NetSender.SendFishingUsePropReq(nPropID)
    LuaCall.ShowMask("kFishServerLobbyUsePropReqRes")
    local req = protoHelper.GetClass("FishingUseFishingProp")
    if req == nil then
        Logger.LogError("error! FishingUseFishingProp")
        return
    end

    req.result = 0
    req.propID = nPropID

    LuaSendPacket(MSGCommand.kFishServerLobbyUsePropReqRes, protoHelper.Encode(req))
end
-- 房间内使用道具
function NetSender.SendFishingRoomUsePropReq(nPropID)
    LuaCall.ShowMask("kFishServerRoomUsePropReqRes")
    local req = protoHelper.GetClass("FishingUseFishingProp")
    if req == nil then
        Logger.LogError("error! FishingUseFishingProp")
        return
    end

    req.result = 0
    req.propID = nPropID

    LuaSendPacket(MSGCommand.kFishServerRoomUsePropReqRes, protoHelper.Encode(req))
end

-- 请求新手任务数据
function NetSender.SendNewPlayerRoomMissionInfoReq()
    local req = protoHelper.GetClass("QueryUserTableDataRequest")
    if req == nil then
        Logger.LogError("error! QueryUserTableDataRequest")
        return
    end

    Logger.Log("SendNewPlayerRoomMissionInfoReq")

    req.userID = PlayerData.UserID
    req.command = MSGCommand.kFishLogicServerNewUserTask2
    local index = req.index:add()

    index.name = "userid"
    index.value.ulongValue = req.userID

    LuaCall.ShowMask("kFishLogicServerNewUserTask2")
    LuaSendPacket(MSGCommand.kGameServerQueryUserTableData, protoHelper.Encode(req))
end

-- 请求新手任务奖励
function NetSender.SendGetNewPlayerRoomMissionReq(nMissionIndex)
    LuaCall.ShowMask("kFishLogicClientGetNewPlayerMissionRewardReqRes")

    local req = protoHelper.GetClass("FishingDailyMissionCompleteRes")
    if req == nil then
        Logger.LogError("error! FishingDailyMissionCompleteRes")
        return
    end

    req.nMissionIndex = nMissionIndex
    req.nResult = 0
    req.nRewardID = 0
    req.nRewardNum = 0
    LuaSendPacket(MSGCommand.kFishLogicClientGetNewPlayerMissionRewardReqRes, protoHelper.Encode(req))
end

-- 请求新手大礼包
function NetSender.SendGetNewPlayerGiftReq()
    LuaCall.ShowMask("kFishLogicClientGetNewPlayerGiftBagReqRes")
    local req = protoHelper.GetClass("FishingBuyGiftBagReqRes")
    if req == nil then
        Logger.LogError("error! FishingBuyGiftBagReqRes")
        return
    end

    req.nResult = 0
    req.nGiftBagLevel = 0

    LuaSendPacket(MSGCommand.kFishLogicClientGetNewPlayerGiftBagReqRes, protoHelper.Encode(req))
end

return NetSender

