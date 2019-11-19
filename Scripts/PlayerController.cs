using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float walkSpeed =3.0f; //플레이어 걷는 속도

    [SerializeField]
    private float runSpeed =7.0f;
    [SerializeField]
    private float crouchSpeed = 1.5f; //앉았을때 속도

    private float applySpeed;

    [SerializeField]
    private float jumpForce = 10.0f;

    //상태변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // 움직임 체크 변수
    private Vector3 lastPos; //전프레임의 플레이어의 현재위치를 기록하는 용도

    //앉았을 때 얼마나 앉을지 결정하는 변수.
    [SerializeField]
    private float crouchPosY; 
    private float originPosY; //처음높이값 저장용 변수
    private float applyCrouchPosY;

    //땅 착지 여부
    private CapsuleCollider capsuleCollider;
    
    [SerializeField]
    private float lookSensitivity =3.0f; //카메라 민감도

    [SerializeField]
    private float cameraRotationLimit =45.0f; //카메라 회전 제한값
    private float currentCameraRotationX = 0f; //정면을 바라보게함(기본값)

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera; //플레이어를 따라다닐 카메라 원하는 카메라를 여기에 직접넣기위한 용도
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;

    private Rigidbody myRigid; //플레이어의 실체 몸체

    //배고픔게이지
    public Slider HungrySlider;

    

    void Start ()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theGunController = FindObjectOfType<GunController>();
        theStatusController = FindObjectOfType<StatusController>();

        //초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
	}
	

	void Update ()
    {
        if (EventManager.instance != null) //이벤트매니져가 있을때
        {
            if (GameManager.canPlayerMove)
            {
                IsGround();
                TryJump();
                TryRun(); //Move()보다 먼저실행
                TryCrouch();
                Move();
                MoveCheck();

                if (!Inventory.inventoryActivated && !ControlDialogue.dialogActivated) //인벤토리 비활성화 상태이거나 대화중이 아닐시에만 카메라움직임
                {
                    CameraRotation();
                    CharacterRotation();
                }
            }
        }
        else if(TutorialDialogue.instance != null) //튜토리얼이 실행되고있을때
        {
            if (TutorialGameManager.t_canPlayerMove)
            {
                IsGround();
                TryJump();
                TryRun(); //Move()보다 먼저실행
                TryCrouch();
                Move();
                MoveCheck();

                if (!Inventory.inventoryActivated && !TutorialControlDialogue.dialogActivated) //인벤토리 비활성화 상태이거나 대화중이 아닐시에만 카메라움직임
                {
                    CameraRotation();
                    CharacterRotation();
                }
            }
        }

        //배고픔게이지 계속감소
        //HungrySlider.value -= 0.0001f;
    }

    //튜토리얼에서 첫번째 튜토리얼 체크용 함수.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Tutorial")
        {
            if (TutorialDialogue.instance.tu_eventNumber == 2)
                TutorialDialogue.instance.tu_eventNumber = 3;
        }
    }

    //앉기 시도
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉는 동작
    private void Crouch()
    {
        isCrouch = !isCrouch; // 이 한줄이 밑에 조건문이랑 같은 역할. (앉아있으면 일어나게, 일어나있으면 앉아있게..)
        //if (isCrouch)
        //    isCrouch = false;
        //else
        //    isCrouch = true;
        theCrosshair.CrouchingAnimation(isCrouch);


        if(isCrouch) //앉을때
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else //서있을때
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
    }

    //앉을때 부드러운 카메라이동을 위한 코루틴
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0; //임시 카운터 변수
        while(_posY != applyCrouchPosY)
        {
            count++;
            //보간을 이용하여 자연스럽게 값변경
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15) //어느정도 보간을 반복하면 코루틴 종료
                break;
            yield return null; //1프레임 대기
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    //지면 체크
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.5f); //대각선이나 살짝 남는공간이 있을수있으므로 여유값0.5를 추가함.
        //theCrosshair.RunningAnimation(!isGround);
        theCrosshair.JumpingAnimation(!isGround);
    }

    //점프 시도
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP()>0) //스페이스+지면에있어야하고+스테미나가 0이상일시 점프시도
        {
            Jump();
        }
    }

    //점프
    private void Jump()
    {
        //앉아있는 상태에서 점프를 하면 앉은상태는 해제되도록 설정.
        if (isCrouch)
            Crouch();

        theStatusController.DecreaseStamina(100); //스태미나 감소

        myRigid.velocity = transform.up * jumpForce;
    }

    //달리기 시도
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0) //뛰다가 SP가 0보다 같거나 작아져도 달리기취소
        {
            RunningCancel();
        }
    }
    //달리기 실행
    private void Running()
    {
        //앉은 상태에서 달리기 실행하면 앉은상태는 해제되도록 설정.
        if (isCrouch)
            Crouch();

        //뛸때 정조준 모드 해제
        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        theStatusController.DecreaseStamina(1); //스테미나 감소.
        applySpeed = runSpeed;
    }
    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }
    //움직임 실행
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        //정규화한 방향값에 속도를 곱해줌
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        //기존포지션 + 델타타임만큼 나눈 움직임값을 더해주기
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        
    }

    private void MoveCheck()
    {
        if(!isRun && !isCrouch && isGround) //뛰고있을때와 앉아있을때는 MoveCheck를 체크안해도 됨 -> 플레이어가 걷고있지않고 웅크리지 않고 + 땅에있을때만 체크
        {
            if (Vector3.Distance(lastPos,transform.position)>=0.01f) //경사면에서 미끄러질때 미세하게움직이는 경우는 제외하고 움직일때 움직임값을 트루
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }

        
    }

    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    //상하 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y"); //임시 카메라 회전값
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX; //현재 카메라 회전값에 임시카메라회전값을 더함
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); // 회전값을 제한( -45~+45)

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //플레이어 뛰는지여부 반환
    public bool GetIsPlayerRunning()
    {
        return isRun;
    }
}
