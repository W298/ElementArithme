using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum StageType { NormalBattle, DangerBattle, Store, BossBattle, Bonus, Mission }; 

public class StageButton : MonoBehaviour
{
    [SerializeField]
    private StageType stageType;
    public int stageIndex;
    public bool isEnable;

    // Start is called before the first frame update
    void Start()
    {
        isEnable= true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToStage()
    {
        switch (stageType)
        {
            case StageType.NormalBattle:
                //SceneManager.LoadScene("");
                //MasterController.Instance.currentStageIndex = stageIndex;
                StageController.Instance.stageClear(stageIndex);
                break;
             case StageType.DangerBattle:
                //SceneManager.LoadScene("");
                break;
            case StageType.Store:
                //SceneManager.LoadScene("");
                break;
            case StageType.BossBattle:
                //SceneManager.LoadScene("");
                break;
            case StageType.Bonus:
                //SceneManager.LoadScene("");
                break;
             case StageType.Mission:
                //SceneManager.LoadScene("");
                break;
            default:
                break;
        }
    }

    public void changeEnableFalse()
    {
        isEnable= false;
    }

    public void changeSprite(Sprite sprite)
    {
        this.GetComponent<Image>().sprite = sprite;
    }
}
