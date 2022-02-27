using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INVASOR_Blackboard : MonoBehaviour
{
    public float maxHidingTime = 10f;
    public float maxFightTime = 5f;
    public float catDetectableRadius = 150f;
    public float catReachedRadius = 10f;
    public float placeReachedRadius = 15f;
    public float minDistanceToHide = 100f;

    public GameObject moveTarget;
    public GameObject spawnPoint;
    public GameObject cat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
