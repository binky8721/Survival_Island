using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    [SerializeField]
    private int hp; //나무의 체력.

    [SerializeField]
    private float destroyTime; // 파편 제거되는 시간.

    [SerializeField]
    private BoxCollider col; //해당 나무 콜라이더 (나무가 파괴되고나서는 지워줘야됨)

    //필요한 게임 오브젝트.
    [SerializeField]
    private GameObject goTree; // 일반 나무.
    [SerializeField]
    private GameObject goDebris; //깨진 나무.
    //[SerializeField]
    //private GameObject goEffectPrefabs; //나무 이팩트
    [SerializeField]
    private GameObject goTreeItemPrefab; //부셔졌을때 나오는 획득가능한 나무아이템

    // 나무 아이템 등장 개수.
    [SerializeField]
    private int generateTreeItemNum;


    //필요한 사운드 이름.
    //[SerializeField]
    //private string strikeSound;
    //[SerializeField]
    //private string destroySound;


    public void HitTree()
    {
        SoundManager.instance.PlaySE("WoodStrike");

        //이팩트프리팹을 바위콜라이더 중심에서 기본회전값으로 생성.
        //var clone = Instantiate(goEffectPrefabs, col.bounds.center, Quaternion.identity);
        //이팩트 일정시간후 삭제
        //Destroy(clone, destroyTime);

        hp--;
        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        //SoundManager.instance.PlaySE(destroySound);
        col.enabled = false; //기존 나무 콜라이더 없앰
        for (int i = 0; i <= generateTreeItemNum; i++)
        {
            Instantiate(goTreeItemPrefab, new Vector3(goTree.transform.position.x, goTree.transform.position.y+5.0f, goTree.transform.position.z), Quaternion.identity); //부서진 바위 위치에 얻을수 있는 바위아이템 여러개 생성
        }
        Destroy(goTree); //기존 나무 파괴


        //나무 조각 활성화후 일정시간뒤 삭제
        goDebris.SetActive(true);
        Destroy(goDebris, destroyTime);
    }
}
