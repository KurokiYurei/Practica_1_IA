using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

public class CatFSM : FiniteStateMachine
{
    public enum State
    {
        INITIAL,
        WANDER,
        SEEK_FOOD,
        FEED,
        REST
    };

    public State currentState = State.INITIAL;

    private CatBlackboard blackboard;

    private WanderAround wander;

    private float elapsedTime;

    void Start()
    {
        //Get the necessary steerings
        wander = GetComponent<WanderAround>();

        //Get the blackboard
        blackboard = GetComponent<CatBlackboard>();

        wander.enabled = false;

    }
    public override void Exit()
    {
        wander.enabled = false;
        base.Exit();
    }

    public override void ReEnter()
    {
        currentState = State.INITIAL;
        base.ReEnter();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:
                break;
            case State.WANDER:
                break;
            case State.SEEK_FOOD:
                break;
            case State.FEED:
                break;
            case State.REST:
                break;
            default:
                break;
        }
    }

    private void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.INITIAL:
                break;
            case State.WANDER:
                break;
            case State.SEEK_FOOD:
                break;
            case State.FEED:
                break;
            case State.REST:
                break;
            default:
                break;
        }

        switch (newState)
        {
            case State.INITIAL:
                break;
            case State.WANDER:
                break;
            case State.SEEK_FOOD:
                break;
            case State.FEED:
                break;
            case State.REST:
                break;
            default:
                break;
        }
        currentState = newState;
    }
}
