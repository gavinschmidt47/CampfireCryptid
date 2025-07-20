using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Scriptable Objects/GlobalData")]
public class GlobalData : ScriptableObject
{
    // Time before end
    public float secondsLeftBeforeNight = 1200f;

    // Time before fire dies
    public float secondsLeftBeforeFireDies = 120f;

    // Minigames Completed
    public bool[] minigamesCompleted = new bool[2];
    // Minigame Progress
    public int[] minigameProgress = new int[2];
    /*
     * [0] = Fishing
     * [1] = Effigies
     */

    // Current Day
    public int currDay = 0;
    public bool gameOver = false;
    public bool fireOut = false;

    // Call these every frame (Pass Time.deltaTime)
    public void TimeSetDown(float currentTime)
    {
        secondsLeftBeforeNight -= currentTime;

        if (secondsLeftBeforeNight <= 0f && minigamesCompleted[0] && minigamesCompleted[1])
        {
            secondsLeftBeforeNight = 0f;
            currDay++;
            NextDay(); // Prepare for the next day
        }
        else if (secondsLeftBeforeNight <= 0)
        {
            // IMPLEMENT LOSS
            Debug.LogWarning("Time has run out before completing all minigames. Game over or reset required.");
            gameOver = true;
        }
    }
    public void FireSetDown(float currentTime)
    {
        // Slow fire death
        if (secondsLeftBeforeFireDies > 0)
            secondsLeftBeforeFireDies -= currentTime * 0.3f;
        else
        {
            secondsLeftBeforeFireDies = 0f;

            fireOut = true;
        }
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

    public void NextDay()
    {
        secondsLeftBeforeNight = 1200f - (currDay * 120f);
        if (secondsLeftBeforeNight < 60f)
        {
            secondsLeftBeforeNight = 60f;
        }
    }
}
