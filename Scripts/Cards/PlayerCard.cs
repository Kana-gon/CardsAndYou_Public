using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//*プレイヤーカードがチュートリアルの役割を担うのは最初だけ。
public class PlayerCard : MonoBehaviour
{
    // Start is called before the first frame update
    private int tutorialCount = 0;
    [SerializeField] TextMeshProUGUI cardText;
    [SerializeField] CardBase cardBase;
    GameObject talkManager;
    void Start()
    {
        cardBase.ShowUI();
        talkManager = GameObject.Find("Parent_of_InactiveObjects").transform.Find("TalkManager").gameObject;
    }
    void Update()
    {
        if (talkManager.activeSelf) cardBase.HideUI();
    }
    void OnMouseOver()
    {
        if (tutorialCount == 0)
        {
            ChangeText("左クリックでカードを持ち上げる");
            tutorialCount = 1;
        }
    }
    void OnMouseDown()
    {
        if (tutorialCount == 1)
        {
            ChangeText("マウスを離してカードを置く");
            tutorialCount = 2;
        }
    }
    void OnMouseExit()
    {
        cardBase.HideUI();
    }
    void OnMouseEnter()
    {
        if (GetComponent<CardBase>().objBaseTransform.GetComponent<CardMng>().cardsClickable)
            cardBase.ShowUI();
    }
    void OnMouseUp()
    {
        if (tutorialCount == 2)
        {
            ChangeText("このカードを持ち上げ\n他のカードの上に置くことで、重ねて、\nインタラクトすることができる");
            tutorialCount = 3;
        }
    }
    void OnTransformParentChanged()
    {
        if (transform.parent.GetComponent<CardBase>() != null)
            if (transform.parent.GetComponent<CardBase>().cardID == "npc_guid" && tutorialCount < 5)
            {
                ChangeText("採取ポイントにこれを重ねると何かが採れる。\nアイテムカードをこれに重ねると\"所持\"できる");
                tutorialCount = 4;
            }
    }
    public void ChangeTutorial(int i)
    {
        tutorialCount = i;
        if (tutorialCount == 5)
        {
            ChangeText("改めてカードを重ね直す事で、会話を進められることもある。");
            cardBase.HideUI();
        }
        if (tutorialCount == 6)
        {
            cardBase.ShowUI();
            ChangeText("一部のカードは、重ねることでスタックできる。\nそうしたカードは、複数同時に所持できる");
        }
        if (tutorialCount == 7)
        {
            ChangeText("スタックされたカードは、右クリックで一つずつ取り出せる。\n適切な数の素材を揃えクラフト台に置き、プレビュー画像をクリックするとクラフト完了。");
        }
    }
    private void ChangeText(string text)//ワンチャンpublic
    {
        cardText.text = text;
    }
}
