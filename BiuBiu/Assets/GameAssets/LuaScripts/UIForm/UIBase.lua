local LuaBehaviour = require "Common/LuaBehaviour"
local LuaHelper = LuaHelper

local openList = {}

---@class UIBase : LuaBehaviour Lua层UI基类
local UIBase = LuaBehaviour:New()

function UIBase:OnInit()
    self.UIListenerList = {}
    self.EventListenerList = {}
    self.tableLoadAsset = {}
end

function UIBase:OnOpen()
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
        LuaHelper.UnloadAsset(v)
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

--- 关闭自己
function UIBase:CloseUI()
    CF:CloseUI(self.uiFormId)
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
    LuaHelper.AddButtonClickListener(button, callback, table)
end


return UIBase

