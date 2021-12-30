//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.IO;
using AureFramework.Procedure;
using UnityEngine;

namespace BiuBiu
{
	// ReSharper disable once UnusedType.Global
	public class ProcedureLobby : ProcedureBase
	{
		private byte[] bytes;
		private float timeRecorder = 0f;

		public override void OnEnter(params object[] args)
		{
			base.OnEnter(args);

			GameMain.Lua.DoString("require('GameMain')");
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}
	}
}