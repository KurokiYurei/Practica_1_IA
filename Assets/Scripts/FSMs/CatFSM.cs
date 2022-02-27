using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
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

        public GameObject food; //the food for the cat, a sardine this time
        private WanderAround wander;
        private Arrive arrive;

        private float elapsedTime;

        void Start()
        {
            //Get the necessary steerings
            wander = GetComponent<WanderAround>();
            arrive = GetComponent<Arrive>();

            //Get the blackboard
            blackboard = GetComponent<CatBlackboard>();

            //food = GameObject.Find("sardine_2").transform.position;

            wander.enabled = false;

        }
        public override void Exit()
        {
            wander.enabled = false;
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
                    ChangeState(State.WANDER);
                    break;

                case State.WANDER:

                    if (true) //Food detected
                    {
                        ChangeState(State.SEEK_FOOD);
                    }
                    break;

                case State.SEEK_FOOD:
                    food = SensingUtils.FindInstanceWithinRadius(gameObject, "FOOD", blackboard.foodReachableRdius);
                    if (food != null) //Food reached
                    {
                        ChangeState(State.FEED);
                    }
                    break;

                case State.FEED:
                    if (elapsedTime >= blackboard.maxEatingTime)
                    {
                        ChangeState(State.WANDER);
                    }
                    elapsedTime += Time.deltaTime;
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
                case State.WANDER:
                    wander.enabled = false;
                    break;
                case State.SEEK_FOOD:
                    arrive.enabled = false;
                    break;
                case State.FEED:
                    blackboard.hunger -= blackboard.foodHungerDecrement;
                    break;
                case State.REST:
                    break;
                default:
                    break;
            }

            switch (newState)
            {
                case State.WANDER:
                    wander.enabled = true;
                    break;
                case State.SEEK_FOOD:
                    arrive.target = food;
                    arrive.enabled = true;
                    break;
                case State.FEED:
                    elapsedTime = 0;
                    break;
                case State.REST:
                    break;
                default:
                    break;
            }
            currentState = newState;
        }
    }
}
