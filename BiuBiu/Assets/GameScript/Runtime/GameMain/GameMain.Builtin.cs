//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------


using AureFramework;
using AureFramework.UI;
using AureFramework.Fsm;
using AureFramework.Event;
using AureFramework.Scene;
using AureFramework.Sound;
using AureFramework.Network;
using AureFramework.Resource;
using AureFramework.Procedure;
using AureFramework.ObjectPool;
using AureFramework.ReferencePool;

namespace DrunkFish
{
	/// <summary>
	/// 框架模块集合
	/// </summary>
	public partial class GameMain
	{
		public static IUIModule UI
		{
			get;
			private set;
		}
		
		public static IFsmModule Fsm
		{
			get;
			private set;
		}

		public static IEventModule Event
		{
			get;
			private set;
		}

		public static ISceneModule Scene
		{
			get;
			private set;
		}

		public static ISoundModule Sound
		{
			get;
			private set;
		}
		
		public static INetworkModule Network
		{
			get;
			private set;
		}
		
		public static IResourceModule Resource
		{
			get;
			private set;
		}

		public static IProcedureModule Procedure
		{
			get;
			private set;
		}
		
		public static IObjectPoolModule ObjectPool
		{
			get;
			private set;
		}

		public static IReferencePoolModule ReferencePool
		{
			get;
			private set;
		}

		private static void InitBuiltinModules()
		{
			UI = Aure.GetModule<IUIModule>();
			Fsm = Aure.GetModule<IFsmModule>();
			Event = Aure.GetModule<IEventModule>();
			Scene = Aure.GetModule<ISceneModule>();
			Sound = Aure.GetModule<ISoundModule>();
			Network = Aure.GetModule<INetworkModule>();
			Resource = Aure.GetModule<IResourceModule>();
			Procedure = Aure.GetModule<IProcedureModule>();
			ObjectPool = Aure.GetModule<IObjectPoolModule>();
			ReferencePool = Aure.GetModule<IReferencePoolModule>();
		}
	}
}