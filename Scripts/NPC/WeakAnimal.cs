using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    //뛰기
    public void Run(Vector3 _targetPos)
    {
        destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized;
        //direction = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized; //동물이 맞았을때 뛰기전 플레이어의 반대방향을 바라보게한다.
        //direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles; //동물이 맞았을때 뛰기전 플레이어의 반대방향을 바라보게한다.

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    //약한 동물은 맞았을시 기본데미지받음 + Run동작 실행하기.
    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)
            Run(_targetPos);
    }
}
