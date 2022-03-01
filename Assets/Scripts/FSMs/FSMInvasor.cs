using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class FSMInvasor : FiniteStateMachine
    {
        public enum State { INITIAL, HIDE, RUN_AWAY, MOVE, GO_TO_CAT, FIGHT };

        public State currentState = State.INITIAL;
        private KinematicState KS;
        private INVASOR_Blackboard blackboard;
        public Arrive arrive;
        public Flee flee;
        public float hidingTime;
        public float fightingTime;

        public bool visible;
        // Start is called before the first frame update
        void Start()
        {
            arrive = GetComponent<Arrive>();
            flee = GetComponent<Flee>();
            blackboard = GetComponent<INVASOR_Blackboard>();
            KS = GetComponent<KinematicState>();

            arrive.enabled = false;
            flee.enabled = false;
        }

        public override void Exit()
        {
            // stop any steering that may be enabled
            flee.enabled = false;
            arrive.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            base.ReEnter();
        }
        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    Respawn();
                    ChangeState(State.MOVE);
                    break;
                case State.HIDE:
                    if (hidingTime >= blackboard.maxHidingTime)
                    {
                        Respawn();
                        ChangeState(State.MOVE);
                    }
                    hidingTime += Time.deltaTime;
                    break;
                case State.RUN_AWAY:
                    gameObject.tag = "FLEEING_INVADER";
                    if (!visible)
                    {
                        ChangeState(State.HIDE);
                    }
                    break;
                case State.MOVE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.moveTarget) < blackboard.placeReachedRadius)
                    {
                        ChangeState(State.HIDE);
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.cat) < blackboard.catDetectableRadius)
                    {
                        ChangeState(State.GO_TO_CAT);
                    }
                    break;
                case State.GO_TO_CAT:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.cat) <= blackboard.catReachedRadius)
                    {
                        ChangeState(State.FIGHT);
                    }
                    break;
                case State.FIGHT:
                    if (fightingTime >= blackboard.maxFightTime)
                    {
                        ChangeState(State.RUN_AWAY);
                    }
                    fightingTime += Time.deltaTime;
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            switch (this.currentState)
            {
                case State.HIDE:

                    break;
                case State.RUN_AWAY:
                    flee.enabled = false;
                    flee.target = null;
                    break;
                case State.MOVE:
                    arrive.enabled = false;
                    arrive.target = null;
                    break;
                case State.GO_TO_CAT:
                    arrive.enabled = false;
                    arrive.target = null;
                    break;
                case State.FIGHT:
                    break;
            }

            switch (newState)
            {
                case State.HIDE:
                    hidingTime = 0f;
                    break;
                case State.RUN_AWAY:
                    flee.target = blackboard.cat;
                    flee.enabled = true;
                    break;
                case State.MOVE:
                    arrive.target = blackboard.moveTarget;
                    arrive.enabled = true;
                    break;
                case State.GO_TO_CAT:
                    arrive.target = blackboard.cat;
                    arrive.enabled = true;
                    break;
                case State.FIGHT:
                    fightingTime = 0f;
                    break;

            }
            currentState = newState;
        }

        private void Respawn()
        {
            KS.enabled = false;
            gameObject.transform.position = blackboard.spawnPoint.transform.position;
            KS.position = blackboard.spawnPoint.transform.position;
            KS.enabled = true;
            //gameObject.tag = "INVADER";
        }

        private void OnBecameVisible()
        {
            visible = true;
        }

        private void OnBecameInvisible()
        {
            visible = false;
        }

        /*private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			Gizmos.DrawWireSphere(gameObject.transform.position, blackboard.catDetectableRadius);

			Gizmos.color = Color.green;

			Gizmos.DrawWireSphere(gameObject.transform.position, blackboard.catReachedRadius);

			Gizmos.color = Color.blue;

			Gizmos.DrawWireSphere(gameObject.transform.position, blackboard.minDistanceToHide);

			Gizmos.color = Color.yellow;

			Gizmos.DrawWireSphere(gameObject.transform.position, blackboard.placeReachedRadius);
		
		}*/
    }
}
