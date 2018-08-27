using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] private float m_Health = 3;
    [SerializeField] private float m_MaxHealth = 3;
    [SerializeField] private bool m_IsShakeCam = false;
    [SerializeField] private bool m_IsInvencible = false;

    [Space(15)]
    [Header("DAMAGE")]
    [SerializeField] private float m_TimeFlashDamage = .1f;
    [SerializeField] private float m_FrameHateDamage = .05f;
    [SerializeField] private Color m_MaxColorDamage = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color m_MinColorDamage = new Color(1f, 1f, 1f, 0f);

    private CharID m_charID;

    private Coroutine m_FlashDamageCoroutine;

    private void Awake()
    {
        m_charID = GetComponent<CharID>();
    }

    void Start () {
		
	}
    

    public void OnDamage(float amount, GunType attackType) {
        if (m_IsShakeCam) ScreenShake.Instance.Shake();
        if (m_IsInvencible) return;

        if (m_charID.WeakAgaintGunType == attackType || m_charID.WeakAgaintGunType == GunType.both) {
            m_Health -= amount;
            
            if (m_FlashDamageCoroutine != null) {
                StopCoroutine(m_FlashDamageCoroutine);
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
}
