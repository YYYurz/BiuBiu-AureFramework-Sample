
local LuaCSharpDataHelper = {}

local CSharpLuaDataHelper = nil
LuaCSharpDataHelper.UIBattleMoneyFormData =  nil

local function Startup(self)
    self:InitLuaArrAccess()
end

local function Stop(self)
    if CSharpLuaDataHelper == nil then
        CSharpLuaDataHelper = CS.Hr.CSharpLuaDataHelper()  
        CSharpLuaDataHelper:InitDataHelper()
    end 
end

local function PinUIBattleMoneyFormDataTable(self)
    if self.UIBattleMoneyFormData == nil then
        self.UIBattleMoneyFormData = LuaCSharpArr.New(123)

    end
    local CSharpAccess = self.UIBattleMoneyFormData:GetCSharpAccess()
    CSharpLuaDataHelper:PinPlayerDataTable(CSharpAccess)
end

function LuaCSharpDataHelper:InitLuaArrAccess()
    if CSharpLuaDataHelper == nil then
        CSharpLuaDataHelper = CS.Hr.CSharpLuaDataHelper()  
        CSharpLuaDataHelper:InitDataHelper()
    end 
end



LuaCSharpDataHelper.Startup = Startup
LuaCSharpDataHelper.PinUIBattleMoneyFormDataTable = PinUIBattleMoneyFormDataTable

return LuaCSharpDataHelper;

