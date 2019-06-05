using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NPCMove : MonoBehaviour
{
    public List<Transform> wayPoint;
    public int nextidx = 0;
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject Player;
	[SerializeField]
	private GameObject Cave;

    //RayCast raycast;
    float Target_Distance = 2f; //채집 타겟과의 거리
	float Small_Animal_Distance = 4f; //작은동물 사냥 가능거리
    float Player_Distance = 2.5f; //플레이어 따라올때 멈추는 거리
	float Cave_Distance = 4f;
 
    //NPC 상태
    public enum NPCState
    {
        FREEMOVE, //자유이동
        SELECT_NPC, //NPC 선택
        FOLLOW_PLAYER, //플레이어 따라다님
        COMMAND_STATE, //명령 이동 중
        COMMAND_EXCUTION, //명령 수행 중
		CAVE_MOVEMENT_STATUS, //동굴 이동 상태
		IDIOT_STATE // 멍청이 상태
    }

    //명령 형태
    public enum CommandState
    {
        NONE, //없음
        FRUIT_PICKING, //과일따기
        STONE_PICKING, //돌캐기
		HIT_SMALL_ANIMALL //토끼 사냥하기
    }

    //시간제어
    public float Instruction_time = 0f;

    //변수
    public NPCState npcstate; //npc 상태
    public CommandState commandstate; //명령 상태

	public GameObject Itemlist; //아이템 변수들을 가지고 있는 오브젝트(feat. 가방)
    public GameObject Main_Camera; 
	Variable variable; // 변수 제어 스크립트
	NPCItem npcitem;
	FruitTree fruittree;
	Mine mine;
    private bool alive; // 코루틴 변수

	public GameObject target; //선택된 타겟

    // 회전을 위한 변수
    private float rotationSpeed = 2f;

    // Pause, Resume을 위한 변수
    protected Vector3 lastAgentVelocity;
    protected NavMeshPath lastAgentPath;

    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.Find("VRPLAYER");
		Cave = GameObject.Find ("동굴");
		Itemlist = GameObject.Find ("variable");
		Main_Camera = GameObject.Find ("Main Camera");

        variable = Itemlist.GetComponent<Variable>();
		npcitem = this.GetComponent<NPCItem> ();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; //true : 목적지에 다가갈수록 속도가 빨라진다. false : 속도가 일정하다.
        agent.updatePosition = true; // position값을 업데이트한다.
        agent.updateRotation = false; // rotation값은 업데이트 하지않는다.

		//자유이동 포지션 가지고 옴
        var group = GameObject.Find("WorkerWayPoint");

        if(group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoint);
            wayPoint.RemoveAt(0); //부모의  transform은 제거한다.

            nextidx = Random.Range(0, wayPoint.Count);
        }

		//초기화
        npcstate = NPCState.FREEMOVE;
        commandstate = CommandState.NONE;
        alive = true;

        nextidx = Random.Range(0, wayPoint.Count);

		//상태 코루틴 시작
        StartCoroutine("FSM");
    }

    IEnumerator FSM()
    {
		//alive가 false가 되기전까지 무한반복
        while(alive)
        {
            switch(npcstate)
            {
                case NPCState.FREEMOVE:
                    MoveWayPoint();
                    break;

                case NPCState.SELECT_NPC:
                    RePos();
                    break;

				case NPCState.FOLLOW_PLAYER:
                    FollowPlayer();
                    break;

				case NPCState.COMMAND_STATE:
                    IndicationPos();
                    break;

                case NPCState.COMMAND_EXCUTION:
                    CommandExcution();
                    break;

			case NPCState.CAVE_MOVEMENT_STATUS:
					Tocavemove ();
					break;

				case NPCState.IDIOT_STATE:
					Idiot ();
					break;
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
		//현재 선택 리스트에 자신의 object가 있다면 자신의 effect도 따라오게 한다
        if(CheckList())
        {
            GameObject.Find(this.name + "Effect").transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, this.transform.position.z);
        }
    }

	//자율이동 상태
    private void MoveWayPoint()
    {
        if (agent.isPathStale) return;

		//목적지에 Target_Distance만큼 가까워 졌으면 새로운 pos를 목적지로 바꾼다
        if (agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= Target_Distance)
        {
            nextidx = Random.Range(0, wayPoint.Count);
        }

        agent.destination = wayPoint[nextidx].position;
		LookToward(wayPoint[nextidx].position);
		ResumeMove();
    }

	//NPC를 선택했을 경우 Player의 위치로 목적지를 설정하고 FOLLOW_PLAYER상태로 변경한다.
    public void RePos()
    {
        npcstate = NPCState.FOLLOW_PLAYER;
        agent.destination = Player.transform.position;
        LookToward(Player.transform.position);
    }

	//플레이어를 따라다니고 있는 상태
    public void FollowPlayer()
    {
		//플레이어와 자신의 거리가 Player_Distance보다 짧으면 멈추고 아니면 따라간다.
		if (Vector3.Distance(this.transform.position, Player.transform.position) <= Player_Distance)
        {
            if (CheckList())
            {
                PauseMove();
            }
        }

        else
        {
            agent.destination = Player.transform.position;
            LookToward(Player.transform.position);
			ResumeMove();
        }
    }

	//명령을 이행하러 이동하는 상태
    public void IndicationPos()
    {
		//명령을 지정하는 순간 선택 리스트에서 제거
		ReemoveList();

		//명령이 사냥이면 바로 상태 넘김
			if (commandstate == CommandState.HIT_SMALL_ANIMALL) {
			npcstate = NPCState.COMMAND_EXCUTION;
		} 

		//아니면 그위치로 이동
		else {
			if (agent.velocity.sqrMagnitude <= 0.1f * 0.1f && agent.remainingDistance <= Target_Distance) {
				npcstate = NPCState.COMMAND_EXCUTION;
				PauseMove ();
			} 
			else {
				agent.destination = target.transform.position;
				LookToward(target.transform.position);
				ResumeMove ();
			}
		}
    }

	//명령 수행중인 상태
    public void CommandExcution()
	{
		//명령이 과일채입일 경우
		if (commandstate == CommandState.FRUIT_PICKING) {
			Fruit_Gathering ();
		}

		//명령이 돌채집일 경우
		else if (commandstate == CommandState.STONE_PICKING) {
			Stone_Gathering ();
		} 

		//명령이 작은동물 공격일 경우
		else if (commandstate == CommandState.HIT_SMALL_ANIMALL) {
			Hit_Small_Animal ();
		}
    }

	//동굴 이동 상태
	public void Tocavemove()
	{
		if (agent.velocity.sqrMagnitude <= 1f * 1f && agent.remainingDistance <= Cave_Distance) {
			if (commandstate == CommandState.FRUIT_PICKING) {
				CaveStorage.Instance.StoreItem (npcitem.have_Item, CaveStorage.ItemType.FRUIT);
			}

			if (commandstate == CommandState.STONE_PICKING) {
				CaveStorage.Instance.StoreItem (npcitem.have_Item, CaveStorage.ItemType.STONE);
			}
			StandardMode ();
		} 

		else {
			agent.destination = Cave.transform.position;
			LookToward(Cave.transform.position);
			ResumeMove();
		}
	}

	//과일채집 명령 수행
    public void Fruit_Gathering()
    {
		//과일개수가 최대저장 개수보다 많은경우 자유이동으로 변경
		if (CaveStorage.Instance.storedFruitObjs.Count >= variable.MAX_Fruit)
        {
            StandardMode();
        }

		//과일 채집시간에 맞춰 과일 개수가 증가함
        else
        {
            if (Instruction_time >= variable.Fruit_Picking_Time)
            {
				fruittree = target.GetComponent<FruitTree> ();
				npcitem.have_Item = fruittree.GetFruit ();
				npcitem.have_Item.transform.position = Vector3.zero;
				npcitem.have_Item.transform.parent = this.transform;
				npcitem.have_Item.transform.localPosition = new Vector3(0, -0.04f, 0.04f);
				npcitem.have_Item.transform.localScale = new Vector3 (1, 1, 1) * 0.2f;

				Instruction_time = 0;
				agent.destination = Cave.transform.position;
				npcstate = NPCState.CAVE_MOVEMENT_STATUS;
            }

			else
			{
				Instruction_time += Time.deltaTime;
			}
        }
    }

	//돌채집 명령 수행
    public void Stone_Gathering()
    {
		//돌개수가 최대 돌 소유 개수보다 많은경우 자유이동으로 변경
		if (CaveStorage.Instance.storedStoneObjs.Count >= variable.MAX_Stone)
		{
			StandardMode();
		}

		//돌 채집시간에 맞춰 돌 개수가 증가하며 반복작업하지 않고 자유이동으로 변경된다
		else
		{
			if (Instruction_time >= variable.Stone_Picking_Time)
			{
				mine = target.GetComponent<Mine> ();
				npcitem.have_Item = mine.GetStone ();
				npcitem.have_Item.transform.position = Vector3.zero;
				npcitem.have_Item.transform.parent = this.transform;
				npcitem.have_Item.transform.localPosition = new Vector3(0, -0.04f, 0.04f);
				npcitem.have_Item.transform.localScale = new Vector3 (1, 1, 1) * 0.2f;

				Instruction_time = 0;
				agent.destination = Cave.transform.position;
				npcstate = NPCState.CAVE_MOVEMENT_STATUS;
			}
			else
			{
				Instruction_time += Time.deltaTime;
			}
		}
    }

	//작은 동물 사냥 명령
	public void Hit_Small_Animal()
	{
		//현재 타겟이된 작은동물의 스크립트를 가지고온다.
		EnemyQuarryAI enemyquarryai = target.GetComponent<EnemyQuarryAI> ();

		//타겟과 나의 거리가 공격가능 거리 안에 들어오면 멈추고 HIT_Time마다 공격한다
		if (Vector3.Distance (this.transform.position, target.transform.position) <= Small_Animal_Distance) {
			if (Instruction_time >= variable.HIT_Time)
			{
				//돌이 아닌 무기 확인해야 함
				if (CaveStorage.Instance.storedStoneObjs.Count <= 0 || enemyquarryai.hp <= 0 || enemyquarryai == null) { 
					StandardMode();
				} 

				else {
					//공격무기 내구 감소
                    //enemyquarryai.hp--;
                    enemyquarryai.AttackedByWorker(this.transform.position);
					Instruction_time = 0;
				}
			}
			else
			{
				Instruction_time += Time.deltaTime;
			}

			PauseMove ();
		}

		//거리가 멀어지면 따라간다.
		else {
			agent.destination = target.transform.position;
			LookToward(target.transform.position);
			ResumeMove ();
		}
	}

	//멍청이 상태
	public void Idiot()
	{
		if (Instruction_time >= variable.Idiot_Time)
		{
			Instruction_time = 0;
			StandardMode ();
		}
		else
		{
			Instruction_time += Time.deltaTime;
			PauseMove ();
		}
	}

	// 리스트에서 본 오브젝트와 이펙트 제거
    public void ReemoveList()
    {
        if (CheckList())
        {
            variable.selectnpc.Remove(this.gameObject);
            variable.selectnpc_count--;
            Destroy(GameObject.Find(this.name + "Effect"));
        }
    }

	//초기화로 되돌림
    public void StandardMode()
    {
        npcstate = NPCState.FREEMOVE;
        commandstate = CommandState.NONE;
        nextidx = Random.Range(0, wayPoint.Count);
        ReemoveList();
    }

	//체크리스트에 본인이 있는지 확인
    bool CheckList()
    {
        for(int i = 0; i < variable.selectnpc_count; i++)
        {
            if (this.name == variable.selectnpc[i].name)
            {
                return true;
            }
        }

        return false;
    } //있으면 true 없으면 false

    // 현재위치에서 target 방향으로 회전한다. 
    protected void LookToward(Vector3 targetPosition)
    {
        Vector3 _direction = (targetPosition - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }

    // agent 움직임 정지
    protected void PauseMove()
    {
		agent.speed = 0;
    }

    // agent 움직임 재개
    protected void ResumeMove()
    {
		agent.speed = 1;
    }
}
