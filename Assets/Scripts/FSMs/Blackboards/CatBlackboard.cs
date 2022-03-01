using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBlackboard : MonoBehaviour
{
    [Header("Eating Behaviour")]
    public float foodDetectableRadius = 25.0f;//Food detectable
    public float foodReachableRadius = 2.0f;//Food reachable
    public float hunger = 80f;
    public float minHunger = 0f; //The hunger of the cat
    public float maxHunger = 80f;
    public float maxEatingTime = 5.0f; //Time eating
    public float hungerDecrement = 2.0f;

    [Header("Resting Behaviour")]
    public float energy = 80f; //Current cat energy
    public float minEnergy = 0f;
    public float maxEnergy = 80f;
    public float energyDecrement = 2.0f;
    public float maxRestingTime = 10.0f; //Cat resting time
    public float homeReachedRadius = 2.0f;
    public GameObject home; //Home of the cat

    [Header("Hunting Behaviour")]
    public float mouseDetectableRadius = 15.0f; //Radius to detect mouse
    public float mouseReachedRadius = 2.0f; //Radius when mouse reached
    public float maxPursuingTime = 5.0f; //Maximum amount of time pursuing
    public float maxKillingTime = 5.0f;

    [Header("Fighting Behaviour")]
    public float invasorDetectableRadius = 15.0f; //Invasor is within detectable radius
    public float invasorReachableRadius = 2.0f; //Invasor is reachable
    public float maxFightingTime = 10.0f;
    public float fightAngleIncrement = 3.0f;
}
