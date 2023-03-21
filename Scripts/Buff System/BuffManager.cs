using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

namespace Buffs
{
    public enum BuffType
    {
        FIRE_DOT,
        STORM_DOT,
        RADIATION_DOT,
        RADIATION_HOT,
        SLUDGE_SPEEDBUFF,
        SLUDGE_SPEEDDEBUFF
    }
    [RequireComponent(typeof(Character_Modifiable), typeof(ShieldHealth))]
    public class BuffManager : MonoBehaviour
    {
        [SerializeField]
        Dot Fire_Dot, Storm_Dot, Radiation_Hot;
        [SerializeField]
        Dot_Shield Radiation_Dot;
        [SerializeField] SpeedMod Sludge_Speed_buff, Sludge_Speed_debuff;
        Buff appliedBuff;
        float currentBufftimer = 0;
        public void Apply(BuffType buffType)
        {
            if (!gameObject.activeInHierarchy) { return; }
            Buff buff = GetBuff(buffType);
            if (appliedBuff != buff || appliedBuff == null)
            {
                appliedBuff?.OnBuffRemoved();
                appliedBuff = buff;
                StartCoroutine(HandleBuff(appliedBuff, appliedBuff.BuffTime));
            }
            else { Refresh(); }
        }
        private Buff GetBuff(BuffType buffType)
        {
            switch (buffType)
            {
                case BuffType.FIRE_DOT: return Fire_Dot;
                case BuffType.STORM_DOT: return Storm_Dot;
                case BuffType.RADIATION_DOT: return Radiation_Dot;
                case BuffType.RADIATION_HOT: return Radiation_Hot;
                case BuffType.SLUDGE_SPEEDBUFF: return Sludge_Speed_buff;
                case BuffType.SLUDGE_SPEEDDEBUFF: return Sludge_Speed_debuff;
            }
            return null;
        }
        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            Character_Modifiable charMod = this.GetComponent<Character_Modifiable>();
            ShieldHealth health = this.GetComponent<ShieldHealth>();
            Debug.Log(health);
            Fire_Dot = Instantiate(Fire_Dot);
            Fire_Dot.Initialize(this.gameObject, health);

            Storm_Dot = Instantiate(Storm_Dot);
            Storm_Dot.Initialize(this.gameObject, health);

            Radiation_Dot = Instantiate(Radiation_Dot);
            Radiation_Dot.Initialize(this.gameObject, health);

            Radiation_Hot = Instantiate(Radiation_Hot);
            Radiation_Hot.Initialize(this.gameObject, health);

            Sludge_Speed_buff = Instantiate(Sludge_Speed_buff);
            Sludge_Speed_buff.Initialize(this.gameObject, charMod);

            Sludge_Speed_debuff = Instantiate(Sludge_Speed_debuff);
            Sludge_Speed_debuff.Initialize(this.gameObject, charMod);
        }
        private void OnDisable()
        {
            StopCoroutine("HandleBuff");
        }
        IEnumerator HandleBuff(Buff buff, float time)
        {
            currentBufftimer = 0;
            buff.OnBuffApplied();
            while (currentBufftimer < time)
            {
                if (appliedBuff) appliedBuff.Update();
                currentBufftimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            appliedBuff = null;
            buff.OnBuffRemoved();
        }
        public void Clear()
        {
            currentBufftimer = Mathf.Infinity;
        }
        public bool TryGetBuff(out Buff buff)
        {
            if (appliedBuff == null)
            {
                buff = null; return false;
            }
            buff = appliedBuff;
            return true;
        }
        public void Refresh()
        {
            if (appliedBuff == null) { return; }
            currentBufftimer = 0;
        }
        [ContextMenu("Test_Fire")]
        public void TestFire()
        {
            Apply(BuffType.FIRE_DOT);
        }
        [ContextMenu("Test_Sludge")]
        public void TestSludge()
        {
            Apply(BuffType.SLUDGE_SPEEDDEBUFF);
        }
    }
}
