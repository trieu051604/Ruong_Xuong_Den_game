using UnityEngine;

public class AudioManagementLevel1 : MonoBehaviour
{
   [SerializeField] private AudioSource effectAudioSource;
   [SerializeField] private AudioClip backgroundClipLevel1;
   [SerializeField] private AudioClip BossAttack;
    public void PlayBackgroundMusicLevel1()
    {
         effectAudioSource.clip = backgroundClipLevel1;
         effectAudioSource.loop = true;
         effectAudioSource.Play();
    }
   
    public void PlayBossAttackSoundLevel1()
    {
        effectAudioSource.PlayOneShot(BossAttack);
    }
   
}
