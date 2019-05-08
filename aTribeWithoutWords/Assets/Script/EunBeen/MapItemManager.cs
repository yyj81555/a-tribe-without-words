using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵에 아이템 생성 및 관리
public class MapItemManager : MonoBehaviour {

    public static MapItemManager instance = null;

    // 게임 오브젝트
    [SerializeField] GameObject fruitObj;
    [SerializeField] GameObject StoneObj;

    // 게임 오브젝트 생성 주기
    [SerializeField] float createFruitSycle;
    [SerializeField] float createStoneSycle;

    /* 이거는 관리할 필요가 없는것같다. 
     * 아이템매니저에서는 지정된 생성 위치에 아이템을 생성하기만 하면 되는 역할이라 아이템 리스트를 건들일이 없는듯.
       아이템을 채집하는 경우 딕셔너리를 통해 지목한 생성위치에 아이템이 존재한다면 채집할수있도록 한다.
    // 맵에 만들어진 아이템 오브젝트 리스트
    public List<GameObject> list_fruits;
    public List<GameObject> list_stones;*/

    // 지정된 생성 위치 (생성위치에서 아이템이 랜덤하게 만들어짐)
    public Transform[] fruitTransform; // 열매나무 위치
    public Transform[] stoneTransform;

    // 생성 위치에 아이템이 존재하는지 파악하기 위한 데이터 사전 변수
    public Dictionary<Transform, GameObject> itemDict; // Key : Transform 이 사전에 존재한다면 해당 위치에는 아이템이 이미 만들어진 것.

    // 실제 생성된 아이템 개수
    public int fruitCount;
    public int stoneCount;

    // 맵에 생성될 수 있는 최대 아이템 개수
    [SerializeField] const int fruitMax = 5;
    [SerializeField] const int stoneMax = 5;

    private float fruitTime;
    private float stoneTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void Start () {
        createFruitSycle = 2f;
        createStoneSycle = 2f;
        fruitCount = 0;
        stoneCount = 0;
    }
	
	void Update () {
        fruitTime += Time.deltaTime;
        stoneTime += Time.deltaTime;

        if (fruitTime > createFruitSycle)
        {
            if (fruitCount < fruitMax)
            {
                // 랜덤한 위치에 생성하되 이미 자리에 있다면 다른 위치에 생성
                int rand = Random.Range(0, fruitTransform.Length - 1);
                if (!(itemDict.ContainsKey(fruitTransform[rand])))
                {
                    itemDict.Add(fruitTransform[rand], CreateFruit(fruitTransform[rand]));
                    fruitCount++;
                }
                fruitTime = 0f;
            }
        }

        if (stoneTime > createStoneSycle)
        {
            if (stoneCount < stoneMax)
            {
                // 랜덤한 위치에 생성하되 이미 자리에 있다면 다른 위치에 생성
                int rand = Random.Range(0, stoneTransform.Length - 1);
                if (!(itemDict.ContainsKey(stoneTransform[rand])))
                {
                    // 아이템 생성 후 데이터 사전 추가
                    itemDict.Add(stoneTransform[rand], CreateStone(stoneTransform[rand]));
                    stoneCount++;
                }
                stoneTime = 0f;
            }
        }
	}

    /*아이템 생성/ 제거는 아래 함수 사용하기*/

    // 특정 위치에 아이템 생성.
    public GameObject CreateFruit(Transform createPos)
    {
        return Instantiate(fruitObj, createPos);
    }

    public GameObject CreateStone(Transform createPos)
    {
        return Instantiate(StoneObj, createPos);
    }
}
