local UIBase = require "UIForm/UIBase"

---@class SoundWindow : UIBase 声音界面
local SoundWindow = UIBase:New()

function SoundWindow:GetItemDataByIndex(itemIndex)
    return self.itemDataList[itemIndex + 1]
end

function SoundWindow:ManualInit()
    self.BtnSound1 = self:GetChild(self.gameObject, "BtnSound1")
    self.BtnSound2 = self:GetChild(self.gameObject, "BtnSound2")
    self.BtnSound3 = self:GetChild(self.gameObject, "BtnSound3")
    self.BtnSound4 = self:GetChild(self.gameObject, "BtnSound4")
    self.UIContentList = self:GetContentList(self.gameObject, "Scroll View")
    
    self:AddClickListener(self.BtnSound1, self.OnPlaySound, self, 1)
    self:AddClickListener(self.BtnSound2, self.OnPlaySound, self, 1001)
    self:AddClickListener(self.BtnSound3, self.OnPlaySound, self, 1002)
    self:AddClickListener(self.BtnSound4, self.OnPlaySound, self, 1003)
end

function SoundWindow:OnInit()
    UIBase.OnInit(self)
    self:ManualInit() 
    
    self.itemDataList = {}
    for i = 1, 100 do
        table.insert(self.itemDataList, "Free Flat Arrow 1 E Icon")
        table.insert(self.itemDataList, "Free Flat Arrow 1 N Icon")
        table.insert(self.itemDataList, "Free Flat Arrow 1 NE Icon")
        table.insert(self.itemDataList, "Free Flat Arrow 2 E Icon")
        table.insert(self.itemDataList, "Free Flat Battery No Icon")
    end
end

function SoundWindow:OnOpen()
    UIBase.OnOpen(self)

    self.UIContentList:RefreshContentList(#self.itemDataList, self)
end

function SoundWindow:OnClose()
    UIBase.OnClose(self)

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

function SoundWindow:OnPlaySound(soundId)
    self.curSoundId = LuaHelper.PlaySound(soundId)
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