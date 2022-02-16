using AureFramework;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 框架模块集合
	/// </summary>
	public partial class GameMain : MonoBehaviour
	{
		public static IDataTableModule DataTable
		{
			get;
			private set;
		}

		public static ILuaModule Lua
		{
			get;
			private set;
		}

		public static IGamePlayModule GamePlay
		{
			get;
			private set;
		}

		public static IEntityModule Entity
		{
			get;
			private set;
		}

		private static void InitCustomModules()
		{
			DataTable = Aure.GetModule<IDataTableModule>();
			Lua = Aure.GetModule<ILuaModule>();
			GamePlay = Aure.GetModule<IGamePlayModule>();
			Entity = Aure.GetModule<IEntityModule>();
		}
	}
}