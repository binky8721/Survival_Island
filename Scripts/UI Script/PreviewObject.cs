using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour {

    //충돌한 오브젝트의 컬라이더
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]
    private int layerGround; //지상레이어
    private const int IGNORE_RAYCAST_LAYER = 2;
    private const int IGNORE_WATER_LAYER = 4;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       //ChangerColor();

    }

    private void ChangerColor()
    {
        if (colliderList.Count > 0) //충돌하는게 하나라도 있으면 빨간색
            SetColor(red);
        else
            SetColor(green);
    }

    private void SetColor(Material mat)
    {
        foreach (Transform tf_child in this.transform) //이스크립트가 붙어있는 자기자신안에있는 자식객체들의 트랜스폼을 가져와서 반복문을 돌려 색깔을 변환
        {
            var newMaterials = new Material[tf_child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_child.GetComponent<Renderer>().materials = newMaterials;

            var mynewMaterials = new Material[this.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < mynewMaterials.Length; i++)
            {
                mynewMaterials[i] = mat;
            }

            this.GetComponent<Renderer>().materials = mynewMaterials;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER && other.gameObject.layer != IGNORE_WATER_LAYER)
        {
            colliderList.Add(other);
        }
            //SetColor(red);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER && other.gameObject.layer != IGNORE_WATER_LAYER)
            colliderList.Remove(other);
            //SetColor(green);

    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}
