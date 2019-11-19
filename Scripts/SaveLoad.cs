using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEditor;
using System.IO;

[System.Serializable] //직렬화 시켜 한줄로 데이터들을 나열시킨다. (저장장치에 읽고 쓰기 쉽게)
public class SaveData
{
    public bool isFirstEventSee;
    public Vector3 playerPos;
    public string sceneName;
    public int questNumber;

    public int firstQuestCurrentAxeNumber;
    public int firstQuestAcquireAxeNumber;
    public int firstQuestCurrentPickAxeNumber;
    public int firstQuestAcquirePickAxeNumber;
    public int secondQuestCurrentMeatNumber;
    public int secondQuestAcquireMeatNumber;
    public int finalQuestCurrentStoneNumber;
    public int finalQuestAcquireStoneNumber;
    public int finalQuestCurrentFireWoodNumber;
    public int finalQuestAcquireFireWoodNumber;

    public int currentWoodNumber;
    public int currentStoneNumber;

    public bool isSecondEventPlay;
    public bool isFinalEventPlay;

    public bool isFirstGetAxe;
    public bool isFirstGetPickaxe;
    public bool isFirstGetSubMachineGun1;

    public bool isFireWoodEventClear = false; //첫번째 모닥불퀘스트 변수
    public bool isFirstNightClear = false; //첫번째 밤 퀘스트 견뎠는지 여부 변수

    public List<string> itemName = new List<string>();

    public List<int> itemType = new List<int>();
    public List<string> itemImage = new List<string>();
    public List<string> itemPrefab = new List<string>();

    public List<string> weaponType = new List<string>();
    public List<int> itemcount = new List<int>();
    /*
    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>(); 
    */
    
}

public enum ItemType
{
    Equipment,
    Used,
    Ingredient,
    ETC
}

public static class ExtensionMethods
{
    public static T ToEnum<T>(this string value)
    {
        // + 변환 오류인 경우 디폴트값 리턴. (디폴트값 : 0번째 value)
        if (!System.Enum.IsDefined(typeof(T), value))
            return default(T);

        

        return (T)System.Enum.Parse(typeof(T), value, true);
    }
}


public class SaveLoad : MonoBehaviour {

    //아이템 정보 저장
    public Item[] itemData = new Item[20];
    public Slot[] slots;

    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private PlayerController thePlayer;
    private Title theTitle;
    private EventManager theEvent;
    private Inventory theInven;
    private QuestUI theQuest;
    private ActionController theAction;
    private DayAndNight theDayAndNight;
	// Use this for initialization
	void Start () {

        
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves";

        //세이브할 경로에 폴더가 없는경우 세이브를 저장할공간폴더를 생성
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    public void SaveData()
    {
        //클릭애니메이션 작동후 다시 원래상태로 돌아가기위한 코드 (이 코드가 없을시 클릭후 hightlight애니메이션이 계속 재생되는 문제발생)
        GameObject.Find("SaveButton").GetComponent<Button>().enabled = false;
        GameObject.Find("SaveButton").GetComponent<Button>().enabled = true;
        GameObject.Find("SaveButton").GetComponent<Animator>().SetTrigger("Normal");

        //플레이어 위치정보 저장
        thePlayer = FindObjectOfType<PlayerController>();
        saveData.playerPos = thePlayer.transform.position;
        //플레이어가 마지막 플레이했던 씬정보 저장
        theTitle = FindObjectOfType<Title>();
        saveData.sceneName = SceneManager.GetActiveScene().name;

        theEvent = FindObjectOfType<EventManager>();
        saveData.isFirstEventSee = theEvent.firstEventSee;

        saveData.isFireWoodEventClear = theEvent.isFireWoodEventClear;
        saveData.isFirstNightClear = theEvent.isFirstNightClear;

        theQuest = FindObjectOfType<QuestUI>();
        saveData.questNumber = theQuest.currentQuestNumber;

        saveData.firstQuestCurrentAxeNumber = theQuest.firstQuestCurrentAxeNumber;
        saveData.firstQuestCurrentPickAxeNumber = theQuest.firstQuestCurrentPickAxeNumber;
        saveData.secondQuestCurrentMeatNumber = theQuest.secondQuestCurrentMeatNumber;
        saveData.finalQuestCurrentStoneNumber = theQuest.finalQuestCurrentStoneNumber;
        saveData.finalQuestCurrentFireWoodNumber = theQuest.finalQuestCurrentFireWoodNumber;

        saveData.currentWoodNumber = theQuest.currentWoodNumber;
        saveData.currentStoneNumber = theQuest.currentStoneNumber;

        saveData.isSecondEventPlay = theQuest.isSecondEventPlay;
        saveData.isFinalEventPlay = theQuest.isFinalEventPlay;

        theAction = FindObjectOfType<ActionController>();
        saveData.isFirstGetAxe = theAction.isFirstGetAxe;
        saveData.isFirstGetPickaxe = theAction.isFirstGetPickaxe;
        saveData.isFirstGetSubMachineGun1 = theAction.isFirstGetSubMachineGun1;


        

        //아이템 슬롯저장
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                continue;
            saveData.itemName.Add(slots[i].item.itemName);
            Debug.Log("index p76:" + i);
            Debug.Log("아이템이름 :"+saveData.itemName[i]);
            Debug.Log("아이템 :" + slots[i].item);
            switch (saveData.itemName[i])
            {
                case "Axe1":
                    itemData[i] = slots[i].item;
                    saveData.itemType.Add(0);
                    saveData.weaponType.Add("AXE");
                    saveData.itemImage.Add("UIasset/Weapons/Axe_Normal");
                    break;
                case "RawMeat":
                    itemData[i] = slots[i].item;
                    saveData.itemType.Add(1);
                    saveData.weaponType.Add("");
                    saveData.itemImage.Add("UIasset/Food/Meat_Raw");
                    break;
                case "Pickaxe1":
                    itemData[i] = slots[i].item;
                    saveData.itemType.Add(0);
                    saveData.weaponType.Add("PICKAXE");
                    saveData.itemImage.Add("UIasset/pickaxe2");
                    break;
                case "Potion Yellow":
                    itemData[i] = slots[i].item;
                    saveData.itemType.Add(1);
                    saveData.weaponType.Add("");
                    saveData.itemImage.Add("UIasset/Potions/Major_Potion_yellow");
                    break;
                case "Rock":
                    itemData[i] = slots[i].item;
                    saveData.itemType.Add(2);
                    saveData.weaponType.Add("");
                    saveData.itemImage.Add("UIasset/Ore/Ore_Coal");
                    break;
                case "SubMachineGun1":
                    itemData[i] = slots[i].item;
                    saveData.itemType.Add(0);
                    saveData.weaponType.Add("GUN");
                    saveData.itemImage.Add("UIasset/item_submachineGun");
                    break;
                case "Wood":
                    itemData[i] = slots[i].item;
                    saveData.itemType.Add(2);
                    saveData.weaponType.Add("");
                    saveData.itemImage.Add("UIasset/item_twig");
                    break;
                default:
                    break;
            }
            if (itemData[i] == null)
                Debug.Log("저장시 아이템비었음(아이템저장실패)"+i);
            saveData.itemcount.Add(slots[i].itemCount);
        }
        

        string json = JsonUtility.ToJson(saveData); //세이브데이터를 json화 시킴

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json); //json화시킨 세이브데이터를 경로위치에 물리적인 파일로 덮어씀
    
        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        SAVE_FILENAME = "/SaveFile.txt";
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves";
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson); //json화된 세이브데이터를 <SaveData>형으로 다시 변환시킴

            //플레이어 위치정보 불러오기
            thePlayer = FindObjectOfType<PlayerController>();
            thePlayer.transform.position = saveData.playerPos;

            theEvent = FindObjectOfType<EventManager>();
            theEvent.firstEventSee = saveData.isFirstEventSee;

            theEvent.isFireWoodEventClear = saveData.isFireWoodEventClear;
            theEvent.isFirstNightClear = saveData.isFirstNightClear;

            theQuest = FindObjectOfType<QuestUI>();
            theQuest.savedQuestNumber = saveData.questNumber;

            theQuest.firstQuestCurrentAxeNumber = saveData.firstQuestCurrentAxeNumber;
            theQuest.firstQuestCurrentPickAxeNumber = saveData.firstQuestCurrentPickAxeNumber;
            theQuest.secondQuestCurrentMeatNumber = saveData.secondQuestCurrentMeatNumber;
            theQuest.finalQuestCurrentStoneNumber = saveData.finalQuestCurrentStoneNumber;
            theQuest.finalQuestCurrentFireWoodNumber = saveData.finalQuestCurrentFireWoodNumber;

            theQuest.currentWoodNumber = saveData.currentWoodNumber;
            theQuest.currentStoneNumber = saveData.currentStoneNumber;

            theQuest.isSecondEventPlay = saveData.isSecondEventPlay;
            theQuest.isFinalEventPlay = saveData.isFinalEventPlay;

            theAction = FindObjectOfType<ActionController>();
            theAction.isFirstGetAxe = saveData.isFirstGetAxe;
            theAction.isFirstGetPickaxe = saveData.isFirstGetPickaxe;
            theAction.isFirstGetSubMachineGun1 = saveData.isFirstGetSubMachineGun1;


            /*
            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInven.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
            }
            */

            //아이템 슬롯 불러오기

            for (int i = 0; i < saveData.itemName.Count; i++)
            {
                switch (saveData.itemName[i])
                {
                    case "Axe1":
                        FindObjectOfType<WeaponManager>().IsChangeGetAxe();
                        break;
                    case "LawMeat":
                        break;
                    case "Pickaxe1":
                        FindObjectOfType<WeaponManager>().IsChangeGetPickAxe();
                        break;
                    case "Potion Yellow":
                        break;
                    case "Rock":
                        break;
                    case "SubMachineGun1":
                        break;
                    case "Wood":
                        break;
                    default:
                        break;
                }
                itemData[i].itemName = saveData.itemName[i];
                itemData[i].itemImage = saveData.itemImage[i];
                itemData[i].weaponType = saveData.weaponType[i];
                itemData[i].itemType = (Item.ItemType)saveData.itemType[i];

                slots[i].AddItem(itemData[i], saveData.itemcount[i]);
                if (itemData[i] == null)
                    Debug.Log("로드시 아이템비었음(아이템 불러오기 실패)");
            }
            

            Debug.Log("로드 완료");
            Debug.Log(loadJson);
        }
        else
            Debug.Log("세이브 파일이 없습니다.");
    }

    public void LoadSceneData()
    {
        SAVE_FILENAME = "/SaveFile.txt";
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves";
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson); //json화된 세이브데이터를 <SaveData>형으로 다시 변환시킴

            //플레이어가 마지막 플레이했던 씬정보 불러오기
            theTitle = FindObjectOfType<Title>();
            theTitle.currentSceneName = saveData.sceneName;

            Debug.Log("씬이름 로드 완료");
            Debug.Log(loadJson);
        }
        else
            Debug.Log("세이브 파일이 없습니다.");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
