//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using AureFramework;
using AureFramework.Network;
using Google.Protobuf;
using UnityEngine;

namespace DrunkFish
{
	public sealed class LogicWorldNetworkModule : AureFrameworkModule, ILogicWorldNetworkModule
	{
		public delegate void MessageHandler(ProtoMessageCmd cmd, IMessage message);

		private readonly Dictionary<ProtoMessageCmd, MessageHandler> handlerDic = new();
		private readonly Queue<ProtoPacket> sendQueue = new();
		private INetworkChannel logicWorldChannel;
		private bool isConnected;

		public bool IsConnected
		{
			get
			{
				return isConnected;
			}
		}
		
		public override void Init()
		{
			GameMain.Network.NetworkConnectedEventHandler += OnNetworkChannelConnected;
			GameMain.Network.NetworkCloseEventHandler += OnNetworkChannelClosed;
			GameMain.Network.NetworkErrorEventHandler += OnNetworkChannelError;
			GameMain.Network.NetworkReceiveEventHandler += OnNetworkChannelReceive;
		}

		public override void Tick(float elapseTime, float realElapseTime)
		{
			
		}

		public override void Clear()
		{
			GameMain.Network.NetworkConnectedEventHandler -= OnNetworkChannelConnected;
			GameMain.Network.NetworkCloseEventHandler -= OnNetworkChannelClosed;
			GameMain.Network.NetworkErrorEventHandler -= OnNetworkChannelError;
			GameMain.Network.NetworkReceiveEventHandler -= OnNetworkChannelReceive;
		}

		public void Connect(string ip, int port)
		{
			if (logicWorldChannel != null)
			{
				return;
			}

			logicWorldChannel = GameMain.Network.CreateNetworkChannel("LogicWorldChannel", new ProtoNetworkHelper());
			logicWorldChannel.Connect(IPAddress.Parse(ip), port);
		}

		public void CloseConnect()
		{
			if (logicWorldChannel == null)
			{
				return;
			}
			
			logicWorldChannel.CloseConnect();
			logicWorldChannel = null;
		}

		public void Send(ProtoMessageCmd cmd, IMessage message)
		{
			if (!isConnected)
			{
				Debug.LogError("LogicWorldChannel not connected.");
				return;
			}
			
			sendQueue.Enqueue(new ProtoPacket
			{
				cmd = cmd,
				message = message,
			});
		}

		public void Subscribe(ProtoMessageCmd cmd, MessageHandler messageHandler)
		{
			if (handlerDic.ContainsKey(cmd))
			{
				handlerDic[cmd] += messageHandler;
			}
			else
			{
				handlerDic[cmd] = messageHandler;
			}
		}

		public void Unsubscribe(ProtoMessageCmd cmd, MessageHandler messageHandler)
		{
			if (handlerDic.ContainsKey(cmd))
			{
				handlerDic[cmd] -= messageHandler;
			}
			
			if(handlerDic[cmd] == null)
			{
				handlerDic.Remove(cmd);
			}
		}
		
		private void OnNetworkChannelConnected(object sender, NetworkConnectedEventArgs networkConnectedEventArgs)
		{
			if (networkConnectedEventArgs.NetworkChannel != logicWorldChannel)
			{
				return;
			}

			isConnected = true;
		}

		private void OnNetworkChannelClosed(object sender, NetworkCloseEventArgs networkCloseEventArgs)
		{
			if (networkCloseEventArgs.NetworkChannel != logicWorldChannel)
			{
				return;
			}

			isConnected = false;
		}

		private void OnNetworkChannelError(object sender, NetworkErrorEventArgs networkErrorEventArgs)
		{
			if (networkErrorEventArgs.NetworkChannel != logicWorldChannel)
			{
				return;
			}

			Debug.LogError($"[Channel:{networkErrorEventArgs.NetworkChannel.Name}]{networkErrorEventArgs.ErrorMessage}");
		}

		private void OnNetworkChannelReceive(object sender, NetworkReceiveEventArgs networkReceiveEventArgs)
		{
			if (networkReceiveEventArgs.NetworkChannel != logicWorldChannel)
			{
				return;
			}

			var protoPacket = (ProtoPacket) networkReceiveEventArgs.Packet;
			var cmd = protoPacket.cmd;
			var message = protoPacket.message;
			if (handlerDic.ContainsKey(cmd))
			{
				handlerDic[cmd].Invoke(cmd, message);
			}
		}
	}
}