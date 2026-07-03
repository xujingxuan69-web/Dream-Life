using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    protected Animator anim_Door;
    protected Collider2D cd_Door;
    protected bool isTrigger = false;

    protected virtual void Start()
    {
        anim_Door = GetComponent<Animator>();
        cd_Door = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        isTrigger = true;
        anim_Door.SetBool("PlayerTrigger", true);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        isTrigger = false;
        anim_Door.SetBool("PlayerTrigger", false);
    }
}
