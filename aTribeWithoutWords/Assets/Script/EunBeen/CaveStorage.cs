using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 동굴 아이템 저장 및 제작
public class CaveStorage : MonoBehaviour {

    // 싱글턴 변수
    private static CaveStorage _instance;

    public static CaveStorage Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CaveStorage>() as CaveStorage;

                if (_instance == null)
                {
                    Debug.Log("CaveStorage 스크립트가 부착된 오브젝트가 없습니다.");
                    return null;
                }
            }

            return _instance;
        }
    }

    // 동굴에 저장될 수 있는 아이템 요소
    public int fruit;
    public int stone;

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
