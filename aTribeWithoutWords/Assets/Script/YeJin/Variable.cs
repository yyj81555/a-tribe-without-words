using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 소지목록
public class Variable : MonoBehaviour
{
    //선택된 부족원 리스트와 길이
    public List<GameObject> selectnpc;
    public int selectnpc_count = 0;

    //게임 오브젝트들
    public int Fruit = 0;
    public int Stone = 0;


    //최대 수집양
    public int MAX_Fruit = 5;
    public int MAX_Stone = 10;
	public int HIT_Time = 3;

    //명령 1회 수행에 걸리는 시간
    public float Fruit_Picking_Time = 2.0f;
	public float Stone_Picking_Time = 4.0f;
	public float Idiot_Time = 5.0f;

	//명령수행 최대 인원 수
	public int Choose_NPCCount = 2;
	public int Stone_NPCCount = 2;
	public int Hit_Small_Animal_NPCCount = 2;

	//명령수행 최소 아이템 개수
	public int Hit_Small_Animal_Stone = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
