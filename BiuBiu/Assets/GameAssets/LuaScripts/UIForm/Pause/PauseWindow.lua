local UIBase = require "UIForm/UIBase"

---@class PauseWindow : UIBase 战斗暂停界面
local PauseWindow = UIBase:New()

function PauseWindow:ManualInit()
    self.BtnExit = self:GetChild(self.gameObject, "Bg/BtnExit")
    self.BtnReturn = self:GetChild(self.gameObject, "Bg/BtnReturn")
    
    self:AddClickListener(self.BtnExit, self.OnClickExit, self)
    self:AddClickListener(self.BtnReturn, self.OnClickReturn, self)
end

function PauseWindow:OnInit()
    UIBase.OnInit(self)
    self:ManualInit() 
end

function PauseWindow:OnOpen()
    UIBase.OnOpen(self)

    LuaHelper.PauseGame()
end

function PauseWindow:OnClose()
    UIBase.OnClose(self)
end

function PauseWindow:OnDestroy()
    UIBase.OnDestroy(self)
    self = nil
    PauseWindow = nil
end

function PauseWindow:OnClickExit()
    LuaHelper.ChangeScene(SceneType.Normal, SceneId.MainLobby)
end

function PauseWindow:OnClickReturn()
    LuaHelper.ResumeGame()
    self:CloseUI()
end

return PauseWindow