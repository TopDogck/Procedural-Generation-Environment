using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public Button button;
    public string nextScene;

    public void ClickButton()
    {
        SceneManager.LoadScene(nextScene);
    }
}
