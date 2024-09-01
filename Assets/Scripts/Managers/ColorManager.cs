using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script holds a list of available GameColors and is used to request them for colored objects (player ball, obstacle segments).
/// Accessed via static instance (ColorManager.Instance)
/// </summary>
public class ColorManager : MonoBehaviour
{
    //The static instance to access ColorManager
    static public ColorManager Instance { get; private set; }

    [Tooltip("The list of available GameColors.")]
    [SerializeField] private ColorListSO colorList;

    private void Awake()
    {
        //Set static instance
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// Returns a random GameColor, except the optionally provided exception.
    /// Exception ignored if no other colors available.
    /// </summary>
    /// <param name="exception">Excepted color that shouldn't be returned if possible.</param>
    public GameColor GetRandomColor(GameColor exception = null)
    {
        return GetRandomColorFromList(colorList.GameColors, exception);
    }

    /// <summary>
    /// Returns a random GameColor from the provided list, except the optionally provided exception.
    /// Exception ignored if no other colors available.
    /// </summary>
    /// <param name="colors">List of GameColors to choose from.</param>
    /// <param name="exception">Excepted color that shouldn't be returned if possible.</param>
    public GameColor GetRandomColorFromList(List<GameColor> colors, GameColor exception = null)
    {
        //If list is empty, log error and return dummy color
        if (colors.Count == 0)
        {
            Debug.LogError("Color list was empty");
            return new GameColor(-1, Color.white);
        }

        //Make list of colors to choose from - considering the exception if it's not the only color in the list
        List<GameColor> validColors = new List<GameColor>(colors);
        if (colors.Contains(exception) && colors.Count > 1) validColors.Remove(exception);

        //Choose and return random color
        int selectedIndex = UnityEngine.Random.Range(0, validColors.Count);
        return validColors[selectedIndex];
    }

    /// <summary>
    /// Returns the list of available colors in randomized order
    /// </summary>
    public List<GameColor> GetRandomizedColorList()
    {
        //Copy list to randomize
        List<GameColor> randomizedList = new List<GameColor>(colorList.GameColors);

        //Fisher–Yates shuffle
        for (int i = randomizedList.Count - 1; i > 0; i--)
        {
            GameColor temp = randomizedList[i];
            int swapPlace = UnityEngine.Random.Range(0, i + 1);
            randomizedList[i] = randomizedList[swapPlace];
            randomizedList[swapPlace] = temp;
        }

        //Return the shuffled list
        return randomizedList;
    }
}
