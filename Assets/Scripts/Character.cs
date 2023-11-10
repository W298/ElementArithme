using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator anim;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (anim.GetBool("isAttack") && timer > 0.5f)
        {
            anim.SetBool("isAttack", false);
        }
        if (anim.GetBool("isHit") && timer > 0.5f)
        {
            anim.SetBool("isHit", false);
        }
    }

    public void Attack()
    {
        anim.SetBool("isAttack", true);
    }

    public void Hit()
    {
        anim.SetBool("isHit", true);
    }

    public void Die()
    {
        anim.SetBool("isEnd", true);
        anim.SetBool("isWin", false);
    }

    public void Win()
    {
        anim.SetBool("isEnd", true);
        anim.SetBool("isWin", true);
    }
}
