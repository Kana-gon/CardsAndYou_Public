using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;


public class TutorialMng : MonoBehaviour
{
    [Title("References")]
    [SerializeField] Animator animator;
    [SerializeField] TextMeshPro tutorialString;
    [SerializeField] CutScene cutscene;
    [SerializeField] NPCCard npcCard;
    void Start()
    {
        tutorialString.color = Color.cyan;
    }
    public void DisplayTutorial(string _text)
    {
        tutorialString.text = _text;
        animator.Play("TutorialDisplay");
    }
    public void DisappearTutorial()
    {
        animator.Play("TutorialDisappear");
    }
    public void SwitchTutorial(string _text)
    {
        DisappearTutorial();
        DisplayTutorial(_text);
    }
    public IEnumerator Tutorial01()
    {
        SwitchTutorial("カードを右クリック / 武器を右クリック+フリック");
        yield return new WaitForSeconds(10);
    }
    [Button("チュートリアル終了~戦闘開始まで")]
    private void TutorialEnd()
    {
        cutscene.tutorialEnd();
        npcCard.ChangeTalkThema("Guid_bringSword");
        npcCard.ChangeTalkThemaIndex(2);
    }
    [Button("テスト：プレイヤー死亡直前会話までスキップ")]
    private void test01()
    {
        npcCard.ChangeTalkThema("Guid_bringSword");
    }
}
