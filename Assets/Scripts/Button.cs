using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public GameObject owner;
    public int myNumber;

    private void OnMouseDown()
    {
        ButtonHolder bh = owner.GetComponent<ButtonHolder>();
        if (bh.IsResponsive())
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = Color.gray;
        }
    }

    private void OnMouseUp()
    {
        ButtonHolder bh = owner.GetComponent<ButtonHolder>();
        if (bh.IsResponsive())
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = Color.white;
            bh.ButtonPressed(myNumber);
        }
    }
}
