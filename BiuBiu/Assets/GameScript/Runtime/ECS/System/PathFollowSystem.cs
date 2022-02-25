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

			var mapConfig = GameMain.GamePlay.CurMapConfig;
			Entities.ForEach((DynamicBuffer<PathPositionBuffer> pathPositionBuffer,ref ControlBuffComponent controlBuffComponent, ref Translation translation, ref PathFollowComponent pathFollowIndexComponent, ref MonsterDataComponent monsterDataComponent) =>
			{
				if (pathFollowIndexComponent.PathIndex >= 0 && controlBuffComponent.BackTime <= 0f)
				{
					var mapIndex = pathPositionBuffer[pathFollowIndexComponent.PathIndex].Index;
					var worldPosition = mapConfig.PointArray[mapIndex].worldPos;
					var direction = math.normalize(worldPosition - translation.Value);
					var newPos = direction * monsterDataComponent.MoveSpeed * Time.DeltaTime;
					newPos += translation.Value;
					var newPosIndex = PathfindingUtils.GetIndexByWorldPosition(newPos, mapConfig.CellSize, mapConfig.MapSize);
					if (mapConfig.PointArray[newPosIndex].index == 0)
					{
						newPos = mapConfig.PointArray[newPosIndex].worldPos;
					}
					translation.Value = newPos;

					if (math.distance(translation.Value, worldPosition) < 0.1f)
					{
						pathFollowIndexComponent.PathIndex--;
					}
				}
			});
		}
	}
}