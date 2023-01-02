---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by YuZhiRui.
--- DateTime: 2022/10/16 1:22
---

local UIBase = require "UIForm/UIBase"

---@class DialogueWindow : UIBase 剧情界面
local DialogueWindow = UIBase:New()

function DialogueWindow:ManualInit()
    self.dialogueContent = self:GetTextPro(self.gameObject, "Dialogue/DialogueContent")
end

function DialogueWindow:OnInit()
    UIBase.OnInit(self)
    self:ManualInit()
end

function DialogueWindow:OnOpen(param)
    UIBase.OnOpen(self)
    
    print(param)
    
    self.speed = 1
    self:LoadDialogue("Assets/GameAssets/Dialogue/Dialogue_1001.asset", Utils.bind(self.OnLoadDialogueSuccess, self))
end

function DialogueWindow:OnUpdate(elapseTime)
    UIBase.OnUpdate(self, elapseTime)
    
end

function DialogueWindow:OnClose()
    UIBase.OnClose(self)
end

function DialogueWindow:OnDestroy()
    UIBase.OnDestroy(self)
    self = nil
    DialogueWindow = nil
end

function DialogueWindow:OnLoadDialogueSuccess(dialogue)
    self.dialogue = dialogue
    self:StartDialogue()
end

function DialogueWindow:StartDialogue()
    
end

return DialogueWindow