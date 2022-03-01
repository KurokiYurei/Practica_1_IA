using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class CatFSM3 : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,
            NORMAL,
            REACHING_INVADER,
            FIGHTING
        }

        private CatFSM catFSM_1;
        private CatFSM2 catFSM_2;
        private Arrive arrive;
        private KeepPosition keepPosition;
        private CatBlackboard blackboard;
        public State currentState = State.INITIAL;

        public float currenFightingTime;

        private GameObject invader;

        void Start()
        {
            catFSM_1 = GetComponent<CatFSM>();
            catFSM_2 = GetComponent<CatFSM2>();
            keepPosition = GetComponent<KeepPosition>();
            arrive = GetComponent<Arrive>();


            blackboard = GetComponent<CatBlackboard>();

            catFSM_1.enabled = false;
            catFSM_2.enabled = false;
            keepPosition.enabled = false;
            arrive.enabled = false;
        }

        public override void Exit()
        {
            catFSM_1.enabled = false;
            catFSM_2.enabled = false;
            keepPosition.enabled = false;
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
                    invader = SensingUtils.FindInstanceWithinRadius(gameObject, "INVADER", blackboard.invasorDetectableRadius);
                    if (invader != null)
                    {
                        print("ESTO, si, detection and that, it's not like I want to detect you");
                        ChangeState(State.REACHING_INVADER);
                        break;
                    }
                    break;
                case State.REACHING_INVADER:
                    if (SensingUtils.DistanceToTarget(gameObject, invader) <= blackboard.invasorReachableRadius)
                    {
                        ChangeState(State.FIGHTING);
                        break;
                    }
                    break;
                case State.FIGHTING:

                    if (currenFightingTime >= blackboard.maxFightingTime)
                    {
                        ChangeState(State.NORMAL);
                    }
                    keepPosition.requiredAngle += Time.deltaTime * blackboard.fightAngleIncrement;
                    currenFightingTime += Time.deltaTime;
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
                case State.NORMAL:
                    catFSM_1.Exit();
                    catFSM_2.Exit();
                    break;
                case State.REACHING_INVADER:
                    arrive.enabled = false;
                    break;
                case State.FIGHTING:
                    //keepPosition.target = null;
                    keepPosition.enabled = false;

                    break;
                default:
                    break;
            }

            //ENTER logic
            switch (newState)
            {
                case State.NORMAL:
                    catFSM_2.ReEnter();
                    break;
                case State.REACHING_INVADER:
                    arrive.target = invader;
                    arrive.enabled = true;
                    break;
                case State.FIGHTING:
                    currenFightingTime = 0;
                    keepPosition.requiredAngle = 0;
                    keepPosition.target = invader;
                    keepPosition.enabled = true;
                    break;
                default:
                    break;
            }

            currentState = newState;
        }
    }
}