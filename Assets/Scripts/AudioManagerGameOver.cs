using UnityEngine;

public class AudioManagerGameOver : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip gameOverBackgroundClip;
    [SerializeField] private AudioClip buttonClickClip;

    public void PlayBackgroundMusicGameOver()
    {
        effectAudioSource.clip = gameOverBackgroundClip;
        effectAudioSource.loop = true;
        effectAudioSource.Play();
    }

    public void PlayButtonClickSoundGameOver()
    {
        effectAudioSource.PlayOneShot(buttonClickClip);
    }
}
