using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script rotates the game object it is attached to
/// </summary>
public class Rotation : MonoBehaviour
{
    [Tooltip("The speed of rotation (in degrees).")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("Should the rotation direction (clockwise or counterclockwise) be chosen randomly?")]
    [SerializeField] private bool randomDirection;

    private void Start()
    {
        //Randomize direction if needed
        if (randomDirection) rotationSpeed *= Mathf.Sign(UnityEngine.Random.Range(-1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate the object according to set speed and passed time
        Vector3 ang = transform.localEulerAngles;
        ang.z = (ang.z + (rotationSpeed * Time.deltaTime));
        transform.localEulerAngles = ang;
    }
}
