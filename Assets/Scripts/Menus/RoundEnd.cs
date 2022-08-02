using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class RoundEnd : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioSource _musicAudioSource;

    [SerializeField] private AudioClip winMusic;

    [SerializeField] private AudioClip loseMusic;

    [SerializeField] private AudioClip firstPlaceMusic;
    
    [SerializeField] private List<TextMeshProUGUI> PlayerScoresText;

    [SerializeField] private List<Slider> Sliders;

    [SerializeField] private List<Image> Images;
    
    [SerializeField] private List<Image> Icons;

    [SerializeField] private GameObject ReplayLevelButton;

    [SerializeField] private GameObject NextLevelButton;

    [SerializeField] private Sprite PlayerWitchSprite;

    [SerializeField] private Sprite NpcWitchSprite;


    public Gradient colorSwap; 
    
    #region Globals

    private readonly int LOWEST_SCORE = 0;

    private readonly int FIRST_LEVEL = 1;
    
    #endregion
    
    
    void Start()
    {    print("{" + GameManager._ScoreList[0] + " " + GameManager._ScoreList[1] + " " + GameManager._ScoreList[2] + " " + GameManager._ScoreList[3] + "}");
        GameManager._ScoreList.Sort();
        _musicAudioSource.clip = GetMusic();
        
        StartCoroutine(ScoreAnimation(GameManager._ScoreList));
        

    }

    
    #region Private Methods


    private int CheckIdenticalScores()
    {
        List<int> indexesWithPlayerScore = new List<int>();
        for (int i = 0; i < GameManager._ScoreList.Count; i++)
        {
            if (GameManager._ScoreList[i] == GameManager.PlayerScore)
            {
                indexesWithPlayerScore.Add(i);
            }
        }

        if (indexesWithPlayerScore.Count == 0)
            return 3;
        
        return indexesWithPlayerScore[indexesWithPlayerScore.Count - 1];
    }
    private void DisplayImages()
    {
        int playerScoreIndex = CheckIdenticalScores();

        for (int i = 0; i < Icons.Count; i++)
        {
            if (i == playerScoreIndex)
                Icons[i].sprite = PlayerWitchSprite;
            else
                Icons[i].sprite = NpcWitchSprite;

        }
        
    }
    
    private AudioClip GetMusic()
    {   
        // player got to first place
        if (GameManager._ScoreList[GameManager._ScoreList.Count - 1] == GameManager.PlayerScore)
        {
            return firstPlaceMusic;
        }
        
        // Player got to last place 
        if (GameManager._ScoreList[LOWEST_SCORE] == GameManager.PlayerScore)
        {   
            // check if player is in tie with another witch
            for (int i = 1; i < GameManager._ScoreList.Count; i++)
            {
                if (GameManager._ScoreList[i] == GameManager.PlayerScore)
                {
                    return winMusic;
                }

            }

            return loseMusic;

            
        }

        return winMusic;
    }

    private void PlayerCanPassToNextLevel()
    {
        NextLevelButton.SetActive(true);
    }

    private void PlayerNeedsToReplayLevel()
    {
        ReplayLevelButton.SetActive(true);
    }

    private void PlayerWonTheGame()
    {
        Debug.Log("Player Won");
    }
    
    private IEnumerator ScoreAnimation(List<int> scores)
    {   
        DisplayImages();
        
        for (int i = 0; i < scores.Count; i++)
        {   
            Debug.Log("got here");    
            for (int j = 0; j <= scores[i]; j++)
            {
                Sliders[i].value = j;
                PlayerScoresText[i].text = j.ToString();
                Images[i].color = colorSwap.Evaluate(scores[i] / Sliders[i].maxValue);
                _audioSource.Play();
                yield return new WaitForSeconds(0.10f);
                
            }
        }
        _audioSource.Pause();
        _musicAudioSource.Play();
        
    }
    
    
    
    #endregion
    
    #region Public Methods
    
    public void NextLevel()
    {
        GameManager.levelNumber++;
        SceneManager.LoadScene("level" + GameManager.levelNumber);
    }

    IEnumerator LoadAndWait(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }
    
    public void ReplayLevel()
    {
        StartCoroutine(LoadAndWait("Level 1"));

    }

    public void ReturnMainMenu()
    {
        GameManager.levelNumber = FIRST_LEVEL;
        StartCoroutine(LoadAndWait("MainMenu"));
    }
    
    #endregion

   
}
