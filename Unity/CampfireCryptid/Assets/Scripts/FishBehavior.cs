using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FishBehavior : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] GameObject bobberPrefab;
    GameObject bobberTarget;
    float detectBobberRange = 100;

    [SerializeField] LayerMask navLayer;

    //Movement
    Vector3 destination;
    bool destinationSet;
    [SerializeField] float movementRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        Swim();

        bobberTarget = GameObject.FindGameObjectWithTag("").get
        if (bobberTarget = null) return;
        else
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
