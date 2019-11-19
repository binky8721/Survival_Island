using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour {

    public string closeWeaponName; // 너클이나 맨손을 구분.

    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range; //공격 범위
    public int damage; //공격력
    //public float workSpeed; //작업 속도.
    public float attackDelay; //공격 딜레이
    public float attackDelayA; //공격 활성화 시점을 위한 딜레이(ex:주먹이 완벽히 나가기 전까지는 데미지x)
    public float attackDelayB; //공격 비활성화 시점을 위한 딜레이(ex:주먹이 완벽히 되돌아오기전까지는 주먹이 닿아도 데미지x)

    public Animator anim; //애니메이션

}
