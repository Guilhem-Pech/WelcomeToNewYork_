using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class TestEnnemy : BaseEntity
{

    [ReadOnly] public Vector3 m_knockBackNormalDir;
    [ReadOnly] public float m_knockBackStrength;
    [ReadOnly] public float m_knockBackDuration;
    public List<MonoBehaviour> ServerScripts;
    private Color colorBase;
    public AudioClip hurtSound;


    private CharacterController charControl;

    [ServerCallback]
    private void Awake()
    {
        this.GetComponent<NavMeshAgent>().enabled = true;
        this.GetComponent<Animator>().enabled = true; // WILL NO LONGER BE NEEDED IN THE NEXT SPRINT
        foreach (MonoBehaviour c in ServerScripts)
            c.enabled = true;
    }


    public void Start()
    {
        if (isServer)
        {
            charControl = GetComponent<CharacterController>();
            currentHealth = maxHealth;
        }
        if (isClient)
        {
            colorBase = this.GetComponentInChildren<SpriteRenderer>().color;
        }

    }

    public override void Update()
    {
        base.Update(); // feedBack Couleur
    }

    [Server]
    public void onHit(Vector2 knockDir, float knockBackStrength, float knockBackDuration, int damage)
    {
        RpcOnHit();
        // Initialisation des paramètres du knockback
        m_knockBackNormalDir = (new Vector3(knockDir.x, 0f, knockDir.y)).normalized;
        m_knockBackStrength = knockBackStrength;
        m_knockBackDuration = knockBackDuration;
        tps = m_knockBackDuration;

        gameObject.GetComponent<Animator>().SetBool("IsKnockedBack", true);
        TakeDamage(damage);
        if (this.currentHealth <= 0)
            Death();
    }


    [ClientRpc]
    public void RpcOnHit()
    {
        AudioSource SoundSource = gameObject.AddComponent<AudioSource>();
        SoundSource.clip = hurtSound;
        SoundSource.Play();
        Destroy(SoundSource, hurtSound.length);
    }



    [Server]
    public override void Death()
    {

        GameObject gm = GameObject.FindGameObjectWithTag("GameController");
        if(gm != null)
         gm.GetComponent<GameManager>().waveMan.ennemiVivant.Remove(this.gameObject);
        else
            // Debug.LogWarning("There is no GameManager !", this);

        Destroy(this.gameObject);
    }

}
