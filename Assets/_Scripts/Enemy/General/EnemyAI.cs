using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour {

    [Serializable]
    public class RaycastDirection {
        public Direction direction;
        [NonSerialized]public Vector2 vector;
        public RaycastHit2D raycast;
        public bool isWallColliding = false;

        public RaycastDirection(Direction direction)
        {
            this.direction = direction;

            switch (this.direction) {
                case Direction.up:
                    vector = Vector2.up;
                    break;
                case Direction.down:
                    vector = Vector2.down;
                    break;
                case Direction.left:
                    vector = Vector2.left;
                    break;
                case Direction.right:
                    vector = Vector2.right;
                    break;
            }
        }
    }
    
    [SerializeField] protected float m_speed = 0.02f;
    [SerializeField] protected float m_SensorRadius = 3f;
    [SerializeField] protected float m_AttackRadius = 0.6f;
    [SerializeField] protected float m_RayDistance = 1f;
    [SerializeField] protected LayerMask m_WallLayerMask;

    protected CharID m_charID;
    protected Rigidbody2D rb2D;
    protected List<RaycastDirection> m_hit = new List<RaycastDirection>();
    private Transform m_playerObj;
    protected EnemyState m_curState = EnemyState.attack;
    protected EnemyState m_HistState = EnemyState.idle;
    private bool m_libRayCast = false;
    protected Vector3 m_prevLoc = Vector3.zero;

    private const int NUM_OF_RAYCASTS = 4;

    public bool IsPlayerSeen {
        get {
            return Vector2.Distance(this.transform.position, PlayerObj.position) < m_SensorRadius;
        }
    }
    public bool IsPlayerAttackArea {
        get {
            return Vector2.Distance(this.transform.position, PlayerObj.position) < m_AttackRadius;
        }
    }
    public Vector2 PlayerDistance {
        get {
            return this.transform.position - PlayerObj.position;
        }
    }
    public Transform PlayerObj {
        get {
            return m_playerObj;
        }
        
    }

    protected virtual void Awake() {
        m_charID = GetComponent<CharID>();
        rb2D = GetComponent<Rigidbody2D>();
        m_playerObj = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start() {
        InitRaicastDirections();
    }

    private void InitRaicastDirections() {

        m_hit.Add(new RaycastDirection(Direction.up));
        m_hit.Add(new RaycastDirection(Direction.down));
        m_hit.Add(new RaycastDirection(Direction.left));
        m_hit.Add(new RaycastDirection(Direction.right));
        
        m_libRayCast = true;
            
    }
    

    void FixedUpdate()
    {
        DrawRay();
    }

    void DrawRay() {

        if (!m_libRayCast) return;
        //TODO: Limitar para rodar apenas quando estiver proximo do player

        for (int i = 0; i < m_hit.Count; i++) {

            m_hit[i].raycast = Physics2D.Raycast(transform.position, m_hit[i].vector, m_RayDistance, m_WallLayerMask);

            Debug.DrawRay(transform.position, m_hit[i].vector * m_RayDistance, Color.cyan);

            m_hit[i].isWallColliding = m_hit[i].raycast.collider != null ? true : false;


        }
        
    }
    
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_SensorRadius);
        Gizmos.DrawWireSphere(transform.position, m_AttackRadius);
    }

}
