using UnityEngine;
using UnityEngine.InputSystem;


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
    public Canvas canvas;

    private float spawnTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Keyboard.current.aKey.isPressed ? -1 :
                      Keyboard.current.dKey.isPressed ? 1 : 0;
        float moveY = Keyboard.current.sKey.isPressed ? -1 :
                      Keyboard.current.wKey.isPressed ? 1 : 0;

        float speed = 200f; 
        Vector2 movement = new Vector2(moveX, moveY) * speed * Time.deltaTime;
        gunCrosshair.anchoredPosition += movement;
        gunCrosshair.anchoredPosition = new Vector2(
            Mathf.Clamp(gunCrosshair.anchoredPosition.x, crosshairLeftBound, crosshairRightBound),
            Mathf.Clamp(gunCrosshair.anchoredPosition.y, crosshairLowerBound, crosshairUpperBound)
        );
        // Every Second, spawn a new eye
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= 1f)
        {
            SpawnEyes();
            spawnTimer = 0f;
        }

        // If space bar is pressed, shoot the eyes
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShootEyes();
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
        // Get all eyes in the scene
        GameObject[] allEyes = GameObject.FindGameObjectsWithTag("Eye");
        foreach (GameObject eye in allEyes)
        {
            // Check if the eye is within the crosshair bounds
            if (IsEyeWithinCrosshair(eye))
            {
                Destroy(eye);
            }
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




}
