using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    //public float floatHeight;
    //public float liftForce;
    //public float damping;
    [SerializeField] private float m_SensorRadius = 3f;
    [SerializeField] private float m_AttackRadius = 0.6f;
    [SerializeField] private float m_RayDistance = 1f;
    [SerializeField] private LayerMask m_WallLayerMask;
    private Rigidbody2D rb2D;
    private RaycastHit2D[] m_hit;
    private Transform m_PlayerObj;

    private const int NUM_OF_RAYCASTS = 4;

    public bool IsPlayerSeen {
        get {
            return Vector2.Distance(this.transform.position, m_PlayerObj.position) < m_SensorRadius;
        }
    }
    public bool IsPlayerAttackArea {
        get {
            return Vector2.Distance(this.transform.position, m_PlayerObj.position) < m_AttackRadius;
        }
    }
    public Vector2 PlayerDistance {
        get {
            return this.transform.position - m_PlayerObj.position;
        }
    }
    
    protected virtual void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        m_PlayerObj = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start() {
        m_hit = new RaycastHit2D[NUM_OF_RAYCASTS];
    }

    void FixedUpdate() {
        //TODO: Limitar para rodar apenas quando estiver proximo do player
        
        m_hit[0] = Physics2D.Raycast(transform.position, Vector2.up, m_RayDistance, m_WallLayerMask);
        m_hit[1] = Physics2D.Raycast(transform.position, Vector2.down, m_RayDistance, m_WallLayerMask);
        m_hit[2] = Physics2D.Raycast(transform.position, Vector2.left, m_RayDistance, m_WallLayerMask);
        m_hit[3] = Physics2D.Raycast(transform.position, Vector2.right, m_RayDistance, m_WallLayerMask);

        Debug.DrawRay(transform.position, Vector2.up * m_RayDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * m_RayDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * m_RayDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * m_RayDistance, Color.red);

        for (int i = 0; i < m_hit.Length; i++) {
            if (m_hit[i].collider != null) {
                //print("OBJ: " + hit[i].transform.name.ToUpper());
            }
        }

        if (IsPlayerSeen) {
            //Debug.Log(">>> >>> THE PLAYER HAS BEEN SEEN!");

        }

        //if (hit.collider != null) {
        //    print("GG: "+ hit.transform.name.ToUpper());
        //    float distance = Mathf.Abs(hit.point.y - transform.position.y);
        //    float heightError = floatHeight - distance;
        //    float force = liftForce * heightError - rb2D.velocity.y * damping;
        //    rb2D.AddForce(Vector3.up * force);
        //}
    }
    
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_SensorRadius);
        Gizmos.DrawWireSphere(transform.position, m_AttackRadius);
    }

}
