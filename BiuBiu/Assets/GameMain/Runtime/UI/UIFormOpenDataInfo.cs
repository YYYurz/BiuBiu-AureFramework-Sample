using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BB
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
            var uiFormOpenInfo = ReferencePool.Acquire<UIFormOpenDataInfo>();
            uiFormOpenInfo.FormID = id;
            uiFormOpenInfo.LuaFile = strLuaFile;
            uiFormOpenInfo.UserData = userData;
            
            return uiFormOpenInfo;
        }

        public virtual void Clear()
        {
            FormID = Constant.UIFormID.Undefined;
            LuaFile = string.Empty;
            UserData = null;
        }
    }

}
