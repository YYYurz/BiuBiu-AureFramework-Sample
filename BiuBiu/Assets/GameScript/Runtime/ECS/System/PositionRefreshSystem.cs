//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace BiuBiu
{
	/// <summary>
	/// 刷新单位位置
	/// </summary>
	public class PositionRefreshSystem : SystemBase
	{
		private float speed = 1f;

		protected override void OnUpdate()
		{
			Entities.ForEach((PositionComponent positionComponent, ref Translation translation) =>
			{
				var newPos = Vector3.Lerp(positionComponent.CurPosition, positionComponent.NextPosition, 0.025f);
				translation.Value = newPos;
				positionComponent.CurPosition = newPos;
			}).Schedule();
		}
	}
}