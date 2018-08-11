using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trap : MonoBehaviour {

    [SerializeField]private float m_animFrequency = 1;
    [SerializeField]private float m_animVelocity = 1;

    [Space(10)]
    [Header("Animation")]
    [SerializeField]private bool m_isWaitCicleAnimation = false;
    [SerializeField] private string m_animationName = "Cicle";
    [SerializeField] private string m_animationParamiter = "SetCicle";

    [Space(10)]
    [Header("Action")]
    public UnityEvent OnTrapCicleInit;

    public UnityEvent OnTrapCicleFinish;

    public UnityEvent OnTrapCoroutineFinished;


    private Animator m_anim;
    private bool m_isTrapActive = false;

    private Coroutine m_coroutine;

    public float AnimFrequency {
        get {
            return m_animFrequency;
        }

        set {
            m_animFrequency = value;
        }
    }
    public float AnimVelocity {
        get {
            return m_animVelocity;
        }

        set {
            m_animVelocity = value;
        }
    }

    private void Awake() {
        if (GetComponent<Animator>()) {
            m_anim = GetComponent<Animator>();
        }
    }
    
    public void ActiveTrap(bool active) {
        if (!m_isTrapActive && active) {
            m_isTrapActive = true;
            m_coroutine = StartCoroutine(ActiveTrap_Coroutine());
        } else if (m_isTrapActive && !active) {
            m_isTrapActive = false;
        }
    }

    private IEnumerator ActiveTrap_Coroutine() {
        
        while (m_isTrapActive) {
            OnTrapCicleInit.Invoke();

            if (m_isWaitCicleAnimation) {
                m_anim.speed = m_animVelocity;
                m_anim.SetTrigger(m_animationParamiter);

                yield return new WaitUntil(() => m_anim.GetCurrentAnimatorStateInfo(0).IsName(m_animationName));
                yield return new WaitUntil(() => !m_anim.GetCurrentAnimatorStateInfo(0).IsName(m_animationName));
            }

            OnTrapCicleFinish.Invoke();
            yield return new WaitForSeconds(m_animFrequency);

        }

        OnTrapCoroutineFinished.Invoke();

        if (m_coroutine != null) {
            StopCoroutine(m_coroutine);
        }
    }
}
