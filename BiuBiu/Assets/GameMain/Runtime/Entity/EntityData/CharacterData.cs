using GameConfig;
using UnityEngine;

namespace BiuBiu
{
    public class CharacterData : TargetableObjectData
    {
        [SerializeField]
        private float curHealth = 0f;

        [SerializeField]
        private float maxHealth = 0f;

        [SerializeField]
        private float moveSpeed = 0f;

        public CharacterData(int entityId, int typeId)
            : base(entityId, typeId)
        {
            var vocationInfo = GameMain.TableData.DataTableInfo.GetDataTableReader<DTVocationTableReader>().GetInfo((uint) typeId);
            maxHealth = vocationInfo.MaxHealth;
            moveSpeed = vocationInfo.MoveSpeed;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public override float MaxHealth => maxHealth;

        /// <summary>
        /// 当前生命。
        /// </summary>
        public override float CurHealth
        {
            get => curHealth;
            set => curHealth = value;
        }

        /// <summary>
        /// 移动速度。
        /// </summary>
        public float MoveSpeed => moveSpeed;
    }
}