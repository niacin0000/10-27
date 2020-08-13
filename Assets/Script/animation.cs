using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{

    public Animator animator;
    public AudioSource audiosource;
    bool Attack = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Attack == false && Input.GetMouseButtonDown(1))
        {
            animator.SetBool("IsAttack", true);
            Attack = true;
        }
    }

    void OnAttack()
    {
        audiosource.Play();

    }

    void OnIdle()
    {
        animator.SetBool("IsAttack", false);
        Attack = false;
    }
}
