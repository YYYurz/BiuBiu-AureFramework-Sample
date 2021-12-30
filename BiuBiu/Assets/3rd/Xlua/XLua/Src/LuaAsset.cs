using System;
using BiuBiu;
using UnityEngine;

[Serializable]
public class LuaAsset : ScriptableObject
{
    public static string LuaDecodeKey = "AureAureAure";
    public bool encode = true;
    public byte[] data;

    public static byte[] Require(string luaPath, string search = "", int retry = 0)
    {
        if(string.IsNullOrEmpty(luaPath))
            return null;
            
        var assetName = search + luaPath.Replace(".", "/") + ".lua";
        var bytes = AssetUtils.LoadBytes(assetName);

        Debug.Assert(bytes != null, $"{luaPath} not found.");
        return bytes;
    }
    
    public byte[] GetDecodeBytes()
    {
        return encode ? Security.XXTEA.Decrypt(data, LuaDecodeKey) : data;
    }
}