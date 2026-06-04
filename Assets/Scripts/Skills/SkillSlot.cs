using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillSlot : NetworkBehaviour
{
    [SerializeField] private Skilldata _slot1;
    [SerializeField] private Skilldata _slot2;
    [SerializeField] private Skilldata _slot3;

    public void OnSkill1()
    {
        if (!IsOwner) return;



        if (_slot1.Skill == Skilldata.SkillType.Attack)
        {
            UseSkill1ServerRpc();
        }

    }
    [ServerRpc]
    private void UseSkill1ServerRpc() 
    {
        GameObject obj = Instantiate(_slot1.SkillPrefab, transform.position, transform.rotation);

        obj.GetComponent<NetworkObject>().Spawn();
         
    }

    public void OnSkill2()
    {
        if (!IsOwner) return;


        
        if (_slot2.Skill == Skilldata.SkillType.Buff)
        {
            UseSkill2ServerRpc();
        }

    }
    [ServerRpc]
    private void UseSkill2ServerRpc()
    {

        GameObject obj = Instantiate(_slot2.SkillPrefab, transform.position, transform.rotation);

        ProteccionData proteccion = obj.GetComponent<ProteccionData>();

        if(proteccion != null)
        {
            proteccion.SetOwner(GetComponent<PlayerHealt>());
        }

        obj.GetComponent<NetworkObject>().Spawn();

    }

    public void OnSkill3()
    {
        if (!IsOwner) return;

        

        if (_slot3.Skill == Skilldata.SkillType.Ultimate)
        {
            UseSkill3ServerRpc();
        }

    }
    [ServerRpc]
    private void UseSkill3ServerRpc()
    {
        GameObject obj = Instantiate(_slot3.SkillPrefab, transform.position, transform.rotation);

        obj.GetComponent<NetworkObject>().Spawn();

    }


}
