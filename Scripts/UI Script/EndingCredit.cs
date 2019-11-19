using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingCredit : MonoBehaviour {

    [SerializeField]
    private GameObject endingText;

    private RectTransform textPos;

    private void Start()
    {
        textPos = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update () {
        if(textPos.localPosition.y<2801.0f)
            textPos.localPosition += new Vector3(0, 1, 0);

        if (Input.GetMouseButton(0))
        {
            textPos.localPosition += new Vector3(0, 5, 0);
        }

        //if(Input.GetKeyUp(KeyCode.E))
        //{
            //Title.instance.ClickTitle();
            //Application.Quit();
        //}
    }

    public void GoToTitle()
    {
        Title.instance.ClickTitle();
    }
}
