using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

	Variable variable; //아이템 변수 스크립트 (feat.가방)
	NPCMove npcmove; //npc 스크립트

	public GameObject Itemlist; //아이템 변수들을 가지고 있는 오브젝트(feat. 가방)

	// Use this for initialization
	void Start () {
		variable = Itemlist.GetComponent<Variable>();
	}

	//줍기버튼 함수
	public void OnClickPickUp()
	{
		
		for(int i = 0; i < variable.selectnpc_count; i++)
		{
			Debug.Log (variable.selectnpc [i].name);
			npcmove = variable.selectnpc[i].GetComponent<NPCMove>();
			npcmove.commandstate = NPCMove.CommandState.FRUIT_PICKING;
		}
	}

	public void OnClickStonePickUp()
	{

		for(int i = 0; i < variable.selectnpc_count; i++)
		{
			Debug.Log (variable.selectnpc [i].name);
			npcmove = variable.selectnpc[i].GetComponent<NPCMove>();
			npcmove.commandstate = NPCMove.CommandState.STONE_PICKING;
		}
	}

	public void OnClickHitAnimall()
	{

		for(int i = 0; i < variable.selectnpc_count; i++)
		{
			Debug.Log (variable.selectnpc [i].name);
			npcmove = variable.selectnpc[i].GetComponent<NPCMove>();
			npcmove.commandstate = NPCMove.CommandState.HIT_SMALL_ANIMALL;
		}
	}
}
