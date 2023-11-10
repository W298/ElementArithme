using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (anim.GetBool("IsAttack") && timer > 0.5f)
        {
            anim.SetBool("IsAttack", false);
        }
        if (anim.GetBool("IsHit") && timer > 0.5f)
        {
            anim.SetBool("IsHit", false);
        }
    }

    public void Attack()
    {
        anim.SetBool("IsAttack", true);
        timer = 0;
    }

    public void Hit()
    {
        anim.SetBool("IsHit", true);
        timer = 0;
    }

    public void Die()
    {
        anim.SetBool("IsEnd", true);
        anim.SetBool("IsWin", false);
    }

    public void Win()
    {
        anim.SetBool("IsEnd", true);
        anim.SetBool("IsWin", true);
    }
}
