using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script should be attached to a color switch pickup preceding an obstacle with less than 4 colors to ensure only viable colors can be selected.
/// </summary>
public class ColorSwitchLimit : MonoBehaviour
{
    [Tooltip("The upcoming obstacle with limited colors. If not set, no color limit will be applied.")]
    [SerializeField] private Obstacle obstacle;

    /// <summary>
    /// Returns list of viable colors or null if no obstacle is set.
    /// </summary>
    public List<GameColor> GetColors()
    {
        if (obstacle == null) return null;
        return obstacle.UsedColors;
    }
}
