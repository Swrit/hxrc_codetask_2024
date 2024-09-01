using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes a color for gameplay purposes.
/// </summary>
[Serializable]
public class GameColor
{
    //Constructor
    public GameColor(int id, Color color)
    {
        this.id = id;
        this.color = color;
    }

    [Tooltip("Id of the color.")]
    public int id;
    [Tooltip("The color.")]
    public Color color;
}
