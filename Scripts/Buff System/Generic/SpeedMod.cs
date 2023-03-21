using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffs
{
    [CreateAssetMenu(fileName = "New SpeedMod", menuName = "Buff/Speed", order = 0)]
    public class SpeedMod : Buff
    {
        [Range(0, 5)]
        [SerializeField] float speedMod;
        Character_Modifiable characterMod;
        public override void OnBuffApplied()
        {
            base.OnBuffApplied();
            characterMod.speedModifier = speedMod;
        }
        public override void OnBuffRemoved()
        {
            base.OnBuffRemoved();
            characterMod.speedModifier = 1;
        }
        public virtual void Initialize(GameObject go, Character_Modifiable characterMod)
        {
            base.Initialize(go);
            this.characterMod = characterMod;
        }
    }
}
