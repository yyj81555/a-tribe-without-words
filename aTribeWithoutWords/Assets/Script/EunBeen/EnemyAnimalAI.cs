using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 방해하는 동물(식량 탈취)
public class EnemyAnimalAI : EnemyAIBase
{
    // EnemyAIBase로 부터 상속받는 변수 확인할 것.
    // ...

    [Header("State Setting")]
    public int hp;

    // 정찰에 대한 변수
    public Transform[] spawnPoints; // 생성지점
    public Transform[] targetPoint; // 타겟지점(동굴, 등)
    private int targetIndex;        // 타겟 지점에 대한 인덱스 (0: 동굴, ...)
    public float patrolSpeed;
    const float waypntLeftDist = 4f; // waypoint까지 남은 거리

    // 약탈에 대한 변수
    float plunderTime;
    float plunderRange;
    float plunderCycleTime; // 약탈 사이클
    public GameObject robbedObj;

    // 도망에 대한 변수
    public float runSpeed;


    public override void Start()
    {
        Init();
        base.Start();
        // 맵에 존재하는 적 리스트에 추가
        GameLevelManager.Instance.emyAniListInMap.Add(this.gameObject);
    }

    void Init()
    {
        spawnPoints = new Transform[2];
        targetPoint = new Transform[1];
        targetIndex = Random.Range(0, targetPoint.Length);

        Transform enemyWayPoints = GameObject.Find("WayPoints").transform.Find("EnemyWayPoint");
        spawnPoints[0] = enemyWayPoints.GetChild(0); // spawn1
        spawnPoints[1] = enemyWayPoints.GetChild(1); // spawn2
        targetPoint[0] = enemyWayPoints.GetChild(2); // cavePoint

        hp = 5;
        patrolSpeed = 3f;
        plunderTime = 0f;
        plunderRange = 1.5f;
        plunderCycleTime = 3f;
        robbedObj = null;
        runSpeed = 5f;


        // 스폰 위치 설정
        this.transform.position = spawnPoints[Random.Range(0, 2)].position;
    }

    #region AI 행동들
    /*
    protected override void Patrol()
    {
        agent.speed = patrolSpeed;

        if (Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position) >= waypntLeftDist)
        {
            agent.SetDestination(waypoints[waypointIndex].transform.position);
            LookToward(waypoints[waypointIndex].transform.position);
        }
        // waypoint에 도달한 경우
        else if(Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position) < waypntLeftDist)
        {
            waypointIndex += 1; // 다음 지점으로 이동
            if(waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
            Debug.Log("다음 정찰지점으로 이동");
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
            state = State.ATTACK;
        }
    }

    // 일꾼을 방해한다.
    protected override void Attack()
    {
        // 공격하기에 너무 멀다면 다시 쫓아간다. (탐지영역을 벗어날 정도로 멀면 OnTriggerExit에서 정찰상태가 된다.)
        if (Vector3.Distance(this.transform.position, targets[0].transform.position) > attackRange)
        {
            //ResumeMove();
            state = State.CHASE;
        }

        // 멈춰서 공격
        //PauseMove();
        LookToward(targets[0].transform.position);

        attackTime += Time.deltaTime;
        if(attackTime > 3f) // 3초마다 공격
        {
            Debug.Log("worker 방해");
            attackTime = 0.0f;
            // target.GetComponent<???>().workerHp -= attackPower;
            // 혹은 공격받았을때 호출되는 함수 호출
        }
    }*/

    // 타겟 지점으로 이동.
    protected override void Patrol()
    {
        agent.speed = patrolSpeed;

        agent.SetDestination(targetPoint[targetIndex].position);
        LookToward(targetPoint[targetIndex].position);
        
        // 목적지에 도착했다면 약탈한다.
        if(Vector3.Distance(this.transform.position, targetPoint[targetIndex].position) < waypntLeftDist)
        {
            state = State.PLUNDER;
        }
    }

    // 동굴 등의 장소를 약탈한다.
    protected override void Plunder()
    {
        // 약탈하기에 너무 멀다면 다시 목적지로 이동한다.
        if (Vector3.Distance(this.transform.position, targetPoint[targetIndex].position) > plunderRange)
        {
            state = State.PATROL;
        }

        // 멈춰서 약탈
        PauseMove();
        LookToward(targetPoint[targetIndex].position);

        plunderTime += Time.deltaTime;
        if(plunderTime > plunderCycleTime)
        {
            plunderTime = 0f;

            // 타겟이 가진 저장고 리스트에서 약탈
            if (targetIndex == 0) // 타겟이 동굴인 경우
                robbedObj = CaveStorage.Instance.RobItem(CaveStorage.ItemType.FRUIT);
            else
                Debug.Log("targetIndex 오류");


            if (robbedObj != null)
            {
                robbedObj.transform.position = Vector3.zero;
                robbedObj.transform.localPosition = this.transform.localPosition + new Vector3(0, 0, 1);  /* 위치 정확한지 모름. 크기변경 필요? */
                robbedObj.transform.parent = this.transform;
            }
            else // 훔칠 오브젝트가 없는 경우
                Debug.Log("동굴에 훔칠 아이템이 없습니다.");

            state = State.RUNAWAY;
        }
    }

    // 물건을 훔쳤을때 도주한다. (일꾼에 의해 공격당할때 일정확률로 도주하기도 함)
    protected override void Runaway()
    {
        agent.speed = runSpeed;

        agent.SetDestination(spawnPoints[0].position);
        LookToward(spawnPoints[0].position);

        // 특정 지점으로 가면 사라진다.
        if (Vector3.Distance(this.transform.position, spawnPoints[0].position) < waypntLeftDist)
        {
            state = State.DIE;
        }
    }

    protected override void Die()
    {
        base.Die();

        GameLevelManager.Instance.emyAniListInMap.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
    #endregion

    // 일꾼에 의해 공격당한 경우
    public void AttackedByWorker()
    {
        if (hp <= 0)
        {
            hp = 0;
            state = State.DIE;
        }
        else
        {
            hp -= 1;
            state = ((Random.Range(0, 100) < 20) ? State.RUNAWAY : state); // 20% 확률로 도망간다. 80%는  행동중인 상태 유지.
        }
    }

    /*Enter와 Exit에서 유의할점은 해당 이벤트가 발생할때만 아래 처리를 한다는 것이다.
       만일 콜라이더내에 머무는 동안 Enemy가 공격하여 worker가 사라진다면 Exit이벤트는 발생하지 않는다.*/
    /*
    // detector콜라이더에 worker가 탐지되면 쫓아감.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Worker")
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
        if(other.tag == "Worker")
        {
            // 타겟 리스트에 포함되어 있다면 타겟에서 제외한다.
            if (targets.Contains(other.gameObject))
            {
                targets.Remove(other.gameObject);

                // 남아있는 타겟이 없다면 정찰상태로 변경
                // 남아있는 타겟이 있었다면 자동으로 추격할 것이다. target[0]에 Queue처럼 새로운 놈이 들어감.
                if (targets.Count == 0)
                {
                    state = State.PATROL;
                }
            }
        }
    }*/
}
