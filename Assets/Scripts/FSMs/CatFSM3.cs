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
            FIGHTING
        }

        private CatFSM2 catFSM_2;
        private Pursue pursue;
        private CatBlackboard blackboard;
        public State currentState = State.INITIAL;

        void Start()
        {
            catFSM_2 = GetComponent<CatFSM2>();
            catFSM_2.enabled = false;

            blackboard = GetComponent<CatBlackboard>();

            pursue = GetComponent<Pursue>();
            pursue.enabled = false;
        }

        public override void Exit()
        {
            catFSM_2.enabled = false;
            pursue.enabled = false;
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
                    break;
                case State.FIGHTING:
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
                    catFSM_2.Exit();
                    break;
                case State.FIGHTING:
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
                case State.FIGHTING:
                    break;
                default:
                    break;
            }
        }
    }
}