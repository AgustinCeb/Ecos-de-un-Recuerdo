using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : NetworkBehaviour
{
    public Rigidbody Rbd;

    //Movimiento
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    private Vector2 _moveInput;

    //Salto
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance;
    [SerializeField] private LayerMask _groundLayer;
    private bool _jump;
    private bool _isGrounded;

    //Camara
    [SerializeField] private Transform _camaraTransform;

    //Input
    [SerializeField] private PlayerInput _playerInput;

    //Dash
    [Header("Dash")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private AudioClip _dashSFX;
    private bool _canDash = true;

    private void Start()
    {
        Rbd= GetComponent<Rigidbody>();

        if (!IsOwner) 
        {
            _playerInput.enabled = false;
            return;
        }

        _camaraTransform = Camera.main.transform;

    }

    

    private void OnDash()
    {
        if (!IsOwner) return; 
        if (!_canDash) return;

        StartCoroutine(Dash());

    }

    private void OnMove(InputValue inputValue)
    {
        if (!IsOwner) return ;

        _moveInput = inputValue.Get<Vector2>();
    }
        
    private void OnJump(InputValue Value)
    {
        if (!IsOwner) return;

        if (Value.isPressed && _isGrounded)
        {
            _jump = true;
        }
        
    }
    private Vector3 GetMoveDirection()
    {
        //movimento
        Vector3 forward = _camaraTransform.forward;
        Vector3 right = _camaraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

         return forward * _moveInput.y + right * _moveInput.x;
    }

    
    private void FixedUpdate()
    {
        if (!IsOwner) return;

       
        Vector3 moveDir = GetMoveDirection();
        
        Vector3 targetVelocity = new Vector3(moveDir.x * _speed, Rbd.linearVelocity.y, moveDir.z * _speed);
        Rbd.linearVelocity = Vector3.Lerp(Rbd.linearVelocity,targetVelocity,0.2f);

        //Compureba si esta en el suelo
        _isGrounded = Physics.CheckSphere(_groundCheck.position,_groundDistance,_groundLayer);

        //salto
        if (_isGrounded && _jump) 
        { 
            Rbd.linearVelocity = new Vector3 (Rbd.linearVelocity.x,0f,Rbd.linearVelocity.z);
            Rbd.AddForce(Vector3.up*_jumpForce,ForceMode.Impulse);
            _jump = false;
        }

        //Rotacion de personaje
        if (moveDir != Vector3.zero) 
        {
            Quaternion rotation = Quaternion.LookRotation(moveDir);

            Rbd.MoveRotation(Quaternion.Slerp(Rbd.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime));
        }
    }
    private IEnumerator Dash()
    {
        
        _canDash = false;

        Vector3 dashDir = GetMoveDirection();

        if (dashDir == Vector3.zero) 
        {
            dashDir = transform.forward;
        }
        SFXManager.instance.PlaySFX(_dashSFX, this.transform, 1, 1);
        Rbd.AddForce(dashDir.normalized * _dashForce,ForceMode.Impulse);
        yield return new WaitForSeconds(_dashCooldown);

        _canDash = true;

    }

}
