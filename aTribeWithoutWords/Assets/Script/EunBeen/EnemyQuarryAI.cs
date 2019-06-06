using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 도망가는 사냥감
public class EnemyQuarryAI : EnemyAIBase
{
    // EnemyAIBase로 부터 상속받는 변수 확인할 것.
    // ...

    [Header("State Setting")]
    public int hp = 3;

    // 정찰에 대한 변수
    Vector3 patrolPos;
    float patrolCycleTime;
    public float patrolSpeed;

    // 도망에 대한 변수
    Vector3 runPos;
    float runawayTime;
    public float runSpeed;

    public override void Start()
    {
        Init();
        base.Start();
        // 맵에 존재하는 사냥감 리스트 추가
        GameLevelManager.Instance.quryListInMap.Add(this.gameObject);
    }

    void Init()
    {
        hp = 3;

        patrolPos = this.transform.position + new Vector3(Random.Range(-5f, 5f), 0.0f, Random.Range(-5f, 5f));
        patrolCycleTime = 0.0f;
        patrolSpeed = 0.5f;

        runPos = Vector3.zero;
        runawayTime = 0.0f;
        runSpeed = 1f;

        // 스폰 위치 설정
        Transform quarrySpawnPoint = GameObject.Find("WayPoints").transform.Find("QuarrySpawnPoint");
        this.transform.position = quarrySpawnPoint.GetChild(Random.Range(0, quarrySpawnPoint.childCount)).transform.position;
    }

    protected override void Patrol()
    {
        patrolCycleTime += Time.deltaTime;

        if (patrolCycleTime > 3f)
        {
            patrolCycleTime = 0.0f;
            // 새로운 위치 계산
            patrolPos = this.transform.position + new Vector3(Random.Range(-10f, 10f), 0.0f, Random.Range(-10f, 10f));
        }

        agent.SetDestination(patrolPos);
        LookToward(patrolPos);
        agent.speed = patrolSpeed;
    }

    protected override void Runaway()
    {
        runawayTime += Time.deltaTime;

        // 공격한 개체로부터 반대 방향으로 도주
        //agent.SetDestination(runPos);
        agent.Move(runPos * Time.deltaTime);
        LookToward(runPos);
        agent.speed = runSpeed;

        // 일정시간동안 도주하면 다시 정찰상태로 변경
        if(runawayTime > 5f)
        {
            state = State.PATROL;
            runawayTime = 0.0f;
        }
    }

    protected override void Die()
    {
        base.Die();

        GameLevelManager.Instance.quryListInMap.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    // 공격당하면 호출(Worker가 공격했을때 호출됨)
    public void AttackedByWorker(Vector3 attackerPos)
    {
        runawayTime = 0.0f;

        // 공격한 개체의 위치로부터 반대 위치 계산
        /* Y 위치때문에 문제 생길수도 있는데 일단 보류 */
        //runPos = this.transform.position + Vector3.Normalize(attackerPos - this.transform.position) * -10f;
        runPos = Vector3.Normalize(attackerPos - this.transform.position) * -10f;

        if (hp <= 0)
        {
            hp = 0;
            state = State.DIE;
        }
        else
        {
            hp -= 1;
            state = State.RUNAWAY;
        }
    }
}
