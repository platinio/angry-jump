using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //singleton
    #region SINGLETON
    private static SoundManager soundManager;
    public static SoundManager instance
    {
        get
        {
            if (!soundManager)
            {
                soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;


                if (!soundManager)
                    Debug.LogError("You need to have a SoundManager script active in the scene");
            }

            return soundManager;
        }

    }
    #endregion SINGLETON

    //inspector
    [SerializeField] private List<AudioClip>    _backgroundSounds   = null;
    [SerializeField] private List<AudioClip>    _clickSounds = null;
    [SerializeField] private AudioClip          _hitSound           = null;    
    [SerializeField] private AudioClip          _gameOverSound      = null;
    [SerializeField] private AudioClip _jumpSound = null;
    [SerializeField] private AudioClip _grabCoinSound = null;
    [SerializeField] [Range(0, 1.0f)] private float _volume = 0.8f;

    //private
    private AudioSource _audioSource        = null;
    private AudioSource _backgroundMusic    = null;

    void Start()
    {
        
        _audioSource        = gameObject.AddComponent<AudioSource>() as AudioSource;
        _backgroundMusic    = gameObject.AddComponent<AudioSource>() as AudioSource;

        _audioSource.playOnAwake        = false;
        _audioSource.loop               = false;
        _backgroundMusic.playOnAwake    = true;
        _backgroundMusic.loop           = true;

        _backgroundMusic.clip = _backgroundSounds[Random.Range(0, _backgroundSounds.Count)];
        _backgroundMusic.volume = _volume;
        //_backgroundMusic.Play();

    }

    //play sounds methods

    public void PlayClickSound() 
    { 
        if(_clickSounds != null)
            Play(_clickSounds[Random.Range(0, _clickSounds.Count)]); 
    }

    public void PlayHitSound()      { Play(_hitSound);      }    
    public void PlayGameOverSound() { Play(_gameOverSound); }
    public void PlayJumpSound()     { Play(_jumpSound);     }
    public void PlayGrabCoinSound() { Play(_grabCoinSound); }


    private void Play(AudioClip sound)
    {
        if (sound == null)
            return;
        if (_audioSource == null)
            return;

        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = sound;
        _audioSource.Play();
    }

}
