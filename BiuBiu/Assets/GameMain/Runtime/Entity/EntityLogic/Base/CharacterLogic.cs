using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace BB
{
    public abstract class CharacterLogic : TargetableObject
    {
        [SerializeField]
        protected CharacterData myCharacterData;

        private CharacterController characterController;
        private Animator animator; 

        private readonly Vector3 gravityDirection = new Vector3(0f, -9.8f, 0f);
        private Vector3 moveDirection = new Vector3(0f, 0f, 0f);
        private float rotationY;

        
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            myCharacterData = userData as CharacterData;
            if (myCharacterData == null)
            {
                Log.Error("My aircraft data is invalid.");
                return;
            }
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            GetComponent<CameraLogic>().OnStartFollowing();

            GameEntry.Event.Subscribe(InputEventArgs.EventId, OnInputEvent);
            animator.Update(1);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            
            GameEntry.Event.Unsubscribe(InputEventArgs.EventId, OnInputEvent);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        
            characterController.Move(gravityDirection * Time.deltaTime);
            TickMove();
        }

        private void TickMove() {
            var direction = InputComponent.MoveDirectionVector * myCharacterData.MoveSpeed;
            characterController.Move(direction * Time.deltaTime);
        }

        private void OnInputEvent(object sender, GameEventArgs args)
        {
            var e = (InputEventArgs) args;
            if (e == null)
            {
                Log.Error("InputEventArgs is Null or Invalid");
                return;
            }

            if (e.InputType == GameEnum.INPUT_TYPE.Dance)
            {
                // var info = animator.GetCurrentAnimatorStateInfo(0);
                // if (info.IsName("cheer_dance"))
                //     animator.SetBool("cheer_dance", false);
                // else
                //     animator.SetBool("cheer_dance", true);
                //
                animator.Play("Base Layer.CheerDance");
            }

            if (e.InputType != GameEnum.INPUT_TYPE.Move)
                return;
            if (e.OffsetX.Equals(0f) && e.OffsetY.Equals(0f))
            {
                animator.SetFloat("walk", 0f);
                return;
            }
 
            if (e.OffsetX > 0)
                rotationY = Mathf.Acos(e.OffsetY / Mathf.Sqrt(e.OffsetX * e.OffsetX + e.OffsetY * e.OffsetY)) * 180 /
                            Mathf.PI;
            else
                rotationY = Mathf.Acos(e.OffsetY / Mathf.Sqrt(e.OffsetX * e.OffsetX + e.OffsetY * e.OffsetY)) * 180 /
                    Mathf.PI * -1;
            animator.SetFloat("walk", 1f);
            // 防止除零导致的NaN错误
            transform.rotation = Quaternion.Euler(new Vector3(0, rotationY, 0));

            moveDirection.Set(e.OffsetX, 0, e.OffsetY);
            moveDirection *= myCharacterData.MoveSpeed;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}