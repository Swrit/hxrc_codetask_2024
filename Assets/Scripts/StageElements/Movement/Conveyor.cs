using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves a set of objects (like obstacle segments) horizontally from one side to another, repeatedly.
/// The object this script is attached to acts as a "center" for position reset check.
/// </summary>
public class Conveyor : MonoBehaviour
{
    [Tooltip("The speed of movement.")]
    [SerializeField] private float speed;
    [Tooltip("Should the movement direction (left or right) be chosen randomly?")]
    [SerializeField] private bool randomDirection;
    [Tooltip("The objects to move.")]
    [SerializeField] private List<Collider2D> conveyorSegments = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        //Randomize direction if needed
        if (randomDirection) speed *= Mathf.Sign(UnityEngine.Random.Range(-1, 1));

        //Sort object list according to position and movement direction
        SortConveyorSegments();
    }

    // Update is called once per frame
    void Update()
    {
        //Move the objects according to set speed and passed time
        float deltaX = speed * Time.deltaTime;
        foreach (Collider2D segment in conveyorSegments)
        {
            MoveSegment(segment, deltaX);
        }

        //If the first moving object is further from the center than the last one, move it to the back of the line and list
        float firstSegmentDistance = Vector3.Distance(transform.position, conveyorSegments[0].transform.position);
        float lastSegmentDistance = Vector3.Distance(transform.position, conveyorSegments[conveyorSegments.Count - 1].transform.position);
        if (firstSegmentDistance > lastSegmentDistance)
        {
            //Distance to move the object
            float swapDistance = firstSegmentDistance + lastSegmentDistance + conveyorSegments[0].bounds.extents.x + conveyorSegments[conveyorSegments.Count - 1].bounds.extents.x;
            //Remember the object for list repositioning
            Collider2D swapSegment = conveyorSegments[0];
            //Move object to the back of the line
            MoveSegment(swapSegment, swapDistance * (Mathf.Sign(speed) * -1));
            //Move object to the back of the list
            conveyorSegments.RemoveAt(0);
            conveyorSegments.Add(swapSegment);
        }
    }

    /// <summary>
    /// Sorts object list according to position and movement direction (first in list is ahead of the rest)
    /// </summary>
    private void SortConveyorSegments()
    {
        //Create new list to store segments in order
        List<Collider2D> sortedSegments = new List<Collider2D>();
        //While conveyorSegments list has elements, find the one with the lowest x coordinate and move it to the sorted list
        while (conveyorSegments.Count > 0)
        {
            //Index of the conveyorSegments member with the lowest x coordinate
            int leftmostPosIndex = 0;
            for (int i = 0; i < conveyorSegments.Count; i++)
            {
                if (conveyorSegments[i].transform.position.x < conveyorSegments[leftmostPosIndex].transform.position.x)
                {
                    leftmostPosIndex = i;
                }
            }
            sortedSegments.Add(conveyorSegments[leftmostPosIndex]);
            conveyorSegments.RemoveAt(leftmostPosIndex);
        }
        //Set the sorted list as the new conveyorSegments list
        conveyorSegments = sortedSegments;
        //If movement direction is left-to-right, reverse the list (rightmost object is ahead)
        if (speed > 0) conveyorSegments.Reverse();
    }

    /// <summary>
    /// Moves a given object by a given delta x
    /// </summary>
    /// <param name="segment">Object to move</param>
    /// <param name="deltaX">Distance to move (negative to move left)</param>
    private void MoveSegment(Collider2D segment, float deltaX)
    {
        Vector3 newPosition = segment.transform.position;
        newPosition.x += deltaX;
        segment.transform.position = newPosition;
    }
}
