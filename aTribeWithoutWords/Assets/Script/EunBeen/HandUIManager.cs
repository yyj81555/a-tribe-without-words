using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUIManager : MonoBehaviour {

    [Header("Attached Object")]
    public GameObject MenuPanel;

    [Header("Buttons Setting")]
    public GameObject AchievementTitle;
    public GameObject InventoryTitle;
    public GameObject OptionTitle;

    private void Start()
    {
        MenuPanel.SetActive(false);

        AchievementTitle.SetActive(false);
        InventoryTitle.SetActive(false);
        OptionTitle.SetActive(false);
    }

    // Menu Open Toggle의 콜백함수
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


    // Menu Pannel이 갖고있는 버튼들에 대한 콜백함수
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

    // 업적 메뉴 세팅
    private void SetAchievement(bool isPressed)
    {
        if (isPressed == true)
        {
            AchievementTitle.SetActive(true);
        }
        else
        {
            AchievementTitle.SetActive(false);
        }
    }

    // 인벤토리 메뉴 세팅
    private void SetInventory(bool isPressed)
    {
        if (isPressed == true)
        {
            InventoryTitle.SetActive(true);
        }
        else
        {
            InventoryTitle.SetActive(false);
        }
    }

    // 옵션 메뉴 세팅
    private void SetOption(bool isPressed)
    {
        if (isPressed == true)
        {
            OptionTitle.SetActive(true);
        }
        else
        {
            OptionTitle.SetActive(false);
        }
    }
}
