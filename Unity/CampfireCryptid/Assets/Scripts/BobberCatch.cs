using System.Collections;
using System.IO.Compression;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BobberCatch : MonoBehaviour
{
    [SerializeField] LayerMask fishLayer;
    GameObject targetFish;

    float fishFound;
    bool waitForFish;
    float fishDelay = 2f;

    // Update is called once per frame
    void Update()
    {
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
        DestroyImmediate(targetFish);
    }

    void LoseFish()
    {
        Debug.Log("Lost it");
        DestroyImmediate(targetFish);
    }
}
