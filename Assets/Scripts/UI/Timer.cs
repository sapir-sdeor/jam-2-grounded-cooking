using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    #region Inspector
    [SerializeField] private float RoundTime;
    
    #endregion
    
    #region Globals

    private float ROUND_END = 0f;

    private string NO_TIME_LEFT = "00:00";
    
    #endregion
    
    #region Private Fields
    
    private TextMeshProUGUI _UITimer;

    private bool _isTimerRunning = false;

    private float _timeRemaining;

    private GameManager _gameManager;
    #endregion
 
    
    
    #region MonoBeahviour Funcs
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _timeRemaining = RoundTime * 60;
        _UITimer = GameObject.FindWithTag("UITimer").GetComponent<TextMeshProUGUI>();
        _isTimerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTimerRunning)
        {
            if (_timeRemaining > ROUND_END)
            {
                _timeRemaining -= Time.deltaTime;
                DisplayTime();
            }

            else
            {
                Debug.Log("Round ended");
                _UITimer.text = NO_TIME_LEFT;
                _isTimerRunning = false;
                _timeRemaining = 0;
                _gameManager.EndRound();
            }
        }
    }
    
    #endregion


    private void DisplayTime()
    {
        
        float minutes = Mathf.FloorToInt(_timeRemaining / 60); 
        float seconds = Mathf.FloorToInt(_timeRemaining % 60);
        _UITimer.text = $"{minutes:00}:{seconds:00}";
    }
}
