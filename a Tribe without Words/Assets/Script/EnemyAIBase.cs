using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 공통적인 적 AI 설계. 
// 상속시킬 변수, 메서드 정의.
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIBase : MonoBehaviour
{
    public enum State
    {
        PATROL,
        CHASE,
        ATTACK,
        RUNAWAY,
        DIE
    }

    public NavMeshAgent agent;
    //public CharacterController charContrler; /* CharacterController외에 다른 컨트롤러 컴포넌트가 있다면 그것을 쓰자 */

    // AI 상태 변수
    public State state;
    protected bool alive;

    // 모든 개체들은 아래와 같은 과정을 거친다.
    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //charContrler = GetComponent<CharacterController>();

        /* updateRotation 수정해야함? 일단 보류 */
        agent.updatePosition = true;
        agent.updateRotation = true;

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

    virtual protected void Runaway()
    {
        Debug.Log("자식 스크립트에서 함수가 호출되어져야 한다.");
    }

    virtual protected void Die()
    {
        alive = false;
    }
}
