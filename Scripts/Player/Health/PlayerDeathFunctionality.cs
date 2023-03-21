using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathFunctionality : MonoBehaviour
{
    [SerializeField] private FloatVariable _respawnTime;
    [SerializeField] private GameObject _friend;

    [SerializeField] private LayerMask _mask;
    [SerializeField] private GameObject _visual;
    [SerializeField] private int _respawnHealAmount = 100;
    [SerializeField] private float _respawnDistance = 3;
    private ShieldHealth _healthScript;
    private PlayerInputScript _playerInput;
    private Collider _collider;
    private Rigidbody _rb;
    private MeshRenderer _meshRenderer;
    [HideInInspector] public bool _isAlive = true;
    


    private void Awake()
    {
        if (!this.gameObject.TryGetComponent<ShieldHealth>(out _healthScript))
        {
            Debug.Log("No Health Script");
        }
        if (!this.gameObject.TryGetComponent<PlayerInputScript>(out _playerInput))
        {
            Debug.Log("No Input Script");
        }
        if (!this.gameObject.TryGetComponent<Rigidbody>(out _rb))
        {
            Debug.Log("No RigidBody");
        }
        if (!this.gameObject.TryGetComponent<Collider>(out _collider))
        {
            Debug.Log("No Collider");
        }
    }

    private void Update()
    {
        if (!_isAlive)
        {
            this.gameObject.transform.position = _friend.transform.position;
        }
    }

    public void PlayerDeath()
    {
        DeActivate();
        _isAlive = false;
        if (_friend.GetComponent<PlayerDeathFunctionality>()._isAlive)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            SceneManager.LoadScene("ErikL_GameOverTest");
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnTime.Value);
        _isAlive = true;
        ReActivate();
        _healthScript.Apply(_respawnHealAmount);
    }

    private void DeActivate()
    {
        if (_visual != null)
        {
            _visual.SetActive(false);
        }
        _playerInput.enabled = false;
        _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        _collider.enabled = false;
        this.gameObject.transform.position = _friend.transform.position;

    }
    private void ReActivate()
    {
        this.gameObject.transform.position = CheckRespawn();
        if (_visual != null)
        {
            _visual.SetActive(true);
        }
        _collider.enabled = true;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _playerInput.enabled = true;
    }

    private Vector3 CheckRespawn()
    {
        RaycastHit hit;
        Transform allyTF = _friend.transform;
        Vector3 rayDir = -allyTF.forward;
        Vector3 rayOrg = allyTF.position + allyTF.up;

        if (Physics.SphereCast(rayOrg, 0.5f, rayDir, out hit, _respawnDistance, _mask))
        {
            return hit.transform.position + hit.normal;
        }
        else
        {
            return rayOrg + rayDir * _respawnDistance;
        }
    }
}
