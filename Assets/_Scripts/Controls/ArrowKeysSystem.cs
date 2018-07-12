/*
 * This system allows the player to use the arrow keys without removing the finger from the button
 * Ele pode ser encontrado nos componentes em:
 * Canvas/Pnl_Bottons/Arrows/Btn_Arrow_*
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ArrowKeysSystem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {

    [SerializeField] private UnityEvent m_OnPointerDown;
    [SerializeField] private UnityEvent m_OnPointerEnter;
    [SerializeField] private UnityEvent m_OnPointerExit;
    

    public void OnPointerDown(PointerEventData eventData) {
        if (m_OnPointerDown != null) {
            if (!InputManager.Instance.IsArrowKeysClicked) InputManager.Instance.IsArrowKeysClicked = true;
            m_OnPointerDown.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (InputManager.Instance.IsArrowKeysClicked) {
            if (m_OnPointerEnter != null) {
                m_OnPointerEnter.Invoke();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (InputManager.Instance.IsArrowKeysClicked) InputManager.Instance.IsArrowKeysClicked = false;
        if (m_OnPointerExit != null) {
            m_OnPointerExit.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        //if (InputManager.Instance.IsArrowKeysClicked) InputManager.Instance.IsArrowKeysClicked = false;
        //if (m_OnPointerExit != null) {
        //    m_OnPointerExit.Invoke();
        //}
    }

    
    

}
