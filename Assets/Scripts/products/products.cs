using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;

public class products : MonoBehaviour
{
    
    [SerializeField] public string code;
    [SerializeField] private AudioClip _barcodeSoundClip;
    
    private float Speed;
    
    
    
    #region privateProperties

    private bool _productLeft;
    private bool _gotScore;
    private bool _finishProduct;
    private Rigidbody2D _rigidB;
    private AudioSource _barcodeSound;
    public static bool _onCounter = false;
    private bool visible = true;
    
    
    #endregion

    #region Public Properties
    
    public bool onWeight = false;
    public int countFinishProduct;
    public bool gotPlaced = false;
    public bool firstOnCounter = false;
    public bool getBarcode;
   
    
    #endregion
    #region MonoBehaviour functions

    private void Start()
    {
        //_onCounter = false;
        Speed = mechsaniceConveyor.speed;
        _rigidB = GetComponent<Rigidbody2D>();
        _barcodeSound = GetComponent<AudioSource>();

        if (gameObject.CompareTag("Block"))
        {
            float y = -2.628943f;
            Vector3 newVec = transform.position;
            newVec.y = y;
            transform.position = newVec;
        }
    }

    void Update()
    {
        if (mechsaniceConveyor.isMoving && !gotPlaced)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("frog") && transform.position.x < 5)
                {
                    _rigidB.bodyType = RigidbodyType2D.Kinematic;
                    child.GetComponent<Animator>().SetTrigger("jump");
                    _rigidB.bodyType = RigidbodyType2D.Dynamic;
                }
            }
            _rigidB.velocity = Vector2.right * Speed;
        }
        
        else if (!gotPlaced && !_finishProduct)
            _rigidB.velocity = Vector2.zero;
      

        if (!gotPlaced || firstOnCounter || _onCounter || CompareTag("Block")) return;
        _onCounter = true;
        firstOnCounter = true;

    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("line")) return;
        gotPlaced = true;
        _rigidB.bodyType = RigidbodyType2D.Dynamic;
        _rigidB.velocity = Vector2.zero;
    }

    /*
     * When the user success to scan/enter the code of the product
     */
    public void SuccessProduct()
    {
        if (_gotScore) return;
        _barcodeSound.clip = _barcodeSoundClip;
        _barcodeSound.Play();
        _gotScore = true;
        GameManager.PlayerScore += 1;
        StartCoroutine(MoveProduct());
    }

    /*
     * When the user send the product without scan
     */
    private void ProductFinish()
    {
        _finishProduct = true;
        GameManager.Success = false;
        GameManager.EnterCode = "";
        GameManager.LeftProduct = true;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("barcodeLine") && getBarcode)
            SuccessProduct();

        if (other.CompareTag("Counter") && transform.position.x >= 5)
        {
            _rigidB.velocity = Vector2.right * Speed;
            _productLeft = true;
            ProductFinish();
        }
        
        else if (other.CompareTag("Counter"))
        {
            _rigidB.gravityScale = 0;
            _rigidB.velocity = Vector2.zero;
            if (transform.position.x > 0)
                onWeight = true;
        }

        if (_productLeft)
        {
            _onCounter = false;
            firstOnCounter = false;
            _productLeft = false;
            gotPlaced = false;
            countFinishProduct = 1;
        }
    }

    private void OnBecameInvisible()
    {
        Debug.Log("invisble");
        visible = false;
    }

  
    IEnumerator MoveProduct()
    {
        
        while (visible) 
        {
           transform.position = transform.position + Vector3.right * 0.2f;
           yield return new WaitForSeconds(0.02f);
       }    
    }
    #endregion
}