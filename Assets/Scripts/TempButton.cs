using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TempButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void temp()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void temp2()
    {
        SceneManager.LoadScene("temp");
    }
}
