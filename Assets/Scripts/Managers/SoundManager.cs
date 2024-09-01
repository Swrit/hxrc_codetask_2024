using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is responsible for playing the sound effects.
/// Accessed via static instance (SoundManager.Instance)
/// </summary>
public class SoundManager : MonoBehaviour
{
    //The static instance to access SoundManager
    static public SoundManager Instance { get; private set; }

    [Tooltip("Audio source to play the sounds")]
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        //Set static instance
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// Plays the given audio clip
    /// </summary>
    /// <param name="soundfile">Audio clip to play</param>
    public void PlaySound(AudioClip soundfile)
    {
        audioSource.PlayOneShot(soundfile);
    }

}
