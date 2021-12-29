using System.Collections.Generic;
using BiuBiu;
using FlatBuffers;
using UnityEngine;

public interface ITableReader {
	/// <summary>
	/// 配置表二进制文件路径
	/// </summary>
	string TablePath
	{
		get;
	}
	
	/// <summary>
	/// 读表类主动加载二进制文件
	/// </summary>
	void LoadDataFile();
}

/// <summary>
/// 读表类基类
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TDataList"></typeparam>
public abstract class TableReader<TData, TDataList> : ITableReader where TData : struct {
	private readonly Dictionary<uint, TData> tableDataDic = new Dictionary<uint,TData>();
	
	public bool GetInfo(uint key, out TData data) {
		return tableDataDic.TryGetValue(key, out data);
	}

	public TData GetInfo(uint key) {
		if (tableDataDic.TryGetValue(key, out var data)) {
			return data;
		}

		return default;
	}

	/// <summary>
	/// 读表类主动加载二进制文件
	/// </summary>
	public void LoadDataFile() {
		var data = GameMain.Resource.LoadAssetSync<TextAsset>(TablePath).bytes; 
		var byteBuffer = new ByteBuffer(data);
		var dataList = GetTableDataList(byteBuffer);

		var dataLen = GetDataLength(dataList);
		tableDataDic.Clear();
		for (var i = 0; i < dataLen; ++i) {
			var td = GetData(dataList, i);
			if (td != null) {
				tableDataDic.Add(GetKey(td.Value), td.Value);
			}
		}
	}

	public abstract string TablePath { get; }
	protected abstract TDataList GetTableDataList(ByteBuffer byteBuffer);
	protected abstract int GetDataLength(TDataList dataList);
	protected abstract TData? GetData(TDataList dataList, int i);
	protected abstract uint GetKey(TData data);
}
