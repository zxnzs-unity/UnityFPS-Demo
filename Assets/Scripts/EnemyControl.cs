using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyControl : MonoBehaviour
{
    public int EnemyHp = 10;
    public GameObject BmboEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetHit(int damage)
    {
        EnemyHp -= damage;
        if(EnemyHp <= 0)
        {
            //±¬Õ¨
            Instantiate(BmboEffect,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
}
