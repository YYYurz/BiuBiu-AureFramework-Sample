-- automatically generated by the FlatBuffers compiler, do not modify

-- namespace: GameConfig

local flatbuffers = require('3rd.flatbuffers')

local UIWindow = {} -- the module
local UIWindow_mt = {} -- the class metatable

function UIWindow.New()
    local o = {}
    setmetatable(o, {__index = UIWindow_mt})
    return o
end
function UIWindow.GetRootAsUIWindow(buf, offset)
    local n = flatbuffers.N.UOffsetT:Unpack(buf, offset)
    local o = UIWindow.New()
    o:Init(buf, n + offset)
    return o
end
function UIWindow_mt:Init(buf, pos)
    self.view = flatbuffers.view.New(buf, pos)
end
function UIWindow_mt:Id()
    local o = self.view:Offset(4)
    if o ~= 0 then
        return self.view:Get(flatbuffers.N.Uint32, o + self.view.pos)
    end
    return 0
end
function UIWindow_mt:UIName()
    local o = self.view:Offset(6)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function UIWindow_mt:UIGroupName()
    local o = self.view:Offset(8)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function UIWindow_mt:AssetName()
    local o = self.view:Offset(10)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function UIWindow_mt:LuaFile()
    local o = self.view:Offset(12)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function UIWindow.Start(builder) builder:StartObject(5) end
function UIWindow.AddId(builder, Id) builder:PrependUint32Slot(0, Id, 0) end
function UIWindow.AddUIName(builder, UIName) builder:PrependUOffsetTRelativeSlot(1, UIName, 0) end
function UIWindow.AddUIGroupName(builder, UIGroupName) builder:PrependUOffsetTRelativeSlot(2, UIGroupName, 0) end
function UIWindow.AddAssetName(builder, AssetName) builder:PrependUOffsetTRelativeSlot(3, AssetName, 0) end
function UIWindow.AddLuaFile(builder, LuaFile) builder:PrependUOffsetTRelativeSlot(4, LuaFile, 0) end
function UIWindow.End(builder) return builder:EndObject() end

return UIWindow -- return the module