using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System;

public class AIOpponent
{
    public GameObject opponentInScene;
    private FiniteStateMachine<AIOpponent> _AIStateMachine;
    private Tree<AIOpponent> _aiTree;
    private enum BehaviorType
    {
        Aggressive,
        Goalie,
        Blocker
    }

    public AIOpponent(GameObject opponent, int behavior)
    {
        opponentInScene = opponent;
        _AIStateMachine = new FiniteStateMachine<AIOpponent>(this);
        _AIStateMachine.TransitionTo<Moving>();
        if (behavior == (int) BehaviorType.Aggressive)
        {
            var aggressiveTree = new Tree<AIOpponent>
                (
                    new Selector<AIOpponent>
                    (
                        new Sequence<AIOpponent>
                        (
                            new InFrontOfBall(),
                            new PushBall()
                        ),
                        new Reposition()
                    )
                );
            _aiTree = aggressiveTree;
        }

    }

    public void MoveTowardBall()
    {
        Vector3 directionTowardBall;
        directionTowardBall = ServicesLocator.ball.transform.position - opponentInScene.transform.position;
        directionTowardBall = directionTowardBall.normalized * AIManager.moveForce;
        opponentInScene.GetComponent<Rigidbody2D>().AddForce(directionTowardBall);
    }

    public void MoveAwayFromBall()
    {
        Vector3 directionTowardBall;
        directionTowardBall = ServicesLocator.ball.transform.position - opponentInScene.transform.position;
        directionTowardBall = directionTowardBall.normalized * AIManager.moveForce;
        opponentInScene.GetComponent<Rigidbody2D>().AddForce(-directionTowardBall);
    }

    public void GetOnRightSide()
    {
        Vector3 targetPosition;
        Vector3 directionTowardBall;
        targetPosition = ServicesLocator.ball.transform.position + new Vector3(10f, 0, 0);
        directionTowardBall = ServicesLocator.ball.transform.position - opponentInScene.transform.position;

        if (directionTowardBall.magnitude <= 5)
        {
            MoveAwayFromBall();
        }
        else
        {
            Vector3 directionTowardTarget = targetPosition - opponentInScene.transform.position;
            directionTowardTarget = directionTowardTarget.normalized * AIManager.moveForce;
            opponentInScene.GetComponent<Rigidbody2D>().AddForce(directionTowardTarget);
        }
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


public class InFrontOfBall : Node<AIOpponent>
{


    public override bool Update(AIOpponent context)
    {
        return context.opponentInScene.transform.position.x >= ServicesLocator.ball.transform.position.x;
    }
}

public class PushBall : Node<AIOpponent>
{
    public override bool Update(AIOpponent context)
    {
        context.MoveTowardBall();
        return true;
    }

}

public class Reposition : Node<AIOpponent>
{
    public override bool Update(AIOpponent context)
    {
        context.GetOnRightSide();
        return true;
    }

}

