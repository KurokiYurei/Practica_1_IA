using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class MiceFSM2 : FiniteStateMachine
    {
        public enum State { INITIAL, NORMAL, FLEE };

        public State m_currentState = State.INITIAL;

        private MiceBlackboard m_blackboard;

        [SerializeField]
        private GameObject m_cat;

        public DoorButtonActivator m_button;
        public GameObject m_door;

        private Flee m_flee;
        private MiceFSM m_miceFSM;

        private Vector2 m_startPosition;
        private Quaternion m_startRotation;


        void Start()
        {
            m_flee = GetComponent<Flee>();
            m_miceFSM = GetComponent<MiceFSM>();
            m_blackboard = GetComponent<MiceBlackboard>();

            m_flee.enabled = false;

            m_startPosition = gameObject.transform.position;
            m_startRotation = gameObject.transform.rotation;
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
            }

            m_currentState = l_newState;
        }

        private void CheckButton()
        {
            Debug.Log("PBUTTON");
            if (SensingUtils.DistanceToTarget(gameObject, m_button.gameObject) <= m_blackboard.m_minDistanceToInteract)
            {
                Debug.Log("BUTTON");
                m_button.openDoor = true;
            }
        }

        private void CheckDoor()
        {
            Debug.Log("PDOOR");
            if (SensingUtils.DistanceToTarget(gameObject, m_door) <= m_blackboard.m_minDistanceToInteract)
            {
                Debug.Log("DOOR");
                Respawn();  
            }
        }

        public void Respawn()
        {
            gameObject.transform.position = m_startPosition;
            gameObject.transform.rotation = m_startRotation;
        }
    }
}
