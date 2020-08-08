using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwap : MonoBehaviour
{
    private Button _button;

    public int scene;

    private void Start()
    {
        _button = gameObject.GetComponent<Button>();

        if(_button == null)
        {
            Debug.LogWarning("Scriptul nu e pe buton.");
        }

        _button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        AudioManager.instance.Stop("Intro");
        SceneManager.LoadScene(sceneBuildIndex: scene);
    }
   
}
