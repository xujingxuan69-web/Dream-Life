using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float colorLoosingSpeed;
    private Material tempMaterial;

    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius ;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        SpriteRenderer[] allSr = GetComponentsInChildren<SpriteRenderer>();
        tempMaterial = new Material(allSr[0].material);
        foreach (var s in allSr)
        {
            s.material = tempMaterial;
        }
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        
        if (cloneTimer < 0)
        {
            tempMaterial.color = new Color(1, 1, 1, tempMaterial.color.a - (colorLoosingSpeed * Time.deltaTime));
            if (tempMaterial.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, int _attackNumber)
    {
        if (_attackNumber > 0)
        {
            anim.SetInteger("AttackNumber", _attackNumber);
            cloneTimer = 10f;
        }
        else
        {
            cloneTimer = _cloneDuration;
        }


        transform.position = _newTransform.position;
        transform.rotation = _newTransform.rotation;
    }


    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        int facingDir = transform.eulerAngles.y == 0 ? -1 : 1;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage(facingDir);
        }
    }

}
