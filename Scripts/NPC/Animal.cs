using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour {

    protected StatusController thePlayerStatus;

    [SerializeField] protected string animalName; //동물의 이름
    [SerializeField] protected int hp; //동물의 체력.
    [SerializeField] protected float walkSpeed; //동물의 걷기 스피드
    [SerializeField] protected float runSpeed; //동물의 뛰기 스피드
    //[SerializeField] protected float turningSpeed; //회전 스피드 (값이 높을수록 동물이 민첩하게 회전하여 움직인다. //돼지는 0.2로 설정하자.

    //protected float applySpeed;

    protected Vector3 destination; //목적지
    //protected Vector3 direction; //방향

    protected float destroyTime; //죽은후 파괴될때까지 시간

    //상태변수
    protected bool isAction; //행동중인지 아닌지 판별. (true일때만 currentTime을 깎다가 currentTime이 0보다작아지면 행동이끝났으므로 false로바꿀꺼임)
    protected bool isWalking; //걷는지 안 걷는지 판별.
    protected bool isRunning; //뛰는지 판별.
    protected bool isDead; //죽었는지 판별
    protected bool isChasing; //추격중인지 판별
    protected bool isAttacking; //공격중인지 판별
    protected bool isDamaged; //데미지를 받고있는지 판별

    [SerializeField] protected float walkTime; //걷기 시간
    [SerializeField] protected float waitTime; //대기 시간
    [SerializeField] protected float runTime; //뛰는 시간
    protected float currentTime; //시간계산에필요

    //필요한 컴포넌트
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;

    [SerializeField]
    protected GameObject goDropItem; //죽었을시 나오는 날고기 아이템

    protected NavMeshAgent nav;
    protected FieldOfViewAngle theViewAngle;
    protected AudioSource theAudio;

    [SerializeField] protected AudioClip[] soundNormal;
    [SerializeField] protected AudioClip soundHurt;
    [SerializeField] protected AudioClip soundDead;

    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
        nav = GetComponent<NavMeshAgent>();
        theViewAngle = GetComponent<FieldOfViewAngle>();
        theAudio = GetComponent<AudioSource>();
        //시작할때 대기행동
        currentTime = waitTime;
        isAction = true;
        destroyTime = 5.0f;
    }

    //자식객체에서 추가로 더 수정할수 있도록 protected virtual로 선언
    protected virtual void Update()
    {
        if (!isDead) //살아있을때만 실행
        {
            Move();
            //Rotation();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
            nav.SetDestination(transform.position + destination * 5f); //3f : 한번움직일때 움직이는 값(하드코딩)
            //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime)); //보는방향으로 워크스피드만큼 움직임
    }

    /*
    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, turningSpeed) //이때 x,z회전값은 고정시킨다.
                , 0.01f); //자기자신위치에 랜덤값을넣은 방향값을 넣어준다..
            rigid.MoveRotation(Quaternion.Euler(_rotation)); //받은회전값을 쿼터니온으로 변환하여 리지드 회전값에 넣어준다.
        }
    }
    */

    //시간경과 함수 (일정시간이 지나면 행동리셋)
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 &&!isChasing && !isAttacking) //추격중,공격중이 아닐경우에만 리셋가능
                ReSet();
        }
    }

    protected virtual void ReSet()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        nav.speed = walkSpeed;
        nav.ResetPath(); //목적지 초기화 // ex:뛰는 행위도중 리셋이 되면 뛰는행동 멈춤.
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f)); //0.2만큼 좌,우측으로 꺾음(하드코딩)
        //direction.Set(0f, Random.Range(0f, 360f), 0f);
    }



    //걷기
    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
    }


    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        //죽지 않았을 경우만 실행
        if (!isDead)
        {
            hp -= _dmg; //데미지만큼 체력감소

            if (hp <= 0)
            {
                Dead(); //
                return;
            }
            //맞았을시 사운드
            PlaySE(soundHurt);
            //맞았을시 애니메이션 실행
            DamageCoroutine();
            //anim.SetTrigger("Hurt");
        }
    }

    protected IEnumerator DamageCoroutine()
    {
        isDamaged = true;

        anim.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.7f);

        isDamaged = false;
    }


    protected virtual void Dead()
    {
        PlaySE(soundDead);
        isWalking = false; //죽었으니 움직이면 안됨
        isRunning = false;
        anim.SetTrigger("Dead");
        isDead = true;
        Instantiate(goDropItem, new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z), Quaternion.Euler(90, 0, 0));//죽었을시 날고기 생성
        Destroy(transform.gameObject, destroyTime);
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); //일상 사운드3개
        PlaySE(soundNormal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
