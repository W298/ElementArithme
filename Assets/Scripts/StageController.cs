using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] buttons;
    public bool[] isClear;
    public bool[] isEnable;
    public int stageIndex;

    [SerializeField]
    private Sprite disableStageSprite;
    [SerializeField]
    private Sprite clearStageSprite;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void stageClear(int i)
    {
        isClear[i-1] = true;
        checkStageEnable(i-1);
        Debug.Log("stage clear" + i);
        buttons[i - 1].GetComponent<StageButton>().changeSprite(clearStageSprite);
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

    void SetDisable(int i)
    {
        buttons[i].GetComponent<StageButton>().changeEnableFalse();
        isEnable[i] = false;
        buttons[i].GetComponent<StageButton>().changeSprite(disableStageSprite);
    }
}
