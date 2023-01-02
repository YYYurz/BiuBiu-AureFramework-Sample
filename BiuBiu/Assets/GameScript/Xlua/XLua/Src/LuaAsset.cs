using System;
using BiuBiu;
using UnityEngine;

[Serializable]
public class LuaAsset : ScriptableObject
{
    public static string LuaDecodeKey = "AureAureAure";
    public bool encode = true;
    public byte[] data;

    public byte[] GetDecodeBytes()
    {
        return encode ? Security.XXTEA.Decrypt(data, LuaDecodeKey) : data;
    }
}