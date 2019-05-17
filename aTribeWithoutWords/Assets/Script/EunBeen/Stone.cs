using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour {

    // 돌을 캘 수 있는가?
    [SerializeField] bool isStoneExist;

    // 빛나는 쉐이더 변수
    // 

	void Start () {
        isStoneExist = false;
    }

    // 돌을 캘수있다면 반짝반짝 빛나게한다.
    private void BrightStone()
    {
        if (isStoneExist)
        {
            // 쉐이더 온
        }
        else
        {
            // 쉐이더 오프
        }
    }

    public bool IsStoneExist()
    {
        return isStoneExist;
    }

    // 채석 가능하도록 설정
    public void SetStoneGettable()
    {
        isStoneExist = true;
        BrightStone();
    }

    // 채석한다.  (일꾼이 채석할때 사용하는 함수)
    public GameObject GetStone()
    {
        GameObject getObj = MapItemGenerator.Instance.CreateStone(Vector3.zero);
        isStoneExist = false;

        return getObj;
    }
}
