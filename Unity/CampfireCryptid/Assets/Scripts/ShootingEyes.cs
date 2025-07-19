using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;


public class ShhotingEyes : MonoBehaviour
{
    public RectTransform gunCrosshair;
    public GameObject eyes;
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
        float moveX = Keyboard.current.aKey.isPressed ? -1 :
                      Keyboard.current.dKey.isPressed ? 1 : 0;
        float moveY = Keyboard.current.sKey.isPressed ? -1 :
                      Keyboard.current.wKey.isPressed ? 1 : 0;

        float speed = 400f; 
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
        if (healthTimer >= 3f)
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
            Debug.Log("You win!");
        }
        if (health.value <= 0)
        {
            Debug.Log("You lose!");
            // Do Game Over Logic here
        }
    }

    public void SpawnEyes()
    {
        // Get random position within bounds
        float randomX = Random.Range(eyeSpawnLeftBound, eyeSpawnRightBound);
        float randomY = Random.Range(eyeSpawnLowerBound, eyeSpawnUpperBound);
        // Instantiate the eyes at the random position
        GameObject newEyes = Instantiate(eyes, new Vector3(randomX, randomY, -1f), Quaternion.identity);
        // Quaternion.identity means no rotation

        // After 3 seconds if it is not shot destory the eye
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
        // Convert crosshair anchoredPosition (UI) to world position
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, gunCrosshair.position);
        Vector3 crosshairWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Mathf.Abs(Camera.main.transform.position.z + 1f)));

        Vector3 eyePosition = eye.transform.position;

        // Check if the eye is within the bounds of the crosshair 
        return (eyePosition.x >= crosshairWorldPos.x - 1.0f && eyePosition.x <= crosshairWorldPos.x + 1.0f &&
                eyePosition.y >= crosshairWorldPos.y - 1.0f && eyePosition.y <= crosshairWorldPos.y + 1.0f);
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
