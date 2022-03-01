using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class MiceFSM : FiniteStateMachine
    {
        public enum State { INITIAL, WANDER_HOME, REACH_CHEESE, EAT, REACH_HOME };

        public State m_currentState = State.INITIAL;

        private MiceBlackboard m_blackboard;

        [SerializeField]
        private GameObject m_home;

        private GameObject m_cheese;
        private Arrive m_arrive;
        private WanderAroundPlusAvoid m_wander;
        private float m_elapsedTime;

        void Start()
        {
            m_arrive = GetComponent<Arrive>();
            m_wander = GetComponent<WanderAroundPlusAvoid>();
            m_blackboard = GetComponent<MiceBlackboard>();

            m_arrive.enabled = false;
            m_wander.enabled = false;
        }

        public override void Exit()
        {
            m_wander.enabled = false;
            m_arrive.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            m_currentState = State.INITIAL;
            base.ReEnter();
        }

        void Update()
        {
            switch (m_currentState)
            {
                case State.INITIAL:
                    ChangeState(State.WANDER_HOME);
                    break;
                case State.WANDER_HOME:
                    m_cheese = SensingUtils.FindInstanceWithinRadius(gameObject, "CHEESE", m_blackboard.m_cheeseDetectionRadious);
                    if (m_cheese != null)
                    {
                        ChangeState(State.REACH_CHEESE);
                        break;
                    }
                    break;
                case State.REACH_CHEESE:
                    if (SensingUtils.DistanceToTarget(gameObject, m_cheese) <= m_blackboard.m_minDistanceToEat)
                    {
                        ChangeState(State.EAT);
                        break;
                    }
                    break;
                case State.EAT:
                    m_elapsedTime++;
                    if (m_elapsedTime > m_blackboard.m_eatTimeout)
                    {
                        ChangeState(State.REACH_HOME);
                        break;
                    }
                    break;
                case State.REACH_HOME:
                    if (SensingUtils.DistanceToTarget(gameObject, m_home) <= m_blackboard.m_minDistanceToSafety)
                    {
                        ChangeState(State.WANDER_HOME);
                        break;
                    }
                    break;
            }
        }

        private void ChangeState(State l_newState)
        {
            //EXIT logic
            switch (m_currentState)
            {
                case State.WANDER_HOME:
                    m_wander.enabled = false;
                    break;
                case State.REACH_CHEESE:
                    m_arrive.enabled = false;
                    break;
                case State.EAT:
                    m_cheese.SetActive(false);
                    break;
                case State.REACH_HOME:
                    m_arrive.enabled = false;
                    break;
            }

            //ENTER logic
            switch (l_newState)
            {
                case State.WANDER_HOME:
                    gameObject.tag = "MOUSE";
                    m_wander.attractor = m_home;
                    m_wander.enabled = true;
                    break;
                case State.REACH_CHEESE:
                    m_arrive.target = m_cheese;
                    m_arrive.enabled = true;
                    break;
                case State.EAT:
                    m_elapsedTime = 0;
                    break;
                case State.REACH_HOME:
                    m_arrive.target = m_home;
                    m_arrive.enabled = true;
                    break;
            }

            m_currentState = l_newState;
        }
    }
}
