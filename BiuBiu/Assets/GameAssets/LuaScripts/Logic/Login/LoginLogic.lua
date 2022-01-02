local LogicBase = require "Logic/LogicBase"

---@class LoginLogic : LogicBase 加载界面
LoginLogic = LogicBase:New()

function LoginLogic:Fire()
    self:FireEvent(UIEventID.EVENT_Test)
end 