using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechsaniceConveyor : MonoBehaviour
{

    private float timeForAnim = TimeMoving;
    public static bool isMoving;
    private const float TimeMoving = 0.5f;

    public const float speed = 3;
    private float _rightScreen = 60; 
    private float _leftScreen = -38;
    public static int CountPress;
    
    void Update()
    {
        foreach (Transform child in transform)
        {
            if (child.transform.position.x > _rightScreen)
                child.transform.position = new Vector3(_leftScreen,child.transform.position.y,0);
        }

        if (isMoving)
        {
            timeForAnim -= Time.deltaTime;
            foreach (Transform child in transform)
                child.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
        }
        if (timeForAnim <= 0 && isMoving)
        {
            isMoving = false;
            foreach (Transform child in transform)
                child.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            timeForAnim = TimeMoving;
        }

        if (Input.GetKey(KeyCode.Space) && !isMoving && manager.ButtonsEnabled)
        {
            CountPress++;
            isMoving = true;
        }
             
    }
    
}
