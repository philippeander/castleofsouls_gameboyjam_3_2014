﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoviment : MonoBehaviour {

    public enum EnemyType { patrol,
                            sinMovimentPatrol,
                            sinMovimentOnly,
                            Vigilante
    }

    [SerializeField] private EnemyType m_enemyType;

    [Space(15)]
    [Header("PARAMITERS")]
    [SerializeField] private float m_attackField = 0.6f;

    [Space(15)]
    [Header("PATROL")]
    [SerializeField] private float m_attackDelay = 1f;
    [SerializeField] private float m_attackFrequency = 1f;
    [SerializeField] private float m_maxPerimiterPatrol = 2;

    private Direction m_curPatrolDir = Direction.down;
    private Vector3 m_startPos = Vector3.zero;
    private float m_curPerimiterPatrol = 0;
    private RaycastDirection m_lastDirectionSelect = new RaycastDirection(Direction.down);
    private RaycastDirection m_newDir = new RaycastDirection(Direction.down);

    //private SwardCollider m_swardCollider;
    private bool m_canAttack = false;
    private Coroutine m_idleCO;
    private Coroutine m_PersueCO;
    private Coroutine m_attackCO;
    private Coroutine m_attackAlign;
    private EnemySensor m_enemySensor;

    private const string NAME_ATTACK_ANIM = "Attack";

    // Use this for initialization
    void Start () {
        m_startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        EnemyManager();
    }

    private void EnemyManager()
    {
        MirrorImage();

        if (m_enemySensor.IsPlayerSeen)
        {
            if (m_enemySensor.IsPlayerAttackArea)
            {
                m_enemySensor.CurState = EnemyState.attack;
            }
            else
            {
                m_enemySensor.CurState = EnemyState.persue;
            }
        }
        else
        {
            m_enemySensor.CurState = EnemyState.patrol;
        }

        if (m_enemySensor.HistState != m_enemySensor.CurState && !GetComponent<Health>().IsDamaged)
        {

            switch (m_enemySensor.CurState)
            {
                case EnemyState.patrol:
                    OnIdlePatrol();
                    break;
                case EnemyState.persue:
                    Persue();
                    break;
                case EnemyState.attack:
                    OnAttack();
                    break;
            }

            m_enemySensor.HistState = m_enemySensor.CurState;
        }

    }

    public void MirrorImage()
    {
        Vector3 curVel = (transform.position - m_enemySensor.PrevLoc) / Time.deltaTime;

        if (curVel.x < 0 || (m_enemySensor.IsPlayerAttackArea && m_enemySensor.PlayerDistance.x > 0))
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if (curVel.x > 0 || (m_enemySensor.IsPlayerAttackArea && m_enemySensor.PlayerDistance.x < 0))
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }

        m_enemySensor.PrevLoc = transform.position;

    }

    private Vector2 DefineNewPath()
    {
        Vector3 newPos = m_startPos;

        List<RaycastDirection> libDirList = new List<RaycastDirection>();


        for (int i = 0; i < m_enemySensor.Hit.Count; i++)
        {
            if (!m_enemySensor.Hit[i].isWallColliding && m_enemySensor.Hit[i].direction != m_lastDirectionSelect.direction)
            {
                libDirList.Add(m_enemySensor.Hit[i]);
            }
        }

        if (libDirList.Count > 0)
        {
            m_newDir = libDirList[UnityEngine.Random.Range(0, libDirList.Count)];
        }
        m_lastDirectionSelect = m_newDir;

        switch (m_newDir.direction)
        {
            case Direction.up:
                newPos = new Vector3(
                        transform.position.x,   // ( X )
                        UnityEngine.Random.Range(transform.position.y, m_startPos.y + m_maxPerimiterPatrol));  // ( Y )

                break;
            case Direction.down:
                newPos = new Vector3(
                        transform.position.x,   // ( X )
                        UnityEngine.Random.Range(transform.position.y, m_startPos.y + (-m_maxPerimiterPatrol))); // ( Y )

                break;
            case Direction.left:
                newPos = new Vector3(
                        UnityEngine.Random.Range(transform.position.x, m_startPos.x + (-m_maxPerimiterPatrol)),// ( X )
                        transform.position.y);  // ( Y )

                break;
            case Direction.right:
                newPos = new Vector3(
                        UnityEngine.Random.Range(transform.position.x, m_startPos.x + m_maxPerimiterPatrol),// ( X )
                        transform.position.y);  // ( Y )

                break;
        }

        //print(">>>> >>>> " + newDir.ToString().ToUpper() + " : " + newPos);


        return newPos;
    }

    public void OnIdlePatrol()
    {
        //if (m_attackCO != null) StopCoroutine(m_attackCO);
        m_canAttack = false;
        m_idleCO = StartCoroutine(OnIdlePatrol_Coroutine());
        if (m_PersueCO != null) StopCoroutine(m_PersueCO);
        if (m_attackAlign != null) StopCoroutine(m_attackAlign);
    }
    public void Persue()
    {
        //if (m_attackCO != null) StopCoroutine(m_attackCO);
        m_canAttack = false;
        m_PersueCO = StartCoroutine(Persue_Coroutine());
        if (m_idleCO != null) StopCoroutine(m_idleCO);
        if (m_attackAlign != null) StopCoroutine(m_attackAlign);
    }
    public void OnAttack()
    {
        m_canAttack = true;
        //if (m_attackCO != null) StopCoroutine(m_attackCO);
        if (m_attackCO == null)
        {
            m_attackCO = StartCoroutine(OnAttack_coroutine());
        }
        m_attackAlign = StartCoroutine(AttackAlign_Coroutine());
        if (m_PersueCO != null) StopCoroutine(m_PersueCO);
        if (m_idleCO != null) StopCoroutine(m_idleCO);
    }


    private IEnumerator OnIdlePatrol_Coroutine()
    {

        while (true)
        {

            Vector3 newPos = DefineNewPath();


            bool isMovementFinished = false;
            while (!isMovementFinished)
            {

                transform.position = Vector2.MoveTowards(transform.position, newPos, m_enemySensor.Speed);

                yield return new WaitForSeconds(GlobalVariables.FRAME_HATE_COROUTINE);

                isMovementFinished = transform.position == newPos || m_newDir.isWallColliding;

            }

            //yield return new WaitForSeconds(GlobalVariables.FRAME_HATE_COROUTINE);
            yield return new WaitForEndOfFrame();
        }

    }

    private IEnumerator Persue_Coroutine()
    {
        for (; ; )
        {
            transform.position = Vector2.MoveTowards(transform.position, m_enemySensor.PlayerObj.position, m_enemySensor.Speed);

            yield return new WaitForSeconds(GlobalVariables.FRAME_HATE_COROUTINE);
        }


    }

    private IEnumerator OnAttack_coroutine()
    {
        while (m_canAttack)
        {



            /*
            if (!GetComponent<Health>().IsDamaged)
            {
                yield return new WaitForSeconds(m_attackDelay);

                m_gunAnimator.SetTrigger(NAME_ATTACK_ANIM);

                yield return new WaitUntil(() => m_gunAnimator.GetCurrentAnimatorStateInfo(0).IsName(NAME_ATTACK_ANIM));

                m_swardCollider.SetAttack(m_enemySensor.CharID.GunType);

                yield return new WaitUntil(() => !m_gunAnimator.GetCurrentAnimatorStateInfo(0).IsName(NAME_ATTACK_ANIM));

                m_swardCollider.FinishAttack();
            }
            */

            yield return new WaitForSeconds(m_attackFrequency);
            
        }

        m_attackCO = null;
    }

    private IEnumerator AttackAlign_Coroutine()
    {
        while (true)
        {

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, m_enemySensor.PlayerObj.position.y), m_enemySensor.Speed);

            yield return new WaitForSeconds(GlobalVariables.FRAME_HATE_COROUTINE);
        }
    }
}
