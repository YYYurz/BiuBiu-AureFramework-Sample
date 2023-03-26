using AureFramework.Procedure;
using TheLoner;

namespace DrunkFish
{
	public class ProcedureBattle : ProcedureBase
	{
		// private IWorld logicWorld;
		
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
			
		}

		private void InitSystem()
		{
			
		}

		private void InitUtility()
		{
			
		}

		private void DestroyLogicWorld()
		{
			
		}
	}
}