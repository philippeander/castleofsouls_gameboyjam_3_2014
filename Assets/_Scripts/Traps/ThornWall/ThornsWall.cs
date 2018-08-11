using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThornsWall : Trap {
    [Serializable]
    public class SpawnerObject {
        public Transform spownerPoint;
        [NonSerialized]public ThornBullet bullet;
    }

    [Space(15)]
    [SerializeField] private Direction m_direction;

    [Space(10)]
    [SerializeField]private GameObject m_thornBulletPrefab;
    [SerializeField]private float m_bulletSpeed = 5f;
    [SerializeField]private float m_bulletLifeTime = 0.5f;
    [SerializeField]private float m_delayBetweenShots = 0.15f;

    [Space(15)]
    [SerializeField]private SpawnerObject[] m_spawnerObjectArray;

    private Coroutine m_coroutine;
    
    void Start() {
        for (sbyte i = 0; i < m_spawnerObjectArray.Length; i++) {
            GameObject newThorn = Instantiate(m_thornBulletPrefab, m_spawnerObjectArray[i].spownerPoint) as GameObject;
            ThornBullet bullet = newThorn.GetComponent<ThornBullet>();
            bullet.Init(m_direction, m_bulletSpeed);
            m_spawnerObjectArray[i].bullet = bullet;
            m_spawnerObjectArray[i].bullet.gameObject.SetActive(false);
        }
	}

    /*//TEST
    private void Update() {
        if (Input.GetKeyUp(KeyCode.J)) {
            ActiveTrap(true);
        } else if (Input.GetKeyUp(KeyCode.K)) {
            ActiveTrap(false);
        }
    }*/

    public void LocalTrapScript() {
        if (m_coroutine != null) StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(LocalTrapScript_Coroutine());
    }

    private IEnumerator LocalTrapScript_Coroutine() {
        for (sbyte i = 0; i < m_spawnerObjectArray.Length; i++) {
            if (i == (m_spawnerObjectArray.Length/2)) { //interval every two thrown
                yield return new WaitForSeconds(m_delayBetweenShots);
            }
            m_spawnerObjectArray[i].bullet.gameObject.SetActive(true);
            m_spawnerObjectArray[i].bullet.transform.position = m_spawnerObjectArray[i].spownerPoint.position;
        }

        yield return new WaitForSeconds(m_bulletLifeTime);

        for (sbyte i = 0; i < m_spawnerObjectArray.Length; i++) {
            if (i == (m_spawnerObjectArray.Length/2)) { //interval every two thrown
                yield return new WaitForSeconds(m_delayBetweenShots);
            }
            m_spawnerObjectArray[i].bullet.gameObject.SetActive(false);
        }
    }
    
}
