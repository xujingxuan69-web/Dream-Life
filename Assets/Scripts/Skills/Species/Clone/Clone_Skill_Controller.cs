using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float colorLoosingSpeed;
    private Material tempMaterial;

    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius ;

    private int facingDir;

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


    public void SetupClone(Transform _newTransform, float _cloneDuration, int _attackNumber, int _facingDir, Vector3 _offset)
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


        transform.position = _newTransform.position + _offset;

        facingDir = _facingDir;

        if (facingDir != 1)
        {
            transform.Rotate(0, 180, 0);
        }
    }


    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().DamageEffect(facingDir);
        }
    }

}
