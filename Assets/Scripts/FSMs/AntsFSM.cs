using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

public class AntFSM : FiniteStateMachine
{
    public enum State
    {
        INITIAL, WANDERING, GO_TO_TARGET
    };

    public State currentState = State.INITIAL;
    private FlockingAround flockingAround;
    private Arrive arrive;
    public GameObject userControlledTarget;
    private float closeToFollowTargetDistance;
    private float closeToTargetDistance;


    void Start()
    {
        flockingAround = GetComponent<FlockingAround>();
        arrive = GetComponent<Arrive>();

        closeToTargetDistance = 10.0f;
        closeToFollowTargetDistance = 30.0f;
    }

    public override void Exit()
    {
        arrive.enabled = false;
        flockingAround.enabled = false;
        base.Exit();
    }

    public override void ReEnter()
    {
        currentState = State.INITIAL;
        base.ReEnter();
    }

    void Update()
    {
        switch(currentState)
        {
            case State.INITIAL:
                flockingAround.seekWeight = 0.3f;
                ChangeState(State.WANDERING);
                break;

            case State.WANDERING:
                if(SensingUtils.DistanceToTarget(gameObject, userControlledTarget) <= closeToFollowTargetDistance)
                {
                    ChangeState(State.GO_TO_TARGET);
                }
                break;

            case State.GO_TO_TARGET:
                if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) > closeToFollowTargetDistance)
                {
                    flockingAround.seekWeight = 0.7f;
                    ChangeState(State.WANDERING);
                }
                if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) <= closeToTargetDistance)
                {
                    flockingAround.seekWeight = 0.3f;
                    ChangeState(State.WANDERING);
                }
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

            case State.WANDERING:
                flockingAround.enabled = false;
                break;

            case State.GO_TO_TARGET:
                arrive.enabled = false;
                break;

            default:
                break;
        }

        switch (newState)
        {
            case State.INITIAL:
                break;

            case State.WANDERING:
                flockingAround.attractor = userControlledTarget;
                flockingAround.enabled = true;
                break;

            case State.GO_TO_TARGET:
                arrive.target = userControlledTarget;
                arrive.enabled = true;
                break;

            default:
                break;
        }
    }
}
