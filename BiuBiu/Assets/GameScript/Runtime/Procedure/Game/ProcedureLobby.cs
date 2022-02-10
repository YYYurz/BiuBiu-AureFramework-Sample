//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Animations;

namespace BiuBiu
{
	public class ProcedureLobby : ProcedureBase
	{
		
		
		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);
			// GameMain.UI.OpenUI(Constant.UIFormID.SoundWindow);

			var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			var archetype = entityManager.CreateArchetype(
				typeof(Translation),
				typeof(TestComponent),
				typeof(LocalToWorld),
				typeof(RenderMesh),
				typeof(RenderBounds),
				typeof(Animation)
			);

			// var archetype1 = entityManager.CreateArchetype(
				// );
			
			// entityManager.CreateEntity()

			var entity = entityManager.CreateEntity(archetype);
			// entityManager.SetComponentData(entity, new TestComponent{ level = 123});
			// entityManager.SetSharedComponentData(entity, new RenderMesh
			// {
			// 	mesh = GameMain.Resource.LoadAssetSync<Mesh>("Me"),
			// 	material = GameMain.Resource.LoadAssetSync<Material>("Mat")
			// });
		}
		
		public override void OnUpdate()
		{
			base.OnUpdate();

		
		}

	}
}