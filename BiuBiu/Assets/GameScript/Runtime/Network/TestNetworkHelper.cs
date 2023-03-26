//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AureFramework.Network;
using UnityEngine;

namespace DrunkFish
{
	public class TestNetworkHelper : INetworkHelper
	{
		private readonly MemoryStream memoryStream = new MemoryStream(1024 * 8);

		public int PacketHeaderLength => 4;

		public void Shutdown()
		{
		}

		public bool SendHeartBeat()
		{
			return false;
		}

		public bool Serialize(Packet packet, Stream destination)
		{
			if (!(packet is TestPacket testPacket))
			{
				Debug.LogError("Packet is invalid.");
				return false;
			}
			
			memoryStream.SetLength(memoryStream.Capacity); //此行防止 Array.Copy 的数据无法写入
			memoryStream.Position = 0L;
			
			var serializer = new BinaryFormatter();
			serializer.Serialize(memoryStream, testPacket);

			var messageByteArray = memoryStream.ToArray();
			var messageLength = (int)memoryStream.Position;

			memoryStream.Position = 0L;
			//写入4字节包长度
			memoryStream.Write(BitConverter.GetBytes(messageLength), 0, 4);
			//写入实际消息包
			memoryStream.Write(messageByteArray, 0, messageLength);

			memoryStream.SetLength(messageLength + 4);
			memoryStream.WriteTo(destination);

			// destination.Position = 0L;
			// var header = DeserializePacketHeader(destination);
			// var pack = DeserializePacket(header, destination) as TestPacket;

			return true;
		}

		public IPacketHeader DeserializePacketHeader(Stream source)
		{
			var header = new TestPacketHeader();
			var bytes = new byte[PacketHeaderLength];
			source.Read(bytes, 0, PacketHeaderLength);
			source.Position = 4;
			header.PacketLength = (int)BitConverter.ToUInt32(bytes, 0);

			return header;
		}

		public Packet DeserializePacket(IPacketHeader packetHeader, Stream source)
		{
			var bf = new BinaryFormatter();
			var testPacket = bf.Deserialize(source) as TestPacket;
			source.Position = 0;

			return testPacket;
		}
	}
}