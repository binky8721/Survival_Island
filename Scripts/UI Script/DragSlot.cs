using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour {

    static public DragSlot instance; //자기자신반환 인스턴스

    public Slot dragSlot;

    //아이템 이미지
    [SerializeField]
    private Image imageItem;

    void Start()
    {
        instance = this; //자기자신을 넣어서 초기화
    }

    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite; //받아온 이미지의 스프라이트를 넣어줌
        SetColor(1); //흰색 배경이미지를 드래그시 보여주는 용도
    }

    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
