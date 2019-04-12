﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 방해하는 동물
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
    private List<GameObject> targets;

    // Var for Attack
    public float attackPower = 1;
    float attackTime = 0f;
    float attackRange = 3f;

    public override void Start()
    {
        base.Start();

        targets = new List<GameObject>();
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
            Debug.Log("다음 정찰지점으로 이동");
        }
        else
        {
            Debug.Log("???");
        }
    }

    protected override void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(targets[0].transform.position);   // 첫번째로 등록된 타겟을 쫓는다.
        
        // 공격 사정거리에 달했다면 공격한다.
        if (Vector3.Distance(this.transform.position, targets[0].transform.position) <= attackRange)
        {
            state = State.ATTACK;
        }
    }

    protected override void Attack()
    {
        // 공격하기에 너무 멀다면 다시 쫓아간다. (탐지영역을 벗어날 정도로 멀면 OnTriggerExit에서 정찰상태가 된다.)
        if (Vector3.Distance(this.transform.position, targets[0].transform.position) > attackRange)
        {
            state = State.CHASE;
        }

        attackTime += Time.deltaTime;
        if(attackTime > 3f) // 3초마다 공격
        {
            Debug.Log("worker 공격");
            attackTime = 0.0f;
            // target.GetComponent<???>().workerHp -= attackPower;
            // 혹은 공격받았을때 호출되는 함수 호출
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

    /*Enter와 Exit에서 유의할점은 해당 이벤트가 발생할때만 아래 처리를 한다는 것이다.
       만일 콜라이더내에 머무는 동안 Enemy가 공격하여 worker가 사라진다면 Exit이벤트는 발생하지 않는다.*/

    // detector콜라이더에 worker가 탐지되면 쫓아감.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "worker")
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
        if(other.tag == "worker")
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
    }
}