using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    protected int HP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void getDamage(int damage)
    {        
        HP -= damage;
        if (HP <= 0)
        {
            die();
        }
        

    }

    protected virtual void die() { }
}
