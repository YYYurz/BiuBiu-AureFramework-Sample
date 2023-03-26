﻿//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace DrunkFish
{
	/// <summary>
	/// 配置表模块接口
	/// </summary>
	public interface IDataTableModule
	{
		/// <summary>
		/// 获取读表类
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetDataTableReader<T>() where T : ITableReader;
	}
}