using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{

    private Dictionary<int, AudioClip> soundMap;
    private List<AudioSource> _audioSource;
    private int _audioSourceCount = 18;

    //Music
    public AudioClip backgroundBeat { get; private set; }
    public AudioClip tutorialBeat { get; private set; }
    public AudioClip[,,] playerTriggers { get; private set; }

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

        tutorialBeat = _loadSoundClip("Tutorial", 0);
        backgroundBeat = _loadSoundClip("BG_Music_Quiet", 1);

        // Indices are in order of: Player count, device count, unique sound count
        playerTriggers = new AudioClip[4, 2, 10];
        playerTriggers[0, 0, 0] = _loadSoundClip("Drummer-Basket_0", 2);
        playerTriggers[0, 1, 0] = _loadSoundClip("Drummer-Basket_1", 3);

        playerTriggers[1, 0, 0] = _loadSoundClip("BassPlayer-Plunger_0", 4);
        playerTriggers[1, 0, 1] = _loadSoundClip("BassPlayer-Plunger_1", 5);
        playerTriggers[1, 0, 2] = _loadSoundClip("BassPlayer-Plunger_2", 6);
        playerTriggers[1, 0, 3] = _loadSoundClip("BassPlayer-Plunger_3", 7);
        playerTriggers[1, 1, 0] = _loadSoundClip("BassPlayer-Flush_0", 8);
        playerTriggers[1, 1, 1] = _loadSoundClip("BassPlayer-Flush_1", 9);
        playerTriggers[1, 1, 2] = _loadSoundClip("BassPlayer-Flush_2", 10);
        playerTriggers[1, 1, 3] = _loadSoundClip("BassPlayer-Flush_3", 11);
        playerTriggers[1, 1, 4] = _loadSoundClip("BassPlayer-Flush_4", 12);
        playerTriggers[1, 1, 5] = _loadSoundClip("BassPlayer-Flush_5", 13);
        playerTriggers[1, 1, 6] = _loadSoundClip("BassPlayer-Flush_6", 14);
        playerTriggers[1, 1, 7] = _loadSoundClip("BassPlayer-Flush_7", 15);
        playerTriggers[1, 1, 8] = _loadSoundClip("BassPlayer-Flush_8", 16);
        playerTriggers[1, 1, 9] = _loadSoundClip("BassPlayer-Flush_9", 17);

        playerTriggers[2, 0, 0] = _loadSoundClip("KeyboardPlayer-Spray_0", 18);
        playerTriggers[2, 0, 1] = _loadSoundClip("KeyboardPlayer-Spray_1", 19);
        playerTriggers[2, 1, 0] = _loadSoundClip("KeyboardPlayer-Wipe_0", 20);
        playerTriggers[2, 1, 1] = _loadSoundClip("KeyboardPlayer-Wipe_1", 21);

        playerTriggers[3, 0, 0] = _loadSoundClip("GuitarPlayer-Scrub_0", 22);
        playerTriggers[3, 1, 0] = _loadSoundClip("GuitarPlayer-Scrub_1", 23);
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