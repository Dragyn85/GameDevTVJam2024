using UnityEngine;
using UnityEngine.Serialization;

public class MusicManagerLevel : MonoBehaviour
{
    [SerializeField] private AudioSource startMusic;
    [SerializeField] private AudioSource playingMusic;
    [SerializeField] private float       transitionRate = .5f;

    private                  float _trackPlaying = 0;


    private int _targetTrack;
    
    void Start()
    {
        startMusic.volume = 1;
    }

    public void TransitonToTrack(int TargetTrack)
    {
        if (_targetTrack != TargetTrack)
        {
            _targetTrack = TargetTrack;

            if (_targetTrack == 0)
            {
                startMusic.Play();
            }
            else
            {
                playingMusic.Play();
            }
        }
    }


    void Update() 
    {
        if (_trackPlaying != (float) _targetTrack)
        {
            if (_targetTrack == 1)
            {
                _trackPlaying += Time.deltaTime * transitionRate;
                if (_trackPlaying >= 1.0)
                {
                    _trackPlaying =  1;
                    startMusic.Stop();
                }
            }
            else
            {
                _trackPlaying -= Time.deltaTime * transitionRate;
                if (_trackPlaying <= 0)
                {
                    _trackPlaying =  0;
                    playingMusic.Stop();
                }
            }

            startMusic.volume   = 1-_trackPlaying;
            playingMusic.volume = _trackPlaying;
        }
    }
}
