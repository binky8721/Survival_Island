using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField]
    protected int attackDamage; //공격 데미지.
    [SerializeField]
    protected float attackDelay; //공격 딜레이.
    [SerializeField]
    private float attackRange = 3.0f; //공격 사정거리.
    [SerializeField]
    protected LayerMask targetMask; //타겟 마스크.


    [SerializeField]
    protected float chaseTime; //총 추격시간
    protected float currentChaseTime; //계산
    [SerializeField]
    protected float chaseDelayTime; //추격 딜레이 (딜레이타임마다 플레이어의 위치를 갱신해서 플레이어를 추격)

    //ex: chaseTime이4초 , chaseDelayTime이 0.5라면 총4초동안 플레이어의 추격위치를 0.5초마다 갱신하여 쫓는다. 

    //플레이어 추격
    public void Chase(Vector3 _targetPos)
    {
        isChasing = true;
        //플레이어의 위치가 destination으로 설정되도록 함.
        destination = _targetPos; 
        nav.speed = runSpeed;
        isRunning = true;
        anim.SetBool("Running", isRunning);
        nav.SetDestination(destination);
    }

    //공격성 동물은 맞았을시 기본데미지받음 + Chase동작 실행하기.
    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)
            Chase(_targetPos);
    }

    protected override void Dead()
    {
        nav.ResetPath(); //공격을 하기전 멈춘다.
        currentChaseTime = chaseTime; //chase코루틴 반복조건 종료.
        StopAllCoroutines(); //공격시도 코루틴 등의 코루틴 수행 방지.
        base.Dead();
    }

    protected IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;
        while (currentChaseTime < chaseTime)
        {
            Chase(theViewAngle.GetTargetPos());
            if (Vector3.Distance(transform.position, theViewAngle.GetTargetPos()) <= 3f) //야생 리노와 타겟(플레이어)의 위치가 매우 가까웠을때(3f)
            {
                //눈에 보였을때.
                if (theViewAngle.View())
                {
                    
                    Debug.Log("몬스터의 플레이어에 대한 공격시도.");
                    StartCoroutine(AttackCoroutine()); //몬스터의 공격시도 코루틴 실행
                }
            }
            yield return new WaitForSeconds(chaseDelayTime); //딜레이 타임마다 플레이어 위치를 갱신해서 플레이어를 추격
            currentChaseTime += chaseDelayTime;
        }
        isChasing = false;
        isRunning = false;
        anim.SetBool("Running", isRunning);
        nav.ResetPath();
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        nav.ResetPath(); //공격을 하기전 멈춘다.
        currentChaseTime = chaseTime; //chase코루틴 반복조건 종료.


        yield return new WaitForSeconds(0.5f);  //0.5초정도 딜레이 이후
        transform.LookAt(theViewAngle.GetTargetPos()); //플레이어를 바라보게 만든다.
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);//공격자세 취할때 딜레이
        RaycastHit _hit;
        if (Physics.Raycast(transform.position + Vector3.up * theViewAngle.up_mount, transform.forward, out _hit, attackRange, targetMask))
        {
            Debug.Log("NPC의 공격!! 공격은 적중!");
            thePlayerStatus.DecreaseHP(attackDamage);

            StatusController theStatus = FindObjectOfType<StatusController>();
            StartCoroutine(theStatus.ShowBloodScreen());

            SoundManager.instance.PlaySE("PlayerDamaged");
        }
        else
        {
            Debug.Log("NPC의 공격!! 공격은 빗나감!");
            SoundManager.instance.PlaySE("PlayerAvoid");
        }

        yield return new WaitForSeconds(attackDelay);

        isAttacking = false; //공격이 다끝났으므로 false
        StartCoroutine(ChaseTargetCoroutine()); //그 후 다시 쫒는코루틴 시작
    }
}
