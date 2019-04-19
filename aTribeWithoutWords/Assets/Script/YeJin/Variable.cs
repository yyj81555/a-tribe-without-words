using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 소지목록
public class Variable : MonoBehaviour
{
    public int EffectCount = 0;
    public int[] chooseNPC = new int[10];

    //게임 오브젝트들
    public int Fruit = 0;
    public int Stone = 0;


    //최대 수집양
    public int MAX_Fruit = 5;
    public int MAX_Stone = 10;

    //명령 1회 수행에 걸리는 시간
    public float Fruit_Picking_Time = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EffectCount = 0;
        for(int i = 1; i <= 3; i++)
        {
            if(GameObject.Find("Worker" + i + "Effect") != null)
            {
                EffectCount++;
                chooseNPC[i] = 1;
            }
            else
            {
                chooseNPC[i] = 0;
            }
        }
    }


}
