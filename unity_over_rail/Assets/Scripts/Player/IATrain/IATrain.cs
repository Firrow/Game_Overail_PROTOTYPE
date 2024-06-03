using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class IATrain : Train
{
    // Flèches (circle actuellement)
    /*public GameObject leftArrow;
    public GameObject rightArrow;*/


    [SerializeField]
    private int playerIndex = 0;
    private int lastChoice;
    private float trainAngle;


    void Start()
    {
        base.Start();

        foreach (GameObject Element in GameObject.FindGameObjectsWithTag("InterfacePlayer"))
        {
            if (Element.GetComponent<InterfacePlayer>().index == playerIndex)
            {
                this.healthBar = Element.GetComponent<InterfacePlayer>().healthBarPlayer.GetComponent<HealthBar>();
                this.bulletBar = Element.GetComponent<InterfacePlayer>().bulletBarPlayer.GetComponent<BulletBar>();
                this.objectSlot = Element.GetComponent<InterfacePlayer>().objectSlotPlayer.GetComponent<ObjectSlot>();
            }
        }
        bulletBar.SetMaxBullet(weapon.GetComponent<Weapon>().MaxBulletQuantity);
        bulletBar.SetBullet(weapon.GetComponent<Weapon>().CurrentBulletQuantity);
        healthBar.SetMaxHealth(MaxHealth);

        //ChangeArrowColor(leftArrow.GetComponent<SpriteRenderer>(), rightArrow.GetComponent<SpriteRenderer>());
        this.choice = 1;
        lastChoice = 1;

        // CODER CHOIX IA ENEMY
        InvokeRepeating("MovementChoice", 0.3f, 0.3f);

        //ArrowColor(cGauche.GetComponent<SpriteRenderer>(), cDroit.GetComponent<SpriteRenderer>(), 1);
    }

    void Update()
    {
        base.Update();
        Debug.Log(currentHealth);
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }




    // IA ALÉATOIRE A TESTER
    private void MovementChoice()
    {
        int random = Random.Range(0, 2);
        this.choice = (random == 0) ? -1 : 1;
    }

    public new void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
