//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using Unity.Entities;
using Unity.Transforms;

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
				typeof(LocalToWorld)
			);

			entityManager.CreateEntity(archetype);

			
		}
		
		public override void OnUpdate()
		{
			base.OnUpdate();

		
		}

	}
}