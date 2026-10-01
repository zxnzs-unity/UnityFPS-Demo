using UnityEngine;


public class RecoilControl : MonoBehaviour
{
    public float X = -3f;
    public float speed = 10;
    public float returnSpeed = 5;

    private float targetRotation;//后坐力的目标角度
    private float currentRotation;//物体当前实际的旋转角度

    // 新增1：实际产生后坐力的手臂物体（脚本挂玩家身上也能正常作用）
    //private Transform aimTransform;

    //void Start()
    //{
    //    //  新增2：自动找到手臂Animator物体，和PlayerControl共用同一个
    //    Animator ani = GetComponentInChildren<Animator>();
    //    if (ani != null)
    //    {
    //        aimTransform = ani.transform;
    //    }
    //}

    // 修改3：Update → LateUpdate，保证在鼠标视角之后执行，不会被覆盖
    private void LateUpdate()
    {
        //// 物体没找到就不执行，避免报错
        //if (aimTransform == null) return;

        //旋转   让targetRotation从当前值，以returnSpeed的速度平滑向0回弹。
        targetRotation = Mathf.Lerp(targetRotation, 0, returnSpeed * Time.deltaTime);
        //恢复   让currentRotation从当前值，平滑地向0过渡
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, speed * Time.deltaTime);
        //应用旋转
        transform.localRotation = Quaternion.Euler(currentRotation, transform.localEulerAngles.y, 0);
    }

    public void Fire()
    {
        targetRotation += X;
    }
}
