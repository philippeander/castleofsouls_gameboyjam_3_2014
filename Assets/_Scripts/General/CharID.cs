using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharID : MonoBehaviour {

    [SerializeField] [TagSelector] private string m_EnemyTag = "Enemy";
    [SerializeField] private GunType m_gunType;
    [SerializeField] private GunType m_weakAgaintGunType;

    public string EnemyTag {
        get {
            return m_EnemyTag;
        }
    }
    public GunType GunType {
        get {
            return m_gunType;
        }
    }
    public GunType WeakAgaintGunType {
        get {
            return m_weakAgaintGunType;
        }
    }
}
