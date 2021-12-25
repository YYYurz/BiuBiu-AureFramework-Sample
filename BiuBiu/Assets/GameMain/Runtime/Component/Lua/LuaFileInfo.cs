 

using BB;
using System;
using UnityEngine;

[Serializable]
public class LuaFileInfo
{
    public LuaFileInfo(string luaName)
    {
        LuaName = luaName;
        AssetName = AssetUtils.GetLuaAsset(luaName);
    }

    [SerializeField]
    private string luaName;
    public string LuaName
    {
        private set
        {
            luaName = value;
        }
        get
        {
            return luaName;
        }
    }

    [SerializeField]
    private string assetName;
    public string AssetName
    {
        private set
        {
            assetName = value;
        }
        get
        {
            return assetName;
        }
    }
}