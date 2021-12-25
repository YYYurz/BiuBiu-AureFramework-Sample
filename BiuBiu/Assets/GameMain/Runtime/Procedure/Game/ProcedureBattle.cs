using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BB
{
    // ReSharper disable once UnusedType.Global
    public class ProcedureBattle : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            Log.Debug("ProcedureBattle OnEnter");

            GameEntry.UI.OpenUI(Constant.UIFormID.LoginWindow);
            GameEntry.Entity.ShowSoldier();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}