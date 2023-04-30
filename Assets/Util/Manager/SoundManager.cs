using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Util.Enum;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource _bgm;
    [SerializeField]private AudioClip[] _audioClip;
    [SerializeField] private Sound _storage;
    [SerializeField] private AudioMixer _mixer;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            _bgm = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this);
        }
    }

    public void Play(SoundID id)
    {
        _storage.GetSound(id, _mixer.FindMatchingGroups("SFX")[0]);
    }
    private void Play(AudioClip clip)
    {
        _bgm.clip = clip;
        _bgm.loop = true;
        _bgm.outputAudioMixerGroup = _mixer.FindMatchingGroups("BGM")[0];
        _bgm.Play();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        for(int i = 0; i < _audioClip.Length; ++i)
        {
            if(scene.name == _audioClip[i].name)
            {
                Play(_audioClip[i]);
                return;
            }
        }
    }

    public void ChangeVolume(string group, float value)
    {
        _mixer.SetFloat(group, Mathf.Log10(value) * 20);
    }
}
