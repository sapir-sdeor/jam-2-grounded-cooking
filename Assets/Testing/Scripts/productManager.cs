using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class productManager : MonoBehaviour
{
    #region privateProperties
    
    private const float Speed = 2;
    private const float LimitX = 4;
    private bool _getUp;
    private bool _stopMove;
    private Rigidbody2D _rigidB;
    private Button _nextButton;
    private Button _prevButton;
    
    #endregion

    #region MonoBehaviour functions

    private void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.CompareTag("Next"))
                _nextButton = child.GetComponent<Button>();
            else if(child.CompareTag("Prev"))
                _prevButton = child.GetComponent<Button>();
        }
        _rigidB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.x < LimitX )
            GetComponent<Rigidbody2D>().velocity = Vector2.right * Speed;
        else if (!_getUp)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _stopMove = true;
        }
    }

    /*
     * When clicking the mouse on the product and it get to the
     * end of conveyor then the player can take the product
     */
    private void OnMouseUp()
    {
        if (Manager.ButtonsEnabled && !_getUp && _stopMove ) //&& !Manager.ProductTaken
        {
            transform.position += (Vector3) Vector2.up;
            _nextButton.GetComponent<SpriteRenderer>().enabled = true;
            _prevButton.GetComponent<SpriteRenderer>().enabled = true;
            Destroy(_rigidB);
            _getUp = true;
           // GameManager.ProductTaken = true;
        }
    }
    
    #endregion
}