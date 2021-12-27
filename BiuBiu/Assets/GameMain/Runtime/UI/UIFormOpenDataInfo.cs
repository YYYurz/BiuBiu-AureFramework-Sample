using AureFramework;
using AureFramework.ReferencePool;

namespace BiuBiu
{
    public class UIFormOpenDataInfo : IReference
    {
        /// <summary>
        /// 界面ID
        /// </summary>
        public int FormID { get; set; } = Constant.UIFormID.Undefined;

        public string LuaFile { get; set; } = string.Empty;

        public object UserData { get; set; } = null;


        public static UIFormOpenDataInfo Create(int id, string strLuaFile, object userData)
        {
            var uiFormOpenInfo = Aure.GetModule<IReferencePoolModule>().Acquire<UIFormOpenDataInfo>();
            uiFormOpenInfo.FormID = id;
            uiFormOpenInfo.LuaFile = strLuaFile;
            uiFormOpenInfo.UserData = userData;
            
            return uiFormOpenInfo;
        }

        public void Clear() {
            FormID = Constant.UIFormID.Undefined;
            LuaFile = string.Empty;
            UserData = null;
        }
    }

}
