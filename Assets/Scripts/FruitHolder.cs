using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitHolder : MonoBehaviour {

    public GameObject peopleSprite;
    public TextMesh peopleDigit;
    public GameObject circle;

    public Sprite persons0;
    public Sprite persons1;
    public Sprite persons2;
    public Sprite persons3;

    Sprite[] personArray;

    public void Start()
    {
       
    }

    public void Clear()
    {
        SpriteRenderer sr = peopleSprite.GetComponent<SpriteRenderer>();
        sr.sprite = persons0;
        circle.SetActive(false);
        peopleDigit.text = "";
    }

    public void JoinTeam(bool joined)
    {
        circle.SetActive(joined);
    }

    public void UpdateTeamSize(int t)
    {
        if (personArray == null)
            personArray = new Sprite[] { persons0, persons1, persons2, persons3 };
        SpriteRenderer sr = peopleSprite.GetComponent<SpriteRenderer>();
        sr.sprite = personArray[t];
        if (t == 0)
        {
            peopleDigit.text = "";
        }
        else
        {
            peopleDigit.text = "" + t;
        }
    }
}
