using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")] //project내에서 New Item->Item로 이 클래스스크립트를 만들수있는 메뉴 추가
public class Item : ScriptableObject //ScriptableObject 게임오브젝트에 붙일필요없는 script로 선언
{

    public string itemName; // 아이템의 이름.
    public ItemType itemType; // 아이템의 유형.
    public string itemImage;
    //public Sprite itemImage; // 아이템의 이미지.
    public GameObject itemPrefab; // 아이템의 프리팹.

    public string weaponType; // 무기 유형.

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

    //아이템 이미지 경로
    //도끼 UIasset/Weapons/Axe_Normal
    //날고기 UIasset/Weapons/Meat_Raw
    //곡괭이 UIasset/pickaxe2
    //포션 UIasset/Potions/Major_Potion_yellow
    //바위 UIasset/Ore/Ore_Coal
    //총 UIasset/item_submachineGun
    //나무 UIasset/item_twig
}
