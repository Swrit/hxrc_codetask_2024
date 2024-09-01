using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a list of stage segments that can be spawned after another segment.
/// It has to be a scriptable object because if a prefab object holds a reference to itself, it becomes a reference to the instance on spawn.
/// </summary>
[CreateAssetMenu()]
public class SpawnableSegmentsListSO : ScriptableObject
{
    /// <summary>
    /// This struct holds the stage segment prefab and the chance of it being spawned
    /// </summary>
    [Serializable]
    private struct SpawnableSegment
    {
        [Tooltip("Spawnable stage segment")]
        public StageSegment segment;
        [Tooltip("Relative likelihood of this segment being chosen")]
        public float spawnChance;
    }

    [Tooltip("List of segments that can spawn after the current one, with spawn chances")]
    [SerializeField] private List<SpawnableSegment> spawnableSegments = new List<SpawnableSegment>();

    /// <summary>
    /// This methods returns a random stage segment from the list, taking into account spawn chance
    /// </summary>
    public StageSegment GetRandomSegment()
    {
        //If spawnable list is empty, log error
        if (spawnableSegments.Count == 0)
        {
            Debug.LogError("Spawnable list empty");
            return null;
        }

        //Calculate sum of all spawn chances
        float sumOfChances = 0f;
        foreach (SpawnableSegment spawnable in spawnableSegments)
        {
            sumOfChances += spawnable.spawnChance;
        }

        //Random number between 0 and sum of chances to select a random segment
        float selection = UnityEngine.Random.Range(0, sumOfChances);
        
        //Variable to hold the chosen segment
        StageSegment selectedSegment = spawnableSegments[0].segment;

        //Iterate through spawnable list, subtracting spawn chances from the random number until it is <=0
        foreach (SpawnableSegment spawnable in spawnableSegments)
        {
            selection -= spawnable.spawnChance;
            if (selection <= 0)
            {
                selectedSegment = spawnable.segment;
                break;
            }
        }

        //Return chosen segment
        return selectedSegment;
    }
}
