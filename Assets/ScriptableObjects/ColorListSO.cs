using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object holds the list of the available GameColors
/// </summary>
[CreateAssetMenu()]
public class ColorListSO : ScriptableObject
{
    /// <summary>
    /// Property to access the list of available GameColors
    /// </summary>
    public List<GameColor> GameColors { get { return gameColors; } }

    [Tooltip("The list of available GameColors")]
    [SerializeField] private List<GameColor> gameColors = new List<GameColor>();

}
