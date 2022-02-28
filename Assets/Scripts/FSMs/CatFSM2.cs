using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class CatFSM2 : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,
            NORMAL,
            REACHING_FOOD,
            FEED,
            REACHING_HOME,
            REST
        }

        public State currentState = State.INITIAL;

        private CatFSM catFSM_1;
        private Arrive arrive;

        private CatBlackboard blackboard;

        private float currentFeedingTime;
        private float currentRestingTime;
        private GameObject food;


        void Start()
        {
            catFSM_1 = GetComponent<CatFSM>();
            arrive = GetComponent<Arrive>();

            blackboard = GetComponent<CatBlackboard>();

            catFSM_1.enabled = false;
            arrive.enabled = false;
        }

        public override void Exit()
        {
            catFSM_1.enabled = false;
            arrive.enabled = false;
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
                    ChangeState(State.NORMAL);
                    break;
                case State.NORMAL:
                    if (blackboard.hunger <= blackboard.minHunger)
                    {
                        food = SensingUtils.FindInstanceWithinRadius(gameObject, "FOOD", blackboard.foodDetectableRadius);
                        if (food != null)
                        {
                            ChangeState(State.REACHING_FOOD);
                            break;
                        }
                    }
                    if(blackboard.energy <= blackboard.minEnergy)
                    {
                        ChangeState(State.REACHING_HOME);
                        break;
                    }
                    blackboard.hunger -= blackboard.hungerDecrement * Time.deltaTime;
                    blackboard.energy -= blackboard.energyDecrement * Time.deltaTime;
                    break;
                case State.REACHING_FOOD:
                    if(SensingUtils.DistanceToTarget(gameObject, food) <= blackboard.foodReachableRadius)
                    {
                        ChangeState(State.FEED);
                        break;
                    }
                    break;
                case State.FEED:
                    if (currentFeedingTime >= blackboard.maxEatingTime)
                    {
                        ChangeState(State.NORMAL);
                        break;
                    }
                    currentFeedingTime += Time.deltaTime;
                    break;
                case State.REACHING_HOME:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.home) <= blackboard.homeReachedRadius)
                    {
                        ChangeState(State.REST);
                        break;
                    }
                    break;
                case State.REST:
                    if (currentRestingTime >= blackboard.maxRestingTime)
                    {
                        ChangeState(State.NORMAL);
                        break;
                    }
                    currentRestingTime += Time.deltaTime;
                    break;
                default:
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            //exit logic
            switch (currentState)
            {
                case State.NORMAL:
                    catFSM_1.Exit();
                    break;
                case State.REACHING_FOOD:
                    arrive.enabled = false;
                    break;
                case State.FEED:
                    blackboard.hunger = blackboard.maxHunger;
                    break;
                case State.REACHING_HOME:
                    arrive.enabled = false;
                    break;
                case State.REST:
                    blackboard.energy = blackboard.maxEnergy;
                    break;
                default:
                    break;
            }

            //enter logic
            switch (newState)
            {
                case State.NORMAL:
                    catFSM_1.ReEnter();
                    break;
                case State.REACHING_FOOD:
                    arrive.target = food;
                    arrive.enabled = true;
                    break;
                case State.FEED:
                    currentFeedingTime = 0;
                    break;
                case State.REACHING_HOME:
                    arrive.target = blackboard.home;
                    arrive.enabled = true;
                    break;
                case State.REST:
                    currentRestingTime = 0;
                    break;
                default:
                    break;
            }
            currentState = newState;
        }
    }
}

