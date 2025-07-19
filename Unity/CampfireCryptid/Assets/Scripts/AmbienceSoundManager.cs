using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AmbienceSoundManager : MonoBehaviour
{
    // Audio source for the ambience sound
    public AudioClip ambienceBase;
    public AudioClip ambienceT1;
    public AudioClip ambienceT2;

    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;

    public void Awake()
    {
        audioSource1.loop = true; // Set the audio to loop
        audioSource1.clip = ambienceBase; // Set the base ambience sound
        audioSource1.volume = 1f; // Set the volume for the base ambience sound
        audioSource1.Play(); // Start playing the base ambience sound

        audioSource2.loop = true; // Set the audio to loop
        audioSource2.clip = ambienceT1; // Set the first ambience sound
        audioSource2.volume = 0f; // Set the volume for the first ambience sound
        audioSource2.Play(); // Start playing the first ambience sound

        audioSource3.loop = true; // Set the audio to loop
        audioSource3.clip = ambienceT2; // Set the second ambience sound
        audioSource3.volume = 0f; // Set the volume for the second ambience sound
        audioSource3.Play(); // Start playing the second ambience sound

        DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed on scene load
    }

    public void GoUp()
    {
        if (audioSource1.volume > 0f)
        {
            StartCoroutine(DecreaseVolume(audioSource1)); // Start decreasing the volume of the base ambience sound
            StartCoroutine(IncreaseVolume(audioSource2)); // Start increasing the volume of the first ambience sound
        }
        else if (audioSource2.volume > 0f)
        {
            StartCoroutine(DecreaseVolume(audioSource2)); // Start decreasing the volume of the first ambience sound
            StartCoroutine(IncreaseVolume(audioSource3)); // Start increasing the volume of the second ambience sound
        }
        else if (audioSource3.volume > 0f)
        {
            return; // If the third ambience sound is already playing, do nothing
        }
    }

    public void GoDown()
    {
        if (audioSource3.volume > 0f)
        {
            StartCoroutine(DecreaseVolume(audioSource3)); // Start decreasing the volume of the second ambience sound
            StartCoroutine(IncreaseVolume(audioSource2)); // Start increasing the volume of the first ambience sound
        }
        else if (audioSource2.volume > 0f)
        {
            StartCoroutine(DecreaseVolume(audioSource2)); // Start decreasing the volume of the first ambience sound
            StartCoroutine(IncreaseVolume(audioSource1)); // Start increasing the volume of the base ambience sound
        }
        else if (audioSource1.volume > 0f)
        {
            return; // If the base ambience sound is already playing, do nothing
        }
    }

    private IEnumerator IncreaseVolume(AudioSource currAudioSource)
    {
        while (currAudioSource.volume < 1f)
        {
            currAudioSource.volume += 0.01f; // Gradually increase the volume
            yield return new WaitForSeconds(0.1f); // Wait for a short duration before the next increase
        }
    }

    private IEnumerator DecreaseVolume(AudioSource currAudioSource)
    {
        while (currAudioSource.volume > 0f)
        {
            currAudioSource.volume -= 0.01f; // Gradually decrease the volume
            yield return new WaitForSeconds(0.1f); // Wait for a short duration before the next decrease
        }
    }
}
