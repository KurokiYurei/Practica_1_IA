using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class CatFSM : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,
            WANDER,
            SEEK_MOUSE,
            KILL_MOUSE
        };

        public State currentState = State.INITIAL;

        private CatBlackboard blackboard;

        public GameObject mouse;


        //Steerings
        private WanderAroundPlusAvoid wander;
        private Arrive arrive;
        private Pursue pursue;

        private float elapsedTime;
        public float pursueTime;
        private float currentKillingTime;


        void Start()
        {
            //Get the necessary steerings
            wander = GetComponent<WanderAroundPlusAvoid>();
            arrive = GetComponent<Arrive>();
            pursue = GetComponent<Pursue>();

            //Get the blackboard
            blackboard = GetComponent<CatBlackboard>();

            wander.enabled = false;
            arrive.enabled = false;
            pursue.enabled = false;

        }
        public override void Exit()
        {
            wander.enabled = false;
            arrive.enabled = false;
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
                    ChangeState(State.WANDER);
                    break;

                case State.WANDER:
                    mouse = SensingUtils.FindInstanceWithinRadius(gameObject, "MOUSE", blackboard.mouseDetectableRadius);
                    if (mouse != null) //If mouse close enough
                    {
                        ChangeState(State.SEEK_MOUSE);
                        break;
                    }
                    break;

                case State.SEEK_MOUSE:
                    GameObject otherMouse = SensingUtils.FindInstanceWithinRadius(gameObject, "MOUSE", blackboard.mouseDetectableRadius);
                    if (otherMouse != null && otherMouse != mouse && SensingUtils.DistanceToTarget(gameObject, mouse) < SensingUtils.DistanceToTarget(gameObject, mouse))
                    {
                        mouse = otherMouse;
                        ChangeState(State.SEEK_MOUSE);
                        break;
                    }
                    if (SensingUtils.DistanceToTarget(gameObject, mouse) <= blackboard.mouseReachedRadius) //Mouse reached
                    {
                        mouse.tag = "MOUSE_CAUGHT";
                        ChangeState(State.KILL_MOUSE);
                        break;
                    }
                    if (pursueTime >= blackboard.maxPursuingTime)
                    {
                        ChangeState(State.WANDER);
                        break;
                    }
                    pursueTime += Time.deltaTime;
                    break;

                case State.KILL_MOUSE:
                    if (currentKillingTime >= blackboard.maxKillingTime)
                    {                
                        ChangeState(State.WANDER);
                        break;
                    }
                    currentKillingTime += Time.deltaTime;
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
                case State.WANDER:
                    wander.enabled = false;
                    break;
                case State.SEEK_MOUSE:
                    pursue.enabled = false;
                    pursue.target = null;
                    break;
                case State.KILL_MOUSE:
                    break;
                default:
                    break;
            }

            //ENTER logic
            switch (newState)
            {
                case State.WANDER:
                    wander.attractor = blackboard.home;
                    wander.enabled = true;
                    break;
                case State.SEEK_MOUSE:
                    pursue.target = mouse;
                    pursue.enabled = true;
                    pursueTime = 0f;
                    break;
                case State.KILL_MOUSE:
                    currentKillingTime = 0;
                    break;
                default:
                    break;
            }
            currentState = newState;
        }
    }
}