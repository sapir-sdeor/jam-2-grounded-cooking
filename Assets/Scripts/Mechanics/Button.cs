using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    #region Private Methods

    private bool _isPressed = false;

    #endregion
    
    #region Private Methods
    private void OnMouseDown()
    {
        _isPressed = true;
        
    }
    
    #endregion
    
    #region Public Methods
    
    public void SetIsPressed()
    {
        _isPressed = false;
    }

    public bool GetIsPressed()
    {
        return _isPressed;
    }
    
    #endregion
    
    
}
    
