using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EffigyScript : MonoBehaviour
{
    public GameObject effigyPrefab; // Prefab for the effigies
    public int numberOfEffigies; // Number of effigies in the scene
    public int maxNumberOfEffigies = 10; // Maximum number of effigies allowed
    public GlobalData globalData; // Reference to the GlobalData scriptable object


    void Start()
    {
        List<Vector3> effigyPositions = new List<Vector3>(); // List to store the positions of the effigies
        effigyPositions.Add(new Vector3(4.41400003f, 1.75999999f, -1.58000004f));
        effigyPositions.Add(new Vector3(-2.82800007f, 1.18200004f, -4.33199978f));
        effigyPositions.Add(new Vector3(-2.82800007f, 2.44799995f, -4.33199978f));
        effigyPositions.Add(new Vector3(-2.82800007f, 6.04799986f, -4.33199978f));
        effigyPositions.Add(new Vector3(0.148000002f, 1.21899998f, -1.78699994f));
        effigyPositions.Add(new Vector3(0.148000002f, 2.86299992f, -1.78699994f));
        effigyPositions.Add(new Vector3(0.148000002f, 4.57000017f, -1.78699994f));
        effigyPositions.Add(new Vector3(-2.1400001f, 0.860000014f, -1.26999998f));
        effigyPositions.Add(new Vector3(-2.18000007f, 2.68000007f, -0.959999979f));
        effigyPositions.Add(new Vector3(-1.95099998f, 5.03999996f, -1.27999997f));
        effigyPositions.Add(new Vector3(-11.257f, 1.18200004f, -1.30299997f));
        effigyPositions.Add(new Vector3(-11.1409998f, 3.50399995f, -1.48800004f));
        effigyPositions.Add(new Vector3(-11.1730003f, 6.1420002f, -1.28299999f));
        effigyPositions.Add(new Vector3(-11.1730003f, 8.86999989f, -1.28299999f));
        effigyPositions.Add(new Vector3(-8.98999977f, 6.67999983f, 2.68000007f));
        effigyPositions.Add(new Vector3(-8.98999977f, 2.77999997f, 2.68000007f));
        effigyPositions.Add(new Vector3(-5.2420001f, 0.845000029f, 2.96799994f));
        effigyPositions.Add(new Vector3(-5.2420001f, 3.49300003f, 2.96799994f));
        effigyPositions.Add(new Vector3(-5.2420001f, 8.43000031f, 2.96799994f));
        effigyPositions.Add(new Vector3(0.714999974f, 2.55999994f, 2.59599996f));
        effigyPositions.Add(new Vector3(0.714999974f, 4.61000013f, 2.59599996f));
        effigyPositions.Add(new Vector3(-0.617999971f, 4.61000013f, 4.32499981f));
        effigyPositions.Add(new Vector3(-0.617999971f, 1.31099999f, 4.32499981f));
        effigyPositions.Add(new Vector3(-0.617999971f, 3.38199997f, 4.32499981f));
        effigyPositions.Add(new Vector3(1.91400003f, 3.38199997f, 4.1079998f));
        effigyPositions.Add(new Vector3(1.91400003f, 1.39400005f, 4.1079998f));
        effigyPositions.Add(new Vector3(4.95300007f, 3.023f, 3.17000008f));
        effigyPositions.Add(new Vector3(4.95300007f, 1.63f, 3.17000008f));
        
        numberOfEffigies = Random.Range(5, maxNumberOfEffigies + 1); // Randomly determine the number of effigies to spawn

        for (int i = 0; i < numberOfEffigies; i++)
        {
            Debug.Log("Spawning effigy " + (i + 1) + " of " + numberOfEffigies);
            int j = Random.Range(0, effigyPositions.Count); // Randomly select a position from the list
            if (!effigyPositions[j].Equals(Vector3.zero)) // Check if the position is not zero
            {
                // Instantiate the effigy at the specified position
                Instantiate(effigyPrefab, effigyPositions[j], Quaternion.identity);
                effigyPositions[j] = Vector3.zero; // Mark this position as used
            }
        }
    }

    // ... (rest of your code)

    void Update()
    {
        globalData.TimeSetDown(Time.deltaTime);
        globalData.FireSetDown(Time.deltaTime);
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit: " + hit.collider.name);
                if (hit.collider.CompareTag("Effigy"))
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Effigy destroyed!");
                    globalData.minigameProgress[1]++;
                    if (globalData.minigameProgress[1] >= 25)
                    {
                        globalData.minigamesCompleted[1] = true;
                        Debug.Log("Effigies minigame completed!");
                    }
                }
            }
        }
        // Check for Enter key press to go to main scene
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
