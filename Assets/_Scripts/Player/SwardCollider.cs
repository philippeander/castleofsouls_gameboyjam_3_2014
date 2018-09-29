/*
 * Este scripts é usado no gatilho, que identifica a colisão do ataque com o inimigo
 * Eles está localizado em:
 * Player/ColliderAttack/Coll_*
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwardCollider : MonoBehaviour {
    
    private GunType m_gunType = new GunType();
    private Collider2D m_Coll;
    private string m_enemyTag = "";
    private GameObject m_fatherObj;
    

    public GunType Sward_Type {
        get {
            return m_gunType;
        }

        set {
            m_gunType = value;
        }
    }
    public string EnemyTag {
        get {
            return m_enemyTag;
        }

        set {
            m_enemyTag = value;
        }
    }

    private void Awake() {
        m_Coll = GetComponent<Collider2D>(); 
    }

    void Start () {
        m_Coll.enabled = false;
	}

    public void Init(GameObject fatherObj, string enemyTag) {
        m_enemyTag = enemyTag;
        m_fatherObj = fatherObj;
    }
	
    public void SetAttack(GunType type) {
        m_gunType = type;
        m_Coll.enabled = true;
    }

    public void FinishAttack() {
        m_Coll.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if(collision.tag == m_enemyTag) {            
            Health health = collision.GetComponent<Health>();
            if (health != null) {
                health.OnDamage(1, 
                                m_fatherObj.transform.position, 
                                m_gunType);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        
    }
}
