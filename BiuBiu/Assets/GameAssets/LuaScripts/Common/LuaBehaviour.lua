local LuaHelper = LuaHelper

---@class LuaBehaviour Lua侧的Behavior基类
local LuaBehaviour = {}

function LuaBehaviour:New(target)	
	target = target or {}
	self.__index = self
	setmetatable(target, self)
	return target
end

function LuaBehaviour:GetText(obj, path)
	return LuaHelper.GetText(obj, path)
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

return LuaBehaviour
