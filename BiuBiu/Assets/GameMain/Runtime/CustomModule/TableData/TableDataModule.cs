//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework;

namespace BiuBiu
{
	/// <summary>
	/// 配置表模块
	/// </summary>
	public sealed class TableDataModule : AureFrameworkModule, ITableDataModule
	{
		private readonly Dictionary<Type, ITableReader> readerDic = new Dictionary<Type, ITableReader>();

		public override int Priority => 20;

		public override void Init()
		{
		}

		public override void Tick(float elapseTime, float realElapseTime)
		{
		}

		public override void Clear()
		{
		}

		/// <summary>
		/// 获取读表类
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetDataTableReader<T>() where T : ITableReader
		{
			if (readerDic.TryGetValue(typeof(T), out var tableReader))
			{
				return (T) tableReader;
			}

			return LoadTable<T>();
		}

		private T LoadTable<T>() where T : ITableReader
		{
			var tableReader = (ITableReader) Activator.CreateInstance(typeof(T));
			tableReader.LoadDataFile();
			readerDic.Add(typeof(T), tableReader);

			return (T) tableReader;
		}
	}
}