using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private CinemachineFreeLook _freelook;
    [SerializeField] private Transform _target;

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

}
