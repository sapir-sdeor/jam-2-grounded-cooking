
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeProductFace : MonoBehaviour
{   
    
    #region Inspector

    [SerializeField] private SpriteRenderer _mySpriteRenderer;
    
    [Tooltip("Put the product sprites that should be presented when changing the face. Order is Important")]
    [SerializeField] private List<Sprite> _productFaces;

    [Tooltip("this will be the index of the sprite with the barcode")] 
    [SerializeField] private int _barcodeIdx = -1;

    [SerializeField] private AudioClip _changeFaceClip;
    
    #endregion
    
    #region Globals
    private const int FIRST_FACE = 0;
    private products _productManager;
    
    
    #endregion
    
    #region Private Properties

    // 0: Next, 1:Prev
    private int _curButton = -1;

    private int _curFaceIdx = 0;

    private AudioSource _productAudio;
    
    
    
    private enum Buttons
    {
        Next,
        Prev
    }

    
    #endregion 
    
    #region MonoBehaviour Functions

    private void Start()
    {
        _productAudio = GetComponent<AudioSource>(); 
        _productManager = GetComponent<products>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.T) && _productManager.firstOnCounter && 
            _productManager.code == "" && manager.ButtonsEnabled)
        {
            _productAudio.clip = _changeFaceClip;
            _productAudio.Play();
            _curButton = (int) Buttons.Next;
            SwitchFace();
            CheckBarcodeValidity();
        }
        
        // if needed we can set another key code to make it show the previous sprite
    }
    
    #endregion
    
    #region Private Methods

    private void SwitchFace()
    {
        switch (_curButton)
        {
            case (int)Buttons.Next:
                
              
                if (_curFaceIdx == _productFaces.Count - 1)
                {   // we need to do a cycle and go back to first face
                    _mySpriteRenderer.sprite = _productFaces[FIRST_FACE];
                    _curFaceIdx = FIRST_FACE;
                }

                else
                {
                   _mySpriteRenderer.sprite = _productFaces[_curFaceIdx + 1];
                   _curFaceIdx += 1;
                }
                
               
                break;
            
            case (int)Buttons.Prev:
                
                if (_curFaceIdx == FIRST_FACE)
                {
                    // we need to do a cycle and return to the last one. 
                    _mySpriteRenderer.sprite = _productFaces[_productFaces.Count - 1];
                    _curFaceIdx = _productFaces.Count - 1;
                }

                else
                {
                    _mySpriteRenderer.sprite = _productFaces[_curFaceIdx - 1];
                    _curFaceIdx -= 1;
                }
                
                break;

        }
    }
    
    /**
     * checks if the current sprite is the sprite with the barcode. 
     */
    public void CheckBarcodeValidity()
    {
        if (_barcodeIdx == _curFaceIdx)
        {
            _productManager.getBarcode = true;
            Debug.Log("The barcode is here!!");
        }
        else
        {
            _productManager.getBarcode = false;
            Debug.Log("The barcode is not here!");
        }
    }
    #endregion
    
}
