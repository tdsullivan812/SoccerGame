using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private FiniteStateMachine<GameController> _gameStateMachine = new FiniteStateMachine<GameController>(this);
    private GameObject button;

    // Start is called before the first frame update
    public void HideButton();
    {
        button.SetActive(false);
    }

    void Start()
    {
        ServicesLocator.gameController = this;
        ServicesLocator.aiManager = new AIManager();
        ServicesLocator.ball = GameObject.FindGameObjectWithTag("Ball");
        ServicesLocator.player = new PlayerScript(GameObject.FindGameObjectWithTag("Player"));
        button = GameObject.Find("Button");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private abstract class TitleScreen : FiniteStateMachine<GameController>.State
    {
        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
        ServicesLocator.gameController.HideButton();
        }

        public override void Update()
        {
            base.Update();
        }
    }

    private abstract class SoccerGame : FiniteStateMachine<GameController>.State
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Update()
        {
            base.Update();
        }
    }

    private abstract class GameOver : FiniteStateMachine<GameController>.State
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
