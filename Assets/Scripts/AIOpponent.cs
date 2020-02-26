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
                        opponentInScene)
                )
        }
    }

    public void MoveTowardBall()
    {
        Vector3 directionTowardBall;
        directionTowardBall = ServicesLocator.ball.transform.position - opponentInScene.transform.position;
        directionTowardBall = directionTowardBall.normalized * AIManager.moveForce;
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


public class InFrontOfBall : Condition<AIOpponent>
{
    private readonly AIOpponent _opponent;

    public InFrontOfBall(Predicate<AIOpponent> predicate)
    {
        this = base.Condition<AIOpponent>(predicate);
    }


    private bool FindBall(AIOpponent opponent)
    {
        return opponent.opponentInScene.transform.position.x >= ServicesLocator.ball.transform.position.x + 10;
    }
}
