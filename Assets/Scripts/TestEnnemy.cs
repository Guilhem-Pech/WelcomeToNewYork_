using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnnemy : BaseEntity
{

    [ReadOnly] public Vector3 m_knockBackNormalDir;
    [ReadOnly] public float m_knockBackStrength;
    [ReadOnly] public float m_knockBackDuration;

    private Color colorBase;
    public AudioClip hurtSound;


    private CharacterController charControl;

    public void Start()
    {
        charControl = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        colorBase = this.GetComponentInChildren<SpriteRenderer>().color;
    }

    public override void Update()
    {
        base.Update(); // feedBack Couleur
    }

    public void onHit(Vector2 knockDir, float knockBackStrength, float knockBackDuration, int damage)
    {
        //On joue le son de dégats
        AudioSource SoundSource = gameObject.AddComponent<AudioSource>();
        SoundSource.clip = hurtSound;
        SoundSource.Play();

        //Initialisation des paramètres du knockback
        m_knockBackNormalDir = (new Vector3(knockDir.x, 0f, knockDir.y)).normalized;
        m_knockBackStrength = knockBackStrength;
        m_knockBackDuration = knockBackDuration;
        tps = m_knockBackDuration;

        //On applique les dégats et le knockback
        takeDamage(damage);
        
        gameObject.GetComponentInChildren<Animator>().SetBool("IsKnockedBack", true);

        //On vérifie si le coup tue l'entité
        if (this.currentHealth <= 0)
            death();
    }

    public override void death()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameController");
        gm.GetComponent<GameManager>().waveMan.ennemiVivant.Remove(this.gameObject);

        Destroy(this.gameObject);
    }

}
