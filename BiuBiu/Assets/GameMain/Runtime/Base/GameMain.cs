// Copyright © 2016-2020 Yu Zhirui.

using UnityEngine;

namespace BiuBiu
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameMain : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinModules();
            InitCustomModules();
        }
    }
}

