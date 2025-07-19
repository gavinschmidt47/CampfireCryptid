using UnityEngine;
using UnityEngine.InputSystem;


public class ShhotingEyes : MonoBehaviour
{
    public GameObject gunCrosshair;
    public GameObject eyes;
    public float upperBound = 5.5f;
    public float lowerBound = -3.5f;
    public float leftBound = -8.5f;
    public float rightBound = 8.5f;
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

        float speed = 4f;
        Vector3 movement = new Vector3(moveX, moveY, 0f) * speed * Time.deltaTime;
        gunCrosshair.transform.position += movement;
        if(gunCrosshair.transform.position.x < leftBound)
        {
            gunCrosshair.transform.position = new Vector3(leftBound, gunCrosshair.transform.position.y, gunCrosshair.transform.position.z);
        }
        if (gunCrosshair.transform.position.y > upperBound)
        {
            gunCrosshair.transform.position = new Vector3(gunCrosshair.transform.position.x, upperBound, gunCrosshair.transform.position.z);
        }
        if (gunCrosshair.transform.position.y < lowerBound)
        {
            gunCrosshair.transform.position = new Vector3(gunCrosshair.transform.position.x, lowerBound, gunCrosshair.transform.position.z);
        }
        if (gunCrosshair.transform.position.x > rightBound)
        {
            gunCrosshair.transform.position = new Vector3(rightBound, gunCrosshair.transform.position.y, gunCrosshair.transform.position.z);
        }
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
        float randomX = Random.Range(leftBound, rightBound);
        float randomY = Random.Range(lowerBound, upperBound);
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
        // Get the position of the eye
        Vector3 eyePosition = eye.transform.position;
        // Get the position of the crosshair
        Vector3 crosshairPosition = gunCrosshair.transform.position;
        // Check if the eye is within the bounds of the crosshair
        return (eyePosition.x >= crosshairPosition.x - 0.5f && eyePosition.x <= crosshairPosition.x + 0.5f &&
                eyePosition.y >= crosshairPosition.y - 0.5f && eyePosition.y <= crosshairPosition.y + 0.5f);
    }



}
