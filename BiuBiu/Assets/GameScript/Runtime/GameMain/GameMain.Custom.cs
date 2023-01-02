using AureFramework;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 框架模块集合
	/// </summary>
	public partial class GameMain : MonoBehaviour
	{
		public static ILuaModule Lua
		{
			get;
			private set;
		}
		
		public static IEffectModule Effect
		{
			get;
			private set;
		}

		public static ILogicWorldModule LogicWorld
		{
			get;
			private set;
		}

		public static IViewWorldModule ViewWorld
		{
			get;
			private set;
		}
		
		public static IDataTableModule DataTable
		{
			get;
			private set;
		}

		private static void InitCustomModules()
		{
			Lua = Aure.GetModule<ILuaModule>();
			Effect = Aure.GetModule<IEffectModule>();
			LogicWorld = Aure.GetModule<ILogicWorldModule>();
			ViewWorld = Aure.GetModule<IViewWorldModule>();
			DataTable = Aure.GetModule<IDataTableModule>();
		}
	}
}