using UnityEngine;
using UnityEngine.AI;

public class FishBehavior : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] GameObject bobberPrefab;
    GameObject bobberTarget;
    float detectBobberRange = 2;

    [SerializeField] LayerMask navLayer, bobberLayer;

    //Movement
    Vector3 destination;
    bool destinationSet;
    [SerializeField] float movementRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("Swim", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

        //Random Swimming Pattern
        // Swim();

        //Detect Nearby Bobber
        Collider[] bobbers = Physics.OverlapSphere(transform.position, 1, bobberLayer);
        foreach (Collider hitCollider in bobbers)
        {
            GameObject hitObject = hitCollider.gameObject;
            bobberTarget = hitObject;
            // Debug.Log("Found Bobber: " + hitObject.name);
        }
        
        //Continue Swimming if Bobber isnt detected
        if (bobberTarget == null) return;
        else //Swim toward bobber
        {
            float distanceToBobber = Vector3.Distance(transform.position, bobberTarget.transform.position);
            if (distanceToBobber >= detectBobberRange) return;
            else
            {
                destination = new Vector3(bobberTarget.transform.position.x, transform.position.y, bobberTarget.transform.position.z);
                agent.SetDestination(destination);
            }
        }
    }

    void Swim()
    {
        if (!destinationSet) FindDestination();
        if (destinationSet) agent.SetDestination(destination);
        if (Vector3.Distance(transform.position, destination) < 10) destinationSet = false;
    }

    void FindDestination()
    {
        float z = Random.Range(-movementRange, movementRange);
        float x = Random.Range(-movementRange, movementRange);

        destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destination, Vector3.down, navLayer))
        {
            destinationSet = true;
        }
    }
}
