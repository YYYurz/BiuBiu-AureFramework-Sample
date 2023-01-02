local LuaHelper = LuaHelper

---@class CommonFunction 公共函数调用接口
CommonFunction = {}

function CommonFunction:IsUIOpen(uiFormID)
	LuaHelper.IsUIOpen(uiFormID)
end

function CommonFunction:OpenUI(uiFormID, param)
	if param then
		LuaHelper.OpenUI(uiFormID, param)
	else
		LuaHelper.OpenUI(uiFormID)
	end
end

function CommonFunction:CloseUI(uiFormID)
	LuaHelper.CloseUI(uiFormID)
end

return CommonFunction

