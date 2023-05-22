//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.IO;
using AureFramework.Network;
using Google.Protobuf;
using UnityEngine;

namespace DrunkFish
{
	public class ProtoNetworkHelper : INetworkHelper
	{
		private static readonly ProtoMessage TempProtoMessage = new();
		
		public int PacketHeaderLength
		{
			get
			{
				return 4;
			}
		}

		public void Shutdown()
		{
			
		}

		public bool SendHeartBeat()
		{
			return false;
		}

		public bool Serialize(Packet packet, Stream destination)
		{
			if (packet is not ProtoPacket protoPacket)
			{
				Debug.LogError("Packet is invalid.");
				return false;
			}
			
			TempProtoMessage.Cmd = (uint) protoPacket.cmd;
			TempProtoMessage.Body = ByteString.Empty;
			if (protoPacket.message != null)
			{
				TempProtoMessage.Body = ByteString.CopyFrom(protoPacket.message.ToByteArray());
			}
			
			var sendBytes = TempProtoMessage.ToByteArray();
			destination.Write(sendBytes);

			return true;
		}

		public IPacketHeader DeserializePacketHeader(Stream source)
		{
			var protoPacketHeader = new ProtoPacketHeader();
			var bytes = new byte[PacketHeaderLength];
			source.Read(bytes, 0, PacketHeaderLength);
			source.Position = PacketHeaderLength;
			protoPacketHeader.PacketLength = (int)BitConverter.ToUInt32(bytes, 0);

			return protoPacketHeader;
		}

		public Packet DeserializePacket(IPacketHeader packetHeader, Stream source)
		{
			var packetLength = packetHeader.PacketLength;
			var bytes = new byte[packetLength];
			source.Read(bytes, 0, packetLength);
			
			var protoMessage = ProtoMessage.Parser.ParseFrom(bytes, 0, packetLength);
			if (protoMessage == null)
			{
				Debug.LogError("DeserializePacket failed.");
				return null;
			}

			var result = new ProtoPacket
			{
				cmd = (ProtoMessageCmd) protoMessage.Cmd,
				message = ProtoMap.GetProtoMsg(protoMessage.Cmd, protoMessage.Body.ToByteArray(), 0, protoMessage.Body.Length)
			};
			
			return result;
		}
	}
}