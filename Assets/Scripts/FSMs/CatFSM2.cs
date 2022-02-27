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
            CAT_FSM_1,
            SEEK_FOOD,
            FEED,
            REST
        }

        public State currentState = State.INITIAL;

        private CatFSM catFSM_1;
        private Arrive arrive;

        private CatBlackboard blackboard;
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
                case State.INITIAL:
                    ChangeState(State.CAT_FSM_1);
                    break;
                case State.CAT_FSM_1:
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
            //exit logic
            switch (currentState)
            {
                case State.INITIAL:
                    break;
                case State.CAT_FSM_1:
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

            //enter logic
            switch (newState)
            {
                case State.INITIAL:
                    break;
                case State.CAT_FSM_1:
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
    }
}

