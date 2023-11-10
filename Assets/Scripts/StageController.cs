using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageData
{
    public List<Card> rewardCards;
    public int gold;
}

public class StageController : MonoBehaviour
{
    private static StageController instance;

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static StageController Instance
    {
        get 
        {
            if(null == instance)
            {
                return null;
            }
            return instance; 
        }
    }

    [SerializeField]
    private GameObject stageButton;
    [SerializeField]
    private GameObject stageRewardCanvas;
    [SerializeField]
    private GameObject BaseCard;
    [SerializeField]
    private GameObject SpecialCard;
    [SerializeField]
    private GameObject currentGold;
    [SerializeField]
    private GameObject whereAreCharacter;

    public GameObject[] buttons;
    public bool[] isClear;
    public bool[] isEnable;
    public int stageIndex;
    public bool isLoad = false;

    [SerializeField]
    private Sprite disableStageSprite;
    [SerializeField]
    private Sprite clearStageSprite;

    public Dictionary<int, StageData> stageData;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLoad && SceneManager.GetActiveScene().name == "StageSelect")
        {
            stageButton = GameObject.FindWithTag("StageButtons").gameObject;
            currentGold = GameObject.FindWithTag("GoldUI").gameObject;
        }

        if(stageButton != null)
        {
            buttons = new GameObject[stageButton.transform.childCount];
            isClear = new bool[buttons.Length];
            isEnable= new bool[buttons.Length]; 

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = stageButton.transform.GetChild(i).gameObject;
                isClear[i] = false;
                isEnable[i] = true;
            }

        }
        stageIndex= 0;
        stageRewardCanvas.SetActive(false);
        SetStageReward();
    }

    // Update is called once per frame
    void Update()
    {
        currentGold.transform.GetChild(1).GetComponent<Text>().text = MasterController.Instance.PlayerInfo.Gold.ToString();
    }
   

    public void stageClear(int i)
    {
        isClear[i-1] = true;
        checkStageEnable(i-1);
        Debug.Log("stage clear" + i);
        buttons[i - 1].GetComponent<StageButton>().changeSprite(clearStageSprite);
        ShowRewardCanvas(i);
        MoveCharacterPos(i);
    }

    void checkStageEnable(int i)
    {
        if(i == 1)
        {
            SetDisable(2);
            SetDisable(5);
            SetDisable(9);
            SetDisable(8);
            SetDisable(11);
            SetDisable(14);


        }
        else if(i == 2) 
        {
            SetDisable(1);
            SetDisable(3);
            SetDisable(6);
        }
        if(i == 3)
        {
            SetDisable(4);
            SetDisable(7);
        }
        else if(i== 4)
        {
            SetDisable(3);
            SetDisable(6);
            SetDisable(5);
            SetDisable(8);
            SetDisable(9);
            SetDisable(11);
            SetDisable(14);
        }

        if(i == 5)
        {
            SetDisable(4);
            SetDisable(7);
            SetDisable(10);
            SetDisable(12);
            SetDisable(13);
            SetDisable(15);
        }
        if(i == 7)
        {
            SetDisable(8);
        }
        else if(i == 8)
        {
            SetDisable(7);
        }

        if (i == 12)
        {
            SetDisable(13);
        }
        else if (i == 13)
        {
            SetDisable(12);
        }

    }


    public void SetStageReward()
    {
        stageData = new Dictionary<int, StageData>();

        StageData tempData = new StageData();
        tempData.rewardCards = new List<Card>();
        tempData.rewardCards.Add(new NumberCard { Number = 10});
        tempData.rewardCards.Add(new NumberCard { Number = 13 });
        tempData.rewardCards.Add(new NumberCard { Number = 16 });
        tempData.gold = 10;

        stageData.Add(1, tempData);

        tempData = new StageData();
        tempData.rewardCards = new List<Card>();
        tempData.rewardCards.Add(new NumberCard { Number = 20 });
        tempData.rewardCards.Add(new NumberCard { Number = 23 });
        tempData.rewardCards.Add(new NumberCard { Number = 16 });
        tempData.gold = 20;

        stageData.Add(2, tempData);

        tempData = new StageData();
        tempData.rewardCards = new List<Card>();
        tempData.rewardCards.Add(new NumberCard { Number = 30 });
        tempData.rewardCards.Add(new NumberCard { Number = 33 });
        tempData.rewardCards.Add(new NumberCard { Number = 16 });
        tempData.gold = 30;

        stageData.Add(3, tempData);

        tempData = new StageData();
        tempData.rewardCards = new List<Card>();
        tempData.rewardCards.Add(new NumberCard { Number = 40 });
        tempData.rewardCards.Add(new NumberCard { Number = 43 });
        tempData.rewardCards.Add(new NumberCard { Number = 16 });
        tempData.gold = 40;

        stageData.Add(4, tempData);

        tempData = new StageData();
        tempData.rewardCards = new List<Card>();
        tempData.rewardCards.Add(new NumberCard { Number = 50 });
        tempData.rewardCards.Add(new NumberCard { Number = 53 });
        tempData.rewardCards.Add(new NumberCard { Number = 16 });
        tempData.gold = 50;

        stageData.Add(5, tempData);

        tempData = new StageData();
        tempData.rewardCards = new List<Card>();
        tempData.rewardCards.Add(new NumberCard { Number = 60 });
        tempData.rewardCards.Add(new NumberCard { Number = 63 });
        tempData.rewardCards.Add(new NumberCard { Number = 16 });
        tempData.gold = 60;

        stageData.Add(6, tempData);

        tempData = new StageData();
        tempData.rewardCards = new List<Card>();
        tempData.rewardCards.Add(new NumberCard { Number = 70 });
        tempData.rewardCards.Add(new NumberCard { Number = 73 });
        tempData.rewardCards.Add(new NumberCard { Number = 16 });
        tempData.gold = 70;

        stageData.Add(7, tempData);

        for(int i = 1; i <= stageData.Count; i++)
        {
            Debug.Log(stageData[i].rewardCards[0]);
            Debug.Log(stageData[i].rewardCards[1]);
            Debug.Log(stageData[i].rewardCards[2]);
            Debug.Log(stageData[i].gold);
        }
    }

    void ShowRewardCanvas(int i)
    {
       /* stageRewardCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = stageData[i].rewardCards[0].ToString();
        stageRewardCanvas.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = stageData[i].rewardCards[1].ToString();
        stageRewardCanvas.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = stageData[i].rewardCards[2].ToString();*/
    }

    void SetDisable(int i)
    {
        buttons[i].GetComponent<StageButton>().changeEnableFalse();
        isEnable[i] = false;
        buttons[i].GetComponent<StageButton>().changeSprite(disableStageSprite);
    }

    void MoveCharacterPos(int i)
    {
        switch (i)
        {
            case 0:
                whereAreCharacter.transform.position = buttons[0].transform.position;
                break;
            case 1:
                whereAreCharacter.transform.position = buttons[1].transform.position;
                break;

        }
    }
}
