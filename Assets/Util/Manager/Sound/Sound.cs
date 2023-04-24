using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using Util.Enum;

[CreateAssetMenu(fileName = "Sound")]
public class Sound : ScriptableObject
{
    [SerializeField] private SoundSource[] sound;

    Dictionary<SoundID, AudioClip> sounds = new Dictionary<SoundID, AudioClip>();

    private void GenerateDictionary()
    {
        for (int i = 0; i < sound.Length; i++)
        {
            sounds.Add(sound[i].SoundId, sound[i].SoundFile);
        }
    }

    public void GetSound(SoundID id, AudioMixerGroup group = null)
    {
        if (sounds.Count == 0)
        {
            GenerateDictionary();
        }

        AudioClip clip = sounds[id];

        if (group != null)
        {
            GameObject go = new GameObject("AudioSource");
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.outputAudioMixerGroup = group;
            source.Play();
            Destroy(go, clip.length);
        }
    }
}

[Serializable]
public struct SoundSource
{
    [SerializeField] private AudioClip soundFile;
    [SerializeField] private SoundID bgmId;

    public AudioClip SoundFile { get { return soundFile; } }
    public SoundID SoundId { get { return bgmId; } }
}

