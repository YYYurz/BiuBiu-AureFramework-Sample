//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace TheLoner
{
	public class ActorMoveSystem : AbstractSystem
	{
		public ActorMoveSystem(IWorld world) : base(world)
		{
			
		}

		public override void OnAwake()
		{
			base.OnAwake();
			
			Logger.LogError("Actor Move System Awake!!!");
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			
			Logger.LogError("Actor Move System OnUpdate");
		}
	}
}