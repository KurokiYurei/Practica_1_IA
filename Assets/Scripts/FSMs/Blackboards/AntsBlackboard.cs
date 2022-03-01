using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsBlackboard : MonoBehaviour
{
    [Header("Flocking")]
    public float closeToFollowTargetDistance = 60.0f; //Distance to detect cursor
    public float closeToTargetDistance = 10.0f; //When in target range

    [Header("Cursor Stats")]
    public float antsWanderingWeight = 0.15f; //Ants wandering weight
    public float antsCursorWeight = 0.7f; //Ants wander cursor weight
    public float antsGoWeight = 1.0f; //Ants go weight
}
