using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {

    float MIN_DRAG = -1.83f;
    float MAX_DRAG = 1.20f;
    int selection = 4;
    public SkillSetting settingScreen;

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        selection = Mathf.FloorToInt(8.0f*(curPosition.x-MIN_DRAG)/(MAX_DRAG-MIN_DRAG));
        if (selection < 0)
        {
            selection = 0;
        }
        else if (selection > 8)
        {
            selection = 8;
        }
        SetSliderStep(selection);
        settingScreen.SetSkillLevel(selection);
    }

    public void SetSliderStep(int s)
    {
        transform.position = new Vector3(MIN_DRAG + s * (MAX_DRAG - MIN_DRAG) / 8.0f, transform.position.y, -0.2f);
    }
}
