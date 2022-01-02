local luaBehaviour = require "Common/LuaBehaviour"
local UIBase = require "UIForm/UIBase"

---@class LogicBase : UIBase Lua层逻辑基类
local LogicBase = luaBehaviour:New()

--- 发送UI事件
function LogicBase:FireEvent(eventID, ...)
    UIBase:NotifyEvent(eventID, ...)
end

return LogicBase
