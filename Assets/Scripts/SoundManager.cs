using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{

    private Dictionary<int, AudioClip> soundMap;
    private List<AudioSource> _audioSource;
    //Change the number of audio sources as needed
    private int _audioSourceCount = 10;

    //Music
    public AudioClip testSound { get; private set; }

    public void playSound(AudioClip sound)
    {
        foreach (AudioSource AS in _audioSource)
        {
            if (!AS.isPlaying)
            {
                AS.PlayOneShot(sound);
                break;
            }
        }
    }

    public void playSound(AudioClip sound, int index)
    {
        if (index > _audioSourceCount)
        {
            index = _audioSourceCount;
        }
        _audioSource[index - 1].PlayOneShot(sound);
    }

    public void playSound(AudioClip sound, int index, float volume)
    {
        if (index > _audioSourceCount)
        {
            index = _audioSourceCount;
        }
        _audioSource[index - 1].PlayOneShot(sound, volume);
    }

    public void stopSound(int index)
    {
        if (index > _audioSourceCount)
        {
            index = _audioSourceCount;
        }
        StartCoroutine(fadeSound(index, 1));
    }

    public void stopAllSounds()
    {
        foreach (AudioSource AS in _audioSource)
        {
             AS.Stop();
        }
    }

    // Use this for initialization
    void Awake()
    {
        _audioSource = new List<AudioSource>();
        soundMap = new Dictionary<int, AudioClip>();
        for (int i = 0; i < _audioSourceCount; i++)
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            _audioSource.Add(temp);
        }

        //Loads a file from Resources/Sounds folder
        testSound = _loadSoundClip("TestSound", 0);

    }

    private AudioClip _loadSoundClip(string filename, int i)
    {
        AudioClip clip = Resources.Load("Sounds/" + filename) as AudioClip;
        soundMap.Add(i, clip);
        return clip;
    }

    public AudioClip getSound(int idx)
    {
        Debug.Log(soundMap.ContainsKey(idx));
        ICollection coll = soundMap.Keys;

        return soundMap[idx];
    }

    IEnumerator fadeSound(int index, float time)
    {
        AudioSource AS = _audioSource[index - 1];
        float delta = AS.volume / time;
        while (AS.volume > 0.05f)
        {
            AS.volume -= delta * Time.deltaTime;
            yield return null;
        }
        AS.Stop();
        AS.volume = 1;
    }

}