using AureFramework;
using UnityEngine;

namespace BiuBiu {
	/// <summary>
	/// 游戏入口。
	/// </summary>
	public partial class GameMain : MonoBehaviour {
		public static ITableDataModule TableData { 
			get; 
			private set;
		}

		public static ILuaModule Lua
		{
			get;
			private set;
		}

		// public static PreloadComponent AssetPreload
		// {
		//     get;
		//     private set;
		// }

		// public static InputComponent InputComponent
		// {
		//     get;
		//     private set;
		// }

		private static void InitCustomModules() {
			TableData = Aure.GetModule<ITableDataModule>();
			Lua = Aure.GetModule<ILuaModule>();
		}
	}
}