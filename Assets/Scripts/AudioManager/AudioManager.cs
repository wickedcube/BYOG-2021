using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public class AudioInfo
    {
        [SerializeField]
        public string voName;
        [SerializeField]
        public string textOnScreen;
        public AudioInfo(string voName, string textOnScreen)
        {
            this.voName = voName;
            this.textOnScreen = textOnScreen;
        }
    }
    
    [SerializeField] AudioSource voSource;
    [SerializeField] AudioSource bgSource;

    [SerializeField] AudioClip[] bgClips;
    [SerializeField] float maxVolBG = 0.25f;
    [SerializeField] float minVolBG = 0.1f;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public float PlayVOAudio(string key)
    {
        AudioClip clip = GetAudioClip(key);
        bgSource.DOFade(1 * minVolBG, 0.15f);
        voSource.clip = clip;
        voSource.Play();
        Sequence sequence = DOTween.Sequence()
            .AppendInterval(clip.length)
            .AppendCallback(() =>
            {
                bgSource.DOFade(1 * maxVolBG, 0.15f);
            });
        return clip.length;
    }

    public Dictionary<string, AudioInfo> audioLookup = new Dictionary<string, AudioInfo>{
        {"First_Reset",new AudioInfo("First_Reset","Blah blah, blah")}
    };

    public AudioClip GetAudioClip(string key)
    {
        return Resources.Load<AudioClip>(key);
    }

    public void PlayBG(int levelNum)
    {
        if (bgClips[levelNum] == bgSource.clip)
            return;

        Sequence seq = DOTween.Sequence()
            .Append(bgSource.DOFade(0* maxVolBG, 0.15f))
            .AppendCallback(() => {
                bgSource.Stop();
                bgSource.loop = true;
                bgSource.clip = bgClips[levelNum];
                bgSource.Play();
                bgSource.DOFade(1* maxVolBG, 0.15f);
            });
        
    }
}
