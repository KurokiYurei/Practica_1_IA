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

        public AudioSource fightAudio;

        public bool visible;
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
                        break;
                    }
                    hidingTime += Time.deltaTime;
                    break;
                case State.RUN_AWAY:
                    gameObject.tag = "FLEEING_INVADER";
                    if (!visible)
                    {
                        ChangeState(State.HIDE);
                        break;
                    }
                    break;
                case State.MOVE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.moveTarget) < blackboard.placeReachedRadius)
                    {
                        ChangeState(State.HIDE);
                        break;
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.cat) < blackboard.catDetectableRadius)
                    {
                        ChangeState(State.GO_TO_CAT);
                        break;
                    }
                    break;
                case State.GO_TO_CAT:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.cat) <= blackboard.catReachedRadius)
                    {
                        ChangeState(State.FIGHT);
                        break;
                    }
                    break;
                case State.FIGHT:
                    if (fightingTime >= blackboard.maxFightTime)
                    {

                        ChangeState(State.RUN_AWAY);
                        break;
                    }
                    fightingTime += Time.deltaTime;
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            //EXIT logic
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

            //ENTER logic
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
                    fightAudio.Play();
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
        }

        private void OnBecameVisible()
        {
            visible = true;
            gameObject.tag = "INVADER";
        }

        private void OnBecameInvisible()
        {
            visible = false;
            gameObject.tag = "FLEEING_INVADER";
        }
    }
}
