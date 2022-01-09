-- 全局模块
require "Global"

---LuaDebugger
if LuaHelper.IsEditor() then
	package.cpath = package.cpath .. ';D:/Study/UnityProject/BiuBiu/EmmyLuaDebugger/?.dll'
	local dbg = require('emmy_core')
	dbg.tcpListen('localhost', 9966)
end

-- 定义为全局模块，整个lua程序的入口类
GameMain = {}

--主入口函数。从这里开始lua逻辑
function GameMain:Start()
	print("GameMain start!!!")
	self:RequireLogic()
end

--- 加载全局Logic模块
function GameMain:RequireLogic()
	local logicPath = {
		
	}
	for _, v in ipairs(logicPath) do
		require(v)
	end
end
 
function GameMain:OnApplicationQuit()
	UpdateManager:GetInstance():Dispose()
	TimerManager:GetInstance():Dispose()
end

GameMain:Start()

