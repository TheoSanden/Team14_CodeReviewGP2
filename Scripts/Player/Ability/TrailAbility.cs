using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailAbility : AbilityScript
{
    [SerializeField] protected LayerMask _trailMask;
    protected GameObjectPooler _pool;
    public void StickToSurface(GameObject pool, Vector3 rayOrigin, Vector3 rayDir)
    {
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDir, out hit,10,_trailMask))
        {
            Vector3 hitPos = hit.point;
            Vector3 up = hit.normal;
            Vector3 right = Vector3.Cross(up, rayDir);
            Vector3 forward = Vector3.Cross(right, up);
            Instantiate(pool , hitPos, Quaternion.LookRotation(forward, up));
        }
    }
    
    public virtual bool AllowMove()
    {
        return true;
    }
}
