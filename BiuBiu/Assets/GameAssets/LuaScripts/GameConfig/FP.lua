-- automatically generated by the FlatBuffers compiler, do not modify

-- namespace: GameConfig

local flatbuffers = require('3rd.flatbuffers')

local FP = {} -- the module
local FP_mt = {} -- the class metatable

function FP.New()
    local o = {}
    setmetatable(o, {__index = FP_mt})
    return o
end
function FP_mt:Init(buf, pos)
    self.view = flatbuffers.view.New(buf, pos)
end
function FP_mt:Raw()
    return self.view:Get(flatbuffers.N.Int64, self.view.pos + 0)
end
function FP.CreateFP(builder, raw)
    builder:Prep(8, 8)
    builder:PrependInt64(raw)
    return builder:Offset()
end

return FP -- return the module
