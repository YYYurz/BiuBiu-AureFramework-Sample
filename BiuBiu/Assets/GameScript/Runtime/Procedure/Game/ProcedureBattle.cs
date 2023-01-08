using AureFramework.Procedure;
using TheLoner;
using UnityEngine;

namespace BiuBiu
{
	public class ProcedureBattle : ProcedureBase
	{
		private IWorld logicWorld;
		
		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			CreateLogicWorld();
		}

		public override void OnExit()
		{
			base.OnExit();

			DestroyLogicWorld();
		}

		private void CreateLogicWorld()
		{
			logicWorld = GameMain.LogicWorld.CreateWorld(1, new BattleInitData());
			InitSystem();
			InitUtility();
			logicWorld.Start();
		}

		private void InitSystem()
		{
			logicWorld.SystemManager.AddSystem<ActorMoveSystem>();
		}

		private void InitUtility()
		{
			
		}

		private void DestroyLogicWorld()
		{
			GameMain.LogicWorld.DestroyWorld(logicWorld);
		}
	}
}