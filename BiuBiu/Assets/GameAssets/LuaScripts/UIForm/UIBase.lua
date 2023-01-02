local LuaBehaviour = require "Common/LuaBehaviour"

local openList = {}

---@class UIBase : LuaBehaviour UI基类
local UIBase = LuaBehaviour:New()

function UIBase:OnInit()
    self.EventListenerList = {}
end

function UIBase:OnOpen(param)
    openList[self.uiFormId] = self
end

function UIBase:OnUpdate(elapseTime)
    
end

function UIBase:OnDestroy()
    
end

function UIBase:OnClose()
    if openList[self.uiFormId] then
        openList[self.uiFormId] = nil
    end
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

--- 关闭自己
function UIBase:CloseUI()
    CF:CloseUI(self.uiFormId)
end

return UIBase

