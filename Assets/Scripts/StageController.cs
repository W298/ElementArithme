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
    public int stageIndex;

    // Start is called before the first frame update
    void Start()
    {
        if(stageButton != null)
        {
            buttons = new GameObject[stageButton.transform.childCount];
            isClear = new bool[buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = stageButton.transform.GetChild(i).gameObject;
                isClear[i] = false;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void stageClear(int i)
    {
        isClear[i] = true;
        
    }

    void checkStageEnable()
    {
        if (isClear[1])
        {
            buttons[2].GetComponent<StageButton>().changeEnableFalse();

            if (isClear[3] || isClear[4])
            {
                buttons[5].GetComponent<StageButton>().changeEnableFalse();
                buttons[7].GetComponent<StageButton>().changeEnableFalse();
                buttons[8].GetComponent<StageButton>().changeEnableFalse();
                buttons[11].GetComponent<StageButton>().changeEnableFalse();
                buttons[14].GetComponent<StageButton>().changeEnableFalse();


                if (isClear[3])
                {
                    buttons[4].GetComponent<StageButton>().changeEnableFalse();
                    buttons[7].GetComponent<StageButton>().changeEnableFalse();
                }
                else if (isClear[4])
                {
                    buttons[3].GetComponent<StageButton>().changeEnableFalse();
                    buttons[6].GetComponent<StageButton>().changeEnableFalse();
                }

                if (isClear[12])
                {
                    buttons[13].GetComponent<StageButton>().changeEnableFalse();
                }
                else if (isClear[13])
                {
                    buttons[12].GetComponent<StageButton>().changeEnableFalse();
                }

            }
        }
        else if (isClear[2])
        {
            buttons[1].GetComponent<StageButton>().changeEnableFalse();

            if (isClear[5])
            {
                buttons[3].GetComponent<StageButton>().changeEnableFalse();
                buttons[4].GetComponent<StageButton>().changeEnableFalse();
                buttons[6].GetComponent<StageButton>().changeEnableFalse();
                buttons[7].GetComponent<StageButton>().changeEnableFalse();
                buttons[10].GetComponent<StageButton>().changeEnableFalse();
                buttons[12].GetComponent<StageButton>().changeEnableFalse();
                buttons[13].GetComponent<StageButton>().changeEnableFalse();
                buttons[15].GetComponent<StageButton>().changeEnableFalse();

                if (isClear[7])
                {
                    buttons[8].GetComponent<StageButton>().changeEnableFalse();
                }
                else if (isClear[8])
                {
                    buttons[7].GetComponent<StageButton>().changeEnableFalse();
                }

            }
        }

        
        


       

    }
}
