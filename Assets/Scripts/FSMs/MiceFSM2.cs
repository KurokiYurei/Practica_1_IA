using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class MiceFSM2 : FiniteStateMachine
    {
        public enum State { INITIAL, NORMAL, FLEE, RESPAWN };

        public State m_currentState = State.INITIAL;

        private MiceBlackboard m_blackboard;

        [SerializeField]
        private GameObject m_cat;

        public GameObject m_button;
        public GameObject m_door;

        private Flee m_flee;
        private MiceFSM m_miceFSM;

        private Vector3 m_startPosition;
        private float m_startRotation;


        void Start()
        {
            m_flee = GetComponent<Flee>();
            m_miceFSM = GetComponent<MiceFSM>();
            m_blackboard = GetComponent<MiceBlackboard>();

            m_flee.enabled = false;

            m_startPosition = gameObject.transform.position;
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
            CheckButton();
            CheckDoor();

            switch (m_currentState)
            {
                case State.INITIAL:
                    ChangeState(State.NORMAL);
                    break;
                case State.NORMAL:
                    m_cat = SensingUtils.FindInstanceWithinRadius(gameObject, "CAT", m_blackboard.m_minDistanceToDanger);
                    if (m_cat != null)
                    {
                        ChangeState(State.FLEE);
                    }
                    break;
                case State.FLEE:
                    if(SensingUtils.DistanceToTarget(gameObject, m_cat)> m_blackboard.m_minDistanceToDanger)
                    {
                        ChangeState(State.NORMAL);
                    }
                    break;
                case State.RESPAWN:
                    gameObject.GetComponent<KinematicState>().position = m_startPosition;

                    ChangeState(State.NORMAL);
                    break;
            }
        }

        private void ChangeState(State l_newState)
        {
            switch (m_currentState)
            {
                case State.NORMAL:
                    m_miceFSM.Exit();
                    break;
                case State.FLEE:
                    m_flee.enabled = false;
                    m_miceFSM.enabled = true;
                    break;
                case State.RESPAWN:

                    break;
            }

            switch (l_newState)
            {
                case State.NORMAL:
                    m_miceFSM.ReEnter();
                    break;
                case State.FLEE:
                    m_flee.target = m_cat;
                    m_flee.enabled = true;
                    break;
                case State.RESPAWN:
                    m_miceFSM.enabled = false;
                    break;
            }

            m_currentState = l_newState;
        }

        private void CheckButton()
        {
            m_button = SensingUtils.FindInstanceWithinRadius(gameObject, "BUTTON", m_blackboard.m_minDistanceToInteract);
            if (m_button != null)
            {
                Debug.Log("BUTTON");
                DoorButtonActivator l_buttonInteract = m_button.GetComponent<DoorButtonActivator>();
                l_buttonInteract.openDoor = true;
            }
        }

        private void CheckDoor()
        {
            m_door = SensingUtils.FindInstanceWithinRadius(gameObject, "DOOR", m_blackboard.m_minDistanceToInteract);
            if (m_door != null)
            {
                Debug.Log("DOOR");
                Respawn();
            }
        }

        public void Respawn()
        {
            Debug.Log("RESPAWN");
            ChangeState(State.RESPAWN);
        }
    }
}
