using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    public void OnClose()
    {
        //Scales down the menu
        LeanTween.scale(gameObject, new Vector3 (0, 0, 0), 0.5f);
    }

    public void OnOpen()
    {
        //Scales the menu back to its original size
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);
    }
}
