using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class BattleMng : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ISTUTORIAL = true;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] GameObject BattleUI;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource damageAudio;
    private List<GameObject> enemys = new List<GameObject>();
    public bool IsBattleMode = false;
    public UnityEvent OnBattleEnd;

    private float hp = 100, hpmax = 100;
    public enum BattleType
    {
        NORMAL,
        TUTRIAL
    };
    public BattleType battleType;
    void Start()
    {
        hp = hpmax;
        ChangeHPText();
        battleType = BattleType.NORMAL;
    }

    // Update is called once per frame

    /// <summary>
    /// 現在HPをvalueに更新する
    /// </summary>
    /// <param name="value"></param>
    public void ChangeHP(float value)
    {
        hp = value;

        ChangeHPText();
    }
    /// <summary>
    /// valueだけ現在HPを足す
    /// </summary>
    /// <param name="value"></param> <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void AddHP(float value)
    {
        hp += value;
        ChangeHPText();
    }
    public void DamageHP(float value)
    {
        damageAudio.Play();
        hp -= value;
        ChangeHPText();
    }
    public void ChangeHPMAX(float value)
    {
        hpmax = value;
        ChangeHPText();
    }
    /// <summary>
    /// valueだけ現在HPMAXを足す
    /// ダメージの場合はマイナスの値を入れてね
    /// </summary>
    /// <param name="value"></param> <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void AddHPMAX(float value)
    {
        hpmax += value;
        ChangeHPText();
    }
    public void AddEnemys(GameObject enemy)
    {
        enemys.Add(enemy);
    }
    void Update()
    {
        if (enemys.All(obj => obj == null) && IsBattleMode)
        {
            BattleEnd();
        }
    }
    [Button("戦闘スタート")]
    public void BattleStart()
    {
        IsBattleMode = true;
        animator.Play("BattleStart");
    }
    [Button("戦闘終了")]
    public void BattleEnd()
    {
        IsBattleMode = false;
        animator.Play("BattleEnd");
        
    }
    
    private void OnAnimationEnd_BattleEnd()
    {
        if (battleType == BattleType.TUTRIAL)
        {
            //Debug.Log("チュートリアル戦闘が終了");
            OnBattleEnd?.Invoke();
        }
    }
    private void ChangeHPText()
    {
        //*チュートリアルでは死なない。本編実装と同時に死亡演出を追加
        if (ISTUTORIAL)
        {
            if (hp <= 0)
            {
                hp = 1;
            }
        }
        hpText.text = $"HP:{hp}/{hpmax}";
    }
}
