using AureFramework.Procedure;

namespace BiuBiu
{
	public class ProcedureBattle : ProcedureBase
	{
		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);
			
			GameMain.GamePlay.CreateGame(1);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		public override void OnExit()
		{
			base.OnExit();
			
			GameMain.GamePlay.QuitCurrentGame();
		}
	}
}