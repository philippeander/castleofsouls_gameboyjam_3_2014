using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayer : MonoBehaviour {

    [SerializeField] private SpriteRenderer[] m_spritesRenderer;

	// Use this for initialization
	void Start () {
        m_spritesRenderer = GetComponentsInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < m_spritesRenderer.Length; i++)
        {
            m_spritesRenderer[i].sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1; ;
        }

	}
}
