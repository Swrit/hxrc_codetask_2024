using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an obstacle consisting of ObstacleSegments
/// </summary>
public class Obstacle : MonoBehaviour
{
    /// <summary>
    /// Property to access a list of GameColors used by an obstacle's segments
    /// </summary>
    public List<GameColor> UsedColors { get { return usedColors; } }

    [Tooltip("List of the obstacle's segments.")]
    [SerializeField] private List<ObstacleSegment> obstacleSegments = new List<ObstacleSegment>();

    //The list of GameColors used by the obstacle's segments
    private List<GameColor> usedColors = new List<GameColor>();

    // Start is called before the first frame update
    void Start()
    {
        //Request shuffled list of colors
        usedColors = ColorManager.Instance.GetRandomizedColorList();

        //Remove extra colors if there are less obstacle segments than colors
        if (usedColors.Count > obstacleSegments.Count)
            usedColors.RemoveRange(obstacleSegments.Count, usedColors.Count - obstacleSegments.Count);

        //Color index to loop through the color list if there are more segments than colors
        int i = 0; 

        //Set segments' colors
        foreach (ObstacleSegment segment in obstacleSegments)
        {
            segment.SetGameColor(usedColors[i]);

            i++;
            if (i == usedColors.Count) i = 0;
        }
    }

}
