using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private FiniteStateMachine<GameController> _gameStateMachine;
    private GameObject button;

    // Start is called before the first frame update
    public void HideButton()
    {
        button.SetActive(false);
    }

    void Start()
    {
        ServicesLocator.gameController = this;
        ServicesLocator.aiManager = new AIManager();
        ServicesLocator.eventManager = new EventManager();
        ServicesLocator.ball = GameObject.FindGameObjectWithTag("Ball");
        ServicesLocator.player = new PlayerScript(GameObject.FindGameObjectWithTag("Player"));
        button = GameObject.Find("Button");
        _gameStateMachine = new FiniteStateMachine<GameController>(this);
        _gameStateMachine.TransitionTo<TitleScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        _gameStateMachine.Update();
    }

    public void GameSetup()
    {
        ServicesLocator.aiManager.InstantiateOpponents();

    }

    public void GameCleanUp()
    {

    }

    public void StartButtonClicked(AGPEvent e)
    {
        _gameStateMachine.TransitionTo<SoccerGame>();
    }

    public void GameEnded(AGPEvent e)
    {
        _gameStateMachine.TransitionTo<GameOver>();
    }

    private class TitleScreen : FiniteStateMachine<GameController>.State
    {
        public override void OnEnter()
        {
            ServicesLocator.eventManager.Register<GameStart>(ServicesLocator.gameController.StartButtonClicked);
        }

        public override void OnExit()
        {
            ServicesLocator.gameController.HideButton();
            ServicesLocator.eventManager.Unregister<GameStart>(ServicesLocator.gameController.StartButtonClicked);
        }

        public override void Update()
        {
        }
    }

    private class SoccerGame : FiniteStateMachine<GameController>.State
    {
        public override void OnEnter()
        {
            ServicesLocator.gameController.GameSetup();
            ServicesLocator.scoreController.gameStart = Time.time;
            ServicesLocator.eventManager.Register<GameFinished>(ServicesLocator.gameController.GameEnded);
        }

        public override void OnExit()
        {
            base.OnExit();
            ServicesLocator.eventManager.Unregister<GameFinished>(ServicesLocator.gameController.GameEnded);
        }

        public override void Update()
        {
            ServicesLocator.scoreController.secondsRemaining = (int) Mathf.Floor(Time.time - ServicesLocator.scoreController.gameStart + ScoreController.gameDuration);
            ServicesLocator.scoreController.UpdateDisplays();
        }
    }

    private class GameOver : FiniteStateMachine<GameController>.State
    {
        public override void OnEnter()
        {
            ServicesLocator.gameController.GameCleanUp();
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
