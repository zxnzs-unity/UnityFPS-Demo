using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    //速度
    public float speed = 2f;
    public float jumpForce = 5f;
    //灵敏度
    public float xScensitivity = 5f;
    public float yScensitivity = 15f;

    [HideInInspector]
    public bool highSpeed = false;
    [HideInInspector]
    public bool isAiming = false;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Rigidbody rb;
    private Animator ani;
    private Vector3 velocity;
    private bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        //// 改动1：关闭刚体阻力，立刻停止
        //rb.drag = 0;
        //rb.angularDrag = 0;

        xRotation = 0f;
        yRotation = 0f;
        ani.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Mouse();
        HighSpeed();
        Move();
        Jump();
    }

    void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
            ani.SetBool("Aim", true);
            float aim = ani.GetFloat("Aiming");
            ani.SetFloat("Aiming", Mathf.Lerp(aim, 1, Time.deltaTime * 10));
        }
        else
        {
            isAiming = false;
            ani.SetBool("Aim", false);
            float aim = ani.GetFloat("Aiming");
            ani.SetFloat("Aiming", Mathf.Lerp(aim, 0, Time.deltaTime * 10));
        }
    }

    void Mouse()
    {
        //  新增：跳过前2帧鼠标输入，解决启动时缓存值导致的视角偏移
        if (Time.frameCount <= 2) return;

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        //上下旋转
        xRotation -= y * yScensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        ani.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //左右旋转
        //transform.Rotate(Vector3.up * x * xScensitivity);
        yRotation += x * xScensitivity;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void Move()
    {
        //获取水平轴输入 -1 0 1
        float horizontal = Input.GetAxisRaw("Horizontal");
        //获取垂直轴输入
        float vertical = Input.GetAxisRaw("Vertical");
        //创建向量
        Vector3 dir = (transform.forward * vertical + transform.right * horizontal).normalized;
        //速度
        velocity = dir * speed;
        velocity.y = rb.velocity.y;
        //改动3：无输入时强制速度归零，取消滑行
        if (horizontal == 0 && vertical == 0) velocity = new Vector3(0,velocity.y,0);
        //移动动画
        ani.SetFloat("Movement", dir.magnitude);
    }

    void HighSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && IsGround())
        {
            highSpeed = true;
            speed = 5;
            ani.SetBool("Holstered", true);
        }
        else
        {
            highSpeed = false;
            speed = 3;
            ani.SetBool("Holstered", false);
        }
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGround())
        {
            jump = true;
        }
    }

    public bool IsGround()
    {
        RaycastHit hit;
        bool res = Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down,
                                    out hit, 0.4f, LayerMask.GetMask("Ground"));
        return res;
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            jump = false;
            velocity.y = jumpForce;
        }
        rb.velocity = velocity;

        //  新增：碰撞防卡修正（斜着走不卡，碰到物体不会粘住）
        if (!IsGround())
        {
            velocity.x *= 0.95f;
            velocity.z *= 0.95f;
        }
        // 防止碰撞时速度被物理引擎强制归零
        if (velocity.magnitude < 0.1f && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            velocity = (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized * speed;
            velocity.y = rb.velocity.y;
        }

        rb.velocity = velocity;

    }
}
