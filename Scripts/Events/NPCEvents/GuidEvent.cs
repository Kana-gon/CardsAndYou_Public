using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidEvent : MonoBehaviour
{
    private Dictionary<string, bool> switchs = new Dictionary<string, bool>();
    [SerializeField] Sprite cardIllust_suits;
    [SerializeField] private bool ISDEBUG = false;
    void Awake()
    {
        switchs.Add("bringSuits", false);
        //switchs.Add("bringResource", false);
        switchs.Add("bringResource_NotEnough", false);
        switchs.Add("bringResource_Enough", false);
        switchs.Add("bringSword", false);
        GetComponent<NPCCard>().talkedEventDelegate = PlayerTalkedToGuid;
        GetComponent<NPCCard>().talkedEventDelegate_withoutPlayer = MouseTalkedToGuid;
        GetComponent<NPCCard>().ChangeTalkThema("Guid_start");
    }
    public void PlayerTalkedToGuid()
    {
        GameObject grandChildCard = GetComponent<CardBase>().childCard.GetComponent<CardBase>().childCard;
        if (grandChildCard != null)
        {//これの子カード(=プレイヤーカード)に子カードがあれば
            Bring(grandChildCard);
        }
    }
    public void MouseTalkedToGuid()
    {
        GameObject childCard = GetComponent<CardBase>().childCard;
        if (childCard != null)
        {//これの子カード(=プレイヤーカード)に子カードがあれば
            Bring(childCard);
        }
    }
    public void Bring(GameObject broughtItem)
    {
        //*if文が関数側にあるのは気持ち悪くない？しかし書き方としてはスッキリ
        if (Event_BringItem("bringSuits", "item_guid'sSuit", broughtItem))
        {
            broughtItem.transform.SetParent(null);
            StartCoroutine(DestroyCardSafety(broughtItem));
            GetComponent<CardBase>().childCard.GetComponent<PlayerCard>().ChangeTutorial(5);
        }
        if (switchs["bringSuits"])
        {
            //Event_BringItem("bringResource", "item_resource");
            if (Event_BringItem("bringResource_NotEnough", "item_resource", broughtItem, 1, 2))
            {
                GetComponent<CardBase>().childCard.GetComponent<PlayerCard>().ChangeTutorial(6);
            }
            if (Event_BringItem("bringResource_Enough", "item_resource", broughtItem, 3, -1))
            {
                GetComponent<CardBase>().childCard.GetComponent<PlayerCard>().ChangeTutorial(7);
            }
        }
        if (switchs["bringResource_Enough"])
            Event_BringItem("bringSword", "item_weapon_sword", broughtItem);
        if (ISDEBUG)
        {
            if (Event_BringItem("bringSuits", "item_guid'sSuit", broughtItem))
            {
                broughtItem.transform.SetParent(null);
                StartCoroutine(DestroyCardSafety(broughtItem));
                GetComponent<CardBase>().childCard.GetComponent<PlayerCard>().ChangeTutorial(5);
            }
            if (Event_BringItem("bringResource_NotEnough", "item_resource", broughtItem, 1, 2))
            {
                GetComponent<CardBase>().childCard.GetComponent<PlayerCard>().ChangeTutorial(6);
            }
            if (Event_BringItem("bringResource_Enough", "item_resource", broughtItem, 3, -1))
            {
                GetComponent<CardBase>().childCard.GetComponent<PlayerCard>().ChangeTutorial(7);
            }

            Event_BringItem("bringSword", "item_weapon_sword", broughtItem);
        }
    }
    private IEnumerator DestroyCardSafety(GameObject target)
    {
        yield return new WaitForSeconds(0.1f);
        if (target != null)
        {
            Destroy(target);
        }
    }

    /// <summary>
    /// アイテムを持ってきた時に、特定の会話IDに移動する。
    /// </summary>
    /// <param name="switchName"></param>
    /// <param name="cardID"></param>
    /// <param name="itemNum_min">イベント発生のためのアイテムの最小個数。デフォルトでは1</param>
    /// <param name="itemNum_max">イベント発生のためのアイテムの最大個数。デフォルトでは-1で、最大値は無限</param>
    /// <returns>条件に合致したかどうか</returns>
    private bool Event_BringItem(string switchName, string cardID, GameObject broughtItem, int itemNum_min = 1, int itemNum_max = -1)
    {
        NPCCard npcCard = GetComponent<NPCCard>();
        if (broughtItem.GetComponent<CardBase>().cardID == cardID && switchs[switchName] == false && broughtItem.GetComponent<CardBase>().num >= itemNum_min)
        {
            if (itemNum_max == -1)
            {
                npcCard.ChangeTalkThema("Guid_" + switchName);
                switchs[switchName] = true;
                return true;
            }
            else if (broughtItem.GetComponent<CardBase>().num <= itemNum_max)
            {
                npcCard.ChangeTalkThema("Guid_" + switchName);
                switchs[switchName] = true;
                return true;
            }
        }
        return false;

    }
    public void illustChange(string illustName)//*汎用性高そうなら辞書型にしてNPCCard.csに移植
    {
        if (illustName == "suits")
        {
            transform.Find("CardIllust").GetComponent<SpriteRenderer>().sprite = cardIllust_suits;
        }
    }
}
