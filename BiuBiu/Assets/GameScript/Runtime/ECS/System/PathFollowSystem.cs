//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BiuBiu
{
	/// <summary>
	/// 刷新单位位置
	/// </summary>
	public class PathFollowSystem : ComponentSystem
	{
		protected override void OnUpdate()
		{
			if (!GameMain.GamePlay.IsStart || GameMain.GamePlay.IsPause)
			{
				return;
			}

			var mapPointArray = GameMain.GamePlay.CurMapConfig.PointArray;
			Entities
				.WithNone<ControlBuffComponent>()
				.ForEach((DynamicBuffer<PathPositionBuffer> pathPositionBuffer, ref Translation translation, ref PathFollowComponent pathFollowIndexComponent, ref MonsterDataComponent monsterDataComponent) =>
			{
				if (pathFollowIndexComponent.PathIndex >= 0)
				{
					var mapIndex = pathPositionBuffer[pathFollowIndexComponent.PathIndex].Index;
					var worldPosition = mapPointArray[mapIndex].worldPos;
					var direction = math.normalize(worldPosition - translation.Value);
					var newPos = direction * monsterDataComponent.MoveSpeed * Time.DeltaTime;
					translation.Value += PathfindingUtils.GetNearestEffectivePosInMap(GameMain.GamePlay.CurMapConfig, newPos);

					if (math.distance(translation.Value, worldPosition) < 0.1f)
					{
						pathFollowIndexComponent.PathIndex--;
					}
				}
			});
		}
	}
}