//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 相机控制类
	/// </summary>
	public sealed partial class CameraController : MonoBehaviour
	{
		private static readonly Vector3 Offset = new Vector3(0f, 10f, -6f);
		private Transform followTarget;

		private void LateUpdate()
		{
			if (followTarget == null)
			{
				return;
			}

			var moveToPos = followTarget.position + Offset;
			transform.position = Vector3.Lerp(transform.position, moveToPos, 10f * Time.deltaTime);
		}

		public void SetFollowTarget(Transform target)
		{
			if (target == null)
			{
				return;
			}
			
			followTarget = target;
		}
	}
}