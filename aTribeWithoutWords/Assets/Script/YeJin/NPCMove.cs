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

    RayCast raycast;
    Variable variable;

    //NPC 상태
    public int NPCState = 0; // 0 : 자유이동, 1 : NPC 선택, 2 : 명령수행 이동중, 3 : 명령 수행 중
    //명령 형태
    public int CommandState = 0; // 0 : 명령 없음, 1 : 과일 채집, 2 : 돌 채집
    //시간제어
    public float Instruction_time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        raycast = GameObject.Find("Main Camera").GetComponent<RayCast>();
        variable = GameObject.Find("ItemList").GetComponent<Variable>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; //true : 목적지에 다가갈수록 속도가 빨라진다.

        var group = GameObject.Find("WorkerWayPoint");

        if(group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoint);
            wayPoint.RemoveAt(0); //부모의  transform은 제거한다.

            nextidx = Random.Range(0, wayPoint.Count);
        }

        MoveWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {
            nextidx = Random.Range(0, wayPoint.Count);
            MoveWayPoint();
        }

        if(GameObject.Find(this.name + "Effect") != null)
        {
            GameObject.Find(this.name + "Effect").transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z);
        }


        if(NPCState == 3 && CommandState == 1)
        {
            Fruit_Gathering();
        }

        else if (NPCState == 3 && CommandState == 2)
        {
            Stone_Gathering();
        }
    }

    private void MoveWayPoint()
    {
        if (agent.isPathStale) return;

        if (NPCState == 0)
        {
             agent.destination = wayPoint[nextidx].position;
             agent.speed = 3.5f;
             agent.isStopped = false;
        }

        if (NPCState == 1 || NPCState == 2)
        {
            for (int i = 1; i <= 3; i++)
            {
                if(this.name == "Worker" + i && variable.chooseNPC[i] == 1) 
                {
                    if(NPCState == 2)
                    {
                        NPCState = 3;
                        variable.chooseNPC[i] = 0;
                        Destroy(GameObject.Find(this.name + "Effect"));
                    }
                    agent.speed = 0f;
                    return;
                }
            }
        }
    }

    public void RePos()
    {
        NPCState = 1;
        agent.destination = Player.transform.position;
        agent.speed = 3.5f;
        agent.isStopped = false;
    }

    public void IndicationPos()
    {
        NPCState = 2;
        agent.destination = GameObject.Find(raycast.hitname).transform.position;
        agent.speed = 3.5f;
        agent.isStopped = false;
    }

    public void Fruit_Gathering()
    {
        if (variable.Fruit >= variable.MAX_Fruit)
        {
            standardMode();
        }
        else
        {
            if (Instruction_time >= variable.Fruit_Picking_Time)
            {
                variable.Fruit++;
                Instruction_time = 0;
            }
            else
            {
                Instruction_time += Time.deltaTime;
            }
        }
    }

    public void Stone_Gathering()
    {
        variable.Stone++;
        standardMode();
    }

    public void standardMode()
    {
        CommandState = 0;
        NPCState = 0;
        nextidx = Random.Range(0, wayPoint.Count);
        MoveWayPoint();
    }
}
