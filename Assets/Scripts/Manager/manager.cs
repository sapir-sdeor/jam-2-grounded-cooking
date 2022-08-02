using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class manager : MonoBehaviour
{
    #region Inspector
    
    [Header("Call manager mechanism")]

    [SerializeField] private GameObject CallManagerMessage;
    
    [SerializeField] private GameObject Manager;
    
   // [SerializeField] private Animator buttonAnimator;

    [SerializeField] private AudioClip ErrorClip;
    
    [SerializeField] private Sprite buttonSprite;
    
    [SerializeField] private Sprite buttonPressed;

    [SerializeField] private GameObject button;
    
    [Header("Sale mechanism")]
    [SerializeField] private GameObject saleGameObject;

    [SerializeField] private int timeToTurnOnSell = 4;

    [SerializeField] private AudioClip SellClip;
    
    #endregion

    #region privatePropeties

    private const float TimeForManagerCame = 5;
    private float _nextManagerTime;
    private float _time;
    private float _timeLeftForSale;
    private float minWaitTime = 40;
    private float maxWaitTime = 80;
    private bool _callManager;
    private bool _activateSell = false;
    private int _curEvent = -1;
    
    private enum Event
    {
        Manager, Sale
    }
    #endregion

    #region staticProperties

    public static bool ButtonsEnabled;
    private static readonly int Press = Animator.StringToHash("press");

    #endregion
    
    #region Constants

    private const float TIME_END = 0;
    
    #endregion

    #region MonoBehaviour functions

    private void Start()
    {
       _time = Random.Range(minWaitTime, maxWaitTime);
       ButtonsEnabled = true;
        _timeLeftForSale = timeToTurnOnSell;
        _curEvent = Random.Range(0, 2);
    }

    private void Update()
    {   
        _time -= Time.deltaTime;
        if (_curEvent == (int) Event.Manager)
        {
            switch (_time <= 0)
            {
                case true when ButtonsEnabled:
                    ButtonsEnabled = false;
                    CallManagerMessage.SetActive(true);
                    _time = 0;
                    AudioManager.StopMusic();
                    AudioManager.PlayComputerSound(ErrorClip);
                    break;
                case true when _callManager:
                    Manager.SetActive(true);
                    button.GetComponent<Image>().sprite = buttonSprite;
                    AudioManager.ResumeMusic();
                    AudioManager.EndComputerSound();
                    _nextManagerTime = Random.Range(minWaitTime, maxWaitTime);
                    _time = _nextManagerTime;
                    ButtonsEnabled = true;
                    _callManager = false;
                    break;
            }

            if (_time < _nextManagerTime - 1.5f && ButtonsEnabled)
            {
                Manager.SetActive(false);
                _curEvent = Random.Range(0, 2);
            }

        }
        
        else if (_curEvent == (int) Event.Sale)
        {
            if (_time <= TIME_END && !_activateSell)
            {   
                AudioManager.PlayComputerSound(SellClip);
                _activateSell = true;
                saleGameObject.SetActive(true);
            }

            if (_activateSell)
            {
                _timeLeftForSale -= Time.deltaTime;

                if (_timeLeftForSale <= TIME_END)
                {   
                    AudioManager.EndComputerSound();
                    turnSellButtonOff();
                    return;
                }
                
            }


        }
    }

    #endregion

    private void turnSellButtonOff()
    {
        AudioManager.EndComputerSound();
        _activateSell = false;
        _nextManagerTime = Random.Range(minWaitTime, maxWaitTime);
        _time = _nextManagerTime;
        _timeLeftForSale = timeToTurnOnSell;
        saleGameObject.SetActive(false);
        _curEvent = Random.Range(0, 2);
        
    }

    public void ActivateButton()
    {
        switch (_curEvent)
        {
            case (int) Event.Manager:
                CallManagerButton();
                break;
            
            case (int) Event.Sale:
                PressSaleButton();
                break;
        }
        
        
    }
    
    public void CallManagerButton()
    {
        if (ButtonsEnabled || _callManager) return;
        _time = TimeForManagerCame;
        button.GetComponent<Image>().sprite = buttonPressed;
        _callManager = true;
        CallManagerMessage.SetActive(false);
    }

   
    /**
     * called if the sale game object appeard and payer pressed it. gets all costumersHappier.
     */
    public void PressSaleButton()
    {
        if (_activateSell)
        {
            saleGameObject.SetActive(false);
            gameObject.GetComponent<customerManager>().ChangeAngerLevelAllCostumers(true);
            _nextManagerTime = Random.Range(minWaitTime, maxWaitTime);
            _time = _nextManagerTime;
            _curEvent = Random.Range(0, 2);
            _activateSell = false;
            _timeLeftForSale = timeToTurnOnSell;
        }
        
        
    }
}
