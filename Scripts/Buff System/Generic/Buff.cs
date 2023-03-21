using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;

namespace Buffs
{
    public class Buff : ScriptableObject
    {
        [SerializeField] Texture2D icon;
        [SerializeField] protected InteractionArgs.ElementType elementType;
        [SerializeField] protected float buffTime = 0;
        [SerializeField] protected VisualEffectAsset vfxAsset;
        private GameObject target;
        private VisualEffect vfxComponent;
        protected bool hasVfx;
        public float BuffTime
        {
            get => buffTime;
        }
        protected bool initialized;
        public virtual void Initialize(GameObject target)
        {
            this.target = target;
            hasVfx = (target.TryGetComponent<VisualEffect>(out vfxComponent) && vfxAsset != null);
            initialized = (target != null);
        }
        public InteractionArgs.ElementType ElementType
        {
            get => elementType;
        }
        public virtual void OnBuffApplied()
        {
            if (hasVfx && initialized)
            {
                vfxComponent.visualEffectAsset = vfxAsset;
                Mesh mesh;
                MeshFilter meshFilter = target.GetComponentInChildren<MeshFilter>();
                SkinnedMeshRenderer skinnedMeshRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();
                if (meshFilter)
                {
                    mesh = meshFilter.mesh;
                }
                else if (skinnedMeshRenderer)
                {
                    mesh = skinnedMeshRenderer.sharedMesh;
                }
                else
                {
                    return;
                }
                vfxComponent.SetMesh("SetMesh", mesh);
                vfxComponent.Play();
            }
        }
        public virtual void OnBuffRemoved()
        {
            if (hasVfx)
            {
                vfxComponent.Stop();
            }
        }
        public virtual void Update()
        {

        }
    }
}
