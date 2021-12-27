using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BiuBiu
{
    public class InputComponent : GameFrameworkComponent
    {
        public static Vector3 MoveDirectionVector = new Vector3();
        
        private bool inputStatus;

        private void Start()
        {
            inputStatus = false;
        }

        private void Update()
        {
            MoveInput();
            DanceInput();
        }

        private void MoveInput()
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            if (!h.Equals(0f) || !v.Equals(0f))
            {
                GameMain.Event.Fire(this, InputEventArgs.Create(GameEnum.INPUT_TYPE.Move, h, v));
                inputStatus = true;
            }
            else if (inputStatus)
            {
                GameMain.Event.Fire(this, InputEventArgs.Create(GameEnum.INPUT_TYPE.Move, 0f, 0f));
                inputStatus = false;
            }
        }

        private void DanceInput()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                GameMain.Event.Fire(this, InputEventArgs.Create(GameEnum.INPUT_TYPE.Dance));
            }
        }

        public static void OnRefreshMoveDirectionVector(Vector2 directionVec) {
            MoveDirectionVector.Set(directionVec.x, 0f, directionVec.y);
        }
    }
}