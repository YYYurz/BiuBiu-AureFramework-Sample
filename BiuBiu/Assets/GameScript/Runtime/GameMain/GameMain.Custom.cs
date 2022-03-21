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
		
		public static IEntityModule Entity
		{
			get;
			private set;
		}
		
		public static IEffectModule Effect
		{
			get;
			private set;
		}

		public static IGamePlayModule GamePlay
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
			Entity = Aure.GetModule<IEntityModule>();
			Effect = Aure.GetModule<IEffectModule>();
			GamePlay = Aure.GetModule<IGamePlayModule>();
			DataTable = Aure.GetModule<IDataTableModule>();
		}
	}
}