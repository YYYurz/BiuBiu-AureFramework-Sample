using System.Collections.Generic; 
using Google.Protobuf; 
using DrunkFish; 
public class ProtoMap{ 
    public static Dictionary<uint, string> CMDMap = new Dictionary<uint, string>() { 
        [1] = "CSReqCmpTime", 
        [2] = "CSResCmpTime", 
    }; 
    public static IMessage GetProtoMsg(uint opcode, byte[] bytes, int startIndex, int count) { 
        switch (opcode) { 
            case 1: return CSReqCmpTime.Parser.ParseFrom(bytes, startIndex, count); 
            case 2: return CSResCmpTime.Parser.ParseFrom(bytes, startIndex, count); 
        } 
        return null; 
    } 
} 
