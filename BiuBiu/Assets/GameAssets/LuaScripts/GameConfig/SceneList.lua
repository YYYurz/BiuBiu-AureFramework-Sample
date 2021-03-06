-- automatically generated by the FlatBuffers compiler, do not modify

-- namespace: GameConfig

local flatbuffers = require('3rd.flatbuffers')

local SceneList = {} -- the module
local SceneList_mt = {} -- the class metatable

function SceneList.New()
    local o = {}
    setmetatable(o, {__index = SceneList_mt})
    return o
end
function SceneList.GetRootAsSceneList(buf, offset)
    local n = flatbuffers.N.UOffsetT:Unpack(buf, offset)
    local o = SceneList.New()
    o:Init(buf, n + offset)
    return o
end
function SceneList_mt:Init(buf, pos)
    self.view = flatbuffers.view.New(buf, pos)
end
function SceneList_mt:Data(j)
    local o = self.view:Offset(4)
    if o ~= 0 then
        local x = self.view:Vector(o)
        x = x + ((j-1) * 4)
        x = self.view:Indirect(x)
        local obj = require('GameConfig.Scene').New()
        obj:Init(self.view.bytes, x)
        return obj
    end
end
function SceneList_mt:DataLength()
    local o = self.view:Offset(4)
    if o ~= 0 then
        return self.view:VectorLen(o)
    end
    return 0
end
function SceneList.Start(builder) builder:StartObject(1) end
function SceneList.AddData(builder, data) builder:PrependUOffsetTRelativeSlot(0, data, 0) end
function SceneList.StartDataVector(builder, numElems) return builder:StartVector(4, numElems, 4) end
function SceneList.End(builder) return builder:EndObject() end

return SceneList -- return the module