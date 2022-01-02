local UIBase = require "UIForm/UIBase"

---@class LoadingWindow : UIBase 加载界面
local LoadingWindow = UIBase:New()

function LoadingWindow:ManualInit()
	--self.SliProgress = self:GetSlider(self.gameObject, "SliProgress")
end

function LoadingWindow:OnCreate()
	UIBase:OnCreate()
	self:ManualInit()
end


function LoadingWindow:OnProgressChange()
	
end

return LoadingWindow