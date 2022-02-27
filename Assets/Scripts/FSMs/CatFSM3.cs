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
            CAT_FSM_2,
            FIGHTING
        }

        public State currentState = State.INITIAL;
        void Start()
        {

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
                    break;
                case State.CAT_FSM_2:
                    break;
                case State.FIGHTING:
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
                case State.CAT_FSM_2:
                    break;
                case State.FIGHTING:
                    break;
                default:
                    break;
            }

            switch (newState)
            {
                case State.INITIAL:
                    break;
                case State.CAT_FSM_2:
                    break;
                case State.FIGHTING:
                    break;
                default:
                    break;
            }
        }
    }
}