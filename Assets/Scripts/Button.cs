using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public GameObject owner;
    public int myNumber;

    private void OnMouseDown()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.gray;
    }

    private void OnMouseUp()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        ButtonHolder bh = owner.GetComponent<ButtonHolder>();
        bh.ButtonPressed(myNumber);
    }
}
