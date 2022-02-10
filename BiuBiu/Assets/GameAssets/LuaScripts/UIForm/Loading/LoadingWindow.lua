local UIBase = require "UIForm/UIBase"

---@class LoadingWindow : UIBase 加载界面
local LoadingWindow = UIBase:New()

function LoadingWindow:ManualInit()
	--self.SliProgress = self:GetSlider(self.gameObject, "SliProgress")
end

function LoadingWindow:OnInit()
	UIBase.OnInit(self)
	self:ManualInit()
	print("LoadingWindow:OnInit")
end

function LoadingWindow:OnOpen()
	UIBase.OnOpen(self)
	print("LoadingWindow:OnOpen")
end

function LoadingWindow:OnClose()
	UIBase.OnClose(self)
	print("LoadingWindow:OnClose")
end

function LoadingWindow:OnProgressChange()
	
end

return LoadingWindow
