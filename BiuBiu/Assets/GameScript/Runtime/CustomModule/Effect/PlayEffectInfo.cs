//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework;
using AureFramework.ReferencePool;
using UnityEngine;

namespace BiuBiu
{
	public sealed partial class EffectModule : AureFrameworkModule, IEffectModule
	{
		/// <summary>
		/// 内部播放特效信息类
		/// </summary>
		public class PlayEffectInfo : IReference
		{
			public Transform ParentTransform
			{
				get;
				private set;
			}

			public Vector3 Position
			{
				get;
				private set;
			}

			public Quaternion Rotation
			{
				get;
				private set;
			}
			
			public static PlayEffectInfo Create(Transform parentTransform, Vector3 position, Quaternion rotation)
			{
				var playSoundInfo = GameMain.ReferencePool.Acquire<PlayEffectInfo>();
				playSoundInfo.ParentTransform = parentTransform;
				playSoundInfo.Position = position;
				playSoundInfo.Rotation = rotation;

				return playSoundInfo;
			}
			
			public void Clear()
			{
				ParentTransform = null;
				Position = default;
				Rotation = default;
			}
		}
	}
}