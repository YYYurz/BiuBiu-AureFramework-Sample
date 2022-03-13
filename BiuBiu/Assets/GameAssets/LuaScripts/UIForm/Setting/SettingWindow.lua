local UIBase = require "UIForm/UIBase"

---@class SettingWindow : UIBase 设置界面
local SettingWindow = UIBase:New()

function SettingWindow:ManualInit()
    self.BtnSound = self:GetChild(self.gameObject, "Bg/BtnSound")
    self.BtnClose = self:GetChild(self.gameObject, "Bg/BtnClose")
    self.SoundOpen = self:GetChild(self.gameObject, "Bg/BtnSound/SoundOpen")
    self.SoundClose = self:GetChild(self.gameObject, "Bg/BtnSound/SoundClose")
    self.SliderSound = self:GetSlider(self.gameObject, "Bg/Slider")

    self.SliderSound.onValueChanged:AddListener(function(volume) self:OnVolumeChange(volume) end);
    self:AddClickListener(self.BtnSound, self.OnClickSound, self)
    self:AddClickListener(self.BtnClose, self.CloseUI, self)
end

function SettingWindow:OnInit()
    UIBase.OnInit(self)
    self:ManualInit() 
end

function SettingWindow:OnOpen()
    UIBase.OnOpen(self)

    self:Refresh()
end

function SettingWindow:OnClose()
    UIBase.OnClose(self)
end

function SettingWindow:OnDestroy()
    self.SliderSound.onValueChanged:RemoveAllListeners()

    UIBase.OnDestroy(self)
    self = nil
    SettingWindow = nil
end

function SettingWindow:Refresh()
    self.SliderSound.value = LuaHelper.Volume;
    if LuaHelper.Mute then
        self.SliderSound.interactable = false
        self.SoundOpen:SetActive(false)
        self.SoundClose:SetActive(true)
    else
        self.SliderSound.interactable = true
        self.SoundOpen:SetActive(true)
        self.SoundClose:SetActive(false)
    end
end

function SettingWindow:OnClickSound()
    if not LuaHelper.Mute then
        LuaHelper.Mute = true
        self.SliderSound.interactable = false
        self.SoundOpen:SetActive(false)
        self.SoundClose:SetActive(true)
    else
        LuaHelper.Mute = false
        self.SliderSound.interactable = true
        self.SoundOpen:SetActive(true)
        self.SoundClose:SetActive(false)
    end
end

function SettingWindow:OnVolumeChange(volume)
    LuaHelper.Volume = volume
end

return SettingWindow