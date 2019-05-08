using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 동굴 아이템 저장 및 제작
public class CaveStorage : MonoBehaviour {

    // 싱글턴 변수
    public static CaveStorage instance = null;

    // 동굴에 저장될 수 있는 아이템 요소
    public int fruit;
    public int stone;

    // 싱글턴 변수 초기화
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start () {
        fruit = 0;
        stone = 0;
	}

    // 아이템 저장목록 확인
    void GetItemList()
    {

    }

    // 아이템 저장

}
