using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveXTween : MonoBehaviour
{

    void Start()
    {
        //Moves buttons to the left of the screen
        LeanTween.moveX(gameObject, -100, 0);
    }


    public void onOpen()
    {
        //Puts the butonns back on screen
        LeanTween.moveX(gameObject, 100, 0.5f);
    }

    public void onClose()
    {
        LeanTween.moveX(gameObject, -100, 0.5f);
    }
}
