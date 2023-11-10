using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
    [SerializeField]
    private GameObject baseCard;
    [SerializeField]
    private GameObject specialCard;
    [SerializeField]
    private GameObject canvas;

    public GameObject[] cards;
    public GameObject costText;
    public int[] costs;

    // Start is called before the first frame update
    void Start()
    {
        cards = new GameObject[3];   
        costs= new int[3];
        for(int i = 0; i < 3; i++)
        {
            costs[i] = (int)Random.Range(10, 50);
            costText.transform.GetChild(i).gameObject.GetComponent<Text>().text = costs[i].ToString();
        }
        SpawnCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCards()
    {
        
        for(int i = 0; i < 2; i++)
        {
            NumberCard tempNumCard = new NumberCard { Number = (int)Random.Range(1, 45) };
            cards[i] = Instantiate(baseCard, new Vector2(100, 100), Quaternion.identity);
            cards[i].GetComponent<BaseCardObject>().Init(tempNumCard);
            cards[i].transform.parent = canvas.transform;
            cards[i].transform.localScale = new Vector3(4, 4);
            cards[i].transform.localPosition = new Vector3(-600 + 600 * i, 0);
            cards[i].GetComponent<CardObject>().CardClickEvent.AddListener(SelectCard);
        }

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

        cards[2] = Instantiate(baseCard, new Vector2(100, 100), Quaternion.identity);
        cards[2].GetComponent<BaseCardObject>().Init(tempOpCard);
        cards[2].transform.parent = canvas.transform;
        cards[2].transform.localScale = new Vector3(4, 4);
        cards[2].transform.localPosition = new Vector3(600, 0);
        cards[2].GetComponent<CardObject>().CardClickEvent.AddListener(SelectCard);

    }
    void SelectCard(Card card)
    {
        MasterController.Instance.AddCard(card);
        MasterController.Instance.PlayerInfo.Gold -= costs[(int)Random.Range(0, 3)];
        SceneManager.LoadScene("StageSelect");
        StageController.Instance.isClear= true;
    }
}
