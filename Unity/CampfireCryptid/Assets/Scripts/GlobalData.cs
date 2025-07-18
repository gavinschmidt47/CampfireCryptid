using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Scriptable Objects/GlobalData")]
public class GlobalData : ScriptableObject
{
    //Time before end
    public float secondsLeftBeforeNight = 600f;

    public void TimeSetDown(float currentTime)
    {
        secondsLeftBeforeNight -= currentTime;
    }
}
