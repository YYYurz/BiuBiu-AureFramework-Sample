local LuaBehaviour = require "Common/LuaBehaviour"
local LuaCall = CS.Hr.LuaCallStatic
local UIUtils = UIUtils

local openList = {}

---@class UIBase : LuaBehaviour Lua层UI基类
local UIBase = LuaBehaviour:New()

function UIBase:OnInit()
    self.UIListenerList = {}
    self.EventListenerList = {}
    self.tableLoadAsset = {}
end

function UIBase:OnOpen()
    if self.uiFormId == nil then
        print("Oh lo")
    end
    openList[self.uiFormId] = self
end

function UIBase:OnDestroy()
    for k, _ in pairs(self.UIListenerList) do
        k:RemoveAllListeners()
        k:Invoke()
    end
    self.UIListenerList = nil
    self.EventListenerList = nil
end

function UIBase:OnClose()
    for _, v in pairs(self.tableLoadAsset) do
        LuaCall.UnloadAsset(v)
    end
    if openList[self.uiFormId] then
        openList[self.uiFormId] = nil
    end
    self.tableLoadAsset = {}

    for k, _ in pairs(self.UIListenerList) do
        k:RemoveAllListeners()
        k:Invoke()
    end
    self.UIListenerList = nil
    self.EventListenerList = nil
end

--- 添加Lua侧的Logic向UI发送的事件
function UIBase:AddEventListener(uiEventID, callBack)
    if type(callBack) ~= "function" then
        return
    end
    self.EventListenerList[uiEventID] = callBack
end

--- 接受到Logic发送的事件，触发回调
function UIBase:NotifyEvent(eventID, ...)
    for _, v in pairs(openList) do
        for id, event in pairs(v.EventListenerList) do
            if id == eventID then
                event(...)
            end
        end
    end
end

function UIBase:AddLoadAsset(nAsset)
    table.insert(self.tableLoadAsset, nAsset)
end

--- 关闭自己UI
function UIBase:CloseUI()
    CF:CloseWindow(self.uiFormId)
end

--- 添加Unity UI组件的事件监听
function UIBase:AddListener(unityEventBase, func)
    unityEventBase.AddListener(unityEventBase, func)
    self.UIListenerList[unityEventBase] = 1
end

function UIBase:AddButtonDownListener(button, callback, table)
    if self.UIListenerList[button] ~= nil then
        print("UIBase : AddButtonDownListener false")
    end
    self.UIListenerList[button] = 1
    UIUtils.AddButtonClickListener(button, callback, table)
end


return UIBase
