using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class PlayerMovement
{
    
    public static void Player_Move(Vector3 moveVec, float speed, Transform tf)
    {
        tf.Translate(moveVec * speed * Time.deltaTime);
    }

    public static void Player_Jump(Rigidbody rb, float force)
    {
        rb.AddForce(Vector3.up * force);
    }
}
