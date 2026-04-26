using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
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



    private void Start()
    {
        Rbd= GetComponent<Rigidbody>();

    }

    private void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }
        
    private void OnJump(InputValue Value)
    {
        _jump = Value.isPressed;
    }

    private void FixedUpdate()
    {
        //Movimiento
        Vector3 forward = _camaraTransform.forward;
        Vector3 right = _camaraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * _moveInput.y + right * _moveInput.x;
        Rbd.linearVelocity = new Vector3(moveDir.x * _speed, Rbd.linearVelocity.y, moveDir.z * _speed);

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

}
