using UnityEngine;

public class EffigyScript : MonoBehaviour
{
    public GameObject effigyPrefab; // Prefab for the effigies
    public int numberOfEffigies; // Number of effigies in the scene
    public int minTimeBetween = 5; // Time in seconds between each spawn
    public int maxTimeBetween = 15; // Time in seconds between each spawn
    public int effigyCurLoc; // Current location of the effigy

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EffigySpawner());
        List<Vector3> effigyPositions = new List<Vector3>(); // List to store the positions of the effigies
        effigyPositions.Add(new Vector3(4.41400003,1.75999999,-1.58000004));
        effigyPositions.Add(new Vector3(-2.82800007,1.18200004,-4.33199978));
        effigyPositions.Add(new Vector3(-2.82800007,2.44799995,-4.33199978));
        effigyPositions.Add(new Vector3(-2.82800007,6.04799986,-4.33199978));
        effigyPositions.Add(new Vector3(0.148000002,1.21899998,-1.78699994));
        effigyPositions.Add(new Vector3(0.148000002,2.86299992,-1.78699994));
        effigyPositions.Add(new Vector3(0.148000002,4.57000017,-1.78699994));
        effigyPositions.Add(new Vector3(-2.1400001,0.860000014,-1.26999998));
        effigyPositions.Add(new Vector3(-2.18000007,2.68000007,-0.959999979));
        effigyPositions.Add(new Vector3(-1.95099998,5.03999996,-1.27999997));
        effigyPositions.Add(new Vector3(-11.257,1.18200004,-1.30299997));
        effigyPositions.Add(new Vector3(-11.1409998,3.50399995,-1.48800004));
        effigyPositions.Add(new Vector3(-11.1730003,6.1420002,-1.28299999));
        effigyPositions.Add(new Vector3(-11.1730003,8.86999989,-1.28299999));
        effigyPositions.Add(new Vector3(-8.98999977,6.67999983,2.68000007));
        effigyPositions.Add(new Vector3(-8.98999977,2.77999997,2.68000007));
        effigyPositions.Add(new Vector3(-5.2420001,0.845000029,2.96799994));
        effigyPositions.Add(new Vector3(-5.2420001,3.49300003,2.96799994));
        effigyPositions.Add(new Vector3(-5.2420001,8.43000031,2.96799994));
        effigyPositions.Add(new Vector3(0.714999974,2.55999994,2.59599996));
        effigyPositions.Add(new Vector3(0.714999974,4.61000013,2.59599996));
        effigyPositions.Add(new Vector3(-0.617999971,4.61000013,4.32499981));
        effigyPositions.Add(new Vector3(-0.617999971,1.31099999,4.32499981));
        effigyPositions.Add(new Vector3(-0.617999971,3.38199997,4.32499981));
        effigyPositions.Add(new Vector3(1.91400003,3.38199997,4.1079998));
        effigyPositions.Add(new Vector3(1.91400003,1.39400005,4.1079998));
        effigyPositions.Add(new Vector3(4.95300007,3.023,3.17000008));
        effigyPositions.Add(new Vector3(4.95300007,1.63,3.17000008));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EffigySpawner()
    {
        while(numberOfEffigies < 10)
        {
            
			Instantiate(prefab, effigyCurLoc, Quaternion.identity);
            //yeild return new WaitForSeconds(Random.Range(minTimeBetween, maxTimeBetween));
            numberOfEffigies++;
        }
    }
}
