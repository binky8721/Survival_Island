using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutSceneVideo : MonoBehaviour {

    //동영상
    private VideoPlayer cutScenePlayer;

    // Use this for initialization
    void Start () {
        cutScenePlayer = gameObject.GetComponent<VideoPlayer>();
        cutScenePlayer.Play();
        StartCoroutine(VideoEndCheck());
    }

    public bool isCutScenePlaying = false;

    IEnumerator VideoEndCheck()
    {
        yield return new WaitForSeconds(1.0f);
        while (cutScenePlayer.isPlaying)
        {
                isCutScenePlaying = true;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Title.instance.ClickStart();
                }
            yield return null;
        }

        isCutScenePlaying = false;
        Title.instance.ClickStart();
    }
    
}
