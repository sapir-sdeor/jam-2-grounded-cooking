using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class ScoreManager : MonoBehaviour
{   
    #region Inspector

    [Tooltip("Drag all the scores objects to here")]
    [SerializeField] private List<GameObject> ScoreObjects;
    
    [Tooltip("put here the index of the player score, it will be effected by the real score and not by the random number generation")]
    [SerializeField] private int PlayerScoreIndex;

    [Tooltip("where is the lousy Witch?")]
    [SerializeField] private int BadWitchIdx;

    [Tooltip("type here what is the time interval you want the score to update(in seconds)")]
    [SerializeField] private float SecondToUpdateScore = 15f;

    [SerializeField] private List<Slider> _scoreUiDisplays;

    [SerializeField] private List<Image> _images;
    #endregion
    
    #region Private Properties

    public Gradient colorSwap;
    
    public List<int> _scores;
    
    private List<TextMeshProUGUI> _scoreTextDisplays;

    // determines what is the frequency the score is calculated and displayed the score
    private float _timeRemainingToCalc;
    
 //   public int witchesNumber = 1;
    #endregion
    
    #region Globals
    
    // TODO: we need to play test and see what are the best values for this!
    private const int INITIAL_SCORE = 0;
    
    private const int MIN_SCORE = 0;

    private const int MAX_SCORE = 5;

    private const int BAD_WITCH_MAX_SCORE = 2;

    private const float TIME_UP = 0f;

    private const int PLAYER = 0;
    
    #endregion
    // Start is called before the first frame update
    void Start()
    {

        if (BadWitchIdx == PlayerScoreIndex)
            Debug.Log("Error: the bad witch and player cant have the same score object");
        
        
        _timeRemainingToCalc = SecondToUpdateScore;

        _scores = new List<int>();
        _scoreTextDisplays = new List<TextMeshProUGUI>();
      
        
        for (int i = 0; i < ScoreObjects.Count; i++)
        {
            _scores.Add(INITIAL_SCORE);
            
            // get the current TMP object in that score object
            TextMeshProUGUI curTxtDisplay = ScoreObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            curTxtDisplay.text = INITIAL_SCORE.ToString();
            _scoreTextDisplays.Add(curTxtDisplay);
        }
        
       
        Debug.Log("TMP: " + _scoreTextDisplays);
    }

  
    private void FixedUpdate()
    {   
        // update player score
        _scores[PlayerScoreIndex] = GameManager.PlayerScore;
        _scoreTextDisplays[0].text = _scores[PlayerScoreIndex].ToString();
        _scoreUiDisplays[0].value = _scores[PlayerScoreIndex];
        
        
        if (_timeRemainingToCalc > TIME_UP)
            _timeRemainingToCalc -= Time.deltaTime;

        else
        {   
            // Update other players score
            CalcScore();
            ChangeScoreDisplay();
            _timeRemainingToCalc = SecondToUpdateScore;
        }
    }

    // this function updates the score board every round
    private void ChangeScoreDisplay()
    {
        for (int i = 0; i < _scores.Count; i++)
        {
            _scoreTextDisplays[i].text = _scores[i].ToString();
            _scoreUiDisplays[i].value = _scores[i];
            _images[i].color = colorSwap.Evaluate((float)_scores[i] / _scoreUiDisplays[i].maxValue);

        }
    }
    
    private void CalcScore()
    {
        
        for (int i = 0; i < _scores.Count; i++)
        {
            if (i == PlayerScoreIndex)
                continue;

            if (i == BadWitchIdx)
                _scores[i] += Random.Range(MIN_SCORE, BAD_WITCH_MAX_SCORE);

            else
                _scores[i] += Random.Range(MIN_SCORE, MAX_SCORE);
        }

    }

    
        
    
}
