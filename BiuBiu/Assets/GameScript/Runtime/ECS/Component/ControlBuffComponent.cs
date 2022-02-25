﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;

namespace BiuBiu
{
	[GenerateAuthoringComponent]
	public struct ControlBuffComponent : IComponentData
	{
		/// <summary>
		/// 击退剩余时间
		/// </summary>
		public float BackTime;
	}
}