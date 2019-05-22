using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    // 돌을 캘 수 있는가?
    [SerializeField] bool isStoneExist;

    // 매터리얼을 위한 변수
    public float speed = 1.0f;
    Color startColor;
    Color endColor;
    float startTime;

	void Start () {
        isStoneExist = false;
        startColor = GetComponent<Renderer>().material.color;
        endColor = new Color(0.7f, 0.7f, 0);
        speed = 1f;
    }

    private void Update()
    {
        BrightMine();
    }

    // 돌을 캘수있다면 반짝반짝 빛나게한다. /* 현재는 노랑색으로만 바꾸었음.. */
    private void BrightMine()
    {
        if (isStoneExist)
        {
            float t = Mathf.Sin(Time.time - startTime) * speed;
            GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
        }
        else
        {
            GetComponent<Renderer>().material.color = startColor;
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
        //BrightMine();
        startTime = Time.time;
    }

    // 채석한다.  (일꾼이 채석할때 사용하는 함수)
    public GameObject GetStone()
    {
        if(!isStoneExist)
        {
            Debug.Log("채석할 돌이 없습니다.");
            return null;
        }

        GameObject getObj = MapItemGenerator.Instance.CreateStone(Vector3.zero);
        isStoneExist = false;

        return getObj;
    }
}
