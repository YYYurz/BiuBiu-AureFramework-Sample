//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using BiuBiu;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AureFramework
{
	public class TestSystem : SystemBase
	{
		private readonly NativeArray<int> a = new NativeArray<int>();
		
		[ReadOnly]
		private NativeArray<int> b = new NativeArray<int>();
		
		// protected override void OnUpdate()
		// {
		// 	// var entityQuery = GetEntityQuery(ComponentType.ReadOnly<TestComponent>());
		// 	// entityQuery.
		// 	Entities.ForEach((ref TestComponent test) =>
		// 	{
		// 		test.level += Time.DeltaTime;
		// 	});
		// }
		protected override void OnUpdate()
		{
			Entities.ForEach((ref TestComponent test) =>
			{
				test = new TestComponent();
			}).ScheduleParallel();
			
		}
	}
}