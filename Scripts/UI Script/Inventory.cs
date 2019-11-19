using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static bool inventoryActivated = false;

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    // 슬롯들
    private Slot[] slots;

    public Slot[] GetSlots() { return slots; } //인벤토리 슬롯들 전부반환 함수 (저장용)

    [SerializeField]
    private Item[] items;

    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
        }
    }

    // Use this for initialization
    void Start ()
    {
        //슬롯부모및에있는 자식들인 각각의 슬롯들의 값을 넣어줌
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        TryOpenInventory();
	}

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        //GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        //GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
    }

    

    public void AcquireItem(Item _item, int _count = 1) //기본적으로 얻는 아이템 디폴트 값은 1개.
    {
        if (Item.ItemType.Equipment != _item.itemType) //장비가 아닌 아이템의 경우
        {
            for (int i = 0; i < slots.Length; i++)
            {
                //i번째 slot 아이템이 비어있지 않을때만 비교
                if (slots[i].item != null)
                {
                    //slot안에 이미 대상 아이템이 있는경우
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        //그 슬롯의 아이템 개수만 늘려준다.
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            //아이템 슬롯에 빈자리가 있는경우
            if (slots[i].item==null)
            {
                //해당 슬롯에다가 아이템을 더해준다.
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }


    public void UseItem(Item _item, int _count = 1) //기본적으로 얻는 아이템 디폴트 값은 1개.
    {

    }

    //아이템이 인벤토리에 몇개있는지 받는 함수
    public int GetItemCount(string _itemName)
    {
        /*
        //퀵슬롯까지 검사하는 코드
        int temp = SearchSlotItem(slots, _itemName);

        //temp가 0이아니라면(찾는 아이템이 인벤토리에 있을시) temp반환
        return temp != 0 ? temp : SearchSlotItem(quickslots, _itemName);
        */

        return SearchSlotItem(slots, _itemName);
    }

    private int SearchSlotItem(Slot[] _slots, string _itemName)
    {
        //슬롯검사
        for (int i = 0; i < _slots.Length; i++)
        { 
            if(_slots[i].item!=null) //아이템이 있을시
            {
                if (_itemName == _slots[i].item.itemName)//아이템을 찾으면 아이템 갯수가 몇개있는지 파악하여 반환
                    return _slots[i].itemCount;
            }

        }

        //찾는아이템이 슬롯에 없는경우 0개 반환
        return 0;
    }

    public void SetItemCount(string _itemName, int _itemCount)
    {
        //퀵슬롯까지 검사
        //if (!ItemCountAdjust(slots, _itemName, _itemCount))
        //    ItemCountAdjust(quickslots, _itemName, _itemCount);

        ItemCountAdjust(slots, _itemName, _itemCount);
    }

    private bool ItemCountAdjust(Slot[] _slots, string _itemName, int _itemCount)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i].item !=null) //아이템이 있을경우
            {
                if (_itemName == _slots[i].item.itemName) //아이템 이름일치하면
                {
                    _slots[i].SetSlotCount(-_itemCount); //갯수차감
                    return true;
                }
            }

        }
        return false;
    }
}
