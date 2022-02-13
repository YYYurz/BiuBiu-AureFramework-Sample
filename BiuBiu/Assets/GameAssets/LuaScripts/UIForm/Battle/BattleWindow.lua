local UIBase = require "UIForm/UIBase"

---@class BattleWindow : UIBase 战斗界面
local BattleWindow = UIBase:New()

function BattleWindow:ManualInit()
	self.QuitBtn = self:GetChild(self.gameObject, "QuitBtn")
	
	self:AddClickListener(self.QuitBtn, self.OnClickQuitGame, self)
end

function BattleWindow:OnInit()
	UIBase.OnInit(self)
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
	BattleWindow = nil
end

function BattleWindow:OnClickQuitGame()
	LuaHelper.ChangeScene(1)
end

return BattleWindow
