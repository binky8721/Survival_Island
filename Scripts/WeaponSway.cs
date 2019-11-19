using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무기 흔들림 스크립트
public class WeaponSway : MonoBehaviour {


    //기존 위치.
    private Vector3 originPos;

    //현재 위치.
    private Vector3 currentPos;

    //최대 무기흔들림(sway) 한계치
    [SerializeField]
    private Vector3 limitPos; //0.18 , 0.1, 0

    //정조준시 최대 무기흔들림 한계치
    [SerializeField]
    private Vector3 fineSightLimitPos; //0.005 , 0.005 , 0

    //흔들림의 부드러운 정도.
    [SerializeField]
    private Vector3 smoothSway; // 0.05, 0.03 , 0

    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;

	// Use this for initialization
	void Start ()
    {
        originPos = this.transform.localPosition; //이 스크립트가 붙여져있는 물체의 로컬좌표를 오리지널 좌표값으로 초기화
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.canPlayerMove &&!Inventory.inventoryActivated && !ControlDialogue.dialogActivated) //인벤토리 비활성화 or 대화 비활성화 상태일때만 무기흔들림 발생
        {
            TrySway();
        }
	}

    private void TrySway()
    {
        //상하좌우로 어떻게든 마우스가 움직였을시 실행
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
            BackToOriginPos(); //마우스가 움직이지 않을시 원래위치로설정
    }

    private void Swaying()
    {
        //마우스 움직임값 임시변수
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        //정조준상태 아닐시 흔들림
        if (!theGunController.isFineSightMode)
        {
            //Clamp:화면밖으로 벗어나지않게 가두기 + Lerp:이동값을 천천히 움직이게함
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
                           originPos.z);
        }
        else //정조준시 흔들림
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                           originPos.z);
        }

        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
