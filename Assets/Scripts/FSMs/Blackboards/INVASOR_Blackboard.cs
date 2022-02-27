using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INVASOR_Blackboard : MonoBehaviour
{
    public float maxHidingTime = 10f;
    public float maxFightTime = 7f;
    public float catDetectableRadius = 3f;
    public float catReachedRadius = 1f;
    public float placeReachedRadius = 1f;
    public float minDistanceToHide = 10f;
    public Vector2 windowsBounds;

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
