using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    #region Inspector
    
    [SerializeField] private GameObject PauseMenu;

    [SerializeField] private GameObject GameUI;
    
    [SerializeField] private GameObject CorrectBarcodeScan;

    [SerializeField] private GameObject EnterBarcode;
    
    [SerializeField] private GameObject ErrorBarcode;

    [SerializeField] private GameObject KeepGoing;

    [SerializeField] private GameObject stealCustomer;
    
    public TextMeshProUGUI textCode;
   
    #endregion

    public static int levelNumber = 1;
    public static List<int> _ScoreList = new List<int>(){30, 40, 20, 10};
    private bool _wrongCode;
    public static bool Success;
    public static bool LeftProduct;
    public static int PlayerScore = 0;
    public static string EnterCode;

    //private bool _isGamePaused = false;

    private const float MinTimeForSteal = 40;
    private const float MaxTimeForSteal = 70;
    private float _timeForSteal;
    
    
    #region MonoBehaviour functions

    private void Start()
    {
        products._onCounter = false;
        _timeForSteal = Random.Range(MinTimeForSteal, MaxTimeForSteal);
        PlayerScore = 0;
    }

    private void Update()
    {

        _timeForSteal -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();

        if (_timeForSteal <= 0)
        {
            var newPosition = new Vector3(-9.5f, -0.4f, 0);
            Instantiate(stealCustomer, newPosition, Quaternion.identity);
            _timeForSteal = Random.Range(MinTimeForSteal, MaxTimeForSteal);
        }
        
        if (LeftProduct)
        {
            textCode.enabled = false;
            EnterBarcode.SetActive(false);
            LeftProduct = false;
        }
        
        foreach (var product in GameObject.FindGameObjectsWithTag("Product"))
        {
            if (product != null && product.GetComponent<products>().firstOnCounter)
            {
                DisplayWeighingCode(product.GetComponent<products>());
            }
        }
        
    }       
    
    private void DisplayWeighingCode(products product)
    {
        if (product.code == "" || !product.onWeight || Success) return;
        if (!_wrongCode)
            EnterBarcode.SetActive(true);

        if (!manager.ButtonsEnabled)
        {
            EnterCode = "";
            return;
        }
        
        textCode.text = product.code;
        textCode.enabled = true;
        EnterCode += Input.inputString;
        switch (EnterCode.Length)
        {
            case 3 when EnterCode == product.code:
                _wrongCode = false;
                Success = true;
                EnterBarcode.SetActive(false);
                StartCoroutine(DisplayMessageAndWait(CorrectBarcodeScan));
                textCode.enabled = false;
                product.SuccessProduct();
                break;
            case 3:
                EnterCode = "";
                EnterBarcode.SetActive(false);
                _wrongCode = true;
                StartCoroutine(DisplayMessageAndWait(ErrorBarcode));
                break;
        }
    }


    public void EndRound()
    {
        _ScoreList = FindObjectOfType<ScoreManager>()._scores;
        if (_ScoreList.Count <= 1) return;
        var playerScore = _ScoreList[0];
        int countScores = 0;
        for (var i = 1; i < _ScoreList.Count; i++)
        {
            if (_ScoreList[i] > playerScore)
            {
                countScores++;
            }
        }

        if (countScores == _ScoreList.Count - 1)
        {
            GameOver();
            return;
        }
        NextRound();
    }

    private void NextRound()
    {
        SceneManager.LoadScene("ScoresDisplay");
    }


    private static void GameOver()
    {
        SceneManager.LoadScene("ScoresDisplay");
    }

    #endregion

    
    
    // this function pauses the game and brings up the pause menu. 
    private void PauseGame()
    {
        GameUI.SetActive(false);
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        GameUI.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }


    IEnumerator DisplayMessageAndWait(GameObject action)
    {
        action.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        action.SetActive(false);
        if (action.CompareTag("Correct"))
        {
            KeepGoing.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            KeepGoing.SetActive(false);
        }
        
    }

    
}