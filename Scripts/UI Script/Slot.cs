using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;



public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public Item item; // 획득한 아이템.
    public int itemCount; // 획득한 아이템의 개수.
    public Image itemImage; //아이템의 이미지
    //public int itemtype;

    //필요한 컴포넌트.
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private ItemEffectDatabase theItemEffectDatabase;
    // private WeaponManager theWeaponManager;

    private void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        // theWeaponManager = FindObjectOfType<WeaponManager>();
    }

    // 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        //alpha값이 0이넘어오면 아이템이미지는 안보이게 되도록 설정

        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }



    //아이템 획득
    public void AddItem(Item _item, int _count =1) //획득한아이템,획득한아이템 개수(기본값=1)
    {
        //사용예 
        //1.(기본적으로 1개획득했을시)
        //AddItem(_item);
        //2.3개 획득했을시
        //AddItem(_item,3);

        item = _item;
        itemCount = _count;
        itemImage.sprite = Resources.Load<Sprite>(item.itemImage);

        //itemImage.sprite = item.itemImage;

        //재료등 여러개 획득하는 아이템의 경우
        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else //장비같은 여러개 획득하는 아이템이 아닌경우
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        SetColor(1);
       
    }

    //아이템 슬롯의 갯수를 변경해주는 함수(아이템 갯수 조정)
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        //아이템 개수가 0이된경우 Slot초기화 시키기
        if(itemCount<=0)
            ClearSlot();
    }

    // 슬롯 초기화 함수
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        
        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    //아이템 클릭시 사용
    public void OnPointerClick(PointerEventData eventData)
    {
        //이 스크립트가 적용된 객체에 마우스 우클릭시 실행
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            //아이템이 있을시
            if(item!=null)
            {
               theItemEffectDatabase.UseItem(item);
               // 소모품일경우 1감소
               if(item.itemType == Item.ItemType.Used)
                    SetSlotCount(-1);
            }
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그 시작시 슬롯위치를 마우스위치로 바꿈.
        if (item != null)
        {
            //드래그 슬롯에 현재 슬롯을 넣고 아이템이미지도 넣기.
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //드래그 중에도 계속 슬롯위치를 마우스위치로 바꿈.
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    //드래그가 끝나면 실행
    public void OnEndDrag(PointerEventData eventData)
    {
        //드래그가 끝났을시 투명하게하고 없애기
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
        //transform.position = originPos;
    }

    //다른 슬롯위에서 드래그를 끝냈을시 실행
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) //빈슬롯을 드래그드롭해서 슬롯체인지 하는경우를 막음.
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        //드래그 대상이 된자리에 있는 아이템의 임시정보 저장
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        //드래그 대상자리에 현재 드래그중인 아이템정보를 옮김
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(_tempItem != null) //임시정보가 있었다면
        {
            //드래그시작 맨처음이었던 자리에 임시정보 옮김
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else //빈슬롯이었다면
        {
            //드래그시작 맨처음자리 초기화
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
