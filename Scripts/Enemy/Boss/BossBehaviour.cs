using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Boss
{

    public class BossBehaviour : MonoBehaviour
    {
        public Ability[] abilities;
        private Ability currentAbility;

        public HealthBar HealthBar;

        public bool isAggro;
        private AggroBubble bubble;

        private SpiderAnimatorController anim;
        public Transform headAnim;
        public float basicAttackCooldown;
        private float currentBasicAttackCooldown;

        public Transform basicAttackStartPos;
        public ProjectileData basicAttackData;

        public float headSpeed;
        public float bodySpeed;

        private void Awake()
        {

            bubble = GetComponentInChildren<AggroBubble>();
            if (bubble == null)
            {
                isAggro = true;
                PerformAbility();
            }
            else
                bubble.Entered += PerformAbility;

            anim = GetComponentInChildren<SpiderAnimatorController>();
        }

        private void PerformAbility()
        {
            isAggro = true;
            currentAbility = GetAbility();
            if (currentAbility == null)
            {
                //do default
                return;
            }

            currentAbility.DonePerforming += OnAbilityDone;
            currentAbility.Perform();
        }

        private void OnAbilityDone()
        {
            currentAbility.DonePerforming -= OnAbilityDone;
            PerformAbility();
        }

        private Ability GetAbility()
        {
            List<Ability> availableAbilities = new List<Ability>();
            foreach (Ability ability in abilities)
            {
                if (ability.IsAvailable())
                {
                    availableAbilities.Add(ability);
                }
            }
            if (availableAbilities.Count < 1) return null;
            int randomIndex = Random.Range(0, availableAbilities.Count);
            return availableAbilities[randomIndex];
        }

        private void Update()
        {
            if (!isAggro) return;
            if (currentBasicAttackCooldown < basicAttackCooldown)
            {
                currentBasicAttackCooldown += Time.deltaTime;
            }
            else
            {
                currentBasicAttackCooldown = 0;
                BasicAttack();
            }

            HeadRotator(HelperSenses.FindFarthestTarget(transform));
        }

        private void HeadRotator(Transform target)
        {
            Debug.Log("dfk");
            // calculate the Quaternion for the rotation
            Quaternion rot = Quaternion.LookRotation(target.position - headAnim.position);

            //Apply the rotation 
            headAnim.rotation = Quaternion.Lerp(headAnim.rotation, rot, Time.deltaTime * headSpeed);

            float angle = headAnim.localEulerAngles.z;
            if (angle > 270)
            {
                angle = Mathf.Clamp(headAnim.localEulerAngles.z, 270, 360);

            }
            else
            {
                angle = Mathf.Clamp(headAnim.localEulerAngles.z, 0, 90);

            }



            Quaternion rot2 = Quaternion.LookRotation(target.position - transform.position);

            //Apply the rotation 
            transform.rotation = Quaternion.Lerp(transform.rotation, rot2, Time.deltaTime * bodySpeed);

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


            // put 0 on the axys you do not want for the rotation object to rotate
            headAnim.localEulerAngles = new Vector3(0, 0, angle);

        }
        private void BasicAttack()
        {
            anim.PlayTargetAnimation("Boss_MainShot");
            EnemyManager.instance.ShootProjectile(basicAttackStartPos.position, headAnim.up, basicAttackData);
        }

    }

}