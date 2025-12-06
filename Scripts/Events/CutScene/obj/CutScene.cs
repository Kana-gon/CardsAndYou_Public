using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.UI;
using System;


public class CutScene : MonoBehaviour
{
    [SerializeField] GameObject playerCard,enemyCard,npcCard,mainCamera,tutorialEndText;
    [SerializeField] Image titleImage;
    [SerializeField] GameObject AnimationObject_playerCard,AnimationObject_square;
    [SerializeField] GameObject talkMng;
    [SerializeField] EventMng eventMng;
    [SerializeField] CardMng cardMng;
    [SerializeField] BattleMng battleMng;
    [SerializeField] TutorialMng tutorialMng;
    [SerializeField] BGMMng bgmMng;
    [SerializeField] EventCamera eventCamera;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource slash,wind,dead;
    [ShowInInspector, LabelText("再生開始位置")]
    [PropertyRange(0, nameof(MaxPlaybackTime))]
    private float slashStartPos;
    private float MaxPlaybackTime => slash != null && slash.clip != null ? slash.clip.length : 0f;
    float CameraShake_duration, CameraShake_strength, CameraShake_vibrato;
    /// <summary>
    /// 関数名で指定された関数を呼び出す
    /// </summary>
    /// <param name="cutSceneName">呼び出す関数の名前</param>
    public void LaunchEvent(string cutSceneName)// from ChatGPT 
    {
        //Debug.Log("cutscene Launched.");
        MethodInfo method = GetType().GetMethod(
            cutSceneName,
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (method != null)
        {
            method.Invoke(this, null);
        }
        else
        {
            //Debug.LogWarning($"{cutSceneName} が見つかりません");
        }

    }
    [Button("プレイヤー死亡イベント")]
    private void playerDie()
    {

        //Debug.Log("Player Died.");
        talkMng.GetComponent<TalkMng>().StopTalk();

        talkMng.SetActive(false);
        PlayerDie_Animation();
    }
    private void DestroyplayerCard()//TODO デリゲートとインボーク使っていい感じに
    {
        if (playerCard.GetComponent<CardBase>().GetChildCard() != null)
            playerCard.GetComponent<CardBase>().GetChildCard().transform.SetParent(playerCard.GetComponent<CardBase>().objBaseTransform);
        Destroy(playerCard);
    }
    [Button("敵01起動イベント")]
    private void enemyActivate()//戦闘開始まで連続で
    {
        eventMng.EnemyAppear_tutrial();
        eventMng.EnemyActivate();
        battleStart();
    }
    [Button("戦闘開始演出")]
    private void battleStart()
    {
        bgmMng.PlayBGM("BGM_demon");
        battleMng.BattleStart();
        StartCoroutine(tutorialMng.Tutorial01());
    }
    private void PlayerDie_Animation()
    {
        cardMng.cardsClickable = false;
        Sequence seq = DOTween.Sequence();
        eventCamera.eventStart();
        bgmMng.PlayBGM("BGM_none");
        var targetPosition = playerCard.transform.position;
        var cam = eventCamera.gameObject;
        targetPosition.z = -10;
        CameraShakeSetting(1,1,15);
        seq.Append(cam.transform.DOMove(targetPosition, 0.4f))
        .Join(cam.GetComponent<Camera>().DOOrthoSize(4.0f, 0.3f)).AppendCallback(() => cardMng.cardsClickable = false)
        .SetEase(Ease.InOutQuad).AppendInterval(2.5f).AppendCallback(() => SoundPlay_Slash()).AppendInterval(0.5f)
        .OnComplete(() =>
        {
            playerCard.transform.SetParent(playerCard.GetComponent<CardBase>().objBaseTransform);
            Invoke(nameof(DestroyplayerCard), 0.1f);
            StaticData.IsPlayerLiving = false;
            //Debug.Log("tween終了");
            AnimationObject_playerCard.transform.position = playerCard.transform.position;
            AnimationObject_square.transform.position = playerCard.transform.position;
            animator.Play("CutScene_PlayerDie");

        }
        );
    }
    private void OnPlayerDieAnimation_End()
    {
        var targetPosition = enemyCard.transform.position;
        var targetPosition2 = mainCamera.transform.position;
        targetPosition.z = -10;
        targetPosition2.z = -10;
        Sequence seq = DOTween.Sequence();
        var cam = eventCamera.gameObject;
        enemyCard.SetActive(true);
        seq.AppendInterval(1.7f).AppendCallback(SoundPlay_Wind).Append(cam.transform.DOMove(targetPosition, 3.4f)).AppendInterval(1.4f)
        .SetEase(Ease.OutQuad).AppendInterval(1.8f).AppendCallback(() =>
        {
            eventCamera.transform.SetParent(enemyCard.transform);
            enemyCard.GetComponent<Animator>().Play("Enemy01_AppearMove");
        }
        ).AppendInterval(2f).//*ここ要調整
        Append(cam.transform.DOMove(targetPosition2, 0.3f)).Join(cam.GetComponent<Camera>().DOOrthoSize(5.0f, 0.3f)).OnComplete(() =>
        {
            eventCamera.eventEnd();
            talkMng.SetActive(true);
            npcCard.GetComponent<NPCCard>().ChangeTalkThema("Guid_playerDied");
            npcCard.GetComponent<NPCCard>().StartTalk();
        }
        );
    }
    private void SoundPlay_Slash()
    {
        slash.time = slashStartPos;
        slash.Play();
    }
    private void SoundPlay_Wind()
    {
        bgmMng.PlayBGM("BGM_wind");
    }
    private void SoundPlay_Dead()
    {
        dead.Play();
    }
    private void BGMPlay_Tutorial()
    {
        bgmMng.PlayBGM("BGM_tutorial");
    }
    private void tutorial_appearDebris()//?TODO?appear系まとめられないかなあ なお書くのが楽になるだけで処理は変わらない、むしろ余計な段階を踏むのでまとめる必要はないかも知れない
    {
        GameObject.Find("Cards").transform.Find("CollectPointCard_Debris").gameObject.SetActive(true);
    }
    private void tutorial_appearMining()
    {
        GameObject.Find("Cards").transform.Find("CollectPointCard_Mining").gameObject.SetActive(true);
    }
    private void tutorial_appearCraftCard_Default()
    {
        GameObject.Find("Cards").transform.Find("CraftCard_Default").gameObject.SetActive(true);
    }
    private void guidIllustChange()
    {
        npcCard.GetComponent<GuidEvent>().illustChange("suits");
    }

    public void tutorialEnd()
    {
        tutorial_appearDebris();
        tutorial_appearMining();
        tutorial_appearCraftCard_Default();
        playerDie();
        //Invoke("enemyActivate",0.1f);
    }
    [Button("タイトルロゴ移動")]
    private void tutorialEnd_titleLogoPerformance()
    {
        //Debug.Log("タイトルロゴを動かします");
        Sequence seq = DOTween.Sequence();
        seq.Append(titleImage.rectTransform.DOAnchorPos(new Vector2(0, 0), 2).SetEase(Ease.OutBounce)).AppendInterval(3).Append(titleImage.DOFade(0f, 0.5f)).OnComplete(() => tutorialEndText.SetActive(true));
    }
    public void CameraShakeSetting(float _duration = 0.5f, float _strength = 0.5f, float _vibrato = 10)//made from ChatGPT
    {
        CameraShake_duration = _duration;
        CameraShake_strength = _strength;
        CameraShake_vibrato = _vibrato;
    }
    public void CameraShake()//made from ChatGPT
    {
        eventCamera.GetComponent<Camera>().DOShakePosition(CameraShake_duration, CameraShake_strength, (int)CameraShake_vibrato);
    }
}
