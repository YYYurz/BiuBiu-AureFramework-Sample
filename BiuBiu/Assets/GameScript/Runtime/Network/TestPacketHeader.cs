//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Network;

namespace DrunkFish
{
	public class TestPacketHeader : IPacketHeader
	{
		public int PacketLength
		{
			get;
			set;
		}
	}
}