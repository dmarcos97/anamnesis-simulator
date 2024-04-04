using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    public void OnResetClicked()
    {
        SceneManager.LoadScene("Simulator");
    }

    public void OnCloseClicked()
    {
        Application.Quit();
    }

}
