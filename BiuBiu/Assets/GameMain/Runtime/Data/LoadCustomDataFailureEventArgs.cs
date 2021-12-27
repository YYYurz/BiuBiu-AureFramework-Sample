//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using AureFramework.Event;

namespace BiuBiu
{
    /// <summary>
    /// 加载配置失败事件。
    /// </summary>
    public sealed class LoadCustomDataFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载配置失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadCustomDataFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载配置失败事件的新实例。
        /// </summary>
        public LoadCustomDataFailureEventArgs()
        {
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取加载配置失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string DataName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载配置失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载配置失败事件。</returns>
        public static LoadCustomDataFailureEventArgs Create(string strName, string strError, object userData)
        {
            LoadCustomDataFailureEventArgs loadConfigFailureEventArgs = ReferencePool.Acquire<LoadCustomDataFailureEventArgs>();
            loadConfigFailureEventArgs.DataName = strName;
            loadConfigFailureEventArgs.ErrorMessage = strError;
            loadConfigFailureEventArgs.UserData = userData;

            return loadConfigFailureEventArgs;
        }

        /// <summary>
        /// 清理加载配置失败事件。
        /// </summary>
        public override void Clear()
        {
            ErrorMessage = null;
            UserData = null;
        }
    }
}
