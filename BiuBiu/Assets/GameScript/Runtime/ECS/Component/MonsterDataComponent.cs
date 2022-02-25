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
	public struct MonsterDataComponent : IComponentData
	{
		public float Health;
		public float AttackDamage;
		public float MoveSpeed;
	}
}