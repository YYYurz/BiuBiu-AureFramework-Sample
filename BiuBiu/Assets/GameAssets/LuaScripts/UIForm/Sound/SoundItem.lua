local ItemBase = require "UIForm/ItemBase"

---@class SoundItem : UIBase 声音Item
local SoundItem = ItemBase:New()

function SoundItem:OnInit()
    self.data = self.controller:GetItemDataByIndex(self.itemIndex)
    self.spriteLoader = self:GetSpriteLoader(self.gameObject, "Image")
end

function SoundItem:OnRefresh()
    self.spriteLoader:Load("TestIconAtlas", self.data)
end

function SoundItem:OnRelease()
    self = nil
    SoundItem = nil
end

return SoundItem