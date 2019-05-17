using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 돌을 동적으로 생성하면 NavMesh에서 문제생길 가능성이 높음. */
// 맵에 아이템 생성 및 관리
public class MapItemGenerator : MonoBehaviour {

    // 싱글턴 변수
    private static MapItemGenerator _instance;

    public static MapItemGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapItemGenerator>() as MapItemGenerator;

                if (_instance == null)
                {
                    Debug.Log("MapItemGenerator 스크립트가 부착된 오브젝트가 없습니다.");
                    return null;
                }
            }

            return _instance;
        }
    }

    // 생성 오브젝트
    [SerializeField] GameObject fruitObj;
    [SerializeField] GameObject stoneObj;

    // 게임 오브젝트 생성 주기
    [SerializeField] float createFruitSycle;
    [SerializeField] float createStoneSycle;
    private float fruitTime = 0;
    private float stoneTime = 0;

    // 아이템 생성 위치
    public List<FruitTree> fruitFarms;  // 열매나무
    public List<Stone> stones; // 돌 생성 위치

    // 맵에 생성될 수 있는 최대 아이템 개수
    private const int fruitMax = 5;  // 한 나무당 과일 개수

    void Start () {
        InitObjScript();
    }

    // 아이템 오브젝트 스크립트 초기화
    void InitObjScript()
    {
        // 맵에 존재하는 FruitFarm 태그 오브젝트들을 찾아 FruitTree 스크립트 추가
        GameObject[] farmObjs = GameObject.FindGameObjectsWithTag("FruitFarm");
        for (int i = 0; i < farmObjs.Length; i++)
        {
            fruitFarms.Add(farmObjs[i].AddComponent<FruitTree>());
        }

        // 맵에 존재하는 Mine 태그 오브젝트들을 찾아 Stone 스크립트 추가
        GameObject[] stoneObjs = GameObject.FindGameObjectsWithTag("Mine");
        for (int i = 0; i < stoneObjs.Length; i++)
        {
            stones.Add(stoneObjs[i].AddComponent<Stone>());
        }
    }

    void Update () {
        fruitTime += Time.deltaTime;
        stoneTime += Time.deltaTime;

        if (fruitTime > createFruitSycle)
        {
            // 랜덤한 나무에 열매를 생성하되, 해당 나무가 열매가 가득 찼다면 생성하지 않음
            int rand = Random.Range(0, fruitFarms.Count);
            if (fruitFarms[rand].fruits.Count < fruitMax)
            {
                fruitFarms[rand].BearFruit();
                fruitTime = 0f;
            }
        }
        
        if (stoneTime > createStoneSycle)
        {
            // 랜덤한 돌을 캘 수 있는 상태로 만든다.
            int rand = Random.Range(0, stones.Count);
            if (!stones[rand].IsStoneExist())
            {
                stones[rand].SetStoneGettable();
                stoneTime = 0f;
                Debug.Log(stones[rand].gameObject.name + " 이용가능");
            }
        }
	}

    // 특정 위치에 아이템 생성
    public GameObject CreateFruit(Vector3 worldPos, GameObject parent = null)
    {
        GameObject newObj;

        if (parent == null)
        {
            newObj = Instantiate(fruitObj);
            newObj.transform.position = worldPos;
        }
        else
        {
            newObj = Instantiate(fruitObj, parent.transform);
            newObj.transform.position = worldPos;
        }
        return newObj;
    }

    public GameObject CreateStone(Vector3 worldPos, GameObject parent = null)
    {
        GameObject newObj;

        if (parent == null)
        {
            newObj = Instantiate(stoneObj);
            newObj.transform.position = worldPos;
        }
        else
        {
            newObj = Instantiate(stoneObj, parent.transform);
            newObj.transform.position = worldPos;
        }
        return newObj;
    }
}
