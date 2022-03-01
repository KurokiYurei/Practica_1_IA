using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class MiceFSM2 : FiniteStateMachine
    {
        public enum State { INITIAL, NORMAL, FLEE, TRAPPED, RESPAWN };

        public State m_currentState = State.INITIAL;

        private MiceBlackboard m_blackboard;

        [SerializeField]
        private GameObject m_cat;

        public GameObject m_button;
        public GameObject m_door;

        private FleePlusAvoid m_flee;
        private MiceFSM m_miceFSM;

        private Vector3 m_startPosition;
        public float m_currentTimeToDie;


        void Start()
        {
            m_flee = GetComponent<FleePlusAvoid>();
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
                        break;
                    }
                    break;
                case State.FLEE:
                    if (SensingUtils.DistanceToTarget(gameObject, m_cat) > m_blackboard.m_minDistanceToDanger)
                    {
                        ChangeState(State.NORMAL);
                        break;
                    }
                    if (gameObject.tag != "MOUSE")
                    {
                        ChangeState(State.TRAPPED);
                        break;
                    }
                    break;
                case State.TRAPPED:
                    if (m_currentTimeToDie >= m_blackboard.m_timeToDie)
                    {
                        ChangeState(State.RESPAWN);
                        break;
                    }
                    m_currentTimeToDie += Time.deltaTime;
                    break;
                case State.RESPAWN:
                    gameObject.transform.position = m_startPosition;
                    gameObject.GetComponent<KinematicState>().position = m_startPosition;
                    ChangeState(State.NORMAL);
                    break;
            }
        }

        private void ChangeState(State l_newState)
        {
            //EXIT logic
            switch (m_currentState)
            {
                case State.NORMAL:
                    m_miceFSM.Exit();
                    break;
                case State.FLEE:
                    m_flee.enabled = false;
                    m_miceFSM.enabled = true;
                    break;
                case State.TRAPPED:
                    break;
                case State.RESPAWN:
                    break;
            }

            //ENTER logic
            switch (l_newState)
            {
                case State.NORMAL:
                    m_miceFSM.ReEnter();
                    break;
                case State.FLEE:
                    m_flee.target = m_cat;
                    m_flee.enabled = true;
                    break;
                case State.TRAPPED:
                    m_miceFSM.enabled = false;
                    m_currentTimeToDie = 0;
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
                DoorButtonActivator l_buttonInteract = m_button.GetComponent<DoorButtonActivator>();
                l_buttonInteract.openDoor = true;
            }
        }

        private void CheckDoor()
        {
            m_door = SensingUtils.FindInstanceWithinRadius(gameObject, "DOOR_OPENED", m_blackboard.m_minDistanceToInteract);
            if (m_door != null)
            {
                HUDController.HUDinstance.AddSaved();
                Respawn();
            }
        }

        public void Respawn()
        {
            gameObject.GetComponent<KinematicState>().position = m_startPosition;
        }
    }
}
