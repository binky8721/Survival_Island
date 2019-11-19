using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeController : CloseWeaponController {

    // 활성화 여부.
    public static bool isActivate = false;



    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {

        //이 코루틴은 isSwing이 true일때만 실행됨 false되면 자동종료
        while (isSwing)
        {
            if (CheckObject()) //충돌했음
            {
                //바위일경우
                if(hitInfo.transform.tag == "Rock")
                {
                    //채굴(바위hp감소)
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
                //육식동물일경우
                else if (hitInfo.transform.tag == "StrongAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<StrongAnimal>().Damage(2, transform.position);
                }
                //초식동물일경우
                else if (hitInfo.transform.tag == "WeakAnimal")
                {
                    //동물에게 일정데미지 입힘
                    //hitInfo.transform.GetComponent<Rhino>().Damage(currentCloseWeapon.damage,transform.position); //도끼의 데미지와 도끼의 현재위치를 넘겨줌
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position); //실험용 데미지1
                }

                isSwing = false; //적중앙에 있을때 중복대미지가 들어가는 것을 방지하기위해 isSwing을 꺼줌.

                //디버그 텍스트
                Debug.Log(hitInfo.transform.name);
                //debugText.gameObject.SetActive(true);
                //debugText.text = hitInfo.transform.name + "<color=yellow>" + " 히트! " + "</color>";
                yield return new WaitForSeconds(0.5f);
            }
            yield return null; //while문 한번돌동안 1프레임대기
        }
        //debugText.gameObject.SetActive(false);

    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
