using UnityEngine;

public class ShieldEnemy : Enemy
{
    private bool hasShield;

    protected override void OnEnable()
    {
        base.OnEnable();
        gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        hasShield = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        hasShield = true;
    }

    public override void TakeDamage()
    {
        if (hasShield)
        {
            Debug.Log("Break Shield");
            hasShield = false;
        }
        else
        {
            Debug.Log("Die");
            OnDie.Invoke();
        }
    }
}
