using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BulletControl : MonoBehaviour
{
    public float speed = 30;
    public GameObject effectPrefab;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var go = Instantiate(effectPrefab, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
        Destroy(go, 1f);
        Destroy(gameObject);
        
        //打到敌人
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyControl>().GetHit(2);
        }

        #region 击中特效
        DestructibleSimple dest = collision.collider.GetComponent<DestructibleSimple>();
        if (dest != null)
        {
            dest.OnHit(collision.contacts[0].point);
        }

        Destroy(gameObject);
        #endregion
    }
}
