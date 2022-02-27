using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBlackboard : MonoBehaviour
{
    public float maxRestingTime = 3.0f; //Cat resting time
    public float foodReachableRdius = 25.0f;//Food reachable
    public float hunger = 0f; //The hunger of the cat
    public float maxEatingTime = 5.0f; //Time eating
    public float foodHungerDecrement = 50.0f;
    public float mouseDetectableRadius = 15.0f; //Radius to detect mouse
    public float mouseReachedRadius = 2.0f; //Radius when mouse reached

}
