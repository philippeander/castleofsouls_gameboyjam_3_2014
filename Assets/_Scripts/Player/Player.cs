using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    static public Player Instance;

    [SerializeField] private float m_Speed = 2f;
    [SerializeField] private float m_RefreshRate = 0.02f;

    [Space(15)]
    [SerializeField] private SwardCollider m_CollAttackTop;
    [SerializeField] private SwardCollider m_CollAttackDown;
    [SerializeField] private SwardCollider m_CollAttackLeft;
    [SerializeField] private SwardCollider m_CollAttackRight;
    
    private bool m_IsWalking = false;
    private Vector2 m_Axis = new Vector2();
    private Vector2 m_LestAxisMoviment = new Vector2();
    private Animator m_Anim;
    private SwardCollider m_CurColliderAttack;
    private bool m_IsAttack;

    private Coroutine m_CurCoroutineMove;

    private void Awake() {
        Instance = this;
        m_Anim = GetComponent<Animator>();
    }
    void Start () {
        m_IsAttack = false;

        InputManager.Instance.ArrowKeysAction += PlayerMove;
        InputManager.Instance.A_BtnAction += A_Btn;
        InputManager.Instance.B_BtnAction += B_Btn;
        
    }
    private void Update() {
        AnimManager();
    }

    #region PLAYER_MOVEMENT
    public void PlayerMove(Vector2 axis) {
        m_Axis = axis;
        if (m_Axis != Vector2.zero) { m_LestAxisMoviment = m_Axis; }
        PlayerMove_StopCoroutine();
        m_CurCoroutineMove = StartCoroutine(PlayerMove_Coroutine());
    }
    private IEnumerator PlayerMove_Coroutine() {
        if(!m_IsWalking )m_IsWalking = true;
        while (m_Axis != Vector2.zero) {
            Vector2 moviment = m_Axis;
            transform.Translate(moviment * m_Speed * Time.deltaTime, Space.World);
            yield return new WaitForSeconds(m_RefreshRate);
        }
        PlayerMove_StopCoroutine();
    }
    private void PlayerMove_StopCoroutine() {
        if (m_IsWalking) m_IsWalking = false;
        if (m_CurCoroutineMove != null) StopCoroutine(m_CurCoroutineMove);
    }
    #endregion

    #region ATTACK_DAMAGE
    public void A_Btn() {
        if (!m_IsAttack) {
            m_IsAttack = true;
            m_Anim.SetTrigger("Attack_White");
        }
    }
    public void B_Btn() {
        if (!m_IsAttack) {
            m_IsAttack = true;
            m_Anim.SetTrigger("Attack_black");
        }
    }

    //Métodos responsáveis pelos danos do ataque.
    //São chamados em eventos de animação
    public void ActiveSwardCollider_White() {
        m_CurColliderAttack = SelectCollider();
        m_CurColliderAttack.SetAttack(SwardType.white);
    }
    public void ActiveSwardCollider_Black() {
        m_CurColliderAttack = SelectCollider();
        m_CurColliderAttack.SetAttack(SwardType.black);
    }
    public void DeactiveSwardCollider() {
        m_IsAttack = false;
        m_CurColliderAttack.FinishAttack();
    }

    private SwardCollider SelectCollider() {
        SwardCollider coll = null;
        if (m_LestAxisMoviment.y > 0) {         //TOP
            coll = m_CollAttackTop;
        }else if (m_LestAxisMoviment.y < 0) {   //DOWN
            coll = m_CollAttackDown;
        } else if (m_LestAxisMoviment.x < 0) {  //LEFT
            coll = m_CollAttackLeft;
        } else if (m_LestAxisMoviment.x > 0) {  //RIGHT
            coll = m_CollAttackRight;
        } else {
            Debug.LogError(">>> >>> Collider is not found!");
        }
        return coll;
    }
    #endregion

    

    public void AnimManager() {
        m_Anim.SetBool("IsWalking", m_IsWalking);

        if (m_IsWalking) {
            m_Anim.SetFloat("X_Axis", m_Axis.x);
            m_Anim.SetFloat("Y_Axis", m_Axis.y);
        } else {
            m_Anim.SetFloat("X_LastMov", m_LestAxisMoviment.x);
            m_Anim.SetFloat("Y_LastMov", m_LestAxisMoviment.y);
        }

        
    }

    

}
