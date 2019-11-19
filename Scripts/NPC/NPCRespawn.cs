using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRespawn : MonoBehaviour {

    //npc 출현할 위치를 담을 위치
    private Vector3 spawnPoint;
    //npc프리팹
    [SerializeField]
    private GameObject monsterPrefab;
    [SerializeField]
    private GameObject generateParent;

    //플레이어위치 받기용 변수
    private PlayerController thePlayer;

    //스폰 범위변수
    private float spawnRange = 10.0f;

    //npc발생 주기
    private float createTime = 100.0f;
    private float createTimeWait = 5.0f;
    private float current_createTime;

    public bool isSpawn = false;
    private bool isSpawnEnd = false;
    //npc최대 발생 개수
    //public int maxMonster = 10;

    /*
    void SetReSpawnPoint()
    {
        for(int i=0;i<4;i++)
        {
            float randomX = Random.Range(thePlayer.transform.position.x - spawnRange, thePlayer.transform.position.x + spawnRange);
            float randomZ = Random.Range(thePlayer.transform.position.z - spawnRange, thePlayer.transform.position.z + spawnRange);
            Vector3 _resPos = new Vector3(randomX, 103.0f, randomZ);
            spawnPoint = _resPos;
        }
    }
    */

	// Use this for initialization
	void Start ()
    {
        thePlayer = FindObjectOfType<PlayerController>();

        //Hierarchy View의 Spawn Point를 찾아 하위에 있는 모든 Transform 컴포넌트를 찾아옴
        //spawnPoint = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        //StartCoroutine(CreateMonsterCoroutine());
    }

    private void Update()
    {
        /*
        if (EventManager.instance.eventNumber == 14 && DayAndNight.isNight)
        {
            StopAllCoroutines(); //코루틴 중복실행방지
            StartCoroutine(CreateMonsterCoroutine());
        }
        else if(EventManager.instance.eventNumber == 14 &&!DayAndNight.isNight)
        {
            EventManager.instance.eventNumber = 15;
        }
        */

        //스폰이벤트 발생조건
        if (EventManager.instance.eventNumber == 14 && DayAndNight.isNight && !isSpawn)
        {
            StopAllCoroutines(); //코루틴 중복실행방지
            StartCoroutine(CreateMonsterCoroutine());
            isSpawn = true;
        }

        //스폰이벤트 종료조건
        if (EventManager.instance.eventNumber == 14 && !DayAndNight.isNight)
        {
            EventManager.instance.eventNumber = 15;
            StopCoroutine(CreateMonsterCoroutine());
            //StartCoroutine(CreateMonsterCoroutine());
            Destroy(generateParent);
        }
    }

    public IEnumerator CreateMonsterCoroutine()
    {
        current_createTime = 0;
        while (current_createTime<createTime)
        {
            Debug.Log("몬스터 생성!");
            
            float randomX = Random.Range(thePlayer.transform.position.x - spawnRange, thePlayer.transform.position.x + spawnRange);
            float randomZ = Random.Range(thePlayer.transform.position.z - spawnRange, thePlayer.transform.position.z + spawnRange);
            Vector3 _resPos = new Vector3(randomX, 103.0f, randomZ);
            spawnPoint = _resPos;

            //몬스터의 동적 생성
            Instantiate(monsterPrefab, spawnPoint, Quaternion.identity).transform.SetParent(generateParent.transform);


            //몬스터의 생성 주기 시간만큼 대기
            yield return new WaitForSeconds(createTimeWait);
            current_createTime += createTimeWait;
        }
    }

    //치트전용 몬스터제거함수
    public void DestroyMonster()
    {
        Destroy(generateParent);
    }

}
