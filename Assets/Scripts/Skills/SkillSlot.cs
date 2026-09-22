using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillSlot : NetworkBehaviour
{
    [SerializeField] private Skilldata _slot1;
    [SerializeField] private Skilldata _slot2;
    [SerializeField] private Skilldata _slot3;

    //Colldown y Costos
    private float _coolDownSkill1;
    private float _coolDownSkill2;
    private float _coolDownSkill3;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        SkillUi skillUi = FindFirstObjectByType<SkillUi>();

        if(skillUi != null)
        {
            skillUi.SetSkill(_slot1,_slot2,_slot3);
        }

    }
    
    public void OnSkill1()
    {
        if (!IsOwner) return;

        if (_coolDownSkill1 > 0)
        {
            Debug.Log("Habilidad 1 En enfiramiento"+_coolDownSkill1+"Seg");
            
            return;
        }


        if (_slot1.Skill == Skilldata.SkillType.Attack)
        {
            UseSkill1ServerRpc();

            _coolDownSkill1 = _slot1.SkillCooldown;
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

        if (_coolDownSkill2 > 0)
        {
            Debug.Log("Habilidad 2 En enfiramiento" + _coolDownSkill2 + "Seg");

            return;
        }

        if (_slot2.Skill == Skilldata.SkillType.Buff)
        {
            UseSkill2ServerRpc();

            _coolDownSkill2 = _slot2.SkillCooldown;
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

        if (_coolDownSkill3 > 0)
        {
            Debug.Log("Habilidad 3 En enfiramiento" + _coolDownSkill3 + "Seg");

            return;
        }

        if (_slot3.Skill == Skilldata.SkillType.Ultimate)
        {
            UseSkill3ServerRpc();

            _coolDownSkill3 = _slot3.SkillCooldown;
        }

    }
    [ServerRpc]
    private void UseSkill3ServerRpc()
    {
        GameObject obj = Instantiate(_slot3.SkillPrefab, transform.position, transform.rotation);

        obj.GetComponent<NetworkObject>().Spawn();

    }

    private void Update()
    {
        if (_coolDownSkill1> 0)
        {
            _coolDownSkill1 -= Time.deltaTime;
        }

        if (_coolDownSkill2 > 0)
        {
            _coolDownSkill2 -= Time.deltaTime;
        }

        if (_coolDownSkill3 > 0)
        {
            _coolDownSkill3 -= Time.deltaTime;
        }
    }

}
