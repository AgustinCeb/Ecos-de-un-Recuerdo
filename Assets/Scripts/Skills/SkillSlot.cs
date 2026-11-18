using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkillSlot : NetworkBehaviour
{
    [SerializeField] private Skilldata _slot1;
    [SerializeField] private Skilldata _slot2;
    [SerializeField] private Skilldata _slot3;

    [SerializeField] private PlayerMana _playerMana;

    [SerializeField] private PlayerStats _playerStats;

    [SerializeField] private Animator _animator;

    private SkillUi _skillUi;
    private int _lastLevel;

    //Colldown y Costos
    private float _coolDownSkill1;
    private float _coolDownSkill2;
    private float _coolDownSkill3;
   


    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == "Game")
        {
            InitializeSkillUI();
        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!IsOwner || scene.name != "Game") return;

        InitializeSkillUI();
    }

    private void InitializeSkillUI()
    {
        SkillUi skillUi = FindFirstObjectByType<SkillUi>(FindObjectsInactive.Include);

        if (skillUi == null)
        {
            Debug.LogError("No se encontró SkillUi en la escena Game.");
            return;
        }

        _skillUi = skillUi;

        _skillUi.SetSkill(_slot1, _slot2, _slot3);

        _lastLevel = _playerStats._playerLevel;

        UpdateSkillUnlocks();

    }

    private void UpdateSkillUnlocks()
    {
        bool skill1Unlocked = _playerStats._playerLevel >= 3;

        bool skill2Unlocked = _playerStats._playerLevel >= 6;

        bool skill3Unlocked = _playerStats._playerLevel >= 9;

        if (_skillUi!= null)
        {
            _skillUi.SetSkill1Unlocked(skill1Unlocked);

            _skillUi.SetSkill2Unlocked(skill2Unlocked);

            _skillUi.SetSkill3Unlocked(skill3Unlocked);
        }

    }

    public void OnSkill1()
    {
        if (!IsOwner) return;

        if (_playerStats._playerLevel < 3)
        {
            Debug.Log("Habilidad No Desbloqueada");
            GameManager.Instance.ShowNotification("Habilidad No Desbloqueada");
            return;
        }

        if (_coolDownSkill1 > 0)
        {
            Debug.Log("Habilidad 1 En enfiramiento" + (_coolDownSkill1.ToString("0F")) + "Seg");
            GameManager.Instance.ShowNotification("Habilidad 1 En enfiramiento" + " " + (_coolDownSkill1.ToString("0")) + "Seg");
            return;
        }
        _animator.SetTrigger("Rayo");

    }

    public void Skill1()
    {
        if (!IsOwner) return;

        PlayerMove playerMove = GetComponent<PlayerMove>();

        Vector3 playerPosition = transform.position;
        Vector3 direction = playerMove.Direction.forward;

        UseSkill1ServerRpc(playerPosition, direction);

        _coolDownSkill1 = _slot1.SkillCooldown;

       
    }

    [ServerRpc]
    private void UseSkill1ServerRpc(Vector3 playerPosition, Vector3 direction) 
    {
        if (!_playerMana.TryUseMana(_slot1.SkillCost))
        {
            Debug.Log("Mana Insuficiente");
            GameManager.Instance.ShowNotification("Mana Insuficiente");
            return;
        }

        GameObject obj = Instantiate(_slot1.SkillPrefab,playerPosition,Quaternion.LookRotation(direction));

        CastigoData skill = obj.GetComponent<CastigoData>();

        skill.Initialize(playerPosition,direction);

        obj.GetComponent<NetworkObject>().Spawn();

    }

    public void OnSkill2()
    {
        if (!IsOwner) return;

        if (_playerStats._playerLevel < 6)
        {
            Debug.Log("Habilidad No Desbloqueada");
            GameManager.Instance.ShowNotification("Habilidad No Desbloqueada");
            return;
        }

        if (_coolDownSkill2 > 0)
        {
            Debug.Log("Habilidad 2 En enfiramiento" + _coolDownSkill2 + "Seg");
            GameManager.Instance.ShowNotification("Habilidad 2 En enfiramiento" + " " + (_coolDownSkill2.ToString("0")) + "Seg");

            return;
        }
        _animator.SetTrigger("Escudo");
    }

    public void Skill2()
    {
        

        UseSkill2ServerRpc();

        _coolDownSkill2 = _slot2.SkillCooldown;

    }
    [ServerRpc]
    private void UseSkill2ServerRpc()
    {

        if (!_playerMana.TryUseMana(_slot2.SkillCost))
        {
            Debug.Log("Mana Insuficiente");
            GameManager.Instance.ShowNotification("Mana Insuficiente");
            return;
        }

        GameObject obj = Instantiate(_slot2.SkillPrefab, transform.position, transform.rotation);

        ProteccionData proteccion = obj.GetComponent<ProteccionData>();

        if(proteccion != null)
        {
            proteccion.SetOwner(GetComponent<PlayerHealt>());
        }

        NetworkObject shieldObject = obj.GetComponent<NetworkObject>();

        shieldObject.SpawnWithOwnership(OwnerClientId);

        shieldObject.TrySetParent(NetworkObject, false);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    public void OnSkill3()
    {
        if (!IsOwner) return;

        if (_playerStats._playerLevel < 9)
        {
            Debug.Log("Habilidad No Desbloqueada");
            GameManager.Instance.ShowNotification("Habilidad No Desbloqueada");
            return;
        }

        if (_coolDownSkill3 > 0)
        {
            Debug.Log("Habilidad 3 En enfiramiento" + _coolDownSkill3 + "Seg");
            GameManager.Instance.ShowNotification("Habilidad 3 En enfiramiento" + " " + (_coolDownSkill3.ToString("0")) + "Seg");
            return;
        }

        _animator.SetTrigger("Ulti");
    }

    public void Skill3()
    {
        

        UseSkill3ServerRpc();

        _coolDownSkill3 = _slot3.SkillCooldown;

    }
    [ServerRpc]
    private void UseSkill3ServerRpc()
    {
        if (!_playerMana.TryUseMana(_slot3.SkillCost))
        {
            Debug.Log("Mana Insuficiente");
            GameManager.Instance.ShowNotification("Mana Insuficiente"); 
            return;
        }

        GameObject obj = Instantiate(_slot3.SkillPrefab, transform.position, transform.rotation);

        NetworkObject ultiObject = obj.GetComponent<NetworkObject>();

        ultiObject.SpawnWithOwnership(OwnerClientId);

        ultiObject.TrySetParent(NetworkObject, false);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

    }

    private void Update()
    {

        if (!IsOwner) return;

        if (_playerStats._playerLevel!= _lastLevel)
        {
            _lastLevel = _playerStats._playerLevel;

            UpdateSkillUnlocks();
        }
    

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

    public override void OnNetworkDespawn()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
