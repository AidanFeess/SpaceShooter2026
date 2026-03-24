using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public float masterVolume = 0.6f;

    public AudioSource musicSource;
    public AudioClip backgroundMusic;

    void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.volume = masterVolume;
            musicSource.Play();
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}