using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AmbienceSoundManager : MonoBehaviour
{
    public AudioClip[] ambienceClips; // Array to hold the ambience audio clips

    public GlobalData globalData; // Reference to the GlobalData scriptable object

    private AudioSource[] audioSources; // Array to hold the audio sources for each clip

    private int currentClipIndex = 3; // Index of the currently playing clip

    public void Start()
    {
        if (globalData.hasAudio)
            Destroy(gameObject); // Destroy this object if audio is disabled in GlobalData

        DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load

        // Initialize the audio sources
        audioSources = new AudioSource[ambienceClips.Length];
        for (int i = 0; i < ambienceClips.Length; i++)
        {
            audioSources[i] = this.AddComponent<AudioSource>();
            audioSources[i].clip = ambienceClips[i];
            audioSources[i].loop = true;
            audioSources[i].playOnAwake = false;
            audioSources[i].volume = 0f; // Set a default volume
            audioSources[i].minDistance = 1f; // Set minimum distance for 3D sound
            audioSources[i].maxDistance = 500f; // Set maximum distance for 3D sound
            audioSources[i].Play(); // Set to 3D sound
        }

        audioSources[currentClipIndex].volume = 1f; // Start playing the first clip

        globalData.hasAudio = true; // Set the audio flag in GlobalData
    }

    public void Update()
    {
        if (globalData.secondsLeftBeforeNight <= 800.006f && globalData.secondsLeftBeforeNight >= 800f)
        {
            StartCoroutine(TransitionDownClip());
        }
        else if (globalData.secondsLeftBeforeNight <= 500.006f && globalData.secondsLeftBeforeNight >= 500f)
        {
            StartCoroutine(TransitionDownClip());
        }
        else if (globalData.secondsLeftBeforeNight <= 120.006f && globalData.secondsLeftBeforeNight >= 120f)
        {
            StartCoroutine(TransitionDownClip());
        }
        else if (globalData.secondsLeftBeforeNight <= 60.006f && globalData.secondsLeftBeforeNight >= 60f)
        {
            StartCoroutine(TransitionDownClip());
        }
    }

    public IEnumerator TransitionDownClip()
    {
        int nextClipIndex = currentClipIndex - 1;
        // Fade out the current clip
        float fadeDuration = 2f; // Duration of the fade effect
        float startVolume = audioSources[currentClipIndex].volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSources[currentClipIndex].volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            audioSources[nextClipIndex].volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        audioSources[currentClipIndex].volume = 0f; // Ensure volume is set to 0
        // Update the index to the next clip
        currentClipIndex--;
    }
}
