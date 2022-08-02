using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stealCustomer : MonoBehaviour
{

    #region Inspector
    [SerializeField] private float stealTime = 1;
    [SerializeField] private float speed = 2;
    [SerializeField] private Sprite stealSprite;
    #endregion

    #region Private

    private Sprite mySprite;
    private bool _steal;
    private bool _finishSteal;
    private bool _stealOnce;
    private GameObject _productToSteal;
    
    #endregion


    #region MonoBehaviour func

    private void Start()
    {
        mySprite = GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {   
        
        stealTime -= Time.deltaTime;
        if (stealTime <= 0 && !_stealOnce)
        {
            _steal = true;
            _stealOnce = true;
            _productToSteal = FindClosestProduct();
            if (_productToSteal == null)
                _finishSteal = true;
        }

        if (_steal && !_finishSteal)
            Steal();
        
        if (_finishSteal)
        {
            GetComponent<SpriteRenderer>().sprite = mySprite;

            GetComponent<SpriteRenderer>().sortingLayerName = "Conveyour";
            transform.Translate(Vector3.right * Time.deltaTime, Space.Self);
        }
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    
    private void OnMouseDown()
    {
        if (!_finishSteal)
            _finishSteal = true;
    }

    #endregion

    #region otherFunctions

    private void Steal()
    {
        if (_productToSteal)
        {
            Vector3 myPos = transform.position;
            Vector3 target = new Vector3(_productToSteal.transform.position.x, myPos.y, 0);
            transform.position = Vector3.MoveTowards(myPos, target
                    , Time.deltaTime * speed);
            GetComponent<SpriteRenderer>().sprite = stealSprite;
            if (transform.position == target)
            {
                _finishSteal = true;
                if (_productToSteal.GetComponent<products>().firstOnCounter)
                {
                    products._onCounter = false;
                    GameManager.LeftProduct = true;
                }
                _productToSteal.SetActive(false);
            }
            if (_productToSteal.transform.position.x > 3)
                _finishSteal = true;
        }
        
    }

    private GameObject FindClosestProduct()
    {
        GameObject[] products = GameObject.FindGameObjectsWithTag("Product");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject product in products)
        {
            Vector3 diff = product.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (!(curDistance < distance) && product.transform.position.x > 0) continue;
            closest = product;
            distance = curDistance;
        }
        return closest;
    }
    
    #endregion
}
