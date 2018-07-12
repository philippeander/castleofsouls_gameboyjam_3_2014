/*
 * Este scripts é usado no gatilho, que identifica a colisão do ataque com o inimigo
 * Eles está localizado em:
 * Player/ColliderAttack/Coll_*
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwardCollider : MonoBehaviour {
    
    private SwardType m_SwardType; 
    private Collider2D m_Coll;

    public SwardType Sward_Type {
        get {
            return m_SwardType;
        }

        set {
            m_SwardType = value;
        }
    }

    private void Awake() {
        m_Coll = GetComponent<Collider2D>(); 
    }

    void Start () {
        m_SwardType = SwardType.white; 
        m_Coll.enabled = false;
	}
	
    public void SetAttack(SwardType type) {
        m_SwardType = type;
        m_Coll.enabled = true;
    }

    public void FinishAttack() {
        m_Coll.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Enemy") {
            Health health = collision.GetComponent<Health>();
            health.OnDamage(1, m_SwardType);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        
    }
}
