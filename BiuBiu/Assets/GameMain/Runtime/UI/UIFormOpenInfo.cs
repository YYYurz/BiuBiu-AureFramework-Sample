using AureFramework;
using AureFramework.ReferencePool;

namespace BiuBiu
{
	public class UIFormOpenInfo : IReference
	{
		/// <summary>
		/// 界面ID
		/// </summary>
		public int UIFormId
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

		public static UIFormOpenInfo Create(int id, string strLuaFile, object userData)
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