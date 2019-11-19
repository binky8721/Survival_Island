using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : WeakAnimal
{
    protected override void Update()
    {
        base.Update();
        if(theViewAngle.View() && !isDead) //플레이어를 보고 있고 살아있을경우
        {
            Run(theViewAngle.GetTargetPos());
        }
    }

    protected override void ReSet()
    {
        base.ReSet(); //기존에있던 부모객체의 리셋을 실행시킨후 
        RandomAction(); //랜덤액션 실행
    }

    //평상시 실행할 액션들을 랜덤으로 수행
    private void RandomAction()
    {
        RandomSound();

        //랜덤난수를 애니메이션 개수만큼 범위설정하여 만듬
        int _random = Random.Range(0, 4); // 4개 (대기,풀뜯기,두리번,걷기)

        switch (_random)
        {
            case 0:
                Wait();
                break;
            case 1:
                Eat();
                break;
            case 2:
                Peek();
                break;
            case 3:
                TryWalk();
                break;
        }
    }

    //서있음(Idle1)
    private void Wait()
    {
        currentTime = waitTime;
    }
    //먹기
    private void Eat()
    {
        currentTime = waitTime; //나중에 EatTime으로 수정할수도있음
        anim.SetTrigger("Eat");
    }
    //두리번(Idle2) //두리번 애니메이션 프레임 수정해야됨
    private void Peek()
    {
        currentTime = waitTime; //나중에 peekTime으로 수정할수도있음
        anim.SetTrigger("Peek");
    }
}
