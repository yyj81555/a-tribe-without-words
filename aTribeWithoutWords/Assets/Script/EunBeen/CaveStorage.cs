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
    public enum ItemType
    {
        FRUIT,
        STONE
    }

    // 저장된 아이템 리스트
    public List<GameObject> storedFruitObjs;
    public List<GameObject> storedStoneObjs;

    // 게임 아이템에 대한 스크립트를 작성할 필요가 있을까? 돌, 과일, 갈대 등

    void Start () {

	}

    /*
    // 아이템 저장목록 확인
    void GetItemList()
    {

    }*/

    // 아이템 저장
    public void StoreItem(GameObject item, ItemType type)
    {
        if (item == null)
        {
            Debug.Log("저장될 아이템이 null 입니다.");
            return;
        }

        // 손으로 집을 수 있도록 처리한다.
        item.AddComponent<Rigidbody>();

        if (type == ItemType.FRUIT)
        {
            item.transform.position = Vector3.zero;
            item.transform.localScale = new Vector3(1, 1, 1);  /* 손으로 집기 용이한 크기로 해야함 */
            item.transform.parent = this.transform.GetChild(3); /* 동굴 오브젝트의 자식에 있는 StoredItems 오브젝트에 넣는다. 인덱스를 사용하기 때문에 주의해야함. */
            item.transform.localPosition = new Vector3(0.55f, -0.3f, 0.83f); // 동굴안에 저장될 위치 지정

            storedFruitObjs.Add(item);
        }
        else if (type == ItemType.STONE)
        {
            item.transform.position = Vector3.zero;
            item.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            item.transform.parent = this.transform.GetChild(3);
            item.transform.localPosition = new Vector3(-0.37f, -0.3f, 0.83f);

            storedStoneObjs.Add(item);
        }
    }
}
