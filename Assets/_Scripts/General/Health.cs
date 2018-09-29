using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

    
    [SerializeField] private float m_MaxHealth = 3;
    [SerializeField] private bool m_IsShakeCam = false;
    [SerializeField] private bool m_IsInvencible = false;

    [Space(15)]
    [Header("DAMAGE")]
    [SerializeField] private float m_TimeFlashDamage = .1f;
    [SerializeField] private float m_FrameHateDamage = .05f;
    [SerializeField] private Color m_MaxColorDamage = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color m_MinColorDamage = new Color(1f, 1f, 1f, 0f);
    [SerializeField] private bool m_isDamageDeslocation = false;
    [SerializeField] private float m_speedDemageDesl = 2f;
    [SerializeField] private float m_DamageDeslocatonTime = 0.5f;
    [SerializeField] private float m_DamageRecoveringTime = 1f;

    [Space(15)]
    [Header("ACTIONS")]
    [SerializeField] private UnityEvent OnReceiveDamage;

    private CharID m_charID;
    private float m_Health = 0;
    private Vector3 m_lastAttackDirection = Vector2.zero;
    private bool m_isDamaged = false;
    private Coroutine m_isDamageCoroutine;
    private Rigidbody2D m_rBody;

    private Coroutine m_FlashDamageCoroutine;

    public bool IsDamaged {
        get {
            return m_isDamaged;
        }

        set {
            m_isDamaged = value;
        }
    }

    private void Awake()
    {
        m_charID = GetComponent<CharID>();
        m_rBody = GetComponent<Rigidbody2D>();
    }

    void Start () {
        m_Health = m_MaxHealth;
	}
    

    public void OnDamage(float amount, Vector3 attackDirection, GunType attackType) {
        m_lastAttackDirection = attackDirection;
        if (m_IsShakeCam) ScreenShake.Instance.Shake();
        if (m_IsInvencible) return;

        

        if (m_charID.WeakAgaintGunType == attackType || m_charID.WeakAgaintGunType == GunType.both)
        {
            m_Health -= amount;

            if (m_FlashDamageCoroutine != null)
            {
                StopCoroutine(m_FlashDamageCoroutine);
            }
            if (m_isDamageDeslocation)
            {
                if (m_isDamageCoroutine != null) StopCoroutine(DamageDeslocation());
                m_isDamageCoroutine = StartCoroutine( DamageDeslocation());
            }
            m_FlashDamageCoroutine = StartCoroutine(FlashDamage());
        }

        if (m_Health <= 0) {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator FlashDamage() {
        
        SpriteRenderer[] renderer = GetComponentsInChildren<SpriteRenderer>();

        float timer = 0f;
        while (timer < m_TimeFlashDamage) {
            for (int i = 0; i < renderer.Length; i++) {
                renderer[i].color = m_MinColorDamage;
            }

            yield return new WaitForSeconds(m_FrameHateDamage);

            for (int i = 0; i < renderer.Length; i++) {
                renderer[i].color = m_MaxColorDamage;
            }

            yield return new WaitForSeconds(m_FrameHateDamage);

            timer += Time.unscaledDeltaTime; //count up using unscaled time.
            
        }
        
    }

    private IEnumerator DamageDeslocation()
    {
        IsDamaged = true;
        print("DD");
        Vector2 dir = m_lastAttackDirection - transform.position;
        dir = -dir.normalized;
        
        m_rBody.velocity = dir * m_speedDemageDesl;

        yield return new WaitForSeconds(m_DamageDeslocatonTime);

        m_rBody.velocity = Vector2.zero;

        yield return new WaitForSeconds(m_DamageRecoveringTime);

        IsDamaged = false;

    }
}
