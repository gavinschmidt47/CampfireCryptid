using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GlobalData globalData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        globalData.secondsLeftBeforeNight = 1200f;
        globalData.secondsLeftBeforeFireDies = 120f;
        globalData.currDay = 0;
        globalData.gameOver = false;
        globalData.fireOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting");
    }

}
