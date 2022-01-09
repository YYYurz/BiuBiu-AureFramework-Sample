using AureFramework;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 框架模块集合
	/// </summary>
	public partial class GameMain : MonoBehaviour
	{
		public static IDataTableModule UIDataTable
		{
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

		private static void InitCustomModules()
		{
			UIDataTable = Aure.GetModule<IDataTableModule>();
			Lua = Aure.GetModule<ILuaModule>();
		}
	}
}