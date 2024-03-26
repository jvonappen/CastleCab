using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyInfo m_info;
    public void SetInfo(EnemyInfo _info) => m_info = _info;

}
