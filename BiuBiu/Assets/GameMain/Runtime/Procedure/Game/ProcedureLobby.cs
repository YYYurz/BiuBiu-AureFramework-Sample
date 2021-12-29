//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using UnityEngine;

namespace BiuBiu {
	// ReSharper disable once UnusedType.Global
	public class ProcedureLobby : ProcedureBase {
		private byte[] bytes;
		private float timeRecorder = 0f;
		
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);

			var a = GameMain.TableData.GetDataTableReader<DTEntityTableReader>().GetInfo(10001).AssetName;
			Debug.Log(a);
		}

		public override void OnUpdate() {
			base.OnUpdate();

		}
	}
}