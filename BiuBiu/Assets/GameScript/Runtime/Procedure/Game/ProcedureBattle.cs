using AureFramework.Procedure;

namespace BiuBiu
{
	public class ProcedureBattle : ProcedureBase
	{
		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);
			
			// GameMain.GamePlay.CreateGame(1);
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