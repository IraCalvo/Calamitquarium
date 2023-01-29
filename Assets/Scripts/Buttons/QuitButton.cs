using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void ButtonPressed()
    {
        Application.Quit();
        Debug.Log("game quit");
    }
}
