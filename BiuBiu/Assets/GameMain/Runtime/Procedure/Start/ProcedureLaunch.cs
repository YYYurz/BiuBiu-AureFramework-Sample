//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;

namespace BiuBiu
{
    // ReSharper disable once UnusedType.Global
    public class ProcedureLaunch : ProcedureBase
    {
        public override void OnEnter(params object[] args) {
            
        }

        public override void OnUpdate() {
            ChangeState<ProcedureCheckVersion>();
        }

        // private void InitLanguageSettings()
        // {
        //
        // }
    }
}