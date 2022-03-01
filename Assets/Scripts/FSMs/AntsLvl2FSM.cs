using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
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
        private float closeToCheeseSpawnerDistance;
        private AntsFSM antFSM;

        void Start()
        {
            flockingAround = GetComponent<FlockingAround>();
            closeToCheeseSpawnerDistance = 2.0f;
            antFSM = GetComponent<AntsFSM>();
            currentState = State.LVL1FSM;
            transportingCheese = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void ReEnter()
        {
            base.ReEnter();
        }

        void Update()
        {
            switch (currentState)
            {
                case State.GO_TO_CHEESE:
                    if (SensingUtils.DistanceToTarget(gameObject, cheeseSpawner.gameObject) <= closeToCheeseSpawnerDistance)
                    {
                        cheese.SetActive(true);
                        transportingCheese = true;
                    }
                    if (transportingCheese)
                    {
                        ChangeState(State.LVL1FSM);
                        break;
                    }
                    break;

                case State.LVL1FSM:
                    if (cheese.activeSelf == false)
                    {
                        ChangeState(State.GO_TO_CHEESE);
                        break;
                    }
                    break;

                default:
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            //EXIT logic
            switch (currentState)
            {
                case State.GO_TO_CHEESE:
                    flockingAround.enabled = false;
                    break;

                case State.LVL1FSM:
                    transportingCheese = false;
                    antFSM.Exit();
                    break;

                default:
                    break;
            }

            //ENTER logic
            switch (newState)
            {
                case State.GO_TO_CHEESE:
                    flockingAround.attractor = cheeseSpawner.gameObject;
                    flockingAround.seekWeight = 1.0f;
                    flockingAround.enabled = true;
                    break;

                case State.LVL1FSM:
                    antFSM.ReEnter();
                    break;

                default:
                    break;
            }
            currentState = newState;
        }
    }
}
