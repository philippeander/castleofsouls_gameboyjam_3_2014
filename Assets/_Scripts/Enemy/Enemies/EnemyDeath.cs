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
    [SerializeField] private float m_perimiterPatrol = 10;
    private Direction m_curPatrolDir;
    private Vector3 m_startPos;
    private float m_curPerimiterPatrol = 0;
    
    
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

    /*
    private void Update() {
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

    }
    */

    public void OnIdlePatrol() {
        m_idleCO = StartCoroutine(OnIdlePatrol_Coroutine());
    }

    private IEnumerator OnIdlePatrol_Coroutine() {
        
        while (true) {

            m_curPatrolDir = (Direction)UnityEngine.Random.Range(0, (int)Enum.GetValues(typeof(Direction)).Length - 1);
            m_curPerimiterPatrol = UnityEngine.Random.Range(0f, m_curPerimiterPatrol);

            while (true) {

            }


            yield return new WaitForSeconds(GlobalVariables.FRAME_HATE_COROUTINE);
        }

    }

    public void ChasePlayer() {
        m_chasePlayerCO = StartCoroutine(ChasePlayer_Coroutine());
    }

    private IEnumerator ChasePlayer_Coroutine() {
        yield return null;
    }
}