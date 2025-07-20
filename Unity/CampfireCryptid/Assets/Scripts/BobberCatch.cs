using System.Collections;
using System.IO.Compression;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BobberCatch : MonoBehaviour
{
    public GlobalData globalData; // Reference to the GlobalData scriptable object
    [SerializeField] LayerMask fishLayer;
    GameObject targetFish;

    float fishFound;
    bool waitForFish;
    float fishDelay = 2f;

    // Update is called once per frame
    void Update()
    {
        globalData.TimeSetDown(Time.deltaTime);
        globalData.FireSetDown(Time.deltaTime);
        Collider[] fish = Physics.OverlapSphere(transform.position, 0.25f, fishLayer);
        if (fish.Length > 0 && !waitForFish)
        {
            Debug.Log("Found One");
            HookFish();
            targetFish = fish[0].gameObject;
        }
        if (waitForFish)
        {
            if (Time.time - fishFound >= fishDelay)
            {
                waitForFish = false;
                if (Random.Range(1, 10) >= 4) CatchFish();
                else LoseFish();
            }
        }
    }

    void HookFish()
    {
        Debug.Log("Wait for it");
        waitForFish = true;
        fishFound = Time.time;
        
    }

    void CatchFish()
    {
        Debug.Log("Success");
        globalData.minigameProgress[0]++;
        if (globalData.minigameProgress[0] >= 25)
        {
            globalData.minigamesCompleted[0] = true;
            Debug.Log("Fishing minigame completed!");
        }
        else
        {
            Debug.Log("Caught fish! Progress: " + globalData.minigameProgress[0]);
        }
        DestroyImmediate(targetFish);
        Destroy(gameObject);
    }

    void LoseFish()
    {
        Debug.Log("Lost it");
        DestroyImmediate(targetFish);
        Destroy(gameObject);
    }
}
