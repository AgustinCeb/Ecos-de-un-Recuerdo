using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            NetworkManager.Singleton.StartHost(); //Iniciamos el Host
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            NetworkManager.Singleton.StartClient(); //Iniciamos como client
        }
    }


}
