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

    // Var for Runaway
    //bool isAttacked = false;
    Vector3 runPos = Vector3.zero;
    float runawayTime = 0.0f;
    public float runSpeed = 5f;

    // Var for Patrol
    Vector3 patrolPos = Vector3.zero;
    float patrolCycleTime = 0.0f;
    public float patrolSpeed = 3f;

    public override void Start()
    {
        base.Start();
        patrolPos = this.transform.position + new Vector3(Random.Range(-5f, 5f), 0.0f, Random.Range(-5f, 5f));
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
        agent.speed = patrolSpeed;
    }

    protected override void Runaway()
    {
        runawayTime += Time.deltaTime;

        // 공격한 개체로부터 반대 방향으로 도주
        agent.SetDestination(runPos);
        agent.speed = runSpeed;

        if(runawayTime > 5f)
        {
            state = State.PATROL;
            runawayTime = 0.0f;
            //isAttacked = false;
        }
    }

    protected override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

    // 공격당하면 호출
    public void AttackedByWorker(Vector3 attackerPos)
    {
        hp -= 1;
        //isAttacked = true;
        runawayTime = 0.0f;

        // 공격한 개체의 위치로부터 반대 위치 계산
        /* Y 위치때문에 문제 생길수도 있는데 일단 보류 */
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

    // 공격당하는 것을 충돌로 간주하고 테스트
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "worker")
        {
            AttackedByWorker(collision.gameObject.transform.position);
        }
    }
}
