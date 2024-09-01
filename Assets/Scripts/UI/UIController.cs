using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script controls the UI
/// </summary>
public class UIController : MonoBehaviour
{
    [Tooltip("Text displays amount of stars collected")]
    [SerializeField] private TextMeshProUGUI starCount;
    [Tooltip("Start screen object")]
    [SerializeField] private GameObject startScreen;
    [Tooltip("Game over screen object")]
    [SerializeField] private GameObject restartScreen;

    // Start is called before the first frame update
    void Start()
    {
        //Subscribe to GameManager events
        GameManager.Instance.OnStarCountChanged += GameManager_OnStarCountChanged;
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    /// <summary>
    /// This method is called on game over
    /// </summary>
    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        //Show restart screen
        restartScreen.SetActive(true);
    }

    /// <summary>
    /// This method is called on new game start
    /// </summary>
    private void GameManager_OnGameStarted(object sender, System.EventArgs e)
    {
        //Hide start and restart screens
        startScreen.SetActive(false);
        restartScreen.SetActive(false);
    }

    /// <summary>
    /// This method is called when player collects a star
    /// </summary>
    private void GameManager_OnStarCountChanged(object sender, int e)
    {
        //Update star counter text
        starCount.text = e.ToString();
    }

}
