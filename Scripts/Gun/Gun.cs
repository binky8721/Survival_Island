using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public string gunName; // 총의 이름.
    public float range; // 사정 거리
    public float accuracy; // 정확도 (낮을수록 정확도가 좋다) 0.1만되도 불량품수준으로 안좋다.
    public float fireRate; // 연사속도.
    public float reloadTime; // 재장전 속도.

    public int damage; // 총의 데미지.

    public int reloadBulletCount; // 총알 재정전 개수.
    public int currentBulletCount; // 현재 탄알집에 남아있는 총알의 개수.
    public int maxBulletCount; // 최대 소유 가능 총알 개수.
    public int carryBulletCount; // 현재 소유하고 있는 총알 개수.

    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; // 정조준시의 반동 세기.

    public Vector3 fineSightOriginPos; //정조준시 정해진 위치
    public Animator anim;
    public ParticleSystem muzzleFlash; //총알발사시 총구섬광효과

    public AudioClip fire_Sound; //총발사시 사운드
}
