using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActionController : MonoBehaviour
{

    [SerializeField]
    private float range; // 습득 가능한 최대 거리.

    private bool pickupActivated = false; // 습득 가능할 시 true.

    private RaycastHit hitInfo; // 충돌체 정보 저장.

    //public static ActionController instance;

    // 아이템 레이어에만 반응하도록 레이어 마스크를 설정. (ex: 땅을 바라봤을때도 반응하면 안됨)
    [SerializeField]
    private LayerMask layerMask; //Item으로 설정

    // 필요한 컴포넌트.
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;
    private WeaponManager theWeaponManager;

    //이벤트 변수
    public bool isFirstGetAxe;
    public bool isFirstGetPickaxe;
    public bool isFirstGetSubMachineGun1;
    


    void Start()
    {
        theWeaponManager = FindObjectOfType<WeaponManager>();
        isFirstGetAxe = false;
        isFirstGetPickaxe = false;
        isFirstGetSubMachineGun1 = false;
    }
    
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item); //부딫힌 충돌채의 ItemPickUp에있는 아이템을 인벤토리에 넣어준다.

                //하드코딩
                if(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName =="RawMeat")
                {
                    QuestUI.instance.AddCurrentMeatNumber();
                }
                //하드코딩
                if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Rock")
                {
                    QuestUI.instance.AddCurrentStoneNumber();
                    QuestUI.instance.currentStoneNumber++;
                }

                if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Wood")
                {
                    QuestUI.instance.currentWoodNumber++;
                }

                Destroy(hitInfo.transform.gameObject); //넣어준후 필드에있는 아이템은 삭제

                //얻은 아이템이 각각의 장비아이템인경우 무기체인지가 가능할수있게 해금.
                if(hitInfo.transform.name =="Axe_Item")
                {
                    theWeaponManager.IsChangeGetAxe();
                    if(!isFirstGetAxe && TutorialDialogue.instance ==null)
                    {
                        isFirstGetAxe = true;
                        EventManager.instance.eventNumber = 3;
                        QuestUI.instance.AddCurrentAxeNumber(); //임시 퀘스트 하드코딩
                    }

                }

                if(hitInfo.transform.name == "Pickaxe_Item")
                {
                    Debug.Log(isFirstGetPickaxe);
                    theWeaponManager.IsChangeGetPickAxe();
                    if(!isFirstGetPickaxe)
                    {
                        Debug.Log("곡괭이이벤트");
                        isFirstGetPickaxe = true;
                        EventManager.instance.eventNumber = 5;
                        QuestUI.instance.AddCurrentPickAxeNumber(); //임시 퀘스트 하드코딩
                    }
                }
                if (hitInfo.transform.name == "SubMachineGun_Item")
                {
                    theWeaponManager.IsChangeGetSubMachineGun1();
                    if (!isFirstGetSubMachineGun1)
                    {
                        isFirstGetSubMachineGun1 = true;
                        EventManager.instance.eventNumber = 7;
                    }
                }


                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        //if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            //LayerMask가 Item이면서 tag도 아이템일경우
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }

            //추가할것(LayerMask가 Item이면서 다른태그일경우)
            //if(hitInfo.transform.tag == "모닥불" && 고기를 들고있을때)
            //{
            //    고기투척하시겠습니까? 정보띄우기
            //}
        }
        else
            InfoDisappear();
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true; //아이템을 주울수있음
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
