using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody Rbd;

    //Movimiento
    [SerializeField] private float _speed;
    private Vector2 _moveInput;


    private void Start()
    {
        Rbd= GetComponent<Rigidbody>();

    }

    private void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }
        

    private void FixedUpdate()
    {
        Rbd.linearVelocity = new Vector3(_moveInput.x * _speed, Rbd.linearVelocity.y, _moveInput.y * _speed);
    }

}
