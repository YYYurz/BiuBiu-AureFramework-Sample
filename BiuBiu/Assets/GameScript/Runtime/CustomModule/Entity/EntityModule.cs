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
using UnityEngine;

namespace BiuBiu
{
	[Serializable]
	public class a
	{
		[SerializeField] public List<string> list;
	}
	
	public sealed class EntityModule : AureFrameworkModule
	{
		
		[SerializeField] private a[] list;

		
		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public override int Priority => 1;
		
		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			
		}
		
		
	}
}