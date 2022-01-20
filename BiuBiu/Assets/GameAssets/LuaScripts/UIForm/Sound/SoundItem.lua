local ItemBase = require "UIForm/ItemBase"

---@class SoundItem : UIBase 声音Item
local SoundItem = ItemBase:New()

function SoundItem:OnOpen()
    UIBase.OnOpen(self)
end

function SoundItem:OnClose()
    UIBase.OnClose(self)
end

function SoundItem:OnDestroy()
    UIBase.OnDestroy(self)
    self = nil
    SoundItem = nil
end

return SoundItem