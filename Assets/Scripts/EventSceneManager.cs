using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EventSceneManager : MonoBehaviour
{
    public GameObject correct;
    public GameObject timerBar;
    public GameObject problemPanel;
    public Image timerImage;
    public Image background;
    public Text problemText;
    public Text correctText;
    public Text[] answerText = new Text[4];
    int correctIndex;
    int correctCount;
    int phase;
    int counter;
    float timer;
    bool isClicked;
    bool stun;
    public GameObject[] list1 = new GameObject[6];
    public GameObject[] list2 = new GameObject[6];

    public GameObject popUP;
    public Text popUPText;
    // Start is called before the first frame update
    void Start()
    {
        correctCount = 0;
        phase = 0;
        counter = 0;
        timer = 0f;
        correctIndex = 0;
        isClicked = false;
        stun = false;
    }

    void MakeProblem()
    {
        int first = Random.Range(1,25);
        int op = Random.Range(0, 3);
        string opResult = "";
        switch (op)
        {
            case 0:
                opResult = "+";
                break;
            case 1:
                opResult = "-";
                break;
            case 2:
                opResult= "*";
                break;
        }
        int second= Random.Range(1,25);
        if(op == 1 && first < second)
        {
            int tmp = first;
            first = second;
            second = tmp;
        }
        string problems = first.ToString() + " " + opResult + " " + second.ToString();
        problemText.text = problems.ToString();
        ExpressionEvaluator.Evaluate(problems, out int result);
        int r = Random.Range(0, 4);
        correctIndex = r;
        for(int i=0; i<4; i++)
        {
            answerText[i].text = Random.Range(1, 100).ToString();
            if(i == r)
            {
                answerText[i].text = result.ToString();
            }
        }
    }

    public void ClickButton(int index)
    {
        if(stun == false)
        {
            isClicked = true;
            if (index == correctIndex)
            {
                correctCount++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        correctText.text = correctCount.ToString();
        switch (phase)
        {
            case 0:
                if (timer > 1f)
                {
                    phase++;
                    background.gameObject.SetActive(true);
                    timer = 0f;
                }
                break;
            case 1:
                if(timer > 0.25f)
                {
                    list1[counter++].gameObject.SetActive(true);
                    if(counter >= 6)
                    {
                        counter = 0;
                        phase++;
                        for(int i=0; i<6; i++)
                        {
                            list1[i].gameObject.SetActive(false);
                        }
                    }
                    timer = 0f;
                }
                break;
            case 2:
                if (timer > 0.25f)
                {
                    list2[counter++].gameObject.SetActive(true);
                    if (counter >= 6)
                    {
                        counter = 0;
                        phase++;
                        for (int i = 0; i < 6; i++)
                        {
                            list2[i].gameObject.SetActive(false);
                        }
                        correct.SetActive(true);
                        problemText.gameObject.SetActive(true);
                        timerBar.SetActive(true);
                        problemPanel.SetActive(true);
                        MakeProblem();
                        stun = true;
                    }
                    timer = 0f;
                }
                break;
            case 3:
                timerImage.fillAmount = (1 - (timer /3));
                if(timer > 0.2f)
                {
                    stun = false;
                }
                if(timer > 3f)
                {
                    timerImage.fillAmount = 1f;
                    timer = 0f;
                    phase++;
                    MakeProblem();
                }
                if (isClicked)
                {
                    timerImage.fillAmount = 1f;
                    isClicked = false;
                    timer = 0f;
                    phase++;
                    MakeProblem();
                }
                break;
            case 4:
                timerImage.fillAmount = (1 - (timer / 3));
                if (timer > 0.2f)
                {
                    stun = false;
                }
                if (timer > 3f)
                {
                    timerImage.fillAmount = 1f;
                    timer = 0f;
                    phase++;
                    MakeProblem();
                }
                if (isClicked)
                {
                    timerImage.fillAmount = 1f;
                    isClicked = false;
                    timer = 0f;
                    phase++;
                    MakeProblem();
                }
                break;
            case 5:
                timerImage.fillAmount = (1 - (timer / 3));
                if (timer > 0.2f)
                {
                    stun = false;
                }
                if (timer > 3f)
                {
                    timerImage.fillAmount = 1f;
                    timer = 0f;
                    phase++;
                }
                if (isClicked)
                {
                    timerImage.fillAmount = 1f;
                    isClicked = false;
                    timer = 0f;
                    phase++;
                }
                break;
            case 6:
                correct.SetActive(false);
                problemText.gameObject.SetActive(false);
                timerBar.SetActive(false);
                problemPanel.SetActive(false);
                background.gameObject.SetActive(false);
                popUP.SetActive(true);
                popUPText.text = "정답 개수 : " + correctCount.ToString();
                break;
        }
    }

    public void StageSelect()
    {
        SceneManager.LoadScene("StageSelect");
        StageController.Instance.eventCount = correctCount;
        StageController.Instance.isClear = true;
    }
}
