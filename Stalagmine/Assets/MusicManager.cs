using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] MusicClips;

    public AudioSource audioSource;

    private void Start()
    {
        audioSource.clip = MusicClips[0];
        audioSource.Play();
    }
}
