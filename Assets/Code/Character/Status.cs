using UnityEngine;

namespace WhalePark18.Character
{

    [System.Serializable]
    public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

    public class Status : MonoBehaviour
    {
        [HideInInspector]
        public HPEvent onHPEvent = new HPEvent();

        [Header("Walk, Run Speed")]
        [SerializeField]
        private float walkSpeed;
        [SerializeField]
        private float runSpeed;

        [Header("HP")]
        [SerializeField]
        private int startHP = 100;                      // 게임 시작 HP
        private int currentHP;                          // 현재 HP
        private int maxHP;                              // 최대 HP

        [SerializeField]
        private int startHPRecoveryForSec = 0;          // 게임 시작 초당 HP 회복량
        private int currentHPRecoveryForSec;            // 현재 초당 HP 회복량

        [Header("Shield")]
        [SerializeField]
        private int startShield = 0;                    // 게임 시작 실드량
        private int currentShield;                      // 현재 실드량
        private int maxShield;                          // 최대 실드량

        [SerializeField]
        private float startShieldRecoveryForSec = 0f;   // 게임 시작 초당 실드 회복 퍼센트
        private float currentShieldRecoveryForSec;      // 현재 초당 실드 회복 퍼센트

        [Header("Attack")]
        [SerializeField]
        private float startDamagePercentIncrease = 1f;  // 게임 시작 피해 % 증가량
        private float currentDamagePercentIncrease;     // 현재 피해 % 증가량

        [SerializeField]
        private int startPiercing = 0;                  // 게임 시작 관통력
        private int currentPiercing;                    // 현재 관통력

        [SerializeField]
        private float startAttackSpeed = 1f;            // 게임 시작 공격 속도
        private float currentAttackSpeed;               // 현재 공격 속도

        [Header("Item")]
        private float startItemEfficiency = 1f;         // 게임 시작 아이템 효율
        private float currentItemEfficiency;            // 현재 아이템 효율

        /// <summary>
        /// Walk, Run Property
        /// </summary>
        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;

        /// <summary>
        /// HP Property
        /// </summary>
        public int MaxHP => maxHP;
        public int CurrentHP => currentHP;

        /// <summary>
        /// Attack Property
        /// </summary>
        public float CurrentDamagePercentIncrease
        {
            set => currentDamagePercentIncrease = value;
            get => currentDamagePercentIncrease;
        }
        public int CurrentPiercing
        {
            set => currentPiercing = value;
            get => currentPiercing;
        }
        public float CurrentAttackSpeed
        {
            set => currentAttackSpeed = value;
            get => currentAttackSpeed;
        }

        /// <summary>
        /// Item Property
        /// </summary>
        public float CurrentItemEfficiency => currentItemEfficiency;

        private void Awake()
        {
            /// 생명력
            maxHP = currentHP = startHP;
            currentHPRecoveryForSec = startHPRecoveryForSec;

            /// 실드
            currentShield = startShield;
            currentShieldRecoveryForSec = startShieldRecoveryForSec;

            /// 공격
            currentDamagePercentIncrease = startDamagePercentIncrease;
            currentPiercing = startPiercing;
            currentAttackSpeed = startAttackSpeed;

            /// 아이템
            currentItemEfficiency = startItemEfficiency;
        }

        public bool DecreaseHP(int damage)
        {
            int previousHP = currentHP;

            currentHP = currentHP - damage > 0 ? currentHP - damage : 0;

            onHPEvent.Invoke(previousHP, currentHP);

            if (currentHP == 0)
            {
                return true;
            }

            return false;
        }

        public void IncreaseHP(int hp)
        {
            int previousHP = currentHP;

            currentHP = currentHP + hp > startHP ? startHP : currentHP + hp;

            onHPEvent.Invoke(previousHP, currentHP);
        }

        public void IncreaseAttackSpeed(float increaseAttackSpeed)
        {
            /// Mathf.Round(): 소숫점 첫째자리에서 반올림
            /// 하지만 아이템 효과와 아이템 효율 스탯에 의해 소숫점 셋째자리를 반올림 해야 함
            /// 따라서 [아이템 효과 * 아이템 효율 스탯] 값에 100을 곱해 소수섬 셋째자리를 
            /// 소수점 첫째자리로 바꾸고 반올림을 한 후, 100을 나눠 복원함
            currentAttackSpeed += Mathf.Round((increaseAttackSpeed * currentItemEfficiency) * 100) / 100;
        }

        public void DisincreaseAttackSpeed(float disincreaseAttackSpeed)
        {
            currentAttackSpeed -= Mathf.Round((disincreaseAttackSpeed * currentItemEfficiency) * 100) / 100;
        }
    }
}