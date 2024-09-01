using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script generates an arc-shaped polygon collider
/// </summary>

[RequireComponent(typeof(PolygonCollider2D))]
public class ArcColliderPathGenerator : MonoBehaviour
{
    [Tooltip("Object that marks the center of the arc.")]
    [SerializeField] private Transform centerMark;
    [Tooltip("Object that marks the start of the arc on the inner side (closer to center).")]
    [SerializeField] private Transform innerStartMark;
    [Tooltip("Thickness of the arc collider, i.e. difference between inner and outer radiuses.")]
    [SerializeField] private float thickness;
    [Tooltip("Degree measure of the arc.")]
    [SerializeField] private float arc = 90f;
    [Tooltip("How many segments should the arc be divided into, more segments = smoother.")]
    [SerializeField] private int segments = 10;

    //The resulting collider
    private PolygonCollider2D polygonCollider;

    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.SetPath(0, GeneratePath());
    }

    /// <summary>
    /// Generates and returns an array of points for an arc-shaped polygon collider using Inspctor-set parameters
    /// </summary>
    private Vector2[] GeneratePath()
    {
        //Calculate local positions of arc center and inner start point
        Vector3 centerMarkLocalPos = transform.InverseTransformPoint(centerMark.position);
        Vector3 innerStartPointLocalPos = transform.InverseTransformPoint(innerStartMark.position);

        //Calculate inner and outer radiuses
        float innerRadius = Vector3.Distance(centerMarkLocalPos, innerStartPointLocalPos);
        float outerRadius = innerRadius + thickness;

        //Lists to hold calculated point coordinates
        List<Vector2> innerPoints = new List<Vector2>();
        List<Vector2> outerPoints = new List<Vector2>();

        //Normal vector pointing from center to the start of the arc
        Vector3 startPointNormal = (innerStartPointLocalPos - centerMarkLocalPos).normalized;

        //Angle measurement of a single arc segment
        float angleStep = arc / segments;
        
        //Calculate points diving the segments, inner and outer
        for (int i = 0; i <= segments; i++)
        {
            float angle = (i == segments) ? arc : i * angleStep;

            Vector3 pointNormal = RotateVector3(startPointNormal, angle);

            innerPoints.Add(pointNormal * innerRadius);
            outerPoints.Add(pointNormal * outerRadius);
        }

        //Create a unified list of points, joining inner and outer points (outer points list needs to be reversed to connect the path)
        outerPoints.Reverse();
        List<Vector2> allPoints = innerPoints;
        allPoints.AddRange(outerPoints);

        //Covert result to array and return
        return allPoints.ToArray();
    }

    /// <summary>
    /// Rotates a given vector by a given angle
    /// </summary>
    /// <param name="vector">The vector to be rotated</param>
    /// <param name="angle">The angle in degrees</param>
    /// <param name="clockWise">Rotation direction</param>
    private Vector3 RotateVector3 (Vector3 vector, float angle, bool clockWise = true)
    {
        float directionMultiplier = clockWise ? -1 : 1;
        Quaternion rotation = Quaternion.Euler(0, 0, angle * directionMultiplier);
        return rotation * vector;
    }

}
