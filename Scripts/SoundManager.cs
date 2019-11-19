using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] //클래스 직렬화.
public class Sound
{
    public string name; //곡의 이름
    public AudioClip clip; //곡
}

public class SoundManager : MonoBehaviour {

    //싱글턴으로 사용
    static public SoundManager instance;
    
    #region singleton
    void Awake() //객체 생성시 최초 실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //다른씬으로 넘어갈시 파괴 방지
        }
        else //씬이 넘어갔다가 다시온경우 파괴하여 1개로 유지 (1개만 존재해야함)
            Destroy(this.gameObject);
    }
    #endregion

    public AudioSource[] audioSourceEffects; //이펙트(효과음) 재생 관리 플레이어
    public AudioSource[] audioSourceBgms; //배경음 재생 관리 플레이어
    

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    public Slider seSlider;
    //public Slider beSlider;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
        PlayBGM("MainBGM");


        //if (seSlider == null)
        //    return;

        //seSlider.onValueChanged.AddListener((value) => { OnVolumeChangedSE(seSlider.value); });

    }

    void Update()
    {
        if (seSlider == null)
            return;

        seSlider.onValueChanged.AddListener((value) => { OnVolumeChangedSE(seSlider.value); });
    }

    void OnVolumeChangedSE(float volume)
    {
        //for (int i = 0; i < audioSourceEffects.Length; i++)
        //    audioSourceEffects[i].volume = volume;

        AudioListener.volume = volume;
    }
    

    //효과음 재생
    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(!audioSourceEffects[j].isPlaying) //재생중이지 않은플레이어 찾기
                    {
                        playSoundName[j] = effectSounds[i].name; //재생중인 효과음 이름 기록
                        audioSourceEffects[j].clip = effectSounds[i].clip; //그 플레이어에 사운드 클립장착
                        audioSourceEffects[j].Play(); //재생
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다.");
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    //모든효과음 재생 취소
    public void StopAllSE() 
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    //재생중인 특정 효과음 재생 취소
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다.");
    }

    //배경음 재생(작성하다맘)
    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                for (int j = 0; j < bgmSounds.Length; j++)
                {
                    if (!audioSourceBgms[j].isPlaying) //재생중이지 않은플레이어 찾기
                    {
                        playSoundName[j] = bgmSounds[i].name; //재생중인 효과음 이름 기록
                        audioSourceBgms[j].clip = bgmSounds[i].clip; //그 플레이어에 사운드 클립장착
                        audioSourceBgms[j].Play(); //재생
                        audioSourceBgms[j].loop = true;
                        audioSourceBgms[j].volume = 0.3f;
                        //Debug.Log("배경음 재생성ㄱ공");
                        return;
                    }
                }
                return;
            }
        }
    }

    //재생중인 특정 배경음 재생 취소
    public void StopBGM(string _name)
    {
        for (int i = 0; i < audioSourceBgms.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceBgms[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다.");
    }
    
    //재생중인 특정 효과음 재생 취소
    public void StopAllBGM()
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            audioSourceBgms[i].Stop();
        }
    }






}
