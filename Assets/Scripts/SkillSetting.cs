using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSetting : MonoBehaviour, ButtonHolder {

    public ViewHolder owner;
    public Slider theSlider;

    int skillLevel;

    public Sprite skill1;
    public Sprite skill2;
    public Sprite skill3;
    public Sprite skill4;
    public Sprite skill5;
    public Sprite skill6;
    public Sprite skill7;
    public Sprite skill8;
    public Sprite skill9;

    Sprite[] skillPics;

    int offlineSkill = 4;
    int onlineSkill = 7;

    public GameObject sample;

    bool online = false;

    public void Start()
    {
        skillPics = new Sprite[] { skill1,skill2,skill3, skill4,skill5, skill6,skill7, skill8,skill9 };
    }

    public void SetUpForOffline()
    {
        online = false;
        SetSkillLevel(offlineSkill);
        theSlider.SetSliderStep(skillLevel);
        transform.gameObject.SetActive(true);
    }

    public void SetUpForOnline()
    {
        online = true;
        SetSkillLevel(onlineSkill);
        theSlider.SetSliderStep(skillLevel);
        transform.gameObject.SetActive(true);
    }

    public void ButtonPressed(int b)
    {
        if (b == 1) // Back button
        {
            if (online)
            {
                onlineSkill = skillLevel;
            }
            else
            {
                offlineSkill = skillLevel;
            }
            owner.BackToMainMenuFromSetting();
        }
        else if (b == 0) // Start button
        {
            if (online)
            {
                owner.HostGameSession();
            }
            else
            {
                owner.ShowSingleplayerScreenWithSkill(skillLevel);
            }
        }
    }

    public void SetSkillLevel(int s)
    {
        skillLevel = s;
        SpriteRenderer sr = sample.GetComponent<SpriteRenderer>();
        sr.sprite = skillPics[s];
    }

    public int GetSkillLevel()
    {
        return skillLevel;
    }
}
