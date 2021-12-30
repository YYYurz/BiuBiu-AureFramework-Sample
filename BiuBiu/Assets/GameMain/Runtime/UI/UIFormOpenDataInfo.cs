using AureFramework;
using AureFramework.ReferencePool;

namespace BiuBiu
{
	public class UIFormOpenDataInfo : IReference
	{
		/// <summary>
		/// 界面ID
		/// </summary>
		public int UIFormId
		{
			get;
			set;
		} = Constant.UIFormID.Undefined;

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

		public static UIFormOpenDataInfo Create(int id, string strLuaFile, object userData)
		{
			var uiFormOpenInfo = Aure.GetModule<IReferencePoolModule>().Acquire<UIFormOpenDataInfo>();
			uiFormOpenInfo.UIFormId = id;
			uiFormOpenInfo.LuaFile = strLuaFile;
			uiFormOpenInfo.UserData = userData;

			return uiFormOpenInfo;
		}

		public void Clear()
		{
			UIFormId = Constant.UIFormID.Undefined;
			LuaFile = string.Empty;
			UserData = null;
		}
	}
}