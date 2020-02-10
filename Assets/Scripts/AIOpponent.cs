using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOpponent
{
    GameObject opponentInScene;
    private FiniteStateMachine<AIOpponent> _AIStateMachine;

    public AIOpponent(GameObject opponent)
    {
        opponentInScene = opponent;
        _AIStateMachine = new FiniteStateMachine<AIOpponent>(this);
        _AIStateMachine.TransitionTo<Moving>();
    }

    public void MoveTowardBall()
    {
        Vector3 directionTowardBall;
        directionTowardBall = ServicesLocator.ball.transform.position - opponentInScene.transform.position;
        directionTowardBall = directionTowardBall.normalized * _moveForce;
        opponentInScene.GetComponent<Rigidbody2D>().AddForce(directionTowardBall);
    }

    public void Update()
    {
        _AIStateMachine.Update();
    }

        
    

    public abstract class Moving : FiniteStateMachine<AIOpponent>.State
    {
        AIOpponent opponent;

        public override void Initialize()
        {
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void Update()
        {
            Context.MoveTowardBall();
        }

    }
}
