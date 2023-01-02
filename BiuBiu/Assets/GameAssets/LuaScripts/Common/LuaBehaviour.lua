local LuaHelper = LuaHelper

---@class LuaBehaviour Lua侧的Behavior基类
local LuaBehaviour = {}

function LuaBehaviour:New(target)	
	target = target or {}
	self.__index = self
	setmetatable(target, self)
	return target
end

function LuaBehaviour:AddClickListener(gameObj, callBack, luaSelf, param)
	if param == nil then
		LuaHelper.AddClickListener(gameObj, callBack, luaSelf)
	else
		LuaHelper.AddClickListener(gameObj, callBack, luaSelf, param)
	end
end

function LuaBehaviour:GetChild(obj, path)
	return LuaHelper.GetChild(obj, path)
end

function LuaBehaviour:GetText(obj, path)
	return LuaHelper.GetText(obj, path)
end

function LuaBehaviour:GetTextPro(obj, path)
	return LuaHelper.GetTextPro(obj, path)
end

function LuaBehaviour:GetImage(obj, path)
	return LuaHelper.GetImage(obj, path)
end

function LuaBehaviour:GetRawImage(obj, path)
	return LuaHelper.GetRawImage(obj, path)
end

function LuaBehaviour:GetButton(obj, path)
	return LuaHelper.GetButton(obj, path)
end

function LuaBehaviour:GetSlider(obj, path)
	return LuaHelper.GetSlider(obj, path)
end

function LuaBehaviour:GetContentList(obj, path)
	return LuaHelper.GetContentList(obj, path)
end

function LuaBehaviour:GetSpriteLoader(obj, path)
	return LuaHelper.GetSpriteLoader(obj, path)
end

function LuaBehaviour:LoadDialogue(filePath, callBack)
	LuaHelper.LoadDialogue(filePath, callBack)
end

return LuaBehaviour
