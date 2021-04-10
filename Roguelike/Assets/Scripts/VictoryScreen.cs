using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public float waitForEnyKey = 2f;

    public GameObject anyKeyText;

    public string mainMenuScene;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        Destroy(PlayerController.instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(waitForEnyKey > 0)
        {
            waitForEnyKey -= Time.deltaTime;
            if(waitForEnyKey <= 0)
            {
                anyKeyText.SetActive(true);
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
