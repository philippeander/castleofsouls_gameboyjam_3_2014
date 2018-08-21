using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyDeath : EnemyAI {
    

    [Space(15)]
    [SerializeField] private Animator m_gunAnimator;
    [SerializeField] private Animator m_avatarAnimator;

    [Space(15)]
    [Header("PARAMITERS")]
    [SerializeField] [TagSelector] private string m_EnemyTag;
    [SerializeField] private float m_attackField = 0.6f;

    [Space(15)]
    [Header("PATROL")]
    [SerializeField] private float m_minPrimiterPatrol = 0.1f;
    [SerializeField] private float m_maxPerimiterPatrol = 2;
    private Direction m_curPatrolDir = Direction.down;
    private Vector3 m_startPos = Vector3.zero;
    private float m_curPerimiterPatrol = 0;
    private RaycastDirection m_lastDirectionSelect = new RaycastDirection(Direction.down);
    private RaycastDirection m_newDir = new RaycastDirection(Direction.down);


    private SwardCollider m_swardCollider;
    private Coroutine m_idleCO;
    private Coroutine m_chasePlayerCO;

    private const string NAME_ATTACK_ANIM = "Attack";

    protected override void Awake() {
        base.Awake();
        m_swardCollider = GetComponentInChildren<SwardCollider>();
        m_swardCollider.Init(m_EnemyTag);
        
    }

    protected override void Start () {
        base.Start();
        m_startPos = transform.position;
        OnIdlePatrol();
        

    }

    
    private void Update() {
        /*
        if (IsPlayerAttackArea && 
            PlayerDistance.y < m_attackField && 
            PlayerDistance.y > -m_attackField &&
            !m_gunAnimator.GetCurrentAnimatorStateInfo(0).IsName(NAME_ATTACK_ANIM)) 
        {
            m_gunAnimator.SetTrigger(NAME_ATTACK_ANIM);
            m_swardCollider.SetAttack(SwardType.both);
        }
            
        if (!m_gunAnimator.GetCurrentAnimatorStateInfo(0).IsName(NAME_ATTACK_ANIM)) {
            m_swardCollider.FinishAttack();
        }

        if (IsPlayerSeen) {
            bool IsPlayerInMyLeftside = PlayerDistance.x > 0;
            transform.localScale = new Vector2( IsPlayerInMyLeftside ? 1 : -1 , transform.localScale.y);
            
        }
        */

        if (Input.GetKeyUp(KeyCode.N)) {
            DefineNewPath();
        }

    }
    

    public void OnIdlePatrol() {
        m_idleCO = StartCoroutine(OnIdlePatrol_Coroutine());
    }

    private IEnumerator OnIdlePatrol_Coroutine() {
        
        while (true) {
            
            Vector3 newPos = DefineNewPath();

            bool isMovementFinished = false;
            while (!isMovementFinished) {

                transform.position = Vector2.MoveTowards(transform.position, newPos, m_speed);

                yield return new WaitForSeconds(GlobalVariables.FRAME_HATE_COROUTINE);

                isMovementFinished = transform.position == newPos || m_newDir.isWallColliding;

            }
            
            //yield return new WaitForSeconds(GlobalVariables.FRAME_HATE_COROUTINE);
            yield return new WaitForEndOfFrame();
        }

    }

    private Vector2 DefineNewPath() {
        Vector3 newPos = m_startPos;

        List<RaycastDirection> libDirList = new List<RaycastDirection>();
        
        
        for (int i = 0; i < m_hit.Count; i++)
        {
            if (!m_hit[i].isWallColliding && m_hit[i].direction != m_lastDirectionSelect.direction) {
                libDirList.Add(m_hit[i]);
            }
        }
        
        if (libDirList.Count > 0)
        {
            m_newDir = libDirList[UnityEngine.Random.Range(0, libDirList.Count)];
        }
        m_lastDirectionSelect = m_newDir;
                
        switch (m_newDir.direction) {
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

                transform.localScale = new Vector2(1, transform.localScale.y);
                break;
            case Direction.right:
                newPos = new Vector3(
                        UnityEngine.Random.Range(transform.position.x, m_startPos.x + m_maxPerimiterPatrol),// ( X )
                        transform.position.y);  // ( Y )

                transform.localScale = new Vector2(-1, transform.localScale.y);
                break;
        }

        //print(">>>> >>>> " + newDir.ToString().ToUpper() + " : " + newPos);


        return newPos;
    }

    public void ChasePlayer() {
        m_chasePlayerCO = StartCoroutine(ChasePlayer_Coroutine());
    }

    public void MirrorImage() {

        Vector3 curVel = (transform.position - m_prevLoc) / Time.deltaTime;

        if (curVel.y > 0)
        {
            // it's moving up
        }
        else
        {
            // it's moving down
        }
        m_prevLoc = transform.position;

    }

    private IEnumerator ChasePlayer_Coroutine() {
        yield return null;
    }
}