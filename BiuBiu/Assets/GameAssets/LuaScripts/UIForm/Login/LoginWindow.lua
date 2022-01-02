local UIBase = require "UIForm/UIBase"

---@class LoginWindow : UIBase 登录界面
local LoginWindow = UIBase:New()

function LoginWindow:ManualInit()
	self.BtnPlay = self:GetButton(self.gameObject, "BtnPlay")

	self:AddListener(self.BtnPlay.onClick, Utils.bind(self.OnLogin, self))
	
	self:AddEventListener(UIEventID.EVENT_Test, self.OnCallBack)
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
	--self:CloseUI()
	CF:OpenWindow(UIFormID.BattleWindow)
end

return LoginWindow