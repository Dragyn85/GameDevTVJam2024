using UnityEngine;
using UnityEngine.Serialization;

public class MusicManagerLevel : MonoBehaviour
{
    [SerializeField] private AudioSource startMusic;
    [SerializeField] private AudioSource playingMusic;

    private                  int   musicPlaying = 0;
    private                  float transitionTimer;
    [SerializeField] private float TransitionTime = 3;

    void Start()
    {
        startMusic.volume = 1;
    }

    public void StartTransiton()
    {
        if (musicPlaying == 0 && transitionTimer==0)
        {
            transitionTimer     = TransitionTime;
            playingMusic.volume = 0;
            playingMusic.Play();
        }
    }
    void Update()
    {
        if (transitionTimer > 0)
        {
            transitionTimer -= Time.deltaTime;
            if (transitionTimer <= 0)
            {
                transitionTimer = 0;
                musicPlaying    = 1;
                startMusic.Stop();
            }

            var ratio = transitionTimer / TransitionTime;
            startMusic.volume   = ratio;
            playingMusic.volume = 1 - ratio;
        }
    }
}
