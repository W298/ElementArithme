using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] Camera camera;
    public Text text;
    float timer = 0f;
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(camera.transform.position.y < 20)
        {
            camera.transform.Translate(new Vector3(0, (float)28 / 5 * Time.deltaTime));
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (float)1/2 * Time.deltaTime);
        }

        timer += Time.deltaTime;
        if (timer > 7f)
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
