using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WeaponControl : MonoBehaviour
{
    //发射位置
    public GameObject FirePoint;
    //子弹
    public GameObject BulletPre;
    //火焰效果 
    public GameObject FirePre;

    public float bulletInterval = 0.3f;
    private float timer = 0;
    private PlayerControl pc;
    private RecoilControl rc;

    // Start is called before the first frame update
    void Start()
    {
        rc = GetComponent<RecoilControl>();
        pc = GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(Input.GetMouseButton(0) && timer >= bulletInterval && !pc.highSpeed)
        {
            timer = 0;
            //后坐力
            rc.Fire();
            //创建子弹
            Instantiate(BulletPre, FirePoint.transform.position, FirePoint.transform.rotation);
            //显示效果
            Destroy(Instantiate(FirePre, FirePoint.transform.position, FirePoint.transform.rotation),0.1f);
        }
    }
}
