using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handles any special effects when an object is destroyed, such as sound or spawning particle systems
/// </summary>
public class Destruction : MonoBehaviour
{
    [Tooltip("A game object that will be spawned upon destruction.")]
    [SerializeField] private GameObject spawnOnDestruction;
    [Tooltip("A sound effect to play upon destruction.")]
    [SerializeField] private AudioClip soundEffect;

    /// <summary>
    /// This method is called to destroy the object and handles any special effects before desctruction.
    /// Note: the object can be destroyed directly, bypassing this method if needed.
    /// </summary>
    public void Destruct()
    {
        if (spawnOnDestruction!=null)
        {
            Instantiate(spawnOnDestruction, transform.position, Quaternion.identity);
        }
        if (soundEffect != null)
        {
            SoundManager.Instance.PlaySound(soundEffect);
        }
        Destroy(gameObject);
    }
}
