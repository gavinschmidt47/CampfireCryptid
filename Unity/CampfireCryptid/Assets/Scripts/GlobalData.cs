using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Scriptable Objects/GlobalData")]
public class GlobalData : ScriptableObject
{
    // Time before end
    public float secondsLeftBeforeNight = 600f;

    // Time before fire dies
    public float secondsLeftBeforeFireDies = 0f;

    // Minigames Completed
    public bool[] minigamesCompleted = new bool[3];
    /*
     * [1] = Fishing
     * [2] = Hunting
     * [3] = Exploring
     */

    // Call these every frame (Pass Time.deltaTime)
    public void TimeSetDown(float currentTime)
    {
        secondsLeftBeforeNight -= currentTime;
    }
    public void FireSetDown(float currentTime)
    {
        // Slow fire death
        if (secondsLeftBeforeFireDies > 0)
            secondsLeftBeforeFireDies -= currentTime * 0.3f;
        else
            secondsLeftBeforeFireDies = 0f;
    }

    // Getters
    public int GetMinigamesCompletedCount()
    {
        int count = 0;
        foreach (bool completed in minigamesCompleted)
        {
            if (completed) count++;
        }
        return count;
    }

    public bool IsMinigameCompleted(int index)
    {
        if (index >= 0 && index < minigamesCompleted.Length)
        {
            return minigamesCompleted[index];
        }
        else
        {
            Debug.LogWarning("Index out of bounds for minigamesCompleted array.");
            return false;
        }
    }

    // Setters
    public void SetMinigameCompleted(int index, bool completed)
    {
        if (index >= 0 && index < minigamesCompleted.Length)
        {
            minigamesCompleted[index] = completed;
        }
        else
        {
            Debug.LogWarning("Index out of bounds for minigamesCompleted array.");
        }
    }
}
