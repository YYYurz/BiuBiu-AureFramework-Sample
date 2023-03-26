//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework;
using AureFramework.ReferencePool;

namespace DrunkFish
{
	public class UIFormOpenInfo : IReference
	{
		/// <summary>
		/// 界面ID
		/// </summary>
		public uint UIFormId
		{
			get;
			set;
		}

		public string LuaFile
		{
			get;
			set;
		}

		public object UserData
		{
			get;
			set;
		}

		public static UIFormOpenInfo Create(uint id, string strLuaFile, object userData)
		{
			var uiFormOpenInfo = Aure.GetModule<IReferencePoolModule>().Acquire<UIFormOpenInfo>();
			uiFormOpenInfo.UIFormId = id;
			uiFormOpenInfo.LuaFile = strLuaFile;
			uiFormOpenInfo.UserData = userData;

			return uiFormOpenInfo;
		}

		public void Clear()
		{
			UIFormId = 0;
			LuaFile = string.Empty;
			UserData = null;
		}
	}
}