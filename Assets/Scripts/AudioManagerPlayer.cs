using UnityEngine;

public class AudioManagerPlayer : MonoBehaviour
{
   [SerializeField] private AudioSource effectAudioSource;
   [SerializeField] private AudioClip PlayerAttack;
   [SerializeField] private AudioClip Collect;

    public void PlayPlayerAttackSound()
    {
         effectAudioSource.PlayOneShot(PlayerAttack);
    }
   
    public void PlayCollectSound()
    {
        effectAudioSource.PlayOneShot(Collect);
    }
}
