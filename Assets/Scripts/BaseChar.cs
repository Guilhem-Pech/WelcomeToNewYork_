using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class BaseChar : BaseEntity
{
    [SyncVar]
    public int maxStamina = 50;

    [SyncVar (hook = nameof(OnChangeStamina))]
    public int currentStamina;

    public bool canMoveWhileAttacking = true;

    public PlayerAnimation playerAnimation;

    protected GameObject UI;

    public StaminaLevel staminaLevel;
    public SpecialLevel specialLevel;

    [SerializeField]
    protected float tpsRecharge;

    [SyncVar (hook = nameof(OnChangeCooldown))]
    public float cooldown;

    [SyncVar]
    public bool attSpeReady = true;
    public bool rechargeSpe = false;
    public GameObject attSpe;

    public abstract void Awake();

    public void OnChangeCooldown(float cur)
    {
        if (isLocalPlayer)
        {
            if (!attSpeReady)
                GetSpecialLevel().SetLevel(cur, tpsRecharge);
            else
                GetSpecialLevel().SetLevel(0, tpsRecharge);
        }      
        cooldown = cur;
    }

    public void OnChangeStamina(int cur)
    {
        if(isLocalPlayer)
            GetStaminaLevelUI().SetLevel(cur,maxStamina);

        currentStamina = cur;
    }

    private SpecialLevel GetSpecialLevel()
    {
        if (specialLevel == null)
            specialLevel = FindObjectOfType<SpecialLevel>();
        return specialLevel;
    }
    private StaminaLevel GetStaminaLevelUI()
    {
        if(staminaLevel == null)
            staminaLevel = FindObjectOfType<StaminaLevel>();
        return staminaLevel;
    }
   
    public virtual void Start()
    {

        if (isServer)
        {
            this.tag = "Player";
            cooldown = tpsRecharge;
            currentStamina = maxStamina;
            currentHealth = maxHealth;
        }
        if (isLocalPlayer)
        {
            if (healthBar == null)
                healthBar = FindObjectOfType<HealthBar>();
            if (staminaLevel == null)
                staminaLevel = FindObjectOfType<StaminaLevel>();
        }

        GameObject gm = GameObject.FindGameObjectWithTag("GameController");
        gm.GetComponent<GameManager>().playerMan.players.Add(this);


    }

    [Server]
    public void GainStamina(int stam)
    {
        currentStamina = currentStamina + stam;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
    }

    [Server]
    public void UseStamina(int stam)
    {
        int minus = currentStamina - stam;
        currentStamina = minus >= 0 ? minus : 0;
    }

    [Server]
    protected abstract void AttackSpeciale(Vector3 playerPosition_, float angle);

    [Server]
    protected abstract void Attack(Vector3 point);
 

    public int GetMaxStamina()
    {
        return maxStamina;
    }
    public int GetStamina()
    {
        return currentStamina;
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
    }

    public override void AddHealth(int heal)
    {
        base.AddHealth(heal);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    [TargetRpc]
    public void TargetAffichMort(NetworkConnection nC)
    {
        GameObject UI = GameObject.Find("UIInGame");
        UI.GetComponent<DeathScreen>().AfficherLabelMort();
    }

    [Server]
    public override void Death()
    {
        base.Death(); 
        TargetAffichMort(connectionToClient);
    }

}
