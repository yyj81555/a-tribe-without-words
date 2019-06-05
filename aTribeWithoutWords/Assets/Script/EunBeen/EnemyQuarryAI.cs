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
    Vector3 patrolPos = Vector3.zero;
    float patrolCycleTime = 0.0f;
    public float patrolSpeed = 0.5f;

    // 도망에 대한 변수
    Vector3 runPos = Vector3.zero;
    float runawayTime = 0.0f;
    public float runSpeed = 1f;

    public override void Start()
    {
        base.Start();
        Init();
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
        Destroy(this.gameObject);
    }

    // 공격당하면 호출(Worker가 공격했을때 호출됨)
    public void AttackedByWorker(Vector3 attackerPos)
    {
        if (hp <= 0) { hp = 0; }
        else { hp -= 1; }

        runawayTime = 0.0f;

        // 공격한 개체의 위치로부터 반대 위치 계산
        /* Y 위치때문에 문제 생길수도 있는데 일단 보류 */
        //runPos = this.transform.position + Vector3.Normalize(attackerPos - this.transform.position) * -10f;
        runPos = Vector3.Normalize(attackerPos - this.transform.position) * -10f;

        if (hp <= 0)
        {
            state = State.DIE;
        }
        else
        {
            state = State.RUNAWAY;
        }
    }
}
