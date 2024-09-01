using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls camera movement and view rect. Camera should be child of the object the script is attached to.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("The controlled camera.")]
    [SerializeField] private Camera cam;
    [Tooltip("The speed at which camera can move.")]
    [SerializeField] private float panningSpeed;
    [Header("Camera aspect ratio")]
    [SerializeField] private float width = 9f;
    [SerializeField] private float height = 16f;

    //The player ball to follow
    private Transform playerBall;
    //How much higher should the camera move (relative to current position)
    private float heightGoal;

    //Current aspect ratio of the game window. Set to 0 to ensure initial view rect calculation.
    private float windowAspectRatio = 0f;

    private void Awake()
    {
        CorrectCameraRect();
    }

    void LateUpdate()
    {
        CorrectCameraRect();
        CheckHeightGoal();
        MoveCamera();
    }

    /// <summary>
    /// Checks if the followed player ball moved above the current height goal and updates it accordingly.
    /// Sets height goal to 0 if player ball is null.
    /// </summary>
    private void CheckHeightGoal()
    {
        //If player ball doesn't exist or isn't set, height goal is set to 0 (no need to move)
        if (playerBall == null)
        {
            heightGoal = 0f;
            return;
        }

        //Checks is player ball is above the current height goal and updates the goal as needed
        if ((playerBall.position.y > transform.position.y + heightGoal))
        {
            heightGoal = playerBall.position.y - transform.position.y;
        }
    }

    /// <summary>
    /// Moves camera according to the current height goal and panning speed.
    /// Decreases height goal accordingly.
    /// </summary>
    private void MoveCamera()
    {
        if (heightGoal > 0)
        {
            Vector3 newPosition = transform.position;
            //Mathf.Min is used to ensure the camera doesn't move higher than the height goal
            float deltaY = Mathf.Min(panningSpeed * Time.deltaTime, heightGoal);
            heightGoal -= deltaY;
            newPosition.y += deltaY;
            transform.position = newPosition;
        }
    }

    /// <summary>
    /// Sets the player ball to be followed by the camera, resets height goal.
    /// </summary>
    /// <param name="playerBallTransform">The new player ball to follow</param>
    public void SetPlayerBall(Transform playerBallTransform)
    {
        playerBall = playerBallTransform;
        heightGoal = 0f;
    }

    /// <summary>
    /// Checks current screen aspect ratio and updates camera view rect as necessary to ensure the Inspector-set "width:height" ratio
    /// </summary>
    private void CorrectCameraRect()
    {
        //Checks current window aspect ratio, returns if it hasn't been changed
        float w = Screen.width;
        float h = Screen.height;
        if (windowAspectRatio == w / h) return;

        //If window is too wide
        if (w/h > width/height)
        {
            float wMargin = (1f - ((width * h) / (height * w)));
            cam.rect = new Rect(wMargin / 2f, 0f, 1f - wMargin, 1f);
        }
        //If window is too high
        else
        {
            float hMargin = 1f - ((height * w) / (width * h));
            cam.rect = new Rect(0f, hMargin / 2f, 1f, 1f - hMargin);
        }

        //Update saved aspect ratio of the window
        windowAspectRatio = w / h;
    }
}
