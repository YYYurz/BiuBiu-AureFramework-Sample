//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BiuBiu
{
	public sealed partial class PlayerController : MonoBehaviour
	{
		private readonly uint[] runStepSoundArray = {
			1008,
			1009,
			1010,
			1011,
			1012,
		};
		
		/// <summary>
		/// 普攻动作事件回调
		/// </summary>
		/// <param name="attackStage"></param>
		private void OnAttackEvent(int attackStage)
		{
			var effectName = "AttackSlash_" + attackStage;
			var trans = transform;
			GameMain.Effect.PlayEffect(effectName, Vector3.up, trans.rotation, trans);

			var soundId = (uint) (1000 + attackStage);
			GameMain.Sound.PlaySound(soundId, 0f);
			
			var damageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<DamageSystem>();
			damageSystem.CreateCircleDamage(trans.position + Vector3.up, playerAreaTargetChecker.AttackRadius, damage);
		}

		/// <summary>
		/// 技能释放动作事件回调
		/// </summary>
		private void OnSkillEvent()
		{
			// 长方形打击盒
			var trans = transform;
			var forwardPosition = trans.forward;
			var rightPosition = trans.right;
			var position = trans.position;
			var pos1 = position + forwardPosition + rightPosition * 2f;
			var pos2 = position + forwardPosition * 13f + rightPosition;
			var pos3 = position + forwardPosition * 13f - rightPosition;
			var pos4 = position + forwardPosition - rightPosition * 2f;
			var vertex1 = new float2(pos1.x, pos1.z);
			var vertex2 = new float2(pos2.x, pos2.z);
			var vertex3 = new float2(pos3.x, pos3.z);
			var vertex4 = new float2(pos4.x, pos4.z);
			var damageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<DamageSystem>();
			damageSystem.CreateQuadrilateralDamage(vertex1, vertex2, vertex3, vertex4, skillDamage);
		}

		/// <summary>
		/// 技能特效事件回调
		/// </summary>
		private void OnSkillEffect()
		{
			var trans = transform;
			var effectPosition = trans.position + trans.forward * 7; 
			GameMain.Effect.PlayEffect("SkillSlash", effectPosition, trans.rotation);
		}

		/// <summary>
		/// 脚步事件回调
		/// </summary>
		private void OnRunStep()
		{
			var index = Random.Range(0, 4);
			GameMain.Sound.PlaySound(runStepSoundArray[index], 0f);
		}
	}
}