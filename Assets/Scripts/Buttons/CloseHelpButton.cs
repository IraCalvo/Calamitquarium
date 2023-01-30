using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseHelpButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject infoBox;

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked();
    }

    private void isClicked()
    {
        infoBox.SetActive(false);
    }
}
