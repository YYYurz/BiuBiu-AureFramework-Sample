using AureFramework.Procedure;
using TheLoner;

namespace BiuBiu
{
	public class ProcedureBattle : ProcedureBase
	{
		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);
			
			GameMain.LogicWorld.CreateWorld(1, new BattleInitData());
			GameMain.LogicWorld.CreateWorld(1, new BattleInitData());
		}

		public override void OnUpdate(float elapseTime, float realElapseTime)
		{
			base.OnUpdate(elapseTime, realElapseTime);
		}

		public override void OnExit()
		{
			base.OnExit();
			
			// GameMain.GamePlay.QuitCurrentGame();
		}
	}
}