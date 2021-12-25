using GameFramework;
using GameFramework.Event;

namespace BB
{
    /// <summary>
    /// 加载配置成功事件。
    /// </summary>
    public sealed class LoadCustomDataSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载配置成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadCustomDataSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载配置成功事件编号的新实例。
        /// </summary>
        public LoadCustomDataSuccessEventArgs()
        {
            Duration = 0f;
            UserData = null;
        }


        /// <summary>
        /// 获取加载配置成功事件编号。
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
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
        /// 创建加载配置成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载配置成功事件。</returns>
        public static LoadCustomDataSuccessEventArgs Create(string strName, float fDuration, object userData)
        {
            LoadCustomDataSuccessEventArgs loadConfigSuccessEventArgs = ReferencePool.Acquire<LoadCustomDataSuccessEventArgs>();
            loadConfigSuccessEventArgs.DataName = strName;
            loadConfigSuccessEventArgs.Duration = fDuration;
            loadConfigSuccessEventArgs.UserData = userData;

            return loadConfigSuccessEventArgs;
        }

        /// <summary>
        /// 清理加载配置成功事件。
        /// </summary>
        public override void Clear()
        {
            DataName = "";
            Duration = 0f;
            UserData = null;
        }
    }
}
