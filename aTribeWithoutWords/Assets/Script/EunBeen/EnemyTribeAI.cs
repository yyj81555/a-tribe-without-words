using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 공격하는 적 부족(일꾼 공격, 방해)
public class EnemyTribeAI : EnemyAIBase
{
    // EnemyAIBase로 부터 상속받는 변수 확인할 것.
    // ...

    public enum EnmyTribeType
    {
        SOLDIER,  // 전사
        SHAMAN    // 주술사
    }

    [Header("State Setting")]
    public int hp;
    public EnmyTribeType tribeType = EnmyTribeType.SOLDIER;  // 적 부족 종류


    // 정찰에 대한 변수
    public GameObject[] waypoints;  // 정찰하는 지점
    private int waypointIndex = 0;
    public float patrolSpeed = 2f;
    const float waypntLeftDist = 4f; // waypoint까지 남은 거리

    // 추격에 대한 변수
    public float chaseSpeed;
    private List<GameObject> targets;

    // 공격에 대한 변수
    public float attackPower;
    private float attackTime;
    private float attackCycleTime;  // 공격 사이클
    private float attackRange;   // 공격 사정거리

    public override void Start()
    {
        Init();
        base.Start();
        // 맵상에 존재하는 적 리스트에 추가
        GameLevelManager.Instance.enemyTribeInMapList.Add(this.gameObject);
    }

    void Init()
    {
        waypoints = new GameObject[3];
        targets = new List<GameObject>();

        Transform enemyWayPoints = GameObject.Find("WayPoints").transform.GetChild(1);
        waypoints[0] = enemyWayPoints.GetChild(0).gameObject; // spawn1
        waypoints[1] = enemyWayPoints.GetChild(1).gameObject; // spawn2
        waypoints[2] = enemyWayPoints.GetChild(2).gameObject; // cavePoint

        // 적 부족타입에 따라 다르게 처리
        switch (tribeType)
        {
            case EnmyTribeType.SOLDIER:
                hp = 10;
                chaseSpeed = 4f;
                attackPower = 3;
                attackCycleTime = 2f;
                attackRange = 5f;
                break;
            case EnmyTribeType.SHAMAN:
                hp = 20;
                chaseSpeed = 3f;
                attackPower = 5;
                attackCycleTime = 4f;
                attackRange = 10f;
                break;
        }

        // 스폰 위치 설정
        this.transform.position = waypoints[Random.Range(0, 2)].transform.position;
    }

    #region AI 행동들
    protected override void Patrol()
    {
        agent.speed = patrolSpeed;

        if (Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position) >= waypntLeftDist)
        {
            agent.SetDestination(waypoints[waypointIndex].transform.position);
            LookToward(waypoints[waypointIndex].transform.position);
        }
        // waypoint에 도달한 경우
        else if (Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position) < waypntLeftDist)
        {
            waypointIndex += 1; // 다음 지점으로 이동
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
        else
        {
            Debug.Log("정찰 오류");
        }
    }

    protected override void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(targets[0].transform.position);   // 첫번째로 등록된 타겟을 쫓는다.
        LookToward(targets[0].transform.position);

        // 공격 사정거리에 달했다면 공격한다.
        if (Vector3.Distance(this.transform.position, targets[0].transform.position) <= attackRange)
        {
            Debug.Log("Target Close");
            state = State.ATTACK;
        }
    }

    protected override void Attack()
    {
        // 공격하기에 너무 멀다면 다시 쫓아간다. (탐지영역을 벗어날 정도로 멀면 OnTriggerExit에서 정찰상태가 된다.)
        if (Vector3.Distance(this.transform.position, targets[0].transform.position) > attackRange)
        {
            ResumeMove();
            state = State.CHASE;
            Debug.Log("Target Too Far");
        }

        // 멈춰서 공격
        PauseMove();
        LookToward(targets[0].transform.position);

        attackTime += Time.deltaTime;
        if (attackTime > attackCycleTime)
        {
            attackTime = 0.0f;
            // target.GetComponent<???>().workerHp -= attackPower;
            // 위와같이 공격받았을때 호출되는 함수 호출

            // 부족 타입에 따라 다른 공격 처리
            if (tribeType == EnmyTribeType.SOLDIER)
            {
                Attack_Soldier();
            }
            else if (tribeType == EnmyTribeType.SHAMAN)
            {
                Attack_Shaman();
                // 샤먼의 바보만들기 공격
                targets[0].GetComponent<NPCMove>().npcstate = NPCMove.NPCState.IDIOT_STATE;
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        GameLevelManager.Instance.enemyTribeInMapList.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
    #endregion

    public void AttackedByWorker()
    {

    }

    /*Enter와 Exit에서 유의할점은 해당 이벤트가 발생할때만 아래 처리를 한다는 것이다.
      만일 콜라이더내에 머무는 동안 Enemy가 공격하여 worker가 사라진다면 Exit이벤트는 발생하지 않는다.
      worker가 공격받다 죽었을때 취하는 함수가 필요함. */

    // detector콜라이더에 worker가 탐지되면 쫓아감.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Worker")
        {
            // 타겟리스트에 저장되어 있지 않다면 저장
            if (!(targets.Contains(other.gameObject)))
            {
                targets.Add(other.gameObject);
                state = State.CHASE;
            }
        }
    }

    // 탐지영역에서 사라지면 다시 탐색
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Worker")
        {
            // 타겟 리스트에 포함되어 있다면 타겟에서 제외한다.
            if (targets.Contains(other.gameObject))
            {
                targets.Remove(other.gameObject);

                // 남아있는 타겟이 없다면 정찰상태로 변경
                // 남아있는 타겟이 있다면 자동으로 추격할 것이다. target[0]에 Queue처럼 새로운 놈이 들어감.
                Debug.Log("" + targets.Count);
                if (targets.Count == 0)
                {
                    state = State.PATROL;
                    Debug.Log("Patrol 상태 전환");
                }
            }
        }
    }

    private void Attack_Soldier()
    {
        Debug.Log("전사 공격");
    }

    private void Attack_Shaman()
    {
        Debug.Log("샤먼 공격");
    }
}
