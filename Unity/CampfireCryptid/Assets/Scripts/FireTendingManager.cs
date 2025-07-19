using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FireTendingManager : MonoBehaviour
{
    public GlobalData globalData; // Reference to the GlobalData scriptable object
    private Camera mainCam; // Reference to the main camera

    [Header("Fire Tending Settings")]
    [SerializeField]
    private int numSticksToSpawn = 10; // Number of sticks to spawn
    [SerializeField]
    private GameObject stickPrefab; // Prefab for the stick object
    [SerializeField]
    private Vector2 spawnRadius = new Vector2(5f, 5f); // Radius within which sticks will be spawned
    [SerializeField]
    private int numSticksToCollect = 5; // Number of sticks required to tend the fire

    [Header("Other objects")]
    public Slider fireSlider; // Reference to the UI slider for fire life
    public Image fireSliderFill; // Reference to the fill image of the fire slider
    public RectTransform fireSliderHandle; // Reference to the handle of the fire slider

    [Header("Post-Processing Effect")]
    [SerializeField]
    private Image staticEffect; // Material for the post-processing effect


    // In-game trackers
    private int SticksCollected = 0; // Counter for collected sticks
    private bool collectingSticks = true; // Flag to check if collecting sticks is active
    private bool isDragging = false; // Flag to check if the player is dragging a stick
    private GameObject draggedStick; // Reference to the currently dragged stick
    private float fireLife = 0f; // Variable to track the fire's life
    private bool transitioning = false; // Flag to check if a transition is in progress
    private float effectAmount = 0f; // Amount of the post-processing effect to apply

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SticksCollected = 0; // Initialize the counter

        // Spawn sticks at random positions within the specified radius
        SpawnSticks(numSticksToSpawn);
        
        // Ensure proper initialization of used game objects
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Main camera not found. Please ensure a camera with the 'MainCamera' tag is present in the scene.");
        }
    }

        // Update is called once per frame
        void Update()
    {
        // Check for mouse input
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get the mouse position
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Create a ray from the camera
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            // Check if the ray hits a stick object
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Sticks"))
            {
                // Check if in collectingSticks
                if (collectingSticks)
                {
                    SticksCollected++;
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    // If not collecting sticks, allow dragging the stick
                    isDragging = true;
                    draggedStick = hit.collider.gameObject; // Store the dragged stick reference
                }
            }
        }

        // Check if the left mouse button is released to place the stick at the fire
        if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            
            // Get the mouse position
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            // Create a ray from the camera
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            // RaycastAll to go past held object
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Fire"))
                {
                    // Move the dragged stick to the mouse position
                    draggedStick.transform.position = new Vector3(hit.point.x, 1.6f, hit.point.z);

                    Destroy(draggedStick); // Destroy the stick after placing it at the fire

                    fireLife += Random.Range(5f, 10f); // Increase fire life by a random amount
                    fireSlider.value = fireLife;

                    // Update the fire slider fill color and handle size based on fire life
                    float t = Mathf.Clamp01(fireLife / 100f);
                    fireSliderFill.color = Color.Lerp(Color.yellow, Color.red, t);
                    fireSliderHandle.localScale = Vector3.Lerp(new Vector3(0.66f, 0.66f, 0.66f), new Vector3(2.6f, 2.6f, 2.6f), t);

                    if (fireLife > 100f)
                    {
                        fireLife = 100f; // Cap the fire life at 100
                    }
                    break;
                }
            }

            // Stop dragging when the left mouse button is released
            isDragging = false;
            draggedStick = null; // Clear the dragged stick reference
        }

        // Move camera if Enter is pressed
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if(collectingSticks && SticksCollected >= numSticksToCollect)
            {
                collectingSticks = false; // Stop collecting sticks

                StartCoroutine(TransitionRoutine(mainCam, new Vector3(0, 5.6f, -25)));

                // Show sticks at the fire
                ShowSticksAtFire(numSticksToCollect); // Show remaining sticks at the fire

                // Reset stick counter
                SticksCollected -= numSticksToCollect;
            }
            else if (!collectingSticks)
            {
                
                // Start transition effect
                StartCoroutine(TransitionRoutine(mainCam, new Vector3(0, 7.8f, -12.26f)));

                collectingSticks = true; // Resume collecting sticks
                SpawnSticks(numSticksToSpawn); // Spawn more sticks if needed
            }
        }

        // Handle dragging of the stick
        if (isDragging && draggedStick != null)
        {
            // Get the mouse position
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            // Create a ray from the camera
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Move the dragged stick to the mouse position
                draggedStick.transform.position = new Vector3(hit.point.x, 1.6f, hit.point.z + 0.3f);
            }
        }
    }

    private void SpawnSticks(int numQueuedSticks)
    {
        // Spawn additional sticks if needed
        for (int i = 0; i < numQueuedSticks; i++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(-spawnRadius.x, spawnRadius.x),
                Random.Range(-spawnRadius.y, spawnRadius.y)
            );
            Instantiate(stickPrefab, new Vector3(randomPosition.x, 1.6f, randomPosition.y), Quaternion.Euler(-90f, 0f, 0f));
        }
    }

    private void ShowSticksAtFire(int numSticks)
    {
        for (int i = 0; i < numSticks; i++)
        { 
            Instantiate(stickPrefab, new Vector3(Random.Range(-spawnRadius.x, spawnRadius.x), 1.6f, -18 + Random.Range(-spawnRadius.y, spawnRadius.y)), Quaternion.Euler(-90f, 0f, 0f)); 
        }
    }

    IEnumerator TransitionRoutine(Camera mainCam, Vector3 newPosition)
    {
        if (transitioning) yield break; // Exit if already transitioning
        staticEffect.gameObject.SetActive(true); // Enable the static effect
        transitioning = true;

        // Fade in effect
        for (float t = 0; t < 1f / 2f; t += Time.deltaTime)
        {
            effectAmount = Mathf.Lerp(0, 1, t / (1f / 2f));

            staticEffect.material.SetFloat("_StaticStrength", effectAmount);

            yield return null;
        }
        effectAmount = 1f;

        staticEffect.material.SetFloat("_StaticStrength", effectAmount);

        // Move the camera up and forwards
        mainCam.transform.position = newPosition;

        // Fade out effect
        for (float t = 0; t < 1f / 2f; t += Time.deltaTime)
        {
            effectAmount = Mathf.Lerp(1, 0, t / (1f / 2f));

            staticEffect.material.SetFloat("_StaticStrength", effectAmount);

            yield return null;
        }
        effectAmount = 0f;

        staticEffect.material.SetFloat("_StaticStrength", effectAmount);

        // Reset the static effect
        staticEffect.gameObject.SetActive(false); // Disable the static effect
        transitioning = false;
    }
}