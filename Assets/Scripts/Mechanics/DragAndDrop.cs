using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{   
    [Tooltip("Use This Parameter to decide on Item Speed")]
    [SerializeField] private int DragSpeed = 30;
    private products _productManager;

    #region Private Properties
    private Camera _mainCamera;
    private Vector3 _dragOffset;
    
    private Rigidbody2D _myRigidbody2D;

    #endregion
    
    #region MoneBehaviour funcs
    private void Awake()
    {
        _mainCamera = Camera.main;
        _myRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _productManager = GetComponent<products>();

    }
    

    private void OnMouseDown()
    {
        if (!_productManager.gotPlaced || !_productManager.firstOnCounter || !manager.ButtonsEnabled) return;
        _dragOffset = transform.position - GetMousePosition();
        
    }

    public void OnMouseDrag()
    {  
        if (!_productManager.gotPlaced || !_productManager.firstOnCounter || !manager.ButtonsEnabled) return;
        transform.position = Vector3.MoveTowards(transform.position, 
            GetMousePosition() * 2f + _dragOffset, DragSpeed * Time.deltaTime);
    }
    

    private void OnMouseUp()
    {
        if (!_productManager.gotPlaced || !_productManager.firstOnCounter) return;
        _myRigidbody2D.gravityScale = 1;
    }
    
    #endregion
    
    #region Private Methods

    private Vector3 GetMousePosition()
    {
       // var mousePos = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition) * 0.2f;
        mousePos.z = 0;
        return mousePos;
    }
    
    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        // this means the 
        if (other.gameObject.CompareTag("Counter"))
        {
            // PrepareForScan();
            // what will this function do?
            Debug.Log("Product is in the weigh zone!");
        }

    }
}
