using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes a pickup item, such as a star or a color switch
/// </summary>
public class Pickup : MonoBehaviour
{
    [Tooltip("The type of the pickup item")]
    [SerializeField] private PickupType pickupType;

    /// <summary>
    /// Returns the type of the pickup item
    /// </summary>
    public PickupType GetPickupType()
    {
        return pickupType;
    }

    /// <summary>
    /// If the object has Destruction script attached, calls its Destruct method, otherwise just destroys the object
    /// </summary>
    public void Die()
    {
        Destruction destruction = GetComponent<Destruction>();
        if (destruction != null)
        {
            destruction.Destruct();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
