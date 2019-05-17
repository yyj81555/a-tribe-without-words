using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour {

    // 열매 오브젝트 관리 (열매 개수는 fruits.Count로 확인)
    public List<GameObject> fruits;

    // 열매 위치
    private Vector3[] fruitLocalPos = { new Vector3(0.142f, 0.137f, -0.106f), new Vector3(-0.123f, 0.145f, -0.094f), new Vector3(0.0966222f, 0.1311166f, 0.152495f),
                                        new Vector3(-0.013f, 0.145f, 0.122f), new Vector3(0.179f, 0.096f, 0f)};


    private void Start()
    {
        fruits = new List<GameObject>();
    }

    private Vector3 GetFruitPos(int count)
    {
        return fruitLocalPos[count];
    }

    // 열매를 나무에 생성
    public void BearFruit()
    {
        GameObject newObj = MapItemGenerator.Instance.CreateFruit(this.transform.position, this.gameObject);
        newObj.transform.localPosition = GetFruitPos(fruits.Count);
        fruits.Add(newObj);
    }
    
    // 열매를 딴다.  (일꾼이 열매를 딸 때 사용하는 함수)
    public GameObject GetFruit()
    {
        GameObject getObj = fruits[-1];
        fruits.RemoveAt(-1);

        return getObj;
    }
}
