using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class CanonProjectile : Ability
    {


        public override event Action DonePerforming;
        public override bool IsAvailable() { return false; }

        public override void Perform() { }

    }
}

