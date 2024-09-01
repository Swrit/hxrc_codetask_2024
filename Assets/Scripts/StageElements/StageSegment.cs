using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes a segment of the stage
/// </summary>
public class StageSegment : MonoBehaviour
{
    /// <summary>
    /// Property to access position of the segment's top edge
    /// </summary>
    public Vector3 TopEdge { get { return topEdgeMark.position; } }
    /// <summary>
    /// Property to access list of stage segments that can be spawned after this one
    /// </summary>
    public SpawnableSegmentsListSO SpawnableList { get { return spawnableList; } }

    [Tooltip("Object that marks the top edge of the segment")]
    [SerializeField] private Transform topEdgeMark;
    [Tooltip("List of stage segments that can be spawned after this one")]
    [SerializeField] private SpawnableSegmentsListSO spawnableList;
    [Tooltip("Only one random object from this list will be set active at spawn")]
    [SerializeField] private List<GameObject> randomizedContents = new List<GameObject>();

    private void Start()
    {
        //If there are randomized contents, choose a random one and set it active, the others inactive
        if (randomizedContents.Count > 0)
        {
            int chosenIndex = UnityEngine.Random.Range(0, randomizedContents.Count);
            for (int i = 0; i < randomizedContents.Count; i++)
            {
                randomizedContents[i].SetActive(i == chosenIndex);
            }
        }
    }

}
