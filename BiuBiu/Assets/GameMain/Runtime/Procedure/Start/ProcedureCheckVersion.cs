
using AureFramework.Procedure;

namespace BiuBiu
{
    public class ProcedureCheckVersion : ProcedureBase
    {

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            ChangeState<ProcedurePreload>();
        }
    }
}
