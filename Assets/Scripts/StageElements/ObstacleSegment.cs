using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes a segment of an obstacle
/// </summary>
public class ObstacleSegment : MonoBehaviour
{
    [Tooltip("Segment's sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    //The GameColor of the segment
    private GameColor gameColor;

    /// <summary>
    /// Sets the color of the segment
    /// </summary>
    /// <param name="newColor">The color to set</param>
    public void SetGameColor(GameColor newColor)
    {
        spriteRenderer.color = newColor.color;
        gameColor = newColor;
    }

    /// <summary>
    /// Returns the GameColor of the obstacle segment
    /// </summary>
    public GameColor GetGameColor()
    {
        return gameColor;
    }

}
