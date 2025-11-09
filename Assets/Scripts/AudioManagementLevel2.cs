using UnityEngine;

public class AudioManagementLevel2 : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip backgroundClipLevel2;
    [SerializeField] private AudioClip BossAttack;

    public void PlayBackgroundMusicLevel2()
    {
        effectAudioSource.clip = backgroundClipLevel2;
        effectAudioSource.loop = true;
        effectAudioSource.Play();
    }
    
    public void PlayBossAttackSound()
    {
        effectAudioSource.PlayOneShot(BossAttack);
    }
    
 }
