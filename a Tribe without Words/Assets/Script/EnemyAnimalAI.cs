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

        // Distance말고 다른 방법을 생각해보는 것도 좋을것같다. 
        // Distance로 하면 두 점 사이의 거리가 좁혀지지 않는 경우가 있음. ex) waypoint가 공중에 있다던지
        if (Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position) >= 4)
        {
            agent.SetDestination(waypoints[waypointIndex].transform.position);
        }
        else if(Vector3.Distance(this.transform.position, waypoints[waypointIndex].transform.position) < 4)
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
    }

    protected override void Attack()
    {

    }

    protected override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

    // worker를 탐지하면 쫓아가도록 테스트.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "worker")
        {
            target = other.gameObject;
            state = State.CHASE;
            Debug.Log("잡아라!");
        }
    }
}
