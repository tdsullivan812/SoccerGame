using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private const float _moveForce = 10;
    private Rigidbody2D _rigidbody;



    // Start is called before the first frame update
    void Start()
    {
        ServicesLocator.player = this;
        ServicesLocator.aiManager = new AIManager();
        ServicesLocator.ball = GameObject.FindGameObjectWithTag("Ball");
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Controller()
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


    void FixedUpdate()
    {
        Controller();
        ServicesLocator.aiManager.MoveTowardBall();
    }
}
