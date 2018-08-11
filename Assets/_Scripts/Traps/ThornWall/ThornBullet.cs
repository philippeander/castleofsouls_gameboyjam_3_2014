using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornBullet : MonoBehaviour {

    [SerializeField] private Direction m_direction;

    private float m_Speed = 5f;
    private Rigidbody2D m_rBody;
    private float m_movementframeHate = 0.02f;

    private Coroutine m_moveCoroutine;

    private Vector2 m_moveDir;
    

    private void Awake() {
        m_rBody = GetComponent<Rigidbody2D>();
    }
    
    public void Init(Direction direction, float speed) {
        m_Speed = speed;
        m_direction = direction;

        float null_Axis = 0;
        switch (m_direction) {
            case Direction.up:
                m_moveDir = new Vector2(null_Axis, m_Speed);
                break;
            case Direction.down:
                m_moveDir = new Vector2(null_Axis, -m_Speed);
                break;
            case Direction.left:
                m_moveDir = new Vector2(-m_Speed, null_Axis);
                break;
            case Direction.right:
                m_moveDir = new Vector2(m_Speed, null_Axis);
                break;
        }
    }

    private void OnEnable() {
        m_moveCoroutine = StartCoroutine(Move_Coroutine());
    }

    private void OnDisable() {
        StopCoroutine(m_moveCoroutine);
    }

    private IEnumerator Move_Coroutine() {
        while (true) {
            m_rBody.velocity = m_moveDir;
            yield return new WaitForSeconds(m_movementframeHate);
        }
    }

}
