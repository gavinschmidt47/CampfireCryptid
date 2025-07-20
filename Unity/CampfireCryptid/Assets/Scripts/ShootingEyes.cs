using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ShhotingEyes : MonoBehaviour
{
    public RectTransform gunCrosshair;
    public GameObject[] eyePrefabs;
    public GlobalData globalData;
    public float eyeSpawnUpperBound = 5.5f;
    public float eyeSpawnLowerBound = -3.5f;
    public float eyeSpawnLeftBound = -8.5f;
    public float eyeSpawnRightBound = 8.5f;
    public float crosshairUpperBound = 177.0f;
    public float crosshairLowerBound = -176.0f;
    public float crosshairLeftBound = -353.0f;
    public float crosshairRightBound = 356.0f;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.2f;
    public Canvas canvas;
    public AudioClip shotgunSound; 
    private AudioSource audioSource;
    public Slider health;
    private float spawnTimer = 0f;
    private float healthTimer = 0f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (health != null)
        {
            health.value = health.maxValue * 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        globalData.TimeSetDown(Time.deltaTime);
        globalData.FireSetDown(Time.deltaTime);
        float moveX = Keyboard.current.aKey.isPressed ? -1 :
                      Keyboard.current.dKey.isPressed ? 1 : 0;
        float moveY = Keyboard.current.sKey.isPressed ? -1 :
                      Keyboard.current.wKey.isPressed ? 1 : 0;

        float speed = 500f; 
        Vector2 movement = new Vector2(moveX, moveY) * speed * Time.deltaTime;
        gunCrosshair.anchoredPosition += movement;
        gunCrosshair.anchoredPosition = new Vector2(
            Mathf.Clamp(gunCrosshair.anchoredPosition.x, crosshairLeftBound, crosshairRightBound),
            Mathf.Clamp(gunCrosshair.anchoredPosition.y, crosshairLowerBound, crosshairUpperBound)
        );
        // Every Second, spawn a new eye
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= 0.5f)
        {
            SpawnEyes();
            spawnTimer = 0f;
        }

        healthTimer += Time.deltaTime;
        if (healthTimer >= 1.5f)
        {
            if (health != null)
            {
                health.value = Mathf.Max(health.minValue, health.value - 10f);
            }
            healthTimer = 0f;
        }

        // If space bar is pressed, shoot the eyes
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShootEyes();
        }
        if (health.value >= 100)
        {
            globalData.secondsLeftBeforeFireDies = 100f;
            SceneManager.LoadScene("Lobby");
        }
        if (health.value <= 0)
        {
            Debug.Log("You lose!");
            // Do Game Over Logic here
        }
    }

    public void SpawnEyes()
    {
        int randomIndex = Random.Range(0, eyePrefabs.Length);
        GameObject selectedEye = eyePrefabs[randomIndex];
        GameObject newEyes = Instantiate(selectedEye, canvas.transform);
        newEyes.tag = "Eye";

        RectTransform eyeRect = newEyes.GetComponent<RectTransform>();
        float randomX = Random.Range(eyeSpawnLeftBound, eyeSpawnRightBound);
        float randomY = Random.Range(eyeSpawnLowerBound, eyeSpawnUpperBound);
        eyeRect.anchoredPosition = new Vector2(randomX, randomY);

        // Move crosshair to be the last child so it's always on top
        gunCrosshair.SetAsLastSibling();

        Destroy(newEyes, 3f);
    }




    public void ShootEyes()
    {
        if (shotgunSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shotgunSound);
        }
        StartCoroutine(ScreenShake());
        // Get all eyes in the scene
        GameObject[] allEyes = GameObject.FindGameObjectsWithTag("Eye");
        bool shotAny = false;
        foreach (GameObject eye in allEyes)
        {
            // Check if the eye is within the crosshair bounds
            if (IsEyeWithinCrosshair(eye))
            {
                Destroy(eye);
                shotAny = true;
            }
        }
        if (shotAny && health != null)
        {
            health.value = Mathf.Min(health.maxValue, health.value + 10f);
        }
    }

    private bool IsEyeWithinCrosshair(GameObject eye)
    {
        RectTransform eyeRect = eye.GetComponent<RectTransform>();
        if (eyeRect == null)
        {
            Debug.LogWarning("Missing RectTransform on: " + eye.name);
            return false;
        }
        RectTransform crosshairRect = gunCrosshair;

        Vector3[] crosshairCorners = new Vector3[4];
        Vector3[] eyeCorners = new Vector3[4];
        crosshairRect.GetWorldCorners(crosshairCorners);
        eyeRect.GetWorldCorners(eyeCorners);

        // Create the crosshair rect and shrink it by a scale factor 
        float shrinkFactor = 0.15f; 
        Vector2 crosshairSize = (crosshairCorners[2] - crosshairCorners[0]);
        Vector2 crosshairCenter = (crosshairCorners[0] + crosshairCorners[2]) * 0.5f;
        Vector2 newSize = crosshairSize * shrinkFactor;
        Vector2 newMin = crosshairCenter - newSize * 0.5f;
        Rect crosshairWorldRect = new Rect(newMin, newSize);

        Rect eyeWorldRect = new Rect(eyeCorners[0], eyeCorners[2] - eyeCorners[0]);

        return crosshairWorldRect.Overlaps(eyeWorldRect);
    }




    private IEnumerator ScreenShake()
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            Camera.main.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }




}
