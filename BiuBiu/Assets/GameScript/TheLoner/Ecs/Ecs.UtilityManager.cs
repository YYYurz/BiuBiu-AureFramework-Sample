//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace TheLoner
{
	public sealed partial class Ecs
	{
		private class UtilityManager : IUtilityManager
		{
			private readonly Dictionary<Type, AbstractUtility> utilityDic = new Dictionary<Type, AbstractUtility>();
			private readonly List<AbstractUtility> utilityList = new List<AbstractUtility>();
			private readonly World world;

			public UtilityManager(World world)
			{
				this.world = world;
			}
			
			public void AwakeAllUtility()
			{
				foreach (var utility in utilityList)
				{
					utility.OnAwake();
				}
			}
			
			public void AddUtility<T>() where T : AbstractUtility
			{
				var type = typeof(T);
				if (utilityDic.ContainsKey(type))
				{
					Logger.LogError("Utility is already exist.");
					return;
				}

				var utility = (T) Activator.CreateInstance(type, world);
				utilityDic.Add(type, utility);
				utilityList.Add(utility);
			}

			public T GetUtility<T>() where T : AbstractUtility
			{
				var type = typeof(T);
				return (T) utilityDic[type];
			}

			public void RemoveAllUtility()
			{
				utilityDic.Clear();
				utilityList.Clear();
			}
		}
	}
}