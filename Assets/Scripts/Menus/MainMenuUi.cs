using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUi : MonoBehaviour
{
    
    [SerializeField] private string StartGameScene = "Type level 1 scene here";

    [SerializeField] private string HowToPlayScene = "Type how to play screen here";

    [SerializeField] private string ScoreBoardScene = "Type score board scene here";

    IEnumerator waitAndLoad(string sceneName)
    {   
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(sceneName);
    }
    
    public void StartGame()
    {
        StartCoroutine(waitAndLoad(StartGameScene));
    }

    public void LoadHowToPlay()
    {   
        StartCoroutine(waitAndLoad(HowToPlayScene));
    }

    public void LoadScoreBoard()
    {   
        StartCoroutine(waitAndLoad(ScoreBoardScene));
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
