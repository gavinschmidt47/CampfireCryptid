using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GlobalData globalData;

    public GameObject gameOverScreen;
    public GameObject starveScreen;


    [Header("Other objects")]
    public Slider fireSlider; // Reference to the UI slider for fire life
    public Image fireSliderFill; // Reference to the fill image of the fire slider
    public RectTransform fireSliderHandle; // Reference to the handle of the fire slider


    // Update is called once per frame
    void Update()
    {
        globalData.TimeSetDown(Time.deltaTime);
        globalData.FireSetDown(Time.deltaTime);

        // Update the fire slider value and fill color based on fire life
        fireSlider.value = globalData.secondsLeftBeforeFireDies; // Update slider value from Global Data
        // Update the fire slider fill color and handle size based on fire life
        float t = Mathf.Clamp01(globalData.secondsLeftBeforeFireDies / 100f);
        fireSliderFill.color = Color.Lerp(Color.yellow, Color.red, t);
        fireSliderHandle.localScale = Vector3.Lerp(new Vector3(0.66f, 0.66f, 0.66f), new Vector3(2.6f, 2.6f, 2.6f), t);

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

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit: " + hit.collider.name);
                if (hit.collider.CompareTag("Effigy"))
                {
                    SceneManager.LoadScene("EffigyRemover");
                }
                else if (hit.collider.CompareTag("Bobber"))
                {
                    SceneManager.LoadScene("Fishing");
                }
                else if (hit.collider.CompareTag("Fire"))
                {
                    SceneManager.LoadScene("FireTending");
                }
            }
        }
    }
}
