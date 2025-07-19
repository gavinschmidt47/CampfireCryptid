using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.InputSystem;

public class CastFishingBobber : MonoBehaviour
{
    [SerializeField] public GameObject bobberPrefab;
    Vector3 bobberPosition;
    Quaternion bobberRotation;

    GameObject bobberInstance;
    bool bobberExists = false;

    void Start()
    {

    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get the mouse position
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Create a ray from the camera
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);
                //Spawns bobber at point where raycast hit the surface
                bobberPosition = hit.point;
                if (!bobberExists) SpawnBobber();
                else ReelInBobber();
            }
        }
    }

    void SpawnBobber()
    {
        bobberInstance = Instantiate(bobberPrefab, bobberPosition, Quaternion.identity);
        bobberExists = true;
    }

    void ReelInBobber()
    {
        DestroyImmediate(bobberInstance);
        bobberExists = false;
    }

}
