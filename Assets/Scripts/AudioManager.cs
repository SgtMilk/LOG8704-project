using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string Type;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [HideInInspector]
        public AudioSource Source;

        public bool play;
        public bool loop;
    }

    //Singleton
    public static AudioManager Instance;

    //All sounds and their associated type - Set these in the inspector
    public Sound[] AllSounds;

    void Start()
    {
        foreach (KeyValuePair< string, Sound >  soundType in _soundDictionary)
        {
            Play(soundType.Key);
        }
    }

    //Runtime collections
    private Dictionary<string, Sound> _soundDictionary = new Dictionary<string, Sound>();
    private AudioSource _musicSource;

    private void Awake()
    {
        //Assign singleton
        Instance = this;

        //Set up sounds
        foreach (var s in AllSounds)
        {
            _soundDictionary[s.Type] = s;
        }
    }



    //Call this method to play a sound
    public void Play(string type)
    {
        //Make sure there's a sound assigned to your specified type
        if (!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound type {type} not found!");
            return;
        }

        if (!s.play == false)
        {
            return;
        }

        //Creates a new sound object
        var soundObj = new GameObject($"Sound_{type}");
        var audioSrc = soundObj.AddComponent<AudioSource>();

        //Assigns your sound properties
        audioSrc.clip = s.Clip;
        audioSrc.volume = s.Volume;
        if (s.loop)
            audioSrc.loop = true;

        //Play the sound
        audioSrc.Play();

        //Destroy the object
        if (!s.loop)
            Destroy(soundObj, s.Clip.length);
    }

    //Call this method to change music tracks
    public void ChangeMusic(string type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound track))
        {
            Debug.LogWarning($"Music track {type} not found!");
            return;
        }

        if (_musicSource == null)
        {
            var container = new GameObject("SoundTrackObj");
            _musicSource = container.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }

        _musicSource.clip = track.Clip;
        _musicSource.Play();
    }
}