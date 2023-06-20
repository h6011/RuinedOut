using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;

    #region Stat

    [Header("Stat")]
    public float WalkSpeed = 5f;
    public float RunSpeed = 7f;
    public float JumpHeight = 1.5f;

    #endregion


    [Header("Tick & Stamina")]
    [SerializeField] private float ReduceStaminaPerTick = 0.2f;
    [SerializeField] private float IncreaseStaminaPerTick = 0.08f;
    [SerializeField] private float CooltimeForUpStamina = 2f;

    private float TimeForCheckCooltime;


    [SerializeField] private float StatTick = 0.05f;
    private float StatTickCurrTime = 0.05f;

    #region Gravity

    [Header("Gravity")]
    public float Gravity = -9.81f * 2;

    #endregion

    #region Ground Stat

    [Header("Ground Stat")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    private bool isWalking = false;
    private bool isRunning = false;

    public bool IsWalking() { return isWalking; }
    public bool IsRunning() { return isRunning; }

    #endregion


    private void StaminaAction()
    {
        if (isRunning)
        {
            PlayerCtrl.Instance.Stamina -= ReduceStaminaPerTick;

            PlayerCtrl.Instance.Stamina = Mathf.Clamp(PlayerCtrl.Instance.Stamina, 0f, PlayerCtrl.Instance.MaxStamina);
        }
        else
        {
            if ( (Time.time - TimeForCheckCooltime) > CooltimeForUpStamina )
            {
                PlayerCtrl.Instance.Stamina += IncreaseStaminaPerTick;

                PlayerCtrl.Instance.Stamina = Mathf.Clamp(PlayerCtrl.Instance.Stamina, 0f, PlayerCtrl.Instance.MaxStamina);
            }
        }
    }

    private void StaminaTickProcess()
    {
        if ((Time.time - StatTickCurrTime) >= StatTick)
        {
            StatTickCurrTime = Time.time;
            StaminaAction();
        }
    }

    private void JumpAction()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //Debug.Log("Jumped");
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
    }

    private void MoveAction()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            TimeForCheckCooltime = Time.time;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (PlayerCtrl.Instance.Stamina > 0)
            {
                controller.Move(move * RunSpeed * Time.deltaTime);
                isRunning = true;
            }
            else
            {
                isRunning = false;
                controller.Move(move * WalkSpeed * Time.deltaTime);
            }
        }
        else
        {
            isRunning = false;
            controller.Move(move * WalkSpeed * Time.deltaTime);
        }

        JumpAction();

        velocity.y += Gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Update()
    {
        MoveAction();
        StaminaTickProcess();
    }
}
