using UnityEngine;

public class WinScreenAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winClip;

    private bool hasPlayed = false;

    void OnEnable()
    {
        if (!hasPlayed && audioSource != null && winClip != null)
        {
            audioSource.clip = winClip;
            audioSource.Play();
            hasPlayed = true;
        }
    }
}
