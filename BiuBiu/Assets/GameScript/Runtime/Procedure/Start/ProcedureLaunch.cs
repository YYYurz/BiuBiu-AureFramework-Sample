//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Net;
using AureFramework.Event;
using AureFramework.Network;
using AureFramework.Procedure;
using UnityEngine;

namespace DrunkFish
{
	public class ProcedureLaunch : ProcedureBase
	{
		private INetworkChannel networkChannel;
		private bool isCanChange;

		private string[] messageContentArray = new[]
		{
			"你好呀，",
			"我叫余志睿。",
			"我来自广东韶关，",
			"是一个靓仔",
			"无敌的帅",
			"帅到底裤穿隆。",
			"没办法",
			"作为一个风度翩翩的猪肉佬",
			"如此的魅力",
			"也是没有办法的",
			"总而言之",
			"言而总之",
			"你好呀，",
			"我叫余志睿。",
			"我来自广东韶关，",
			"是一个靓仔",
			"无敌的帅",
			"帅到底裤穿隆。",
			"没办法",
			"作为一个风度翩翩的猪肉佬",
			"如此的魅力",
			"也是没有办法的",
			"总而言之",
			"言而总之",
		};
		
		public override void OnEnter(params object[] args)
		{
			GameMain.Event.Subscribe<NetworkConnectedEventArgs>(OnNetworkConnected);

			// networkChannel = GameMain.Network.CreateNetworkChannel("DefaultNetworkChannel", new TestNetworkHelper());
			// networkChannel.Connect(IPAddress.Parse("192.168.3.90"), 23321);
			
			Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
		}

		private float timer = 0f;
		private int index = 0;
		public override void OnUpdate(float elapseTime, float realElapseTime)
		{
			base.OnUpdate(elapseTime, realElapseTime);

			// timer += elapseTime;
			// if (timer >= 1f)
			// {
			// 	for (var i = 0; i < 3; i++)
			// 	{
			// 		networkChannel.Send(new TestPacket
			// 		{
			// 			message = messageContentArray[index]
			// 		});
			//
			// 		index = index + 1 >= messageContentArray.Length ? 0 : index + 1;
			// 	}
			//
			// 	timer = 0f;
			// }

			// if (!isCanChange)
			// {
			// 	return;
			// }
			//
			ChangeState<ProcedureCheckVersion>();
		}

		public override void OnExit()
		{
			base.OnExit();
			
			GameMain.Event.Unsubscribe<NetworkConnectedEventArgs>(OnNetworkConnected);
		}
		
		private void OnNetworkConnected(object sender, AureEventArgs e)
		{
			isCanChange = true;
		}
	}
}