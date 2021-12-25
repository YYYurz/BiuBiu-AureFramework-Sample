using System;
using System.Collections.Generic;
using FlatBuffers;

public interface ITableReader
{
    string TablePath { get; }
    void LoadDataFile(byte[] data);
}

public abstract class TableReader<TData, TDataList, TDataReader> : ITableReader
    where TData : struct
    where TDataReader : ITableReader, new()
{
    private static TDataReader _gInstance;
    public static TDataReader Instance
    {
        get
        { 
            if (_gInstance != null) {
                return _gInstance;
            }
            _gInstance = new TDataReader();
            return _gInstance;
        }
    }

    public Dictionary<uint, TData> TableDatas { get; private set; }

    public bool GetInfo(uint key, out TData data) => TableDatas.TryGetValue(key, out data);

    public TData? GetInfoN(uint key)
    {
        TData data;
        if (TableDatas.TryGetValue(key, out data))
        {
            return data;
        }
        return null;
    }

    public TData GetInfo(uint key)
    {
        var ret = GetInfoN(key);
        return ret ?? default(TData);
    }

    private static string GetFilePath(string filename) => filename;

    public void LoadDataFile(byte[] data)
    {
        var filePath = GetFilePath(TablePath);
        //byte[] data = null;// Ark.GameUtilsResourceMgr.LoadLuaDataFile(filePath);
        var byteBuffer = new ByteBuffer(data);
        var dataList = GetTableDataList(byteBuffer);

        var dataLen = GetDataLength(dataList);
        TableDatas = new Dictionary<uint, TData>(dataLen);
        for (var i = 0; i < dataLen; ++i)
        {
            var td = GetData(dataList, i);
            if (td != null) {
                TableDatas.Add(GetKey(td.Value), td.Value);
            }
        }
    }

    // ReSharper disable once UnusedMember.Global
    public static string GetWord(string word) => word;

    public abstract string TablePath { get; }
    protected abstract TDataList GetTableDataList(ByteBuffer byteBuffer);
    protected abstract int GetDataLength(TDataList dataList);
    protected abstract TData? GetData(TDataList dataList, int i);
    protected abstract uint GetKey(TData data);
}


// ReSharper disable once UnusedMember.Global
public static class TableReaderRef
{
    private static readonly Dictionary<string, Dictionary<uint, IFlatbufferObject>> Tables = new Dictionary<string, Dictionary<uint, IFlatbufferObject>>();

    private static string GetBytesFilePath(string tbName) => "data/" + tbName + ".bytes";

    private static Dictionary<uint, IFlatbufferObject> LoadTable(string tbName)
    {
        Dictionary<uint, IFlatbufferObject> dict;
        if (Tables.TryGetValue(tbName, out dict))
        {
            return dict;
        }

        var filePath = GetBytesFilePath(tbName);
        byte[] data = null;// Ark.GameUtilsResourceMgr.LoadLuaDataFile(filePath);
        var byteBuffer = new ByteBuffer(data);

        var tList = Type.GetType($"GameConfig.{tbName}List");
        var t = Type.GetType($"GameConfig.{tbName}");

        if (tList != null)
        {
            var getMethod = tList.GetMethod($"GetRootAs{tbName}List", new Type[] { typeof(ByteBuffer) });
            if (t != null)
            {
                var idProperty = t.GetProperty("Id");

                if (getMethod != null)
                {
                    var obj = getMethod.Invoke(null, new object[] { byteBuffer });

                    var lenProperty = tList.GetProperty("DataLength");
                    if (lenProperty != null)
                    {
                        var dataLen = (int)lenProperty.GetValue(obj, null);

                        var dataMethod = tList.GetMethod("Data");

                        dict = new Dictionary<uint, IFlatbufferObject>(dataLen);
                        for (var i = 0; i < dataLen; ++i)
                        {
                            // ReSharper disable once PossibleNullReferenceException
                            var dt = dataMethod.Invoke(obj, new object[] { i });
                            var flatbufferObject = dt as IFlatbufferObject;
                            if (flatbufferObject == null) {
                                continue;
                            }
                            if (idProperty != null) {
                                dict.Add((uint)idProperty.GetValue(flatbufferObject, null),
                                    flatbufferObject);
                            }
                        }
                    }
                }
            }
        }
        Tables.Add(tbName, dict);
        return dict;
    }

    // ReSharper disable once UnusedMember.Global
    public static IFlatbufferObject GetInfo(string tbName, uint key) => LoadTable(tbName)?[key];

    // ReSharper disable once UnusedMember.Global
    public static Dictionary<uint, IFlatbufferObject> GetTable(string tbName) => LoadTable(tbName);
}

