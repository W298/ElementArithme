using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
    private GameObject stageRewardCanvasPrefab;
    [SerializeField]
    private GameObject eventRewardCanvasPrefab;
    [SerializeField]
    private GameObject BaseCard;
    [SerializeField]
    private GameObject SpecialCard;
    [SerializeField]
    private GameObject currentGold;
    [SerializeField]
    private GameObject whereAreCharacter;

    public GameObject[] buttons;
    public bool[] stageClearArr;
    public bool[] isEnable;
    public int stageIndex;
    public int eventCount;
    public bool isLoad = false;
    public GameObject stageRewardCanvas;
    public GameObject eventRewardCanvas;
    public GameObject[] rewardCards;
    public bool isClear = false;

    [SerializeField]
    private Sprite disableStageSprite;
    [SerializeField]
    private Sprite clearStageSprite;

    public Dictionary<int, StageData> stageData;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "StageSelect")
        {
            stageButton = GameObject.FindWithTag("StageButtons").gameObject;
            currentGold = GameObject.FindWithTag("GoldUI").gameObject;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = stageButton.transform.GetChild(i).gameObject;
            }
            rewardCards = new GameObject[3];
            UpdateStageSprite();

            if (!isLoad)
            {
                buttons = new GameObject[stageButton.transform.childCount];
                stageClearArr = new bool[buttons.Length];
                isEnable = new bool[buttons.Length];

                if (stageButton != null)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i] = stageButton.transform.GetChild(i).gameObject;
                        stageClearArr[i] = false;
                        isEnable[i] = true;
                    }
                }

                stageIndex = 0;
                SetReward();
                isLoad = true;
            }
        }
        if(isClear)
        {
            stageClear(stageIndex);
            isClear = false;
        }

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  
    }

    // Update is called once per frame
    void Update()
    {
        if(currentGold != null)
        {
            currentGold.transform.GetChild(1).GetComponent<Text>().text = MasterController.Instance.PlayerInfo.Gold.ToString();

        }
    }
   

    public void stageClear(int i)
    {
        stageClearArr[i-1] = true;
        checkStageEnable(i-1);
        Debug.Log("stage clear" + i);
        UpdateStageSprite();
        ShowRewardCanvas(i);
        MoveCharacterPos(i);
        if(eventCount != 0)
        {
            GetCorrentCount(eventCount);
            eventCount = 0;
        }
        MasterController.Instance.PlayerInfo.Gold += stageData[i].gold;
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


    public void SetReward()
    {
        stageData = new Dictionary<int, StageData>();

        for(int k = 0; k <= buttons.Length; k++)
        {
            SetStageReward(k, (int)Random.Range(5, k * 10));
        }



       /* for (int i = 1; i <= stageData.Count; i++)
        {
            Debug.Log(stageData[i].rewardCards[0]);
            Debug.Log(stageData[i].rewardCards[1]);
            Debug.Log(stageData[i].rewardCards[2]);
            Debug.Log(stageData[i].gold);
        }*/
    }

    void SetStageReward(int stagenum, int gold)
    {
        StageData tempData = new StageData();
        tempData.rewardCards = new List<Card>();
        tempData.rewardCards.Add(new NumberCard { Number = (int)Random.Range(10, 100) });
        tempData.rewardCards.Add(new NumberCard { Number = (int)Random.Range(10, 100) });

        switch((int)Random.Range(0, 5))
        {
            case 0:
                tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Multiply });
                break;
            case 1:
                tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Add });
                break;
            case 2:
                tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Subtract });
                break;
            case 3:
                tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Divide });
                break;
            case 4:
                tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Sin });
                break;
            /* case 5:
                 tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Cos });
                 break;
             case 6:
                 tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Tan });
                 break;*/
            default:
                break;
        }
        
        tempData.gold = gold;

        stageData.Add(stagenum, tempData);
    }

    void ShowRewardCanvas(int i)
    {
        stageRewardCanvas = Instantiate(stageRewardCanvasPrefab, new Vector2(0, 0), Quaternion.identity);
        for(int j = 0; j < 3; j++)
        {
            rewardCards[j] = Instantiate(BaseCard, new Vector2(100, 100), Quaternion.identity);
            rewardCards[j].GetComponent<BaseCardObject>().Init(stageData[i].rewardCards[j]);
            rewardCards[j].transform.parent = stageRewardCanvas.transform;
            rewardCards[j].transform.localScale = new Vector3(4, 4);
            rewardCards[j].transform.localPosition = new Vector3(-600 + 600 * j, 0);
            rewardCards[j].GetComponent<CardObject>().CardClickEvent.AddListener(SelectRewardCard);
        }
       /* stageRewardCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = stageData[i].rewardCards[0].ToString();
        stageRewardCanvas.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = stageData[i].rewardCards[1].ToString();
        stageRewardCanvas.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = stageData[i].rewardCards[2].ToString();*/
    }

    void SetDisable(int i)
    {
        if(buttons != null)
        {
            buttons[i].GetComponent<StageButton>().changeEnableFalse();
            isEnable[i] = false;
            buttons[i].GetComponent<StageButton>().changeSprite(disableStageSprite);
        }
    }

    void UpdateStageSprite()
    {
        for (int j = 0; j < buttons.Length; j++)
        {
            if (stageClearArr[j]) buttons[j].GetComponent<StageButton>().changeSprite(clearStageSprite);
            if (!isEnable[j]) buttons[j].GetComponent<StageButton>().changeSprite(disableStageSprite);
        }
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

    void SelectRewardCard(Card card)
    {
        MasterController.Instance.AddCard(card);
        Destroy(stageRewardCanvas);
    }

    public void SetStageIndex(int i)
    {
        stageIndex = i;
        MasterController.Instance.currentStageIndex= i;
    }

    public void GetCorrentCount(int i)
    {
        eventRewardCanvas = Instantiate(eventRewardCanvasPrefab, new Vector2(0, 0), Quaternion.identity);

        for (int j = 0; j < Mathf.Min(i, 2); j++)
        {
            NumberCard tempNumCard = new NumberCard { Number = (int)Random.Range(1, 45) };
            rewardCards[j] = Instantiate(BaseCard, new Vector2(100, 100), Quaternion.identity);
            rewardCards[j].GetComponent<BaseCardObject>().Init(tempNumCard);
            rewardCards[j].transform.parent = eventRewardCanvas.transform;
            rewardCards[j].transform.localScale = new Vector3(4, 4);
            rewardCards[j].transform.localPosition = new Vector3(-600 + 600 * j, 0);
        }

        if (i == 3)
        {
            OperatorCard tempOpCard = new OperatorCard();

            switch ((int)Random.Range(0, 5))
            {
                case 0:
                    tempOpCard.Type = OperatorType.Multiply;
                    break;
                case 1:
                    tempOpCard.Type = OperatorType.Add;
                    break;
                case 2:
                    tempOpCard.Type = OperatorType.Subtract;
                    break;
                case 3:
                    tempOpCard.Type = OperatorType.Divide;
                    break;
                case 4:
                    tempOpCard.Type = OperatorType.Sin;
                    break;
                /* case 5:
                     tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Cos });
                     break;
                 case 6:
                     tempData.rewardCards.Add(new OperatorCard { Type = OperatorType.Tan });
                     break;*/
                default:
                    break;
            }

            rewardCards[2] = Instantiate(BaseCard, new Vector2(100, 100), Quaternion.identity);
            rewardCards[2].GetComponent<BaseCardObject>().Init(tempOpCard);
            rewardCards[2].transform.parent = eventRewardCanvas.transform;
            rewardCards[2].transform.localScale = new Vector3(4, 4);
            rewardCards[2].transform.localPosition = new Vector3(600, 0);

        }


        for (int j = 0; j < i; j++)
        {
            MasterController.Instance.AddCard(rewardCards[j].GetComponent<CardObject>().card);
        }
    }

}
