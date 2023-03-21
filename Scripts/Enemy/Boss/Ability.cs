using System;
using UnityEngine;

namespace Boss
{
    public abstract class Ability: MonoBehaviour
    {
        public virtual void Perform() { }
        public virtual bool IsAvailable() { return false; }

        public virtual event Action DonePerforming;

    }
}


