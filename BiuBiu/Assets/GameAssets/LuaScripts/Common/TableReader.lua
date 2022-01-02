local flatbuffers = require('3rd/flatbuffers')

---@class reader
local TableReader = {
	tableList = {},
}

local function GetTableLoadInfo(tbName)
	local bytesFileName = "Assets/GameAssets/DataTables/bytes/" .. tbName .. ".bytes"

	local className
	local index = string.find(tbName, "_")
	if index == nil then
		className = tbName .. "List"
	else
		className = string.sub(tbName, 1, index - 1) .. "List"
	end
	return className, bytesFileName
end

--- 获取配置表格数据
function TableReader:GetInfo(tb_name, key)
	local tb = self:GetTable(tb_name)
	if tb == nil then
		return nil
	end
	return tb[key]
end

--- 获取配置表格整张表
function TableReader:GetTable(tbName)
	local tableData = self.tableList[tbName]
	if tableData ~= nil then
		return tableData
	end

	local className, bytesFileName = GetTableLoadInfo(tbName)
	local tb_list = require("GameConfig/".. className)
	local fun_name = "GetRootAs".. className
	local fun = tb_list[fun_name]
	if fun == nil then
		Log.Error("Can not found function:"..fun_name)
		return nil
	end
	
	tableData = {}
	local data = LuaCallStatic.GetBytesFile(bytesFileName)
	local buf = flatbuffers.binaryArray.New(data)
	local dataList = fun(buf, 0)
	local len = dataList:DataLength()
	for i = 1, len do
		local dataItem = dataList:Data(i)
		if dataItem then
			local id = dataItem:Id()
			tableData[id] = dataItem
		end
	end
	
	self.tableList[tbName] = tableData
	return tableData
end

return TableReader
