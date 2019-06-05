using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 공통적인 적 AI 설계. 
// 상속시킬 변수, 메서드 정의.
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyAIBase : MonoBehaviour
{
    public enum State
    {
        PATROL,   // 정찰
        CHASE,    // 추격
        ATTACK,   // 공격
        PLUNDER,  // 약탈
        RUNAWAY,  // 도망
        DIE       // 사망
    }

    public NavMeshAgent agent;

    private Animator animator;

    // AI 상태 변수
    public State state;
    private State lastState;
    private bool stateChanged;
    protected bool alive;

    // 회전을 위한 변수
    private float rotationSpeed = 2f;

    // Pause, Resume을 위한 변수
    protected Vector3 lastAgentVelocity;
    protected NavMeshPath lastAgentPath;

    // 모든 개체들은 아래와 같은 과정을 거친다.
    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //charContrler = GetComponent<ThirdPersonCharacter>();

        animator = GetComponent<Animator>();

        // updateRotation = true 하게 되면 agent가 목표위치로 움직이면서 회전하는 것을 반영하게 됨. -> 여기에 단순히 애니메이션을 씌우게 되면 이상하게 움직임.
        // updateRotation = false로 하고 ThirdPersonCharacter.cs 에서 작성된 자체 계산 함수(Move)를 이용하도록 한다.
        agent.updatePosition = true;
        agent.updateRotation = false;

        state = EnemyAIBase.State.PATROL;
        lastState = State.DIE;
        stateChanged = true;
        alive = true;

        StartCoroutine("FSM");
    }

    // alive가 true인 상태동안 무한 반복
    IEnumerator FSM()
    {
        while (alive)
        {
            // 상태가 변경되었을 경우에만 애니메이션을 한번 재생하도록 한다.
            if (state != lastState) {
                lastState = state;
                stateChanged = true;
            }

            switch (state)
            {
                case State.PATROL:
                    Patrol();
                    if (stateChanged)
                    {
                        animator.SetTrigger("Walk");
                        stateChanged = false;
                    }
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
                    if (stateChanged)
                    {
                        animator.SetTrigger("Run");
                        stateChanged = false;
                    }
                    break;

                case State.DIE:
                    Die();
                    break;
            }
            yield return null;
        }
    }
    #region AI 행동들

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
        Debug.Log(this.gameObject.name + " 죽음");
        alive = false;
    }

    #endregion


    /* AI가 다 가져야 하는 함수 */

    // 현재위치에서 target 방향으로 회전한다. 
    protected void LookToward(Vector3 targetPosition)
    {
        Vector3 _direction = (targetPosition - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }

    // agent 움직임 정지
    protected void PauseMove()
    {
        lastAgentVelocity = agent.velocity;
        lastAgentPath = agent.path;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

    // agent 움직임 재개
    protected void ResumeMove()
    {
        agent.velocity = lastAgentVelocity;
        agent.SetPath(lastAgentPath);
    }
}
