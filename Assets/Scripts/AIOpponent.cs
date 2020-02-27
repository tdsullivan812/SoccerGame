using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System;

public class AIOpponent
{
    public GameObject opponentInScene;
    //private FiniteStateMachine<AIOpponent> _AIStateMachine;
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
        //_AIStateMachine = new FiniteStateMachine<AIOpponent>(this);
        //_AIStateMachine.TransitionTo<Moving>();

        //Aggressive behavior tree
        if (behavior == (int)BehaviorType.Aggressive)
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

        //Goalie behavior tree
        else if (behavior == (int)BehaviorType.Goalie)
        {
            var goalieTree = new Tree<AIOpponent>
                (
                    new Selector<AIOpponent>
                    (
                        new Sequence<AIOpponent>
                        (
                            new IsInFrontOfGoal(),
                            new AlignWithBall()
                        ),
                        new AlignWithGoal()
                    )
                );
            _aiTree = goalieTree;
        }

        //Blocker behavior tree
        else
        {
            var blockerTree = new Tree<AIOpponent>
                (
                    new Selector<AIOpponent>
                    (
                        new Sequence<AIOpponent>
                        (
                            new IsInFrontOfPlayer(),
                            new AlignWithPlayer()
                        ),
                        new CircleAroundPlayer()
                    )


                );
            _aiTree = blockerTree;
        }

    }

    //Aggressive AI methods
    #region 
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
    #endregion //

    #region

    #endregion
    public void Update()
    {
        //_AIStateMachine.Update();

        _aiTree?.Update(this);
    }


    /*
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
    }*/

        //Aggressive AI nodes
    #region
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
    #endregion

    //Goalie AI nodes
    #region
    public class IsInFrontOfGoal : Node<AIOpponent>
    {
        public override bool Update(AIOpponent context)
        {
            return Mathf.Abs(context.opponentInScene.transform.position.x - AIManager.goaliePosition) <= 10;
        }
    }

    public class AlignWithBall : Node<AIOpponent>
    {
        public override bool Update(AIOpponent context)
        {
            if (ServicesLocator.ball.transform.position.y > context.opponentInScene.transform.position.y)
            {
                context.opponentInScene.GetComponent<Rigidbody2D>().AddForce(Vector2.up * AIManager.moveForce);
            }
            else if (ServicesLocator.ball.transform.position.y < context.opponentInScene.transform.position.y)
            {
                context.opponentInScene.GetComponent<Rigidbody2D>().AddForce(Vector2.down * AIManager.moveForce);
            }

            return true;
        }

    }

    public class AlignWithGoal : Node<AIOpponent>
    {
        public override bool Update(AIOpponent context)
        {
            if (context.opponentInScene.transform.position.x - AIManager.goaliePosition < -1)
            {
                context.opponentInScene.GetComponent<Rigidbody2D>().AddForce(Vector2.right * AIManager.moveForce);

            }
            else if (context.opponentInScene.transform.position.x - AIManager.goaliePosition > 1)
            {
                context.opponentInScene.GetComponent<Rigidbody2D>().AddForce(Vector2.left * AIManager.moveForce);

            }

            return true;
        }
    }
    #endregion

    #region
    public class IsInFrontOfPlayer : Node<AIOpponent>
    {
        public override bool Update(AIOpponent context)
        {
            return (Vector3.Distance(context.opponentInScene.transform.position, ServicesLocator.player.player.transform.position + Vector3.right * 5) <= 3);
        }
    }

    public class AlignWithPlayer : Node<AIOpponent>
    {
        public override bool Update(AIOpponent context)
        {
            if (Mathf.Abs(context.opponentInScene.transform.position.y - ServicesLocator.player.player.transform.position.y) < -1)
            {
                context.opponentInScene.GetComponent<Rigidbody2D>().AddForce(Vector2.up * AIManager.moveForce);
            }
            else if(Mathf.Abs(context.opponentInScene.transform.position.y - ServicesLocator.player.player.transform.position.y) > 1)
            {
                context.opponentInScene.GetComponent<Rigidbody2D>().AddForce(Vector2.down * AIManager.moveForce);
            }
            return true;
        }
    }

    public class CircleAroundPlayer : Node<AIOpponent>
    {
        public override bool Update(AIOpponent context)
        {
            Vector3 targetPosition;
            Vector3 directionTowardPlayer;
            targetPosition = ServicesLocator.player.player.transform.position + new Vector3(10f, 0, 0);
            directionTowardPlayer = ServicesLocator.player.player.transform.position - context.opponentInScene.transform.position;

            if(directionTowardPlayer.magnitude <= 5)
            {
                context.opponentInScene.GetComponent<Rigidbody2D>().AddForce(-directionTowardPlayer.normalized * AIManager.moveForce);
            }
            else
            {
                var towardTarget = (targetPosition - context.opponentInScene.transform.position).normalized;
                context.opponentInScene.GetComponent<Rigidbody2D>().AddForce((Vector2)towardTarget * AIManager.moveForce);
            }

            return true;
        }
    }
    #endregion
}

