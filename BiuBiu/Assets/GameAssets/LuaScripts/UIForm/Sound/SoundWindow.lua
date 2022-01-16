local UIBase = require "UIForm/UIBase"

---@class SoundWindow : UIBase 声音界面
local SoundWindow = UIBase:New()

function SoundWindow:ManualInit()
    self.BtnSound1 = self:GetButton(self.gameObject, "BtnSound1")
    self.BtnSound2 = self:GetButton(self.gameObject, "BtnSound2")
    self.BtnSound3 = self:GetButton(self.gameObject, "BtnSound3")
    self.BtnSound4 = self:GetButton(self.gameObject, "BtnSound4")

    self:AddListener(self.BtnSound1.onClick, function() self:OnPlaySound(1) end)
    self:AddListener(self.BtnSound2.onClick, function() self:OnPlaySound(1005) end)
    self:AddListener(self.BtnSound3.onClick, function() self:OnStopSound(self.curSoundId) end)
    self:AddListener(self.BtnSound4.onClick, function() self:CloseUI() end)
end

function SoundWindow:OnInit()
    UIBase.OnInit(self)
    self:ManualInit()

    print("SoundWindow OnInit")
end

function SoundWindow:OnOpen()
    UIBase.OnOpen(self)

    print("SoundWindow OnOpen")
end

function SoundWindow:OnClose()
    UIBase.OnClose(self)

    print("SoundWindow OnClose")
end

function SoundWindow:OnDestroy()
    UIBase.OnDestroy(self)
    self = nil
    SoundWindow = nil
end

function SoundWindow:OnLogin()
    self:CloseUI()
    CF:OpenUI(UIFormID.BattleWindow)
end

function SoundWindow:OnPlaySound(Id)
    self.curSoundId = LuaHelper.PlaySound(Id)
end

function SoundWindow:OnPauseSound(soundId)
    LuaHelper.PauseSound(soundId, 1)
end

function SoundWindow:OnResumeSound(soundId)
    LuaHelper.ResumeSound(soundId, 1)
end

function SoundWindow:OnStopSound(soundId)
    LuaHelper.StopSound(soundId, 1)
end

return SoundWindow