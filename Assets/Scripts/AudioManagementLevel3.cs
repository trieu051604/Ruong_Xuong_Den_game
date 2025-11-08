using UnityEngine;

public class AudioManagementLevel3 : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip backgroundClipLevel3;
    [SerializeField] private AudioClip BossAttack;

    public void PlayBackgroundMusicLevel3()
    {
        effectAudioSource.clip = backgroundClipLevel3;
        effectAudioSource.loop = true;
        effectAudioSource.Play();
    }

    public void PlayBossAttackSound()
    {
        effectAudioSource.PlayOneShot(BossAttack);
    }

}
