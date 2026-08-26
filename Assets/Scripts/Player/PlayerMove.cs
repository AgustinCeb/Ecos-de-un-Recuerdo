using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : NetworkBehaviour
{
    public Rigidbody Rbd;

    //Movimiento
    [Header("Movimiento")]
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    private Vector2 _moveInput;

    //Salto
    [Header("Salto")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance;
    [SerializeField] private LayerMask _groundLayer;
    private bool _jump;
    private bool _isGrounded;

    //Camara
    [Header("Camara")]
    [SerializeField] private Transform _camaraTransform;

    //Input
    [Header("Input")]
    [SerializeField] private PlayerInput _playerInput;

    //Dash
    [Header("Dash")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private AudioClip _dashSFX;
    private bool _canDash = true;

    //Rampas
    [Header("Angulo para Rampas")]
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _rampCheckDistance;
    [SerializeField] private float _downForce;
    private bool _isFalling;
    private bool _isJumping;

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
            _isFalling = false;
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

    private bool isOnRamp(out RaycastHit hit)
    {
        if (Physics.Raycast(_groundCheck.position, Vector3.down, out hit, _groundDistance + 0.2f, _groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);


            Debug.DrawRay(
                _groundCheck.position,
                Vector3.down * _rampCheckDistance,
                Color.red
            );

            Debug.DrawRay(
                hit.point,
                hit.normal,
                Color.blue
            );


            return angle > 0f && angle <= _maxAngle;
        
        }

        return false;
    }


    private Vector3 GetRampDirection(Vector3 direction)
    {
        direction.y=0f;
        direction.Normalize();

        Debug.DrawRay(
        transform.position,
        direction * _rampCheckDistance,
        Color.red);

        if (Physics.Raycast(_groundCheck.position, direction, out RaycastHit hit, _rampCheckDistance, _groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            Debug.DrawRay(hit.point, hit.normal, Color.blue);

            if(angle > 0f && angle<= _maxAngle)
            {
                Vector3 rampDirection = Vector3.ProjectOnPlane(direction, hit.normal).normalized;

                Debug.DrawRay(
                hit.point,
                hit.normal,
                Color.green);

                return rampDirection;

            }

        }

        return direction;

    }

    


    private void FixedUpdate()
    {
        if (!IsOwner) return;

       
        Vector3 moveDir = GetRampDirection(GetMoveDirection());

        //comprueba si esta en una rampa
        bool onRamp = isOnRamp(out RaycastHit rampHit);
        
        Vector3 targetVelocity = new Vector3(moveDir.x * _speed, Rbd.linearVelocity.y, moveDir.z * _speed);
        Rbd.linearVelocity = Vector3.Lerp(Rbd.linearVelocity,targetVelocity,0.2f);

        //Compureba si esta en el suelo
        _isGrounded = Physics.CheckSphere(_groundCheck.position,_groundDistance,_groundLayer);

        //salto
        if (_isGrounded && _jump) 
        { 
            Rbd.useGravity = true;

            Rbd.linearVelocity = new Vector3 (Rbd.linearVelocity.x,0f,Rbd.linearVelocity.z);
            Rbd.AddForce(Vector3.up*_jumpForce,ForceMode.Impulse);
            _jump = false;
            _isJumping = true;
            
        }

        //Rotacion de personaje
        if (moveDir != Vector3.zero) 
        {
            Quaternion rotation = Quaternion.LookRotation(moveDir);

            Rbd.MoveRotation(Quaternion.Slerp(Rbd.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime));
        }

        //Gravedad En Rampas
        if (onRamp && !_jump)
        {
            Rbd.useGravity = false;

            Rbd.AddForce(-rampHit.normal * _downForce,ForceMode.Force);
        }
        else
        {
            Rbd.useGravity = true;
        }

        if(_isJumping && Rbd.linearVelocity.y <= 0)
        {
            _isJumping = false;
            _isFalling = true;
        }

        if (_isFalling)
        {
            Rbd.AddForce(Vector3.down * _downForce,ForceMode.Acceleration);
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

        dashDir = GetRampDirection(dashDir);
        Rbd.linearVelocity = new Vector3(Rbd.linearVelocity.x, 0f, Rbd.linearVelocity.z);

        SFXManager.instance.PlaySFX(_dashSFX, this.transform, 1, 1);
        Rbd.AddForce(dashDir.normalized * _dashForce,ForceMode.Impulse);
        yield return new WaitForSeconds(_dashCooldown);

        _canDash = true;

    }

}
