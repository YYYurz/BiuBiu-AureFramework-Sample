using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BiuBiu
{
    public class DataTableAssets
    {
        private readonly Dictionary<Type, ITableReader> dicDataTableReaders = new Dictionary<Type, ITableReader>();

        public void ParseDataTable(object asset, ParseConfigDataInfo parseConfigDataInfo)
        {
            var textAsset = asset as TextAsset;
            var tableReader = parseConfigDataInfo.UserData as ITableReader;
            if (textAsset != null) tableReader?.LoadDataFile(textAsset.bytes);
            dicDataTableReaders[parseConfigDataInfo.ConfigClassType] = tableReader;
        }

        public T GetDataTableReader<T>()
        {
            if (dicDataTableReaders.TryGetValue(typeof(T), out var tableReader))
            {
                return (T)tableReader;
            }

            return default;
        }
    }
}
