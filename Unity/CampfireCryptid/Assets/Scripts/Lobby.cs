using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GlobalData globalData;

    public GameObject gameOverScreen;
    public GameObject starveScreen;

    // Update is called once per frame
    void Update()
    {
        globalData.TimeSetDown(Time.deltaTime);
        globalData.FireSetDown(Time.deltaTime);

        if (globalData.gameOver)
        {

            if (!globalData.IsMinigameCompleted(0))
                starveScreen.SetActive(true);
            else
                gameOverScreen.SetActive(true);

            // Disable the game object to stop further updates
            Time.timeScale = 0f; // Pause the game
            this.enabled = false; // Disable this script
        }
        if (globalData.fireOut)
        {
            // Change scene when fire is out
            UnityEngine.SceneManagement.SceneManager.LoadScene("ShootingEyes");
        }
    }
}
