//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Network;
using Google.Protobuf;

namespace DrunkFish
{
	public class ProtoPacket : Packet
	{
		public ProtoMessageCmd cmd;
		public IMessage message;
	}
}