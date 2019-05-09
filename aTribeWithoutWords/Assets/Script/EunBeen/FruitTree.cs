using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour {

    public int myFruitNum; // 현재 나무가 갖고있는 열매 개수
    // 열매 위치
    public Vector3[] fruitLocalPos = { new Vector3(0.0966222f, 0.1311166f, 0.152495f), new Vector3(-0.013f, 0.145f, 0.122f),
            new Vector3(-0.123f, 0.145f, -0.094f), new Vector3(0.142f, 0.137f, -0.106f), new Vector3(0.179f, 0.096f, 0f)}; 
    private int index = 0;

    // 열매 초기화
    public void InitFruitTree()
    {
        myFruitNum = 0;
        index = 0;
    }

    public Vector3 GetFruitPos()
    {
        if (index >= fruitLocalPos.Length)
            index = 0;
        return fruitLocalPos[index++];
    }

    /*
    // 열매를 딴다.
    public void GetFruitItem()
    {
        myFruitNum--;
    }*/
}
