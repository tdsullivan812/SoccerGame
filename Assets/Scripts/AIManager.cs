﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager
{
    private const int _numberOfOpponents = 3;
    public const float goaliePosition = 30;
    private Vector3[] _opponentStartPositions = new Vector3[_numberOfOpponents];
    public const float moveForce = 10;
    private static GameObject _opponentPrefab = Resources.Load<GameObject>("Opponent");
    private static AIOpponent[] _listOfOpponents = new AIOpponent[_numberOfOpponents];




    public AIManager()
    {
        SetStartPositions();
       
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
            _listOfOpponents[i] = new AIOpponent(GameObject.Instantiate( _opponentPrefab, _opponentStartPositions[i], Quaternion.identity), i % 3);

        }
    }

    public void Update()
    {
        for(int i = 0; i < _listOfOpponents.Length; i++)
        {
            _listOfOpponents[i].Update();
        }
    }

   
    


    

}


