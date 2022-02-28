using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class AntsFSM : FiniteStateMachine
    {
        public enum State
        {
            INITIAL, WANDERING, GO_TO_TARGET
        };

        public State currentState = State.INITIAL;
        private FlockingAround flockingAround;
        public GameObject userControlledTarget;
        private float closeToFollowTargetDistance;
        private float closeToTargetDistance;


        void Start()
        {
            flockingAround = GetComponent<FlockingAround>();

            closeToTargetDistance = 10.0f;
            closeToFollowTargetDistance = 30.0f;
            currentState = State.INITIAL;
        }

        public override void Exit()
        {
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
            switch (currentState)
            {
                case State.INITIAL:
                    flockingAround.seekWeight = 0.3f;
                    ChangeState(State.WANDERING);
                    break;

                case State.WANDERING:
                    if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) <= closeToFollowTargetDistance)
                    {
                        ChangeState(State.GO_TO_TARGET);
                    }
                    break;

                case State.GO_TO_TARGET:
                    if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) > closeToFollowTargetDistance)
                    {
                        flockingAround.seekWeight = 0.15f;
                        ChangeState(State.WANDERING);
                    }
                    if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) <= closeToTargetDistance)
                    {
                        flockingAround.seekWeight = 0.7f;
                        ChangeState(State.WANDERING);
                    }
                    break;

                default:
                    break;
            }
            Debug.Log("Lvl 1 is: " + currentState);
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
                    flockingAround.enabled = false;
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
                    flockingAround.seekWeight = 1.0f;
                    flockingAround.attractor = userControlledTarget;
                    flockingAround.enabled = true;
                    break;

                default:
                    break;
            }
        }
    }
}
