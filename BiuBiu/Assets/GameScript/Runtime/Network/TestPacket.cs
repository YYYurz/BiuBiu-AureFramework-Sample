//------------------------------------------------------------
// Drunk Fish Demo
// Developed By YYYurz.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.Network;

namespace DrunkFish
{
	[Serializable]
	public class TestPacket : Packet
	{
		public string message;
	}
}