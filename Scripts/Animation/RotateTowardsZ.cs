using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotateTowardsZ : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {

        // calculate the Quaternion for the rotation
        Quaternion rot = Quaternion.LookRotation(target.position - transform.position);

        //Apply the rotation 
        transform.rotation = rot;

        // put 0 on the axys you do not want for the rotation object to rotate
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
