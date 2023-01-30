using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    [SerializeField] GameObject infoBox;

    public void ButtonPressed()
    {
        infoBox.SetActive(true);
    }
}
