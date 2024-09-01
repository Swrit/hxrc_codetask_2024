using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls gameflow - starting the game, game over, and keeps count of the collected stars.
/// Accessed via static instance (GameManager.Instance)
/// </summary>
public class GameManager : MonoBehaviour
{
    //The static instance to access GameManager
    static public GameManager Instance { get; private set; }

    //This event is invoked when star count changes
    public event EventHandler<int> OnStarCountChanged;
    //This event is invoked when game starts
    public event EventHandler OnGameStarted;
    //This event is invoked upon game over
    public event EventHandler OnGameOver;

    [Tooltip("Reference to the stage controller.")]
    [SerializeField] private StageController stageController;

    //Current star count
    private int starCount = 0;
    //Is the game currently active? False before game starts or during game over
    private bool gameActive = false;

    private void Awake()
    {
        //Set static instance
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        //Subscribe to InputManager's event to detect mouse clicks
        InputManager.Instance.OnLeftMouseClick += InputManager_OnLeftMouseClick;
    }

    /// <summary>
    /// This method is called when mouse click is detected.
    /// </summary>
    private void InputManager_OnLeftMouseClick(object sender, EventArgs e)
    {
        //Start the game if it's not currently active.
        if (!gameActive) StartGame();
    }

    /// <summary>
    /// This method handles starting the game
    /// </summary>
    private void StartGame()
    {
        //Request stage controller to start the game
        //Get player ball reference and subscribe to the required events
        PlayerBall playerBall = stageController.StartGame();
        playerBall.OnStarPickedUp += PlayerBall_OnStarPickedUp;
        playerBall.OnPlayerDeath += PlayerBall_OnPlayerDeath;
        //Reset star count
        ChangeStarCount(0);
        //Mark the start of the game
        OnGameStarted?.Invoke(this, EventArgs.Empty);
        gameActive = true;
    }

    /// <summary>
    /// This method handles game over
    /// </summary>
    private void GameOver()
    {
        //Mark the game over
        OnGameOver?.Invoke(this, EventArgs.Empty);
        gameActive = false;
    }

    /// <summary>
    /// This method is called when player dies.
    /// </summary>
    private void PlayerBall_OnPlayerDeath(object sender, PlayerBall e)
    {
        //Unsubscribe from the player ball events
        e.OnStarPickedUp -= PlayerBall_OnStarPickedUp;
        e.OnPlayerDeath -= PlayerBall_OnPlayerDeath;
        //End the game
        GameOver();
    }

    /// <summary>
    /// This method is called when player collects a star.
    /// </summary>
    private void PlayerBall_OnStarPickedUp(object sender, EventArgs e)
    {
        //Add 1 to star count
        ChangeStarCount(starCount + 1);
    }

    /// <summary>
    /// Updates the star count
    /// </summary>
    /// <param name="newCount">New star count</param>
    private void ChangeStarCount(int newCount)
    {
        starCount = newCount;
        OnStarCountChanged?.Invoke(this, starCount);
    }
}
