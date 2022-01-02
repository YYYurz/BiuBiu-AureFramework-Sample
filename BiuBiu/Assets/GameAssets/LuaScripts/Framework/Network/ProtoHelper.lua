local pb = require("Framework.Network.Fish_pb")
local ProtoHelper = {}

local function Decode(strClassName, bytes)
    local class = pb[strClassName]()
    class:ParseFromString(bytes)

    return class
end

local function GetClass(strClassName)
    return pb[strClassName]()
end

local function Encode(class)
    return class:SerializeToString()
end

ProtoHelper.Decode = Decode
ProtoHelper.Encode = Encode
ProtoHelper.GetClass = GetClass

return ProtoHelper

