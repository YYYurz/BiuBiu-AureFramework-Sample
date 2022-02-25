//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// ECS实体模块接口
	/// </summary>
	public interface IEntityModule
	{
		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="archetype"> 原型类型 </param>
		/// <returns></returns>
		Entity CreateEntity(EntityArchetype archetype);

		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="archetype"> 原型类型 </param>
		/// <param name="num"> 数量 </param>
		/// <returns></returns>
		NativeArray<Entity> CreateEntity(EntityArchetype archetype, int num);

		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="entityPresetName"> 实体预设名称 </param>
		/// <returns></returns>
		Entity CreateEntity(string entityPresetName);

		/// <summary>
		/// 创建实体
		/// </summary>
		/// <param name="entityPresetName"> 实体预设名称 </param>
		/// <param name="num"> 数量 </param>
		/// <returns></returns>
		NativeArray<Entity> CreateEntity(string entityPresetName, int num);

		/// <summary>
		/// 销毁所有实体
		/// </summary>
		void DestroyAllCacheEntity();
	}
}