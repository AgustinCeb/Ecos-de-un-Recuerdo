using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class PlayerCamera : NetworkBehaviour
{
    [Header("Camara")]
    [SerializeField] private CinemachineFreeLook _freelook;
    [SerializeField] private Transform _target;

    [Header("Rotacion")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerObj;
    [SerializeField] private Rigidbody _rbd;

    [SerializeField] private float _rotationSpeed;

    private PlayerMove _playerMove;

   

    private void Awake()
    {
        _playerMove = GetComponentInParent<PlayerMove>();
    }

    private void Start()
    {
        if(IsOwner)
        {
            _freelook.gameObject.SetActive(true);

            _freelook.Follow = _target;
            _freelook.LookAt = _target;

        }
        else
        {
            _freelook.gameObject.SetActive(false);
        }


    }

    private void Update()
    {
        //Rotacion de Orientacion
        Vector3 viewDir = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
        _orientation.forward = viewDir.normalized;

        float horizontalInput = _playerMove.MoveInput.x;
        float verticalInput = _playerMove.MoveInput.y;
        Vector3 inputDir = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            _playerObj.forward = Vector3.Slerp(_playerObj.forward,inputDir.normalized,Time.deltaTime * _rotationSpeed);
        }

    }


}
