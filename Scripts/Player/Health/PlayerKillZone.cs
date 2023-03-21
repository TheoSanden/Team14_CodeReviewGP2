using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillZone : MonoBehaviour
{
    PlayerDeathFunctionality pdf;
    const float Y_KILLVALUE = -3.5f;
    private void Start()
    {

        if (!this.gameObject.TryGetComponent<PlayerDeathFunctionality>(out pdf))
        {
            Debug.LogWarning("No PlayerDeathFunctionality found.", this.gameObject);
        }
    }
    private void Update()
    {
        if (transform.position.y < Y_KILLVALUE)
        {
            pdf.PlayerDeath();
        }
    }
}
