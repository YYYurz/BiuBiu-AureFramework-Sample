local UIBase = require "UIForm/UIBase"

---@class LoginWindow : UIBase 登录界面
local LoginWindow = UIBase:New()

function LoginWindow:ManualInit()
	self.BtnPlay = self:GetChild(self.gameObject, "BtnPlay")
	self.BtnHelp = self:GetChild(self.gameObject, "BtnHelp")
	self.BtnSetting = self:GetChild(self.gameObject, "BtnSetting")

	self:AddClickListener(self.BtnPlay, self.OnClickLogin, self)
	self:AddClickListener(self.BtnHelp, self.OnClickHelp, self)
	self:AddClickListener(self.BtnSetting, self.OnClickSetting, self)
end

function LoginWindow:OnInit()
	UIBase.OnInit(self)
	self:ManualInit()
end

function LoginWindow:OnOpen()
	UIBase.OnOpen(self)
end

function LoginWindow:OnClose()
	UIBase.OnClose(self)
end

function LoginWindow:OnDestroy()
	UIBase.OnDestroy(self)
	self = nil
	LoginWindow = nil
end

function LoginWindow:OnClickLogin()
	LuaHelper.ChangeScene(SceneType.Battle, GameId.Game_1)
end

function LoginWindow:OnClickHelp()
	CF:OpenUI(UIFormId.HelpWindow)
end

function LoginWindow:OnClickSetting()
	CF:OpenUI(UIFormId.SettingWindow)
end

return LoginWindow
