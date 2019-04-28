using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class PointHand : MonoBehaviour {

    public GameObject[] finger = new GameObject[5];
    public GameObject palm;
    public GameObject indexfinger;
    public Vector3[] fingerps = new Vector3[5];
    public Vector3 palmps;
    public Vector3 indexfingerps;
    [SerializeField]
    private float rayLength;

    public string hitname = null;

    Variable variable;
    NPCMove npcmove;

    public GameObject prefab;
    public GameObject Itemlist;

    public int ChooseCount = 2;

    // Use this for initialization
    void Start () {
        variable = Itemlist.GetComponent<Variable>();
    }
	/* 
     */
	// Update is called once per frame
	void Update () {
        fingerps[0] = finger[0].transform.eulerAngles;
        fingerps[1] = finger[1].transform.eulerAngles;
        fingerps[2] = finger[2].transform.eulerAngles;
        fingerps[3] = finger[3].transform.eulerAngles;
        fingerps[4] = finger[4].transform.eulerAngles;
        palmps = palm.transform.eulerAngles;
        indexfingerps = indexfinger.transform.position;

        if (fingerps[0].y < 40 && fingerps[1].x >300 && fingerps[2].x < 150
            && fingerps[3].x < 150 && fingerps[4].x < 150)
        {
            Debug.Log("지목");

            Ray ray = new Ray();

            ray.origin = indexfingerps;
            ray.direction = indexfinger.transform.forward;

            //ray = Camera.main.ScreenPointToRay(indexfingerps);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength))
            {
               // Debug.Log(hit.collider.name);
                hitname = hit.collider.name;

                //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 5f);

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

                else if (hit.transform.gameObject.tag == "FruitFarm")
                {
                    for (int i = 0; i < variable.selectnpc_count; i++)
                    {
                        npcmove = variable.selectnpc[i].GetComponent<NPCMove>();

                        if (npcmove.npcstate == NPCMove.NPCState.FOLLOW_PLAYER)
                        {
                            npcmove.commandstate = NPCMove.CommandState.FRUIT_PICKING;
                            npcmove.npcstate = NPCMove.NPCState.COMMAND_STATE;

							npcmove.target = hit.transform.gameObject;
                        }
                    }
                }

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
                    else
                    {
                        for (int i = 0; i < variable.selectnpc_count; i++)
                        {
                            npcmove = variable.selectnpc[i].GetComponent<NPCMove>();

                            if (npcmove.npcstate == NPCMove.NPCState.FOLLOW_PLAYER)
                            {
                                npcmove.commandstate = NPCMove.CommandState.STONE_PICKING;
                                npcmove.npcstate = NPCMove.NPCState.COMMAND_STATE;

								npcmove.target = hit.transform.gameObject;
                            }
                        }
                    }
                }
            }
        }
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
