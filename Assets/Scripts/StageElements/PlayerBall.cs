using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes and controls a player ball
/// </summary>
public class PlayerBall : MonoBehaviour
{
    //This event is invoked upon player death
    public event EventHandler<PlayerBall> OnPlayerDeath;
    //This event is invoked when player collects a star
    public event EventHandler OnStarPickedUp;

    [Tooltip("Gravity acceleration to apply, should be >0")]
    [SerializeField] private float gravity = 10f;
    [Tooltip("Jump speed, should be >0")]
    [SerializeField] private float jumpSpeed = 10f;
    [Tooltip("Player ball sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    //Rigibbody
    private Rigidbody2D rb;
    //Player ball's current GameColor
    private GameColor gameColor;

    //Radius of the player ball
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        //Get reference to rigidbody
        rb = GetComponent<Rigidbody2D>();
        //Get radius from collider
        radius = GetComponent<Collider2D>().bounds.extents.y;
        
        //Get a random GameColor
        SetGameColor(ColorManager.Instance.GetRandomColor());

        //Subscribe to InputManager's mouse click event
        InputManager.Instance.OnLeftMouseClick += InputManager_OnLeftMouseClick;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is not below camera or die
        CameraCheck();
    }

    private void FixedUpdate()
    {
        //Apply gravity
        rb.velocity += Vector2.down * gravity * Time.fixedDeltaTime;
    }

    /// <summary>
    /// This method is called when player clicks left mouse button
    /// </summary>
    private void InputManager_OnLeftMouseClick(object sender, EventArgs e)
    {
        //Apply jump velocity
        rb.velocity = Vector2.up * jumpSpeed;
    }

    /// <summary>
    /// This method is called when player ball enter a trigger collider (pickups or obstacles)
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the trigger is an obstacle, die if obstacle is of a different color
        ObstacleSegment obstacle = other.gameObject.GetComponent<ObstacleSegment>();
        if (obstacle != null)
        {
            if (obstacle.GetGameColor().id != gameColor.id)
            {
                Die();
            }
            return;
        }

        //Check if trigger is a pickup, process accordingly
        Pickup pickup = other.gameObject.GetComponent<Pickup>();
        if (pickup != null)
        {
            ProcessPickup(pickup);
            pickup.Die();
            return;
        }

    }

    /// <summary>
    /// Process collected pickup item
    /// </summary>
    /// <param name="pickup">The collected pickup item</param>
    private void ProcessPickup(Pickup pickup)
    {
        switch (pickup.GetPickupType())
        {
            case PickupType.Star:
                OnStarPickedUp?.Invoke(this, EventArgs.Empty);
                break;
            case PickupType.ColorSwitch:
                //Try to get list of valid colors (if result is null, any color is valid)
                List<GameColor> limitedColors = pickup.GetComponent<ColorSwitchLimit>()?.GetColors();
                if (limitedColors != null)
                {
                    //Get a random valid color
                    SetGameColor(ColorManager.Instance.GetRandomColorFromList(limitedColors, gameColor));
                }
                else
                {
                    //Get any random color
                    SetGameColor(ColorManager.Instance.GetRandomColor(gameColor));
                }
                break;
        }
    }

    /// <summary>
    /// Sets player ball's new GameColor
    /// </summary>
    /// <param name="newGameColor">The new GameColor to set</param>
    private void SetGameColor(GameColor newGameColor)
    {
        spriteRenderer.color = newGameColor.color;
        gameColor = newGameColor;
    }

    /// <summary>
    /// Checks if player ball is not below camera, kills player if it is
    /// </summary>
    private void CameraCheck()
    {
        //Calculate position of the ball's top
        Vector3 ballTop = transform.position;
        ballTop.y += radius;

        //Die if ball's top is below camera's view
        if (Camera.main.WorldToScreenPoint(ballTop).y < 0)
        {
            Die();
        }
    }

    /// <summary>
    /// If the object has Destruction script attached, calls its Destruct method, otherwise just destroys the object
    /// </summary>
    private void Die()
    {
        OnPlayerDeath?.Invoke(this, this);

        Destruction destruction = GetComponent<Destruction>();
        if (destruction != null)
        {
            destruction.Destruct();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This method is called when object is destroyed
    /// </summary>
    private void OnDestroy()
    {
        //Unsubscribe from events
        InputManager.Instance.OnLeftMouseClick -= InputManager_OnLeftMouseClick;
    }

}
