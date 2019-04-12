using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격하는 동물
public class EnemyAnimalAI : EnemyAIBase
{
    // EnemyAIBase로 부터 상속받는 변수 확인할 것.
    // ...

    [Header("State Setting")]
    public int hp = 10;

    // Var for patrol
    public GameObject[] waypoints;  // 정찰하는 지점
    private int waypointIndex = 0;
    public float patrolSpeed = 3f;
    float waypntLeftDist = 4f; // waypoint까지 남은 거리

    // Var for chase
    public float chaseSpeed = 5f;
    private GameObject target;

    public override void Start()
    {
        base.Start();
    }

    protected override void Patrol()
    {
        agent.speed = patrolSpeed;

        if (Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position) >= waypntLeftDist)
        {
            agent.SetDestination(waypoints[waypointIndex].transform.position);
        }
        // waypoint에 도달한 경우
        else if(Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position) < waypntLeftDist)
        {
            waypointIndex += 1; // 다음 지점으로 이동
            if(waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
            Debug.Log("다음 지점으로 이동");
        }
        else
        {
            Debug.Log("???");
        }
    }

    protected override void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(target.transform.position);

        // 타겟의 주변에 도착했다면
        if(Vector3.Distance(this.transform.position, target.transform.position) <= 3f)
        {
            state = State.ATTACK;
        }
    }

    protected override void Attack()
    {

        // 타겟이 
        if (Vector3.Distance(this.transform.position, target.transform.position) > 3f)
        {
            state = State.CHASE;
        }
    }

    protected override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

    public void AttackedByWorker()
    {

    }

    // worker를 탐지하면 쫓아가도록 테스트.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "worker")
        {
            // 추격하는 중에는 다른 개체를 추격하지 않는다.
            if (state != State.CHASE)
            {
                target = other.gameObject;
                state = State.CHASE;
                Debug.Log("잡아라!");
            }
        }
    }

    // 탐지영역에서 사라지면 다시 탐색
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "worker")
        {
            // 탐지영역에서 사라진 개체가 target이라면
            if (target == other.gameObject)
                state = State.PATROL;
        }
    }
}
