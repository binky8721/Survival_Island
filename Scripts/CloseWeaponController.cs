using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//미완성 클래스 (자식클래스가 완성시켜줌)
public abstract class CloseWeaponController : MonoBehaviour {


    
    
    //현재 장착된 Hand형 타입 무기.
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    //공격 상태변수
    protected bool isAttack = false;
    protected bool isSwing = false;

    //광선을 쐈을때 닿은 물체의 정보
    protected RaycastHit hitInfo;

    [SerializeField]
    protected LayerMask layerMask;

    [SerializeField]
    protected Text debugText;



    protected void TryAttack()
    {
        if (EventManager.instance != null) //1스테이지 실행중일때
        {
            if (!Inventory.inventoryActivated && !ControlDialogue.dialogActivated) //인벤토리 활성화시 이거나 대화도중은 무기공격 방지
            {
                //마우스 좌클릭(계속꽉눌러도 처리)
                if (Input.GetButton("Fire1"))
                {
                    if (!isAttack)
                    {
                        //코루틴 실행.
                        StartCoroutine(AttackCoroutine());
                    }
                }
            }
        }
        else if (TutorialDialogue.instance != null) //튜토리얼 실행중일때
        {
            if (!Inventory.inventoryActivated && !TutorialControlDialogue.dialogActivated) //인벤토리 활성화시 이거나 대화도중은 무기공격 방지
            {
                //마우스 좌클릭(계속꽉눌러도 처리)
                if (Input.GetButton("Fire1"))
                {
                    if (!isAttack)
                    {
                        //코루틴 실행.
                        StartCoroutine(AttackCoroutine());
                    }
                }
            }
        }

    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;

        //어택애니메이션 트리거 작동
        currentCloseWeapon.anim.SetTrigger("Attack");

        //공격활성화 적중되기 전까지 딜레이
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true; //스윙값이 true가 된순간 공격이 적중됐는지 체크


        StartCoroutine(HitCoroutine()); //isSwing이 true값인 동안 충돌체크 계속반복

        //공격비활성화(팔을 접는동안)까지 딜레이
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        //팔을 완전히 접으면 공격할수 있게 대기(이미 딜레이값이 된것들은 뺀나머지 시간동안대기)
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);

        isAttack = false;
    }

    //미완성 추상 코루틴으로 남긴다음 자식클래스에서 완성시킬것임 (AxeController,HandController)
    protected abstract IEnumerator HitCoroutine();
    //{
        /*
        //이 코루틴은 isSwing이 true일때만 실행됨 false되면 자동종료
        while (isSwing)
        {
            if (CheckObject()) //충돌했음
            {
                isSwing = false; //적중앙에 있을때 중복대미지가 들어가는 것을 방지하기위해 isSwing을 꺼줌.

                //디버그 텍스트
                Debug.Log(hitInfo.transform.name);
                debugText.gameObject.SetActive(true);
                debugText.text = hitInfo.transform.name + "<color=yellow>" + " 히트! " + "</color>";
                yield return new WaitForSeconds(0.5f);
            }
            yield return null; //while문 한번돌동안 1프레임대기
        }
        debugText.gameObject.SetActive(false);
        */
    //}

    protected bool CheckObject()
    {
        //레이를 캐릭터 앞쪽 방향으로 쏴서 맞은물체가 있으면 true
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }

    // 완성된 함수이지만, 자식클래스에서 추가 편집이 가능한 함수.
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
