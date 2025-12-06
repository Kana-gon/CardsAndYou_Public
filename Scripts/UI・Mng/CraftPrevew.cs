using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftPrevew : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject productImage;
    [SerializeField]
    TextMeshPro productNumText;
    public bool clicked = false;
    [SerializeField]
    private Renderer button;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void DisplayPrevew(GameObject productObj,int productNum){
        button.material.color = Color.white;
        productNumText.text = productNum.ToString();
        productImage.GetComponent<SpriteRenderer>().sprite = productObj.transform.Find("CardIllust").GetComponent<SpriteRenderer>().sprite;
        productImage.transform.localScale = new Vector3(0.5f, 0.5f, 1);//*一旦はみ出しOKで
        gameObject.SetActive(true);
    }

    public void HidePrevew(){
        gameObject.SetActive(false);
    }
    void OnMouseDown()
    {
        clicked = true;
        button.material.color = Color.gray;
    }
    // void OnMouseUp()
    // {
    //     clicked = false;
    // }
}
