using GameFramework;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace BiuBiu
{
    public abstract class PreloadAssetList
    {
        protected List<PreloadAssetInfo> mLisAssetInfo = new List<PreloadAssetInfo>();

        public List<PreloadAssetInfo> GetAssetInfoList()
        {
            return mLisAssetInfo;
        }

        public virtual void Clear()
        {
            mLisAssetInfo.Clear();
        }
    }

    public class PreloadLuaFileList : PreloadAssetList
    {
        public void SetLuaFileInfo(List<LuaFileInfo> lisLuaFiles)
        {
            foreach (var item in lisLuaFiles)
            {
                var preAssetInfo = new PreloadAssetInfo(item.AssetName, 0, GameEnum.GAME_ASSET_TYPE.LuaFile, null, item.LuaName);
                mLisAssetInfo.Add(preAssetInfo);
            }
        }
    }

    public class PreloadDataTableList : PreloadAssetList
    {
        public void AddOneAssetInfo(Type type)
        {
            ITableReader tableReader = (ITableReader)Activator.CreateInstance(type);
            var preAssetInfo = new PreloadAssetInfo(tableReader.TablePath
                , 0
                , GameEnum.GAME_ASSET_TYPE.Scriptable
                , type
                , new ParseConfigDataInfo(type.Name, type, tableReader, GameEnum.GAME_ASSET_TYPE.DataTable));
    
            mLisAssetInfo.Add(preAssetInfo);
        }
    }

    public class PreloadUIFormList : PreloadAssetList
    {
        public void AddOneAssetInfo(int nFormID, object userData = null)
        {
            var preAssetInfo = new PreloadAssetInfo("UIForm" + nFormID
                , 0
                , GameEnum.GAME_ASSET_TYPE.UIForm
                , null
                , userData);

            preAssetInfo.UIFormID = nFormID;
            mLisAssetInfo.Add(preAssetInfo);
        }
    }

    public class PreloadPrefabList : PreloadAssetList
    {
        public void AddOneAssetInfo(string assetPath, object userData = null)
        {
            var preAssetInfo = new PreloadAssetInfo(assetPath
                , 0
                , GameEnum.GAME_ASSET_TYPE.Prefab
                , null
                , userData);
            mLisAssetInfo.Add(preAssetInfo);
        }

    }
}
