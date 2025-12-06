using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class BGMMng : SerializedMonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]AudioSource BGM;
    [SerializeField] private Dictionary<string,AudioClip> clips = new Dictionary<string,AudioClip>();
    [SerializeField] float bgmBolume;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayBGM(string bgmName)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(BGM.DOFade(0f, 0.3f)).OnComplete(()=>{
            BGM.Stop();
            BGM.clip = clips[bgmName];
            BGM.DOFade(bgmBolume, 0.3f);
            BGM.Play();
        });
        
    }
}
