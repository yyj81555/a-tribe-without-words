using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class PointHand : MonoBehaviour {
    
    public GameObject[] fingerL = new GameObject[5];
    public GameObject[] fingerR = new GameObject[5];
    public GameObject palmL;
    public GameObject palmR;
    public GameObject indexfingerL;
    public GameObject indexfingerR;
    public Vector3[] fingerpsL = new Vector3[5];
    public Vector3[] fingerpsR = new Vector3[5];
    public Vector3 palmpsL;
    public Vector3 palmpsR;
    public Vector3 indexfingerpsL;
    public Vector3 indexfingerpsR;
    public GameObject Player;

    private int speed=5;

    [SerializeField]
    private float rayLength;

    public string hitname = null;

    Variable variable;
    NPCMove npcmove;

    public GameObject prefab;
    public GameObject Itemlist;

    public int ChooseCount = 2;

    public bool[] foldfinger = new bool[10];
    public bool[] palmupdown = new bool[2];

    // Use this for initialization
    void Start () {
        variable = Itemlist.GetComponent<Variable>();
    }
	/* 
     */
	// Update is called once per frame
	void Update () {
        fingerpsL[0] = fingerL[0].transform.eulerAngles;
        fingerpsL[1] = fingerL[1].transform.eulerAngles;
        fingerpsL[2] = fingerL[2].transform.eulerAngles;
        fingerpsL[3] = fingerL[3].transform.eulerAngles;
        fingerpsL[4] = fingerL[4].transform.eulerAngles;
        fingerpsR[0] = fingerR[0].transform.eulerAngles;
        fingerpsR[1] = fingerR[1].transform.eulerAngles;
        fingerpsR[2] = fingerR[2].transform.eulerAngles;
        fingerpsR[3] = fingerR[3].transform.eulerAngles;
        fingerpsR[4] = fingerR[4].transform.eulerAngles;
        palmpsL = palmL.transform.eulerAngles;
        palmpsR = palmR.transform.eulerAngles;
        indexfingerpsL = indexfingerL.transform.position;
        indexfingerpsR = indexfingerR.transform.position;

        CheckFinger();
        CheckPalm();

        //지목 모션
        if (CheckLandmarkMotion()== true)
        {
            Debug.Log("지목");

            Ray ray = new Ray();

            ray.origin = indexfingerpsL;
            ray.direction = indexfingerL.transform.forward;

            //ray = Camera.main.ScreenPointToRay(indexfingerps);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength))
            {
               // Debug.Log(hit.collider.name);
                hitname = hit.collider.name;

                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 5f);

                if (hit.transform.gameObject.tag == "Worker")
                {
                    if (CheckList() && variable.selectnpc_count < ChooseCount)
                    {
                        GameObject obj = Instantiate(prefab, new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + 2, hit.collider.gameObject.transform.position.z), Quaternion.identity);
                        obj.name = hitname + "Effect";

                        variable.selectnpc.Add(hit.transform.gameObject);
                        variable.selectnpc_count++;

                        npcmove = hit.transform.gameObject.GetComponent<NPCMove>();
                        npcmove.npcstate = NPCMove.NPCState.SELECT_NPC;
                    }
                }

                //else if (hit.transform.gameObject.tag == "FruitFarm")
                //{
                //    for (int i = 0; i < variable.selectnpc_count; i++)
                //    {
                //        npcmove = variable.selectnpc[i].GetComponent<NPCMove>();

                //        if (npcmove.npcstate == NPCMove.NPCState.FOLLOW_PLAYER)
                //        {
                //            npcmove.commandstate = NPCMove.CommandState.FRUIT_PICKING;
                //            npcmove.npcstate = NPCMove.NPCState.COMMAND_SRATE;

                //            npcmove.target = hitname;
                //        }
                //    }
                //}

                else if (hit.transform.gameObject.tag == "Mine")
                {
                    if (variable.selectnpc_count >= 2)
                    {
                        Debug.Log("수행인원이 너무 많습니다.");

                        int imsi_count = variable.selectnpc_count;

                        for (int i = 0; i < imsi_count; i++)
                        {
                            npcmove = variable.selectnpc[0].GetComponent<NPCMove>();
                            npcmove.StandardMode();
                        }
                    }
                    //else
                    //{
                    //    for (int i = 0; i < variable.selectnpc_count; i++)
                    //    {
                    //        npcmove = variable.selectnpc[i].GetComponent<NPCMove>();

                    //        if (npcmove.npcstate == NPCMove.NPCState.FOLLOW_PLAYER)
                    //        {
                    //            npcmove.commandstate = NPCMove.CommandState.STONE_PICKING;
                    //            npcmove.npcstate = NPCMove.NPCState.COMMAND_SRATE;

                    //            npcmove.target = hitname;
                    //        }
                    //    }
                    //}
                }
            }
        }

        if (CheckStoneMotion() == true)
        {
            Debug.Log("돌줍기");
        }

        if (CheckFruitMotion() == true)
        {
            Debug.Log("열매따기");
        }
        if (CheckGoMotion() == true)
        {
            Debug.Log("Go!");
            float fMove = Time.deltaTime * speed;
            Player.transform.Translate(Vector3.forward * fMove);
        }
    }

    void CheckFinger()
    {
        //true = 핀거, false = 구부린것
        if (fingerpsL[0].x > 330)
            foldfinger[0] = false;
        else if (fingerpsL[0].x <= 330)
            foldfinger[0] = true;

        if (fingerpsL[1].x > 280)
            foldfinger[1] = true;
        else if (fingerpsL[1].x <= 280)
            foldfinger[1] = false;

        if (fingerpsL[2].x > 280)
            foldfinger[2] = true;
        else if (fingerpsL[2].x <= 280)
            foldfinger[2] = false;

        if (fingerpsL[3].x > 280)
            foldfinger[3] = true;
        else if (fingerpsL[3].x <= 280)
            foldfinger[3] = false;

        if (fingerpsL[4].x > 60)
            foldfinger[4] = true;
        else if (fingerpsL[4].x <= 60)
            foldfinger[4] = false;

        if (fingerpsR[0].x > 330)
            foldfinger[5] = false;
        else if (fingerpsR[0].x <= 330)
            foldfinger[5] = true;

        if (fingerpsR[1].x > 300)
            foldfinger[6] = true;
        else if (fingerpsR[1].x <= 280)
            foldfinger[6] = false;

        if (fingerpsR[2].x > 280)
            foldfinger[7] = true;
        else if (fingerpsR[2].x <= 300)
            foldfinger[7] = false;

        if (fingerpsR[3].x > 300)
            foldfinger[8] = true;
        else if (fingerpsR[3].x <= 300)
            foldfinger[8] = false;

        if (fingerpsR[4].x > 60)
            foldfinger[9] = true;
        else if (fingerpsR[4].x <= 60)
            foldfinger[9] = false;
    }

    void CheckPalm()
    { //true = 손등, false = 손바닥
        if (palmpsL.z > 200)
            palmupdown[0] = true;
        else if(palmpsL.z <= 200)
            palmupdown[0] = false;
        if (palmpsR.z > 200)
            palmupdown[1] = true;
        else if (palmpsR.z <= 200)
            palmupdown[1] = false;
    }

    bool CheckLandmarkMotion()//지목 모션
    {

        if (foldfinger[0] == false && foldfinger[1] == true && foldfinger[2] == false
            && foldfinger[3] == false && foldfinger[4] == false && palmupdown[0] == true)
        {
            return true;
        }
        if (foldfinger[5] == false && foldfinger[6] == true && foldfinger[7] == false
            && foldfinger[8] == false && foldfinger[9] == false && palmupdown[1] == true)
        {
            return true;
        }
        return false;
    }

    bool CheckStoneMotion()//돌줍기 모션
    {
        if (foldfinger[0] == false && foldfinger[1] == false && foldfinger[2] == false
            && foldfinger[3] == false && foldfinger[4] == false && palmupdown[0] == true)
            return true;
        return false;
    }

    bool CheckFruitMotion()//과일 채집 모션
    {
        if (foldfinger[5] == false && foldfinger[6] == false && foldfinger[7] == false
            && foldfinger[8] == false && foldfinger[9] == false && palmupdown[1] == false)
            return true;
        return false;
    }

    bool CheckHuntMotion()//사냥 모션
    {
        return true;
    }

    bool CheckUIMotion()//UI 모션
    {
        return true;
    }

    bool CheckGoMotion()//Go 모션
    {
        if (foldfinger[0] == false && foldfinger[1] == true && foldfinger[2] == true
            && foldfinger[3] == true && foldfinger[4] == true && palmupdown[0] == true)
            return true;
        return false;
    }

    bool CheckList()
    {
        for (int i = 0; i < variable.selectnpc_count; i++)
        {
            if (hitname == variable.selectnpc[i].name)
            {
                return false;
            }
        }

        return true;
    } //있으면 false 없으면 true
}
