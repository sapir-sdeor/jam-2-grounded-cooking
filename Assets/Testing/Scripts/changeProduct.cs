
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class changeProduct : MonoBehaviour
{   
    
    #region Inspector

    [SerializeField] private SpriteRenderer _mySpriteRenderer;
    
    [Tooltip("Put the product sprites that should be presented when changing the face. Order is Important")]
    [SerializeField] private List<Sprite> _productFaces;

    [Tooltip("this will be the index of the sprite with the barcode")] 
    [SerializeField] private int _barcodeIdx = -1;
    
    #endregion
    
    #region Globals
    private const int FIRST_FACE = 0;
    
    #endregion
    
    #region Private Properties

    private Button _nextButton;

    private Button _prevButton;

    // 0: Next, 1:Prev
    private int _curButton = -1;

    private int _curFaceIdx = 0;

    private enum Buttons
    {
        Next,
        Prev
    }
    
    #endregion

    public bool foundBarcode = false;
    #region MonoBehaviour Functions
    
     // Start is called before the first frame update
    void Start()
    {
        foundBarcode = false;
        foreach(Transform child in transform)
        {
            if(child.CompareTag("Next"))
                _nextButton = child.GetComponent<Button>();
            else if(child.CompareTag("Prev"))
                _prevButton = child.GetComponent<Button>();
        }
    }
        
        
    // Update is called once per frame
    void Update()
    {
        CheckPressedButtons();
        CheckBarcodeValidity();
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
                
                _nextButton.SetIsPressed();
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
                _prevButton.SetIsPressed();
                break;

        }
        
        
    }
    
    private void CheckPressedButtons()
    {
        if (_nextButton.GetIsPressed())
        {   
            _curButton = (int) Buttons.Next;
            SwitchFace();
            _nextButton.SetIsPressed();
        }
        
        else if (_prevButton.GetIsPressed())
        {
            _curButton = (int) Buttons.Prev;
            SwitchFace();
            _prevButton.SetIsPressed();
        }

        
    }

    /**
     * checks if the current sprite is the sprite with the barcode. 
     */
    public void CheckBarcodeValidity()
    {
        if (_barcodeIdx == _curFaceIdx && !foundBarcode)
        {
            Destroy(gameObject, 0.4f);
            GameManagerTesting.CountProduct--;
            foundBarcode = true;
            GameManagerTesting.ProductTaken = false;
            Debug.Log("The barcode is here!!");
        }

        else
        {
            Debug.Log("The barcode is not here!");
        }
    }
    #endregion
    
}
