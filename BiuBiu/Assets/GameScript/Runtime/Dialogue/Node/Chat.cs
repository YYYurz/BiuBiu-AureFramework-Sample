﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace BiuBiu
{
	public class Chat : DialogueNodeBase
	{
		[Input(backingValue = ShowBackingValue.Never)] public DialogueNodeBase input;
		[Output(backingValue = ShowBackingValue.Never)] public DialogueNodeBase output;
		public string content;
	}
}