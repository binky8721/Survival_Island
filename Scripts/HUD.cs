using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    // 필요하면 HUD호출, 필요 없을시 HUD 비활성화
    [SerializeField]
    public GameObject goBulletHUD;

    // 텍스트에 총알 개수 반영
    [SerializeField]
    private Text[] textBullets; //0:Carry 1:Reload 3:Current

    void Update ()
    {
        CheckBullet();
	}

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        textBullets[0].text = currentGun.carryBulletCount.ToString();
        textBullets[1].text = currentGun.reloadBulletCount.ToString();
        textBullets[2].text = currentGun.currentBulletCount.ToString();
    }
}
