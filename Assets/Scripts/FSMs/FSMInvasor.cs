using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

[RequireComponent(typeof(INVASOR_Blackboard))]
[RequireComponent(typeof(Arrive))]
[RequireComponent(typeof(Flee))]
public class FSMInvasor : FiniteStateMachine
{
    public enum State {INITIAL, HIDE, RUN_AWAY, MOVE, FIGHT};

    public State currentState = State.INITIAL;

    private INVASOR_Blackboard blackboard;
    
    private Arrive arrive;
	private Flee flee;
    private float hidingTime;
	private float fightingTime;
    // Start is called before the first frame update
    void Start()
    {
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<INVASOR_Blackboard>();

        arrive.enabled = false;
    }

    public override void Exit()
    {
        // stop any steering that may be enabled
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
        switch(currentState)
        {
			case State.INITIAL:
				gameObject.transform.position = blackboard.spawnPoint.transform.position;
				ChangeState(State.MOVE);
				break;
			case State.HIDE:
				if(hidingTime >= blackboard.maxHidingTime)
                {
					ChangeState(State.MOVE);
                }
				hidingTime += Time.deltaTime;
				break;
			case State.RUN_AWAY:
				if(SensingUtils.DistanceToTarget(gameObject, blackboard.cat) > blackboard.minDistanceToHide)
                {
					gameObject.transform.position = blackboard.spawnPoint.transform.position;
					ChangeState(State.HIDE);
				}
				break;
			case State.MOVE:
				if(SensingUtils.DistanceToTarget(gameObject, blackboard.moveTarget) > blackboard.placeReachedRadius)
                {
					gameObject.transform.position = blackboard.spawnPoint.transform.position;
					ChangeState(State.HIDE);
				}

				if(SensingUtils.DistanceToTarget(gameObject, blackboard.cat) < blackboard.catDetectableRadius)
                {
					ChangeState(State.FIGHT);
                }
				break;
			case State.FIGHT:
				if(fightingTime >= blackboard.maxFightTime)
                {
					ChangeState(State.RUN_AWAY);
                }
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
			case State.FIGHT:
				arrive.enabled = false;
				arrive.target = null;
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
			case State.FIGHT:
				arrive.target = blackboard.cat;
				arrive.enabled = true;
				fightingTime = 0f;
				break;			

		} 
		currentState = newState;
	}
}
