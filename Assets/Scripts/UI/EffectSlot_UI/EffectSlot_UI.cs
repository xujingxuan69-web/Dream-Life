using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSlot_UI : MonoBehaviour
{
    public Image effectImage;
    public Text effectDescription;

    public void HideSlotImage()
    {
        effectImage.enabled = false;
    }
    public void ShowSlotImage()
    {
        effectImage.enabled = true;
    }
}
