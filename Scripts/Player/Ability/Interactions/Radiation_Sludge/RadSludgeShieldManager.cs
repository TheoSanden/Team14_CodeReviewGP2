using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadSludgeShieldManager : MonoBehaviour
{
    [SerializeField] private GameObject _shieldGO;
    [SerializeField] private IntVariable _shieldAmount;
    [SerializeField] private FloatVariable _areaOfEffect;
    [SerializeField] private float _shieldGracePeriod = 1.5f;
    [SerializeField] private LayerMask _mask;
    private bool _canShield = true;

    
    public void AreaShield(Vector3 position)
    {
        if (_canShield)
        {
            _canShield = false;

            if (_shieldGO != null)
            {
                Instantiate(_shieldGO, position, default);
            }
            
            // CheckColliders(position);

            StartCoroutine(ShieldGracePeriod());
        }
        
    }

    // private void CheckColliders(Vector3 position)
    // {
    //     Collider[] colliders = Physics.OverlapSphere(position, _areaOfEffect.Value, _mask);
    //     foreach (Collider col in colliders)
    //     {
    //         if (col.CompareTag("Player"))
    //         {
    //             col.GetComponent<ShieldHealth>().ModifyShield(_shieldAmount.Value);
    //
    //             Shield.PlayOneShot(getShield);
    //         }
    //     }
    // }
    
    IEnumerator ShieldGracePeriod()
    {
        yield return new WaitForSeconds(_shieldGracePeriod);
        _canShield = true;
    }
}
