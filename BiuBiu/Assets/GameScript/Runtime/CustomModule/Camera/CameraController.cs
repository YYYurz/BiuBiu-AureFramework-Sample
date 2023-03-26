//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections;
using AureFramework.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DrunkFish
{
	/// <summary>
	/// 相机控制
	/// </summary>
	public sealed class CameraController : MonoBehaviour
	{
		private static readonly Vector3 Offset = new Vector3(0f, 10f, -6f);
		private Transform followTargetTrans;
		private Transform cameraTrans;

		private void Awake()
		{
			cameraTrans = GetComponentInChildren<Camera>().transform;
			
			// GameMain.Event.Subscribe<HitMonsterEventArgs>(OnHitMonster);
		}

		private void LateUpdate()
		{
			if (followTargetTrans == null)
			{
				return;
			}

			var moveToPos = followTargetTrans.position + Offset;
			transform.position = Vector3.Lerp(transform.position, moveToPos, 10f * Time.deltaTime);
		}

		private void OnDestroy()
		{
			// GameMain.Event.Unsubscribe<HitMonsterEventArgs>(OnHitMonster);
		}

		/// <summary>
		/// 相机跟随
		/// </summary>
		/// <param name="target"></param>
		public void SetFollowTarget(Transform target)
		{
			if (target == null)
			{
				return;
			}
			
			followTargetTrans = target;
		}
		
		/// <summary>
		/// 震屏
		/// </summary>
		/// <param name="duration"> 时长 </param>
		/// <param name="magnitude"> 幅度 </param>
		private IEnumerator Shake(float duration, float magnitude)
		{
			var originalPos = cameraTrans.localPosition;
			var elapsedTime = 0f;

			while (elapsedTime < duration)
			{
				var x = Random.Range(-1f, 1f) * magnitude;
				var y = Random.Range(-1f, 1f) * magnitude;
				
				cameraTrans.localPosition = new Vector3(x, y, originalPos.z);
				elapsedTime += Time.deltaTime;

				yield return null;
			}

			cameraTrans.localPosition = originalPos;
		}
		
		private void OnHitMonster(object sender, AureEventArgs e)
		{
			// var args = (HitMonsterEventArgs) e;
			// StartCoroutine(args.IsStrong ? Shake(0.15f, 0.5f) : Shake(0.15f, 0.2f));
		}
	}
}