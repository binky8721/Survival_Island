using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //인스펙터창에서 따로만든 클래스가 뜰수있도록 설정(수정용도)
public class ItemEffect
{
    //Tooltip으로 Inspector창에서도 주의사항을 볼수있도록함.

    [Tooltip("해당 아이템 이름이 틀리면 오류나므로 정확히 사용.")]
    public string itemName; //아이템의 이름(키값으로 사용)
    [Tooltip("(HP, SP, DP, HUNGRY, THIRSTY, SATISFY)만 가능함.)")]
    public string[] part; // 아이템을 사용할 부위
    [Tooltip("각 부위에 적용될 효과수치")]
    public int[] num; // 아이템효과 수치
}

public class ItemEffectDatabase : MonoBehaviour //아이템사용 관련된것 모두관리하는 스크립트
{
    public Item itemData;

    [SerializeField]
    private ItemEffect[] itemEffects;

    //필요한 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatus;
    [SerializeField]
    private WeaponManager theWeaponManager;

    //하드코딩 방지하기위한 변수의 상수화.
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    //아이템 사용
    public void UseItem(Item _item)
    {
        //아이템 타입이 장비아이템일경우
        if (_item.itemType == Item.ItemType.Equipment)
        {
            // 장착 실행
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName)); //ex: "GUN" ,"SubMachineGun1"
        }

        //아이템 타입이 재료아이템일 경우
        if(_item.itemType == Item.ItemType.Ingredient)
        {

        }

        if (_item.itemType == Item.ItemType.Used) //아이템타입이 소모품인경우
        {
            for (int x = 0; x < itemEffects.Length; x++) //이팩트 개수만큼 확인
            {
                if(itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++) //해당이팩트의 부위개수만큼 확인
                    {
                        switch(itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                thePlayerStatus.IncreaseSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                thePlayerStatus.IncreaseDP(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                //thePlayerStatus.IncreaseSatisfy(itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위. (HP,SP,DP,HUNGRY,THIRSTY,SATISFY)만 가능합니다."); //부위를 잘못넣었을시
                                break;
                        }
                        Debug.Log(_item.itemName + " 을 사용했습니다.");
                    }
                    return; //바깥 반복문 추가수행 할필요없으므로 리턴
                }
               
            }
            Debug.Log("ItemEffectDatabase에 일치하는 itemName 없습니다."); //아이템이름을 잘못적용해서 일치하는 아이템이 없을시
        }

        //.asset 파일에 데이터 저장
        //UnityEditor.EditorUtility.SetDirty(itemData);
    }
    
}
