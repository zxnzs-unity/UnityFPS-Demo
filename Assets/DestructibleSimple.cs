using UnityEngine;

public class DestructibleSimple : MonoBehaviour
{
    public GameObject breakEffect; // 拖入灰尘粒子预制体

    public void OnHit(Vector3 hitPoint)
    {
        // 1. 生成爆炸灰尘特效
        if (breakEffect != null)
        {
            Instantiate(breakEffect, hitPoint, Quaternion.identity);
        }
        // 2. 物体直接消失
        Destroy(gameObject);
    }
}