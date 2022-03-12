local UIBase = require "UIForm/UIBase"

---@class HelpWindow : UIBase 帮助界面
local HelpWindow = UIBase:New()

function HelpWindow:ManualInit()
    self.BtnClose = self:GetChild(self.gameObject, "Bg/BtnClose")
    
    self:AddClickListener(self.BtnClose, self.CloseUI, self)
end

function HelpWindow:OnInit()
    UIBase.OnInit(self)
    self:ManualInit() 
end

function HelpWindow:OnOpen()
    UIBase.OnOpen(self)
end

function HelpWindow:OnClose()
    UIBase.OnClose(self)
end

function HelpWindow:OnDestroy()
    UIBase.OnDestroy(self)
    self = nil
    HelpWindow = nil
end

return HelpWindow