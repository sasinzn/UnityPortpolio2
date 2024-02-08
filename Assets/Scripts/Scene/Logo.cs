using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public void OnClickSingleGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickMultiGame()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
