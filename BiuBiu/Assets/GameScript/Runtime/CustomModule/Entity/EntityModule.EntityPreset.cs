//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework;
using UnityEngine;

namespace BiuBiu
{
	public sealed partial class EntityModule : AureFrameworkModule
	{
		/// <summary>
		/// 实体预设
		/// </summary>
		[Serializable]
		public sealed class EntityPreset
		{
			/// <summary>
			/// 名称
			/// </summary>
			[SerializeField] public string entityName;
			
			/// <summary>
			/// Component数据类型集合
			/// </summary>
			[SerializeField] public string[] componentDataTypeNameList;
		}
	}
}