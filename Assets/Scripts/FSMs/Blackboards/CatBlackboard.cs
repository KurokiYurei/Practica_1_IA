using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBlackboard : MonoBehaviour
{
    public float foodDetectableRadius = 25.0f;//Food detectable
    public float foodReachableRadius = 2.0f;//Food reachable
    public float hunger = 80f;
    public float minHunger = 0f; //The hunger of the cat
    public float maxHunger = 80f;
    public float maxEatingTime = 5.0f; //Time eating
    public float hungerDecrement = 2.0f;
    public float energy = 80f;
    public float minEnergy = 0f;
    public float maxEnergy = 80f;
    public float energyDecrement = 2.0f;
    public float maxRestingTime = 10.0f; //Cat resting time
    public float mouseDetectableRadius = 15.0f; //Radius to detect mouse
    public float mouseReachedRadius = 2.0f; //Radius when mouse reached
    public float homeReachedRadius = 2.0f;
    public float invasorDetectableRadius = 15.0f; //Invasor is within detectable radius
    public float invasorReachableRadius = 2.0f; //Invasor is reachable
    public float maxFightingTime = 10.0f;

    public GameObject home;
}
