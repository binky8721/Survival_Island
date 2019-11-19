using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; //바위의 체력.

    [SerializeField]
    private float destroyTime; // 파편 제거되는 시간.

    [SerializeField]
    private SphereCollider col; //구체 콜라이더 (바위가 파괴되고나서는 지워줘야됨)

    //필요한 게임 오브젝트.
    [SerializeField]
    private GameObject goRock; // 일반 바위.
    [SerializeField]
    private GameObject goDebris; //깨진 바위.
    [SerializeField]
    private GameObject goEffectPrefabs; //채굴 이팩트
    [SerializeField]
    private GameObject goRockItemPrefab; //부셔졌을때 나오는 획득가능안 돌맹이아이템.

    // 돌맹이 아이템 등장 개수.
    [SerializeField]
    private int generateRockItemNum;


    //필요한 사운드 이름.
    [SerializeField]
    private string strikeSound;
    [SerializeField]
    private string destroySound;


    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);
        //이팩트프리팹을 바위콜라이더 중심에서 기본회전값으로 생성.
        var clone = Instantiate(goEffectPrefabs, col.bounds.center, Quaternion.identity);
        //이팩트 일정시간후 삭제
        Destroy(clone, destroyTime);

        hp--;

        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroySound);
        col.enabled = false; //기존 바위 콜라이더 없앰
        for (int i = 0; i <= generateRockItemNum; i++)
        { 
            Instantiate(goRockItemPrefab, goRock.transform.position, Quaternion.identity); //부서진 바위 위치에 얻을수 있는 바위아이템 여러개 생성
        }
        Destroy(goRock); //기존 바위 파괴


        //바위 조각 활성화후 일정시간뒤 삭제
        goDebris.SetActive(true);
        Destroy(goDebris, destroyTime);
    }
}
