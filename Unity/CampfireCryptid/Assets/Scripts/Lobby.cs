using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GlobalData globalData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        globalData.TimeSetDown(Time.deltaTime);
        globalData.FireSetDown(Time.deltaTime);
    }
}
