using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

public class AntsLvl2FSM : FiniteStateMachine
{
    public enum State
    {
        GO_TO_CHEESE, LVL1FSM
    };

    public State currentState = State.GO_TO_CHEESE;
    private FlockingAround flockingAround;
    public GameObject cheese;
    public Transform cheeseSpawner;
    private bool transportingCheese;
    private float closeToCheeseDistance;
    void Start()
    {
        flockingAround = GetComponent<FlockingAround>();
        closeToCheeseDistance = 2.0f;
    }

    public override void Exit()
    {
        flockingAround.enabled = false;
        base.Exit();
    }

    public override void ReEnter()
    {
        transportingCheese = false;
        currentState = State.GO_TO_CHEESE;
        base.ReEnter();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.GO_TO_CHEESE:
                if(SensingUtils.DistanceToTarget(gameObject, cheeseSpawner.gameObject) <= closeToCheeseDistance)
                {
                    cheese.SetActive(true);
                    transportingCheese = true;
                }
                if(transportingCheese)
                {
                    ChangeState(State.LVL1FSM);
                }
                break;

            case State.LVL1FSM:
                
                break;

            default:
                break;
        }
    }

    private void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.GO_TO_CHEESE:
                flockingAround.enabled = false;
                break;

            case State.LVL1FSM:

                break;

            default:
                break;
        }

        switch (newState)
        {
            case State.GO_TO_CHEESE:
                flockingAround.attractor = cheeseSpawner.gameObject;
                flockingAround.seekWeight = 1.0f;
                flockingAround.enabled = true;
                break;

            case State.LVL1FSM:
                Exit();
                break;

            default:
                break;
        }
    }
}