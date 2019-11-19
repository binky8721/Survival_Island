using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoWild : StrongAnimal {

    

    protected override void Update()
    {
        base.Update();
        if (theViewAngle.View() && !isDead && !isAttacking &&!isDamaged) //플레이어를 보고 있고 살아있을경우 && 공격중이아닐경우 에만 추적을 실행
        {
            StopAllCoroutines(); //코루틴 중복실행방지
            StartCoroutine(ChaseTargetCoroutine());
        }


    }

 
}
