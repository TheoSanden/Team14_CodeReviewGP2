using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStormManager : MonoBehaviour
{
    [SerializeField] private GameObject _fireStorm;
    [SerializeField] private float _spawnGracePeriod = 0.5f;
    [SerializeField] private LayerMask _mask;
    private bool _canSpawn = true;


    public void SpawnFireStorm(Vector3 position)
    {
        Debug.Log("FireStorm");
        if (_fireStorm == null || !_canSpawn)
            return;

        _canSpawn = false;
        Instantiate(_fireStorm, SpawnOnGround(position), default);
        StartCoroutine(SpawnGracePeriod());

        
    }

    private Vector3 SpawnOnGround(Vector3 position)
    {
        RaycastHit hit;
        Vector3 rayOrg = position + Vector3.up * 5;
        if (Physics.SphereCast(rayOrg, 1, Vector3.down, out hit, 10, _mask))
        {
            return hit.point;
        }
        else
        {
            return position;
        }
    }
    
    
    IEnumerator SpawnGracePeriod()
    {
        yield return new WaitForSeconds(_spawnGracePeriod);
        _canSpawn = true;
    }
    
}


