local UIUtils = UIUtils

---@class LuaBehaviour Lua侧的Behavior基类
local LuaBehaviour = {}

function LuaBehaviour:New(target)	
	target = target or {}
	self.__index = self
	setmetatable(target, self)
	return target
end

function LuaBehaviour:GetText(obj, path)
	return UIUtils.GetText(obj, path)
end

function LuaBehaviour:GetImage(obj, path)
	return UIUtils.GetImage(obj, path)
end

function LuaBehaviour:GetRawImage(obj, path)
	return UIUtils.GetRawImage(obj, path)
end

function LuaBehaviour:GetButton(obj, path)
	return UIUtils.GetButton(obj, path)
end

function LuaBehaviour:GetSlider(obj, path)
	return UIUtils.GetSlider(obj, path)
end

return LuaBehaviour