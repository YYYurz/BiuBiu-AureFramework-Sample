﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------


using AureFramework;
using AureFramework.Fsm;
using AureFramework.Resource;
using AureFramework.Event;
using AureFramework.ObjectPool;
using AureFramework.Procedure;
using AureFramework.ReferencePool;
using AureFramework.Scene;
using AureFramework.UI;

namespace BiuBiu
{
	public partial class GameMain
	{
		public static IFsmModule Fsm
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

		public static IEventModule Event
		{
			get;
			private set;
		}

		public static IUIModule UI
		{
			get;
			private set;
		}

		public static IReferencePoolModule ReferencePool
		{
			get;
			private set;
		}

		public static IObjectPoolModule ObjectPool{
			get;
			private set;
		}

		public static ISceneModule Scene
		{
			get;
			private set;
		}

		private static void InitBuiltinModules() {
			Fsm = Aure.GetModule<IFsmModule>();
			Resource = Aure.GetModule<IResourceModule>();
			Procedure = Aure.GetModule<IProcedureModule>();
			Event = Aure.GetModule<IEventModule>();
			UI = Aure.GetModule<IUIModule>();
			ReferencePool = Aure.GetModule<IReferencePoolModule>();
			ObjectPool = Aure.GetModule<IObjectPoolModule>();
			Scene = Aure.GetModule<ISceneModule>();
		}
	}
}