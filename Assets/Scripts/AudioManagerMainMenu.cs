using UnityEngine;

public class AudioManagerMainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip buttonClickClip;

    public void PlayBackgroundMusicMainMenu()
    {
        effectAudioSource.clip = backgroundClip;
        effectAudioSource.loop = true;
        effectAudioSource.Play();
    }

    public void PlayButtonClickSoundMainMenu()
    {
        effectAudioSource.PlayOneShot(buttonClickClip);
    }
}
