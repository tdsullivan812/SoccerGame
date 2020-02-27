using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript
{
    GameObject player;
    private const float _moveForce = 10;
    private Rigidbody2D _rigidbody;


    public PlayerScript(GameObject playerInScene)
    {
        player = playerInScene;
        _rigidbody = player.GetComponent<Rigidbody2D>();
    }

    public void Controller()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _rigidbody.AddForce(Vector2.up * _moveForce);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody.AddForce(Vector2.left * _moveForce);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.AddForce(Vector2.down * _moveForce);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody.AddForce(Vector2.right * _moveForce);
        }
    }

    // Update is called once per frame


   
}
