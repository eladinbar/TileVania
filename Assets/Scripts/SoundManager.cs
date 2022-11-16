using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;

    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
}
