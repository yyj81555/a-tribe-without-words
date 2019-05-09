using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 돌을 동적으로 생성하면 NavMesh에서 문제생길 가능성이 높음 */
// 맵에 아이템 생성 및 관리
public class MapItemManager : MonoBehaviour {

    public static MapItemManager instance = null;

    // 게임 오브젝트
    [SerializeField] public GameObject fruitObj;
    //[SerializeField] public GameObject stoneObj;

    // 게임 오브젝트 생성 주기
    [SerializeField] float createFruitSycle;
    //[SerializeField] float createStoneSycle;

    // 아이템 생성 위치 지정 (생성위치에서 아이템이 랜덤하게 만들어짐)
    public List<FruitTree> fruitFarm;  // 열매나무
    //public Transform[] fruitTransform; // 열매 위치
    //public Vector3[] stonePos; // 돌 생성 위치

    // 생성 위치에 아이템이 존재하는지 파악하기 위한 데이터 사전 변수
    //public Dictionary<Vector3, GameObject> itemDict; // Key : Vector3 이 사전에 존재한다면 해당 위치에는 아이템이 이미 만들어진 것.

    // 실제 생성된 아이템 개수
    //public int stoneCount;

    // 맵에 생성될 수 있는 최대 아이템 개수
    public int fruitMax = 5;  // 한 나무당 과일 개수
    public int stoneMax = 5;  // 맵에 있는 총 바위 개수

    private float fruitTime = 0;
    private float stoneTime = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // 아이템 변수 초기화
    void InitItem() {
        createFruitSycle = 2f;
        createStoneSycle = 2f;
        //fruitCount = 0;
        stoneCount = 0;

        // 맵에 존재하는 FruitFarm 태그가 붙은 오브젝트를 찾아 FruitTree 스크립트를 추가한다.
        GameObject[] farms = GameObject.FindGameObjectsWithTag("FruitFarm");
        for (int i = 0; i < farms.Length; i++) {
            fruitFarm.Add(farms[i].AddComponent<FruitTree>());
            fruitFarm[i].InitFruitTree();
        }
    }

    void Start () {
        InitItem();
    }
	
	void Update () {
        fruitTime += Time.deltaTime;
        stoneTime += Time.deltaTime;

        if (fruitTime > createFruitSycle)
        {

            // 랜덤한 나무 위치에 열매를 생성하되 해당 나무가 열매가 가득 찼다면 생성하지 않음
            int rand = Random.Range(0, fruitFarm.Count - 1);
            if (fruitFarm[rand].myFruitNum < fruitMax)
            {
                CreateFruit(fruitFarm[rand].GetFruitPos());
                fruitFarm[rand].myFruitNum++;
            }
            fruitTime = 0f;
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

    // 특정 위치에 아이템 생성
    public GameObject CreateFruit(Vector3 createPos)
    {
        return Instantiate(fruitObj, createPos, fruitObj.transform.rotation);
    }

    public GameObject CreateStone(Vector3 createPos)
    {
        return Instantiate(stoneObj, createPos, stoneObj.transform.rotation);
    }
}
