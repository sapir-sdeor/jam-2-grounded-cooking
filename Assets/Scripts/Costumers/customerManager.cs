using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customerManager : MonoBehaviour
{
    
    [SerializeField] private List<customers> customersList;
   
    
    
    

    #region privatePropeties
    
    public static int _customersCount;
    
    private Vector3 _newPosition;
    private bool _firstCustomer;
    private float _timeForNewCustomer;
    private float _time;

    #endregion
    
    
    void Start()
    {
        _customersCount = 0;
        _newPosition = new Vector3(-9.5f, -0.4f, 0);
    }

    
    void Update()
    {
        if (_customersCount == 0)
        {
            AddCustomer(1);
            _firstCustomer = true;
        }

        if (_firstCustomer)
            _timeForNewCustomer += Time.deltaTime;

        if (_timeForNewCustomer > 6 && _firstCustomer)
        {
            AddCustomer(2);
            _firstCustomer = false;
            _timeForNewCustomer = 0;
        }

        if (_customersCount == 1 && !_firstCustomer)
            _firstCustomer = true;
        
    }
    
    void AddCustomer(int id)
    {
        Vector2 position = _newPosition;
        customers newCustomer = customersList[Random.Range(0, customersList.Count)];
        newCustomer.customerId = id;
        Instantiate(newCustomer, position, Quaternion.identity);
        _customersCount++;
    }

    public void ChangeAngerLevelAllCostumers(bool happier)
    {
        int AngerAddition = happier ? -1 : 1;
        foreach (var costumer in customersList)
        {
            costumer.SetAngerLevel(costumer.CurAngerLevel + AngerAddition);
        }
    }


}
