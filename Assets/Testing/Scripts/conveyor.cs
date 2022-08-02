using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyor : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    private float _rightScreen = 18; 
    private float _leftScreen = -18;

    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
        if (transform.position.x > _rightScreen)
        {
            transform.position = new Vector3(_leftScreen,transform.position.y,0);
        }
    }
    
}