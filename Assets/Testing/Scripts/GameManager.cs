using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManagerTesting : MonoBehaviour
{
    #region Inspector
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private GameObject paperMessage;
    [SerializeField] private GameObject managerCameMessage;
    #endregion

    #region privatePropeties
    
    private const float PaperOver = 20;
    private const float ManagerTime = 10;
    private const int ProductLimit = 10;
    private float _timeForPaper = 20;
    private float minWaitTime = 2;
    private float maxWaitTime = 7;
    private Vector3 _newPosition;
    private bool _callManager;
    
    #endregion

    #region staticProperties
    
    public static bool ButtonsEnabled = true;
    public static bool ProductTaken = false;
    public static int CountProduct;

    #endregion

    #region MonoBehaviour functions

    void Start()
    {
        _newPosition = new Vector3(-9.53743649f, 1.21942508f, 0);
        StartCoroutine(AppearProducts());
    }

    private void Update()
    {
        _timeForPaper -= Time.deltaTime;
        if (_timeForPaper <= 0 && ButtonsEnabled)
        {
            ButtonsEnabled = false;
            print("No more paper, wait for the manager");
            paperMessage.SetActive(true);
            _timeForPaper = 0;
        }
        else if (_timeForPaper <= 0 && _callManager)
        {
            print("The manager came, you can continue");
            managerCameMessage.SetActive(true);
            _timeForPaper = PaperOver;
            ButtonsEnabled = true;
            _callManager = false;
        }

        if (_timeForPaper < PaperOver - 1.5f && ButtonsEnabled)
            managerCameMessage.SetActive(false);
        if (CountProduct == ProductLimit)
            GameOver();
    }

    private void GameOver()
    {
        print("Game Over");
        Time.timeScale = 0;
    }

    #endregion
    
   
    /*
     * Appears product on the screen in randomly time and
     * randomly products
     */
    private IEnumerator AppearProducts()
    {
        while (true)
        {
            Vector2 position = _newPosition;
            GameObject product = prefabs[Random.Range(0, prefabs.Count)];
            Instantiate(product, position, Quaternion.identity);
            CountProduct++;
            yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
        }
    }

    public void CallManagerButton()
    {
        if (ButtonsEnabled) return;
        _timeForPaper = ManagerTime;
        _callManager = true;
        paperMessage.SetActive(false);
    }
}