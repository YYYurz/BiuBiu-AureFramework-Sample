local UIUtils = UIUtils

---@class CommonFunction 公共函数调用接口
CommonFunction = {}

function CommonFunction:IsUIOpen(uiFormID)
	UIUtils.IsUIOpen(uiFormID)
end

function CommonFunction:OpenWindow(uiFormID)
	UIUtils.OpenUI(uiFormID)
end

function CommonFunction:CloseWindow(uiFormID)
	UIUtils.CloseUI(uiFormID)
end

return CommonFunction
