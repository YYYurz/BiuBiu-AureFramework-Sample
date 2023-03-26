//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Resource;
using DrunkFish;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AureFramework
{
	/// <summary>
	/// SPrite加载类，加载图集内图片时使用
	/// </summary>
	public sealed class UISpriteLoader : MonoBehaviour
	{
		private static readonly LoadAssetCallbacks LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetBegin, OnLoadAssetSuccess, null, OnLoadAssetFailed);
		private static readonly Dictionary<string, SpriteAtlas> CacheAtlasDic = new Dictionary<string, SpriteAtlas>();
		private static readonly Dictionary<string, int> LoadingAtlasDic = new Dictionary<string, int>();
		private static readonly Dictionary<string, int> AtlasReferenceDic = new Dictionary<string, int>();
		private static readonly List<string> WaitReleaseAtlasList = new List<string>();
		private string curAtlasName;
		private string curSpriteName;
		private bool isWaiting;
		
		[SerializeField] private Image image;

		private void Update()
		{
			if (isWaiting && CacheAtlasDic.ContainsKey(curAtlasName))
			{
				image.sprite = CacheAtlasDic[curAtlasName].GetSprite(curSpriteName);
				RecordAtlasReference(curAtlasName, true);
				isWaiting = false;
			}
		}

		private void OnDestroy()
		{
			if (!isWaiting && !string.IsNullOrEmpty(curAtlasName))
			{
				RecordAtlasReference(curAtlasName, false);
			}
		}

		/// <summary>
		/// 加载Sprite
		/// </summary>
		/// <param name="atlasName"> 图集资源名称 </param>
		/// <param name="spriteName"> Sprite名称 </param>
		public void Load(string atlasName, string spriteName)
		{
			if (!isWaiting && !string.IsNullOrEmpty(curAtlasName))
			{
				RecordAtlasReference(curAtlasName, false);
			}
			
			if (CacheAtlasDic.ContainsKey(atlasName))
			{
				RecordAtlasReference(atlasName, true);
				image.sprite = CacheAtlasDic[atlasName].GetSprite(spriteName);
				isWaiting = false;
			}
			else
			{
				if (!LoadingAtlasDic.ContainsKey(atlasName))
				{
					GameMain.Resource.LoadAssetAsync<SpriteAtlas>(atlasName, LoadAssetCallbacks, null);
				}
				isWaiting = true;
			}

			curAtlasName = atlasName;
			curSpriteName = spriteName;
		}

		/// <summary>
		/// 图集引用计数
		/// </summary>
		/// <param name="atlasName"></param>
		/// <param name="isAdd"></param>
		private static void RecordAtlasReference(string atlasName, bool isAdd)
		{
			if (!AtlasReferenceDic.ContainsKey(atlasName) && isAdd)
			{
				AtlasReferenceDic.Add(atlasName, 1);
			}
			else if (AtlasReferenceDic.ContainsKey(atlasName))
			{
				if (isAdd)
				{
					++AtlasReferenceDic[atlasName];
				}
				else
				{
					if (--AtlasReferenceDic[atlasName] <= 0)
					{
						AtlasReferenceDic.Remove(atlasName);
						WaitReleaseAtlasList.Add(atlasName);
						ReleaseUnusedAtlas();
					}
				}
			}
		}

		/// <summary>
		/// 释放无引用的图集
		/// </summary>
		private static void ReleaseUnusedAtlas()
		{
			foreach (var atlasName in WaitReleaseAtlasList)
			{
				GameMain.Resource.ReleaseAsset(CacheAtlasDic[atlasName]);
				CacheAtlasDic.Remove(atlasName);
			}
		}

		private static void OnLoadAssetBegin(string assetName, int taskId)
		{
			if (!LoadingAtlasDic.ContainsValue(taskId))
			{
				LoadingAtlasDic.Add(assetName, taskId);				
			}
		}

		private static void OnLoadAssetSuccess(string assetName, int taskId, Object asset, object userData)
		{
			LoadingAtlasDic.Remove(assetName);
			CacheAtlasDic.Add(assetName, (SpriteAtlas) asset);
		}

		private static void OnLoadAssetFailed(string assetName, int taskId, string errorMessage, object userData)
		{
			LoadingAtlasDic.Remove(assetName);
			Debug.LogError($"UISpriteLoader : Load atlas failed, atlas name :{assetName}, error message :{errorMessage}");
		}
	}
}