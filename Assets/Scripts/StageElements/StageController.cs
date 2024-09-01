using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the game stage and is responsible for filling it with items and obstacles and spawning the player ball
/// </summary>
public class StageController : MonoBehaviour
{
    [Tooltip("Player ball prefab")]
    [SerializeField] private PlayerBall playerBallPrefab;
    [Tooltip("Marks where the player ball is to be spawned")]
    [SerializeField] private Transform playerBallSpawnPoint;

    //Current player ball
    private PlayerBall playerBall = null;

    [Tooltip("Camera controller")]
    [SerializeField] private CameraController cameraController;

    [Tooltip("How far above to fill the stage")]
    [SerializeField] private float fillThreshold;
    [Tooltip("How far below to clean up the stage")]
    [SerializeField] private float cleanupThreshold;

    [Tooltip("How far can the player move before stage position is reset")]
    [SerializeField] private float positionResetThreshold;

    [Tooltip("The list of stage segments that can be spawned as the first segment of the stage")]
    [SerializeField] private SpawnableSegmentsListSO firstSpawnable;

    //Currently active stage segments
    private List<StageSegment> activeSegments = new List<StageSegment>();

    // Update is called once per frame
    void Update()
    {
        //No need to do anything if there's no player ball (game hasn't started/game over)
        if (playerBall == null) return;

        //Get Y of the camera
        float cameraY = cameraController.transform.position.y;

        //Clean up/fill the stage and reset position if necessary
        CleanupSegments(cameraY);
        FillSegments(cameraY);
        ResetPositionCheck(cameraY);
    }

    /// <summary>
    /// Resets the stage and starts a new game
    /// </summary>
    /// <returns>The spawned player ball</returns>
    public PlayerBall StartGame()
    {
        ResetStage();

        FillSegments(cameraController.transform.position.y);

        //Spawn new player ball
        playerBall = Instantiate(playerBallPrefab, playerBallSpawnPoint.position, Quaternion.identity);

        cameraController.SetPlayerBall(playerBall.transform);

        return playerBall;
    }

    /// <summary>
    /// Resets the stage to prepare for a new game
    /// </summary>
    private void ResetStage()
    {
        //If player ball still exists, destroy it
        if (playerBall != null) Destroy(playerBall.gameObject);
        //Reset camera controller
        cameraController.SetPlayerBall(null);
        cameraController.transform.position = transform.position;
        //Destroy all active stage segments and clear the list
        foreach (StageSegment segment in activeSegments)
        {
            Destroy(segment.gameObject);
        }
        activeSegments.Clear();
    }

    /// <summary>
    /// Cleans up stage segments that are too far below the camera
    /// </summary>
    /// <param name="cameraY">Current camera Y position</param>
    private void CleanupSegments(float cameraY)
    {
        //No need to do anything if there are no active segments
        if (activeSegments.Count == 0) return;

        //Iterate through the segment list backwards, destroying and removing from the list any that are below cleanupThreshold
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            if (activeSegments[i].TopEdge.y < cameraY - cleanupThreshold)
            {
                Destroy(activeSegments[i].gameObject);
                activeSegments.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Fills stage with new segments above the camera
    /// </summary>
    /// <param name="cameraY">Current camera Y position</param>
    private void FillSegments(float cameraY)
    {
        //If no active segments, add 1 segment to start
        if (activeSegments.Count == 0) AddSegment();

        //Keep adding segments until fillThreshold is reached
        while (activeSegments[activeSegments.Count - 1].TopEdge.y < cameraY + fillThreshold)
        {
            AddSegment();
            break;
        }
    }

    /// <summary>
    /// Adds a new stage segment
    /// </summary>
    private void AddSegment()
    {
        //Variables to hold list of viable segments to spawn and the position where to spawn
        SpawnableSegmentsListSO spawnableSegmentsList;
        Vector3 position;

        //If no active segments, choose from firstSpawnable list and spawn at StageController position
        if (activeSegments.Count == 0)
        {
            spawnableSegmentsList = firstSpawnable;
            position = transform.position;
        }
        //If there already are active segments, get spawnable list and position from the last segment
        else
        {
            int lastSegmentIndex = activeSegments.Count - 1;
            spawnableSegmentsList = activeSegments[lastSegmentIndex].SpawnableList;
            position = activeSegments[lastSegmentIndex].TopEdge;
        }

        //Spawn a stage segment from the appropriate spawnable list and add it to the list of active segments
        StageSegment newSegment = Instantiate(spawnableSegmentsList.GetRandomSegment().gameObject, position, Quaternion.identity, transform).GetComponent<StageSegment>();
        activeSegments.Add(newSegment);

    }

    /// <summary>
    /// Resets stage position to avoid getting too far away from world origin
    /// </summary>
    /// <param name="cameraY">Current camera Y position</param>
    private void ResetPositionCheck(float cameraY)
    {
        //If camera abobe positionResetThreshold, move it, player ball and all stage segments down
        if (cameraY > positionResetThreshold)
        {
            MoveObjectDown(cameraController.transform, cameraY);
            MoveObjectDown(playerBall.transform, cameraY);
            foreach (StageSegment segment in activeSegments)
            {
                MoveObjectDown(segment.transform, cameraY);
            }
        }
    }

    /// <summary>
    /// Moves an object down by the given distance
    /// </summary>
    /// <param name="objectTransform">Object to move</param>
    /// <param name="distanceToMove">Distance to move (should be >0)</param>
    private void MoveObjectDown(Transform objectTransform, float distanceToMove)
    {
        Vector3 newPosition = objectTransform.position;
        newPosition.y -= distanceToMove;
        objectTransform.position = newPosition;
    }

}
