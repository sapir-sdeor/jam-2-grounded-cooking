using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Manager : MonoBehaviour
{
    public static bool ButtonsEnabled = true;
    private const float PaperOver = 20;
    private const float ManagerTime = 10;
    private float _timeForPaper = 20;
    private float minWaitTime = 10;
    private float maxWaitTime = 30;
    private Vector3 _newPosition;
    private bool _callManager;
    
    private Dictionary<int, Func<int>> cases = new Dictionary<int, Func<int>>()
    {
        {1, OutOfPaper},
        {2, CustomerWantManager},
        {3, CustomerReturnProduct},
    };

    /*Random the time of starting the event case*/
    private void Start()
    {
        
        _timeForPaper = Random.Range(minWaitTime, maxWaitTime);
    }

    void Update()
    {
        _timeForPaper -= Time.deltaTime;
        if (_timeForPaper <= 0 && ButtonsEnabled)
        {
            ButtonsEnabled = false;
            CheckCase();
            _timeForPaper = 0;
        }
        else if (_timeForPaper <= 0 && _callManager)
        {
            print("The manager came, you can continue");
            _timeForPaper = PaperOver;
            ButtonsEnabled = true;
            _callManager = false;
        }
    }

    private void CheckCase()
    {
        var whichCase = Random.Range(0, cases.Count);
        cases[whichCase].Invoke();
    }

    private static int OutOfPaper()
    {
        print("No more paper, call the manager");
        return 0;
    }
    
    private static int CustomerWantManager()
    {
        print("The customer want the manager, call him");
        return 0;
    }
    
    private static int CustomerReturnProduct()
    {
        print("The customer want to return the product");
        return 0;
    }

    void CallManager()
    {
        if (ButtonsEnabled) return;
        _timeForPaper = ManagerTime;
        _callManager = true;
    }
    
    
    
}
