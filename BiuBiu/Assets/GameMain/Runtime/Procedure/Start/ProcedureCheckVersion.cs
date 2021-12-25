using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace BB
{
    public class ProcedureCheckVersion : ProcedureBase
    {

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            // GameEntry.Resource.InitResources(OnInitResourcesComplete);
        }
        
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            ChangeState<ProcedurePreload>(procedureOwner);
        }
        
        private void OnInitResourcesComplete()
        {
            Log.Info("Init resources complete.");
        }
    }
}
