using Random = UnityEngine.Random;


public class IATrain : Train
{
    // Flèches (circle actuellement)
    /*public GameObject leftArrow;
    public GameObject rightArrow;*/


    void Start()
    {
        base.Start();
        this.choice = 1;

        // CODER CHOIX IA ENEMY
        InvokeRepeating("MovementChoice", 0.3f, 0.3f);

        //ArrowColor(cGauche.GetComponent<SpriteRenderer>(), cDroit.GetComponent<SpriteRenderer>(), 1);
    }

    void Update()
    {
        base.Update();
    }
    
    // IA ALÉATOIRE A TESTER
    private void MovementChoice()
    {
        int random = Random.Range(0, 2);
        this.choice = (random == 0) ? -1 : 1;
    }

    public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
