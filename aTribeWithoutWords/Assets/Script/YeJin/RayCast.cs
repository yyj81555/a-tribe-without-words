using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class RayCast : MonoBehaviour
{
	//raycast
    [SerializeField] 
	private float rayLength; //rayCast 길이
    public string hitname = null; //검출된 타켓의 이름

	Variable variable; //아이템 변수 스크립트 (feat.가방)
    NPCMove npcmove; //npc 스크립트

    public GameObject prefab; //느낌표 프리팹
	public GameObject Itemlist; //아이템 변수들을 가지고 있는 오브젝트(feat. 가방)

    // Start is called before the first frame update
    void Start()
    {
        variable = Itemlist.GetComponent<Variable>();
    }

    // Update is called once per frame
    void Update()
    {
		//마우스 왼쪽 버튼 클릭
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                Debug.Log(hit.collider.name);
                hitname = hit.collider.name;

				//검출된 타겟이 Worker 일때
				if (hit.transform.gameObject.tag == "Worker") {
					//본 타겟이 리스트에 없고, 최대 선택인원수 보다 적으면 effect를 생성하고 리스트에 추가한다.
					if (CheckList () && variable.selectnpc_count < variable.Choose_NPCCount) {
						GameObject obj = Instantiate (prefab, new Vector3 (hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + 0.3f, hit.collider.gameObject.transform.position.z), Quaternion.identity) as GameObject;
						obj.name = hitname + "Effect";
						obj.transform.parent = hit.collider.gameObject.transform;

						variable.selectnpc.Add (hit.transform.gameObject);
						variable.selectnpc_count++;

						npcmove = hit.transform.gameObject.GetComponent<NPCMove> ();
						npcmove.npcstate = NPCMove.NPCState.SELECT_NPC; //선택된 npc의 상태를 select npc로 변경한다.
					}
				} 

				//검출된 타겟이 FruitFarm 일때
				else if (hit.transform.gameObject.tag == "FruitFarm") {
					//선택된 모든 npc에게 명령을 지정함
					for (int i = 0; i < variable.selectnpc_count; i++) {
						npcmove = variable.selectnpc [i].GetComponent<NPCMove> ();

						//현재 사용자를 따라오는 상태라면 FRUIT_RICKING, COMMAND_STATE 상태로 변경한다. 
						if (npcmove.npcstate == NPCMove.NPCState.FOLLOW_PLAYER) {
							npcmove.commandstate = NPCMove.CommandState.FRUIT_PICKING;
							npcmove.npcstate = NPCMove.NPCState.COMMAND_STATE;

							npcmove.target = hit.transform.gameObject; //npc가 추적하는 target을 넘겨준다.
						}
					}
				} 

				//검출된 타겟이 Mine 일때
				else if (hit.transform.gameObject.tag == "Mine") {
					//돌 채집 인원이 많으면 초기로 돌림
					if (variable.selectnpc_count >= variable.Stone_NPCCount) {
						Debug.Log ("수행인원이 너무 많습니다.");

						int imsi_count = variable.selectnpc_count;

						for (int i = 0; i < imsi_count; i++) {
							npcmove = variable.selectnpc [0].GetComponent<NPCMove> ();
							npcmove.StandardMode ();
						}
					} 

					//아니라면 STONE_PICKING, COMMAND_STATE 상태로 변경하고 타겟을 넘겨준다.
					else {
						for (int i = 0; i < variable.selectnpc_count; i++) {
							npcmove = variable.selectnpc [i].GetComponent<NPCMove> ();

							if (npcmove.npcstate == NPCMove.NPCState.FOLLOW_PLAYER) {
								npcmove.commandstate = NPCMove.CommandState.STONE_PICKING;
								npcmove.npcstate = NPCMove.NPCState.COMMAND_STATE;

								npcmove.target = hit.transform.gameObject;
							}
						}
					}
				}

				//검출된 타겟이 Samll_Animal 일때
				else if (hit.transform.gameObject.tag == "Small_Animal") 
				{
					//수행인원과 돌개수를 확인하여 적절하지 않다면 초기로 돌린다.
					if (variable.selectnpc_count >= variable.Hit_Small_Animal_NPCCount || variable.Stone < variable.Hit_Small_Animal_Stone) {
						Debug.Log ("수행인원이 너무 많거나 돌이 부족합니다.");

						int imsi_count = variable.selectnpc_count;

						for (int i = 0; i < imsi_count; i++) {
							npcmove = variable.selectnpc [0].GetComponent<NPCMove> ();
							npcmove.StandardMode ();
						}
					} 

					//아니라면 HIT_SMALL_ANIMAL, COMMAND_STATE 상태로 변경하고 타겟을 넘겨준다.
					else {
						for (int i = 0; i < variable.selectnpc_count; i++) {
							npcmove = variable.selectnpc [i].GetComponent<NPCMove> ();

							if (npcmove.npcstate == NPCMove.NPCState.FOLLOW_PLAYER) {
								npcmove.commandstate = NPCMove.CommandState.HIT_SMALL_ANIMALL;
								npcmove.npcstate = NPCMove.NPCState.COMMAND_STATE;

								npcmove.target = hit.transform.gameObject;
							}
						}
					}
				}
            } 
        }
    }

	//리스트에 검출된 타겟이 존재하는가 확인
    bool CheckList()
    {
        for(int i = 0; i < variable.selectnpc_count; i++)
        {
            if (hitname == variable.selectnpc[i].name)
            {
                return false;
            }
        }

        return true;
    } //있으면 false 없으면 true
}
