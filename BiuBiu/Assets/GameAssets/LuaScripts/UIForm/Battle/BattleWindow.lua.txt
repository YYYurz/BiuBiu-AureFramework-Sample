local UIBase = require "UIForm/UIBase"

---@class BattleWindow : UIBase 战斗界面
local BattleWindow = UIBase:New()

function BattleWindow:ManualInit()
	
end

function BattleWindow:OnCreate()
	UIBase.OnCreate(self)
	self:ManualInit()
end

function BattleWindow:OnOpen()
	UIBase.OnOpen(self)
end

function BattleWindow:OnClose()
	UIBase.OnClose(self)
end

function BattleWindow:OnDestroy()
	UIBase.OnDestroy(self)
	self = nil
	LoginWindow = nil
end

return BattleWindow