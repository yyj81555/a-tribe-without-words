using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

// 공통적인 적 AI 설계. 
// 상속시킬 변수, 메서드 정의.
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyAIBase : MonoBehaviour
{
    public enum State
    {
        PATROL,   // 정찰
        CHASE,    // 쫓기
        ATTACK,   // 공격
        PLUNDER,  // 약탈
        RUNAWAY,  // 도망
        DIE       // 사망
    }

    public NavMeshAgent agent;
    // public ThirdPersonCharacter charContrler;
    // ThirdPersonCharacter를 활용하면 agent의 이동에 따라 변하는 회전, 속도 등을 고려한 자연스러운 애니메이션을 구현가능.
    // 이것을 직접 구현하려면 꽤 어려움. ThirdPersonCharacter를 분석해서 수정하는것이 빠를거같음.

    // AI 상태 변수
    public State state;
    protected bool alive;

    // 모든 개체들은 아래와 같은 과정을 거친다.
    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //charContrler = GetComponent<ThirdPersonCharacter>();

        // updateRotation = true 하게 되면 agent가 목표위치로 움직이면서 회전하는 것을 반영하게 됨. -> 여기에 단순히 애니메이션을 씌우게 되면 이상하게 움직임.
        // updateRotation = false로 하고 ThirdPersonCharacter.cs 에서 작성된 자체 계산 함수(Move)를 이용하도록 한다.
        agent.updatePosition = true;
        agent.updateRotation = false;

        state = EnemyAIBase.State.PATROL;
        alive = true;

        StartCoroutine("FSM");
    }

    // alive가 true인 상태동안 무한 반복
    IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.PATROL:
                    Patrol();
                    break;
                case State.CHASE:
                    Chase();
                    break;
                case State.ATTACK:
                    Attack();
                    break;
                case State.PLUNDER:
                    Plunder();
                    break;
                case State.RUNAWAY:
                    Runaway();
                    break;
                case State.DIE:
                    Die();
                    break;
            }
            yield return null;
        }
    }

    virtual protected void Patrol()
    {
        Debug.Log("자식 스크립트에서 함수가 호출되어져야 한다.");
    }

    virtual protected void Chase()
    {

        Debug.Log("자식 스크립트에서 함수가 호출되어져야 한다.");
    }

    virtual protected void Attack()
    {
        Debug.Log("자식 스크립트에서 함수가 호출되어져야 한다.");
    }

    virtual protected void Plunder()
    {
        Debug.Log("자식 스크립트에서 함수가 호출되어져야 한다.");
    }

    virtual protected void Runaway()
    {
        Debug.Log("자식 스크립트에서 함수가 호출되어져야 한다.");
    }

    virtual protected void Die()
    {
        alive = false;
    }
}
