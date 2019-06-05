using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

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
        item.AddComponent<InteractionBehaviour>();

        if (type == ItemType.FRUIT)
        {
            item.transform.position = Vector3.zero;
            item.transform.localScale = new Vector3(1, 1, 1);  /* 손으로 집기 용이한 크기로 해야함 */
            item.transform.parent = this.transform.GetChild(3); /* 동굴 오브젝트의 자식에 있는 StoredItems 오브젝트에 넣는다. 인덱스를 사용하기 때문에 주의해야함. */
            item.transform.localPosition = new Vector3(0.55f, -0.3f, 0.83f); // 동굴안에 저장될 위치 지정

            storedFruitObjs.Add(item);

            // 저장된 아이템 개수 증가
            //storedFruitNum++;
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



    // 장비 제작, 멧돼지에 의해 뺏기는 등으로인해 아이템을 소모한 경우, 동굴에 저장된 아이템 개수를 최신화해주어야 함. 
    /* 단, 플레이어가 아이템을 들고 동굴 밖으로 나간경우에 대해 처리해줄 수 있는 방법이 없음. 일단 보류 */

    // 장비 제작으로 인한 아이템 소모 / 제거
    public void ConsumeItem(GameObject rmvItem, ItemType type)
    {
        if (type == ItemType.FRUIT)
        {
            if (storedFruitObjs.Contains(rmvItem))
            {
                storedFruitObjs.Remove(rmvItem);
                Destroy(rmvItem);
            }
            else
                Debug.Log("소모하려는 아이템이 저장된 리스트에 없는 아이템입니다.");
        }
        else if (type == ItemType.STONE)
        {
            if (storedStoneObjs.Contains(rmvItem))
            {
                storedStoneObjs.Remove(rmvItem);
                Destroy(rmvItem);
            }
            else
                Debug.Log("소모하려는 아이템이 저장된 리스트에 없는 아이템입니다.");
        }
    }

    // 저장된 아이템을 강탈당한 경우
    public GameObject RobItem(ItemType type)
    {
        if (type == ItemType.FRUIT)
        {
            if (storedFruitObjs.Count > 0)
            {
                GameObject robObj = storedFruitObjs[storedFruitObjs.Count - 1];
                storedFruitObjs.RemoveAt(storedFruitObjs.Count - 1);

                // 불필요한 컴포넌트 제거
                Destroy(robObj.GetComponent<Rigidbody>());
                Destroy(robObj.GetComponent<InteractionBehaviour>());

                return robObj;
            }
            else
                return null;
        }
        else if (type == ItemType.STONE)
        {
            if (storedStoneObjs.Count > 0)
            {
                GameObject robObj = storedStoneObjs[storedStoneObjs.Count - 1];
                storedStoneObjs.RemoveAt(storedStoneObjs.Count - 1);

                // 불필요한 컴포넌트 제거
                Destroy(robObj.GetComponent<Rigidbody>());
                Destroy(robObj.GetComponent<InteractionBehaviour>());

                return robObj;
            }
            else
                return null;
        }
        else
            return null;
    }
}
