local UIBase = require "UIForm/UIBase"

local SoundWindow = UIBase:New()

function SoundWindow:ManualInit()
    self.BtnSound1 = self:GetButton(self.gameObject, "BtnSound1")
    self.BtnSound2 = self:GetButton(self.gameObject, "BtnSound2")
    self.BtnSound3 = self:GetButton(self.gameObject, "BtnSound3")
    self.BtnSound4 = self:GetButton(self.gameObject, "BtnSound4")

    self:AddListener(self.BtnSound1.onClick, function() self:OnPlaySound("TestSound1", "Sound") end)
    self:AddListener(self.BtnSound2.onClick, function() self:OnPlaySound("TestSound2", "Sound") end)
    self:AddListener(self.BtnSound3.onClick, function() self:OnPlaySound("TestSound3", "Sound") end)
    self:AddListener(self.BtnSound4.onClick, function() self:OnPlaySound("SoundReverse", "Bgm") end)
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

function SoundWindow:OnPlaySound(soundName, soundGroupName)
    LuaHelper.PlaySound(soundName, soundGroupName)
end

return SoundWindow