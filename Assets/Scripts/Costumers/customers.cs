using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class customers : MonoBehaviour
{
    #region Inspector

    [SerializeField] protected List<GameObject> productObjects;
    [SerializeField] protected List<GameObject> instantiateProducts;
    [SerializeField] protected List<Sprite> costumerMoodSprites;
    [SerializeField] protected float finishTimeExpected;
    [SerializeField] protected float timeForEachProduct = 8;
    
    
    #endregion

    public int customerId;
    public Vector3 _newPosition;
    
    
    //Gal: I changed all this section to protected because i want the special costumers be the childs of this script
    #region Protected Properties
    
    protected bool _startMove;
    protected bool _customerFinish;
    
    protected int _productsCount;
    protected float _overallTime;
    protected int _timeBetween;
    protected int _countForFinish;
    protected static bool _changeMainCustomer;

   
    #endregion

    protected void Start()
    {
        finishTimeExpected = productObjects.Count * timeForEachProduct;
        _newPosition = new Vector3(-10f, -2f, 0);
        StartCoroutine(AppearProducts());
        _changeMainCustomer = false;
    }
    
    
    
    protected void Update()
    {
        if (_productsCount > 0)
            _overallTime += Time.deltaTime; 
        
        if ((transform.position.x < 1.5f && customerId == 1) || _startMove)
            Move();

        if (CheckIfFinish() && (customerId == 1 || customerId == 3))
            _customerFinish = true;

        if ((_customerFinish || CheckAngerLevel()) && customerId == 1)
            CustomerLeave();
        
        if (_changeMainCustomer && customerId == 2)
        {
            customerId = 1;
            customerManager._customersCount--;
            _changeMainCustomer = false;
        }

        if (transform.position.x > 10)
            CustomerLeftScreen();
        CheckIfMad();
    }


    //if get to half time start mad, if get to 0.75 - very mad
    protected void CheckIfMad()
    {
        if (_overallTime > (finishTimeExpected / 2) && _overallTime < finishTimeExpected)
        {
            
            CurAngerLevel = _overallTime > (finishTimeExpected * 0.75)
                ? (int) AngerLevel.High
                : (int) AngerLevel.Medium;
            
            GetComponent<SpriteRenderer>().sprite = costumerMoodSprites[CurAngerLevel];
        }
    }
    
    
    protected void Move()
    {
        transform.Translate(Vector3.right * Time.deltaTime, Space.Self);
    }
    
    protected virtual void CustomerLeave()
    {   
       
        _startMove = true;
        _changeMainCustomer = true;
        customerId = 3;
        if (_customerFinish)
            GameManager.PlayerScore += 3;
    }

    /*
     * Check if the player success with all the products
     */
    protected bool CheckIfFinish()
    {
        _countForFinish = 0;
        foreach (var product in instantiateProducts)
        {
            if (product && !product.CompareTag("Block"))
                _countForFinish += product.GetComponent<products>().countFinishProduct;
        }
        return _countForFinish == productObjects.Count - 1;
    }
    
    
    protected IEnumerator AppearProducts()
    {
        while (true)
        {
            if (_productsCount < productObjects.Count && customerId == 1
                && mechsaniceConveyor.CountPress > _timeBetween)
            {
                GameObject product = productObjects[_productsCount];
                GameObject newProduct = Instantiate(product, _newPosition, Quaternion.identity);
                instantiateProducts.Add(newProduct);
                _productsCount++;
            }
            //todo: check if want random time
            _timeBetween = mechsaniceConveyor.CountPress;
            yield return new WaitForSeconds(3);
        }
    }

    protected void CustomerLeftScreen()
    {
        _startMove = false;
        _customerFinish = false;
        Destroy(gameObject);
    }
    
    #region Added by gal
    
    /**
     * added by gal
     */
    public int CurAngerLevel = (int) AngerLevel.Low;
    
    protected enum AngerLevel
    {
        Low, Medium, High
    }
    
   

    public void SetAngerLevel(int newAngerLevel)
    {
        CurAngerLevel = newAngerLevel;
        if (CurAngerLevel == (int) AngerLevel.Low - 1)
        {   
            CurAngerLevel = (int) AngerLevel.Low;
        }

        if (CurAngerLevel == (int) AngerLevel.High + 1)
        {
            CurAngerLevel = (int) AngerLevel.High;
        }
        GetComponent<SpriteRenderer>().sprite = costumerMoodSprites[CurAngerLevel];
    }

    protected bool CheckAngerLevel()
    {
        if (CurAngerLevel == (int) AngerLevel.High)
        {
            _customerFinish = false;
            return true;
            //CustomerLeave();
        }
        return false;
    }
    
    #endregion
    
}
