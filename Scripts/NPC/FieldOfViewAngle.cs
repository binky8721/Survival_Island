using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour {

    [SerializeField] private float viewAngle = 130; //시야각(120도);
    [SerializeField] private float viewDistance = 10; //시야거리(10미터);
    [SerializeField] private LayerMask targetMask; //타켓 마스크(플레이어);

    [SerializeField] public float up_mount = 2;

    //private Rhino theRhino;
    private PlayerController thePlayer;
    private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        thePlayer = FindObjectOfType<PlayerController>();
        //theRhino = GetComponent<Rhino>();
    }

    public Vector3 GetTargetPos()
    {
        return thePlayer.transform.position;
    }

    //디버그 확인용 경계선계산
    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle*Mathf.Deg2Rad),0f,Mathf.Cos(_angle*Mathf.Deg2Rad)); //삼각함수를 이용하여 시야각 계산후 리턴
    }

    //npc의 시야각 
    public bool View()
    {
        /* 디버그 확인용 경계선
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle *0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        //시야각 표시용 디버깅레이 (Scene에서 확인가능)
        Debug.DrawRay(transform.position + transform.up* up_mount, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up* up_mount, _rightBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up* up_mount, transform.forward, Color.blue);
        */

        //OverlapSphere() 주변에 있는 컬라이더들을 뽑아내서 저장시키는데 사용.
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask); //시야각에있고 시야거리내에있는 콜라이더를 가지고있는 모든객체를 저장

        for(int i=0;i<_target.Length;i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player") //시야각 내에 플레이어가 있을때
            {
                Vector3 _direction = (_targetTf.position - (transform.position+transform.up*up_mount)).normalized; //플레이어와 npc간의 방향벡터를 구한후 정규화한다.
                float _angle = Vector3.Angle(_direction, transform.forward); //플레이어와 npc 간의 각도계산

                if(_angle<viewAngle*0.5f) //계산한 각도가 npc시야각 내에 있다면 
                {
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up*up_mount, _direction, out _hit, viewDistance)) //ray를 시야거리만큼(viewDistance) 쏴서 장애물 검사
                    {
                        if (_hit.transform.name == "Player")
                        {
                            //Debug.Log("플레이어가 "+transform.name+"(npc)시야 내에 있습니다.");
                            //Debug.DrawRay(transform.position + transform.up * up_mount, _direction, Color.blue);
                            return true;
                            //theRhino.Run(_hit.transform.position); //발견과 동시에 플레이어의 반대방향으로 도망가기.
                        }
                        else if(_hit.transform.name == "Terrain")
                        {
                            //Debug.Log(transform.name+"(npc)가 이 지형을 보고있습니다.");
                        }
                    }
                }
            }
            //플레이어가 뛰고있다면
            if(thePlayer.GetIsPlayerRunning())
            {
                if (CalcPathLength(thePlayer.transform.position) <= viewDistance) //계산된 경로길이가 돼지가 볼수있는 시야거리보다 작을경우
                {
                    //Debug.Log("돼지가 주변에서 뛰고 있는 플레이어의 움직임을 파악했습니다.");
                    return true;
                }
            }
        }
        return false;
    }
    

    private float CalcPathLength(Vector3 _targetPos)
    {
        NavMeshPath _path = new NavMeshPath(); //navmesh으로 생성된 경로를 저장하기위한 변수
        nav.CalculatePath(_targetPos, _path); //target의 위치로 이동하는 경로를 계산후 생성된 변수에 저장.

        Vector3[] _wayPoint = new Vector3[_path.corners.Length + 2]; //+2를 하는이유 : 생성된 패스코너의 개수에 플레이어의위치+동물의위치값을 더해줘야된다.
        _wayPoint[0] = transform.position; //첫경로에는 동물의위치
        _wayPoint[_path.corners.Length + 1] = _targetPos; //마지막경로에는 타켓(플레이어)의 위치 를 넣어준다.

        float _pathLength = 0;
        for (int i = 0; i < _path.corners.Length; i++)
        {
            _wayPoint[i + 1] = _path.corners[i]; //웨이포인트에 경로를 넣음.
            _pathLength += Vector3.Distance(_wayPoint[i], _wayPoint[i + 1]); //경로 길이 계산
        }

        return _pathLength; //계산된 경로길이 총합을 리턴
    }
}
