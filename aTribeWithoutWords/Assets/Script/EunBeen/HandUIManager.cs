using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUIManager : MonoBehaviour {

    [Header("Attached Object")]
    public GameObject MenuPanel;

    [Header("Buttons Setting")]
    public GameObject AchievementText;
    public GameObject InventoryText;
    public GameObject OptionText;

    private void Start()
    {
        MenuPanel.SetActive(false);

        AchievementText.SetActive(false);
        InventoryText.SetActive(false);
        OptionText.SetActive(false);
    }

    #region Menu Open Toggle의 콜백함수
    public void OnPressedToggle()
    {
        MenuPanel.SetActive(true);
        Debug.Log("Pressed!");
    }

    public void OnUnPressedToggle()
    {
        MenuPanel.SetActive(false);
        Debug.Log("UnPressed!");
    }
    #endregion


    #region Menu Pannel이 갖고있는 버튼들에 대한 콜백함수

    public void OnAchievementButton()
    {
        SetAchievement(true);
        SetInventory(false);
        SetOption(false);
    }

    public void OnInventoryButton()
    {
        SetAchievement(false);
        SetInventory(true);
        SetOption(false);
    }

    public void OnOptionButton()
    {
        SetAchievement(false);
        SetInventory(false);
        SetOption(true);
    }

    private void SetAchievement(bool isPressed)
    {
        if (isPressed == true)
        {
            AchievementText.SetActive(true);
        }
        else
        {
            AchievementText.SetActive(false);
        }
    }

    private void SetInventory(bool isPressed)
    {
        if (isPressed == true)
        {
            InventoryText.SetActive(true);
        }
        else
        {
            InventoryText.SetActive(false);
        }
    }

    private void SetOption(bool isPressed)
    {
        if (isPressed == true)
        {
            OptionText.SetActive(true);
        }
        else
        {
            OptionText.SetActive(false);
        }
    }
    #endregion
}
