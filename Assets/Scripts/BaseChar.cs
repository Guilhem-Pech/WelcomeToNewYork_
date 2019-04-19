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

    [SyncVar(hook = nameof(OnAttSpeReady))]
    public bool attSpeReady = true;
    public bool rechargeSpe = false;
    public GameObject attSpe;

    public abstract void Awake();

    public void OnChangeCooldown(float cur)
    {
        if (GetSpecialLevel() == null)
            return;

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
      currentStamina = cur;
    }

    public void OnAttSpeReady(bool isIt)
    {
        attSpeReady = isIt;
        if (!isLocalPlayer || GetSpecialLevel() == null)
            return;

        if (isIt)
        {
            GetSpecialLevel().SetLevel(0, tpsRecharge);
            GetSpecialLevel().TurnOnEffect();
        }
        else
            GetSpecialLevel().TurnOffEffect();
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

        StartCoroutine(RegisterToGamemanager());
    }

    private IEnumerator RegisterToGamemanager()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        while (gm == null)
        {
            gm = FindObjectOfType<GameManager>(); ;
            yield return null;
        }

        if (!gm.playerMan.players.Contains(this))
        {
            gm.playerMan.players.Add(this);
        }
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
        OnAttSpeReady(true);
        base.OnStartLocalPlayer();
    }

    [TargetRpc]
    public void TargetAffichMort(NetworkConnection nC)
    {
       // Debug.Log("YALAAAAAAAAAAAAAAH");
        GameObject UI = GameObject.Find("UI");
        UI.GetComponent<DeathScreen>().AfficherLabelMort();
    }

    [Server]
    public override void Death()
    {
        base.Death();
        TargetAffichMort(connectionToClient);
    }

}
