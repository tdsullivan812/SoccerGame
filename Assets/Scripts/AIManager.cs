using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager
{
    private const int _numberOfOpponents = 2;
    private Vector3[] _opponentStartPositions = new Vector3[_numberOfOpponents];
    private const float _moveForce = 10;
    private static GameObject _opponentPrefab = Resources.Load<GameObject>("Opponent");
    private static GameObject[] _listOfOpponents = new GameObject[_numberOfOpponents];



    public AIManager()
    {
        SetStartPositions();
        InstantiateOpponents();
    }

    public void SetStartPositions()
    {
        for (int i = 0; i < _numberOfOpponents; i++)
        {
            _opponentStartPositions[i] = new Vector3(20, -1 + 2 * i, 0);
        }
       
    }
    
    public void InstantiateOpponents()
    {
        for (int i = 0;  i < _numberOfOpponents; i++)
        {
            _listOfOpponents[i] = GameObject.Instantiate( _opponentPrefab, _opponentStartPositions[i], Quaternion.identity);
        }
    }

    public void MoveTowardBall()
    {
        Vector3 directionTowardBall;

        for (int i = 0; i < _numberOfOpponents; i++)
        {
            directionTowardBall = ServicesLocator.ball.transform.position - _listOfOpponents[i].transform.position;
            directionTowardBall = directionTowardBall.normalized * _moveForce;
            _listOfOpponents[i].GetComponent<Rigidbody2D>().AddForce(directionTowardBall);
            
        }
    }

    

}
