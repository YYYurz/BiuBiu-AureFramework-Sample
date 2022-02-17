local UIBase = require "UIForm/UIBase"

---@class LoginWindow : UIBase 登录界面
local LoginWindow = UIBase:New()

function LoginWindow:ManualInit()
	self.BtnPlay = self:GetChild(self.gameObject, "BtnPlay")

	self:AddClickListener(self.BtnPlay, self.OnLogin, self)
end

function LoginWindow:OnInit()
	UIBase.OnInit(self)
	self:ManualInit()
	
	print("LoginWindow OnInit")
end

function LoginWindow:OnOpen()
	UIBase.OnOpen(self)
	
	print("LoginWindow OnOpen")
end

function LoginWindow:OnClose()
	UIBase.OnClose(self)
	
	print("LoginWindow OnClose")
end

function LoginWindow:OnDestroy()
	UIBase.OnDestroy(self)
	self = nil
	LoginWindow = nil
end

function LoginWindow:OnLogin()
	LuaHelper.ChangeScene(2, 1)
end

return LoginWindow
