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
            INITIAL, WANDERING, WANDER_CURSOR, GO_TO_TARGET
        };

        public State currentState = State.INITIAL;
        private FlockingAround flockingAround;
        public GameObject userControlledTarget;

        public GameObject antsWanderPoint;

        private AntsBlackboard blackboard;




        void Start()
        {
            flockingAround = GetComponent<FlockingAround>();

            blackboard = GetComponent<AntsBlackboard>();

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
                    ChangeState(State.WANDERING);
                    break;

                case State.WANDERING:
                    if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) <= blackboard.closeToFollowTargetDistance)
                    {
                        ChangeState(State.GO_TO_TARGET);
                        break;
                    }
                    break;
                case State.WANDER_CURSOR:
                    if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) > blackboard.closeToTargetDistance)
                    {
                        ChangeState(State.GO_TO_TARGET);
                        break;
                    }
                    break;

                case State.GO_TO_TARGET:
                    if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) > blackboard.closeToFollowTargetDistance)
                    {
                        ChangeState(State.WANDERING);
                        break;
                    }
                    if (SensingUtils.DistanceToTarget(gameObject, userControlledTarget) <= blackboard.closeToTargetDistance)
                    {
                        ChangeState(State.WANDER_CURSOR);
                        break;
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

                case State.WANDER_CURSOR:
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
                    flockingAround.seekWeight = blackboard.antsWanderingWeight;
                    flockingAround.attractor = antsWanderPoint;
                    flockingAround.enabled = true;
                    break;

                case State.WANDER_CURSOR:
                    flockingAround.seekWeight = blackboard.antsCursorWeight;
                    flockingAround.attractor = userControlledTarget;
                    flockingAround.enabled = true;
                    break;

                case State.GO_TO_TARGET:
                    flockingAround.seekWeight = blackboard.antsGoWeight;
                    flockingAround.attractor = userControlledTarget;
                    flockingAround.enabled = true;
                    break;

                default:
                    break;
            }
            currentState = newState;
        }
    }
}
