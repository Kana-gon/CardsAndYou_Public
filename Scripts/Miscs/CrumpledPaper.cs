using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrumpledPaper : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject dropCard;
    [SerializeField] private Animator animator;
    [SerializeField] AudioSource source;
    [SerializeField] AudioSource source_2;
    private Tween tween;
    bool isAudioPlayed = false;
    void OnMouseEnter()
    {
        if (!isAudioPlayed && GameObject.Find("Cards").GetComponent<CardMng>().cardsClickable)
        {
            animator.Play("clamp_open");
            source.time = 0.3f;
            source.Play();
            isAudioPlayed = true;
        }
    }
    void GenerateCard()
    {
        var _dropCard = Instantiate(dropCard, GameObject.Find("Cards").transform);
        _dropCard.transform.position = transform.position;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    void DestroyThis()//*音声を途切れさせないための苦肉の策
    {
        Destroy(gameObject);

    }
    public void MovePaper(Vector3 target, float duration)
    {
        tween = transform.DOMove(target, duration).SetEase(Ease.OutQuart);
        source_2.time = 0.1f;
        source_2.Play();
    }
}
