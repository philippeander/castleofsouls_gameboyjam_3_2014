using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySensor : MonoBehaviour {
    
    [SerializeField] private float m_speed = 0.02f;
    [SerializeField] private float m_SensorRadius = 3f;
    [SerializeField] private float m_AttackRadius = 0.6f;
    [SerializeField] private float m_RayDistance = 1f;
    [SerializeField] private LayerMask m_obstaclesLayerMask;

    private CharID m_charID;
    private Rigidbody2D rb2D;
    private List<RaycastDirection> m_hit = new List<RaycastDirection>();
    private Transform m_playerObj;
    private EnemyState m_curState = EnemyState.attack;
    private EnemyState m_HistState = EnemyState.idle;
    private bool m_libRayCast = false;
    private Vector3 m_prevLoc = Vector3.zero;

    private const int NUM_OF_RAYCASTS = 4;

    public float Speed {
        get {
            return m_speed;
        }

        set {
            m_speed = value;
        }
    }
    public float SensorRadius {
        get {
            return m_SensorRadius;
        }

        set {
            m_SensorRadius = value;
        }
    }
    public float AttackRadius {
        get {
            return m_AttackRadius;
        }

        set {
            m_AttackRadius = value;
        }
    }
    public float RayDistance {
        get {
            return m_RayDistance;
        }

        set {
            m_RayDistance = value;
        }
    }
    public LayerMask ObstaclesLayerMask {
        get {
            return m_obstaclesLayerMask;
        }

        set {
            m_obstaclesLayerMask = value;
        }
    }

    public CharID CharID {
        get {
            return m_charID;
        }

        set {
            m_charID = value;
        }
    }
    public Rigidbody2D Rb2D {
        get {
            return rb2D;
        }

        set {
            rb2D = value;
        }
    }
    public List<RaycastDirection> Hit {
        get {
            return m_hit;
        }

        set {
            m_hit = value;
        }
    }
    public Transform PlayerObj1 {
        get {
            return m_playerObj;
        }

        set {
            m_playerObj = value;
        }
    }
    public EnemyState CurState {
        get {
            return m_curState;
        }

        set {
            m_curState = value;
        }
    }
    public EnemyState HistState {
        get {
            return m_HistState;
        }

        set {
            m_HistState = value;
        }
    }
    public bool LibRayCast {
        get {
            return m_libRayCast;
        }

        set {
            m_libRayCast = value;
        }
    }
    public Vector3 PrevLoc {
        get {
            return m_prevLoc;
        }

        set {
            m_prevLoc = value;
        }
    }
    
    public bool IsPlayerSeen {
        get {
            return Vector2.Distance(this.transform.position, PlayerObj.position) < SensorRadius;
        }
    }
    public bool IsPlayerAttackArea {
        get {
            return Vector2.Distance(this.transform.position, PlayerObj.position) < AttackRadius;
        }
    }
    public Vector2 PlayerDistance {
        get {
            return this.transform.position - PlayerObj.position;
        }
    }
    public Transform PlayerObj {
        get {
            return PlayerObj1;
        }

    }
    
    protected virtual void Awake()
    {
        CharID = GetComponent<CharID>();
        Rb2D = GetComponent<Rigidbody2D>();
        PlayerObj1 = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        InitRaicastDirections();
    }

    private void InitRaicastDirections()
    {

        Hit.Add(new RaycastDirection(Direction.up));
        Hit.Add(new RaycastDirection(Direction.down));
        Hit.Add(new RaycastDirection(Direction.left));
        Hit.Add(new RaycastDirection(Direction.right));

        LibRayCast = true;

    }
    
    void FixedUpdate()
    {
        DrawRay();
    }

    void DrawRay()
    {

        if (!LibRayCast) return;
        //TODO: Limitar para rodar apenas quando estiver proximo do player

        for (int i = 0; i < Hit.Count; i++)
        {

            Hit[i].raycast = Physics2D.Raycast(transform.position, Hit[i].vector, RayDistance, ObstaclesLayerMask);

            Debug.DrawRay(transform.position, Hit[i].vector * RayDistance, Color.cyan);

            Hit[i].isWallColliding = Hit[i].raycast.collider != null ? true : false;


        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SensorRadius);
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
