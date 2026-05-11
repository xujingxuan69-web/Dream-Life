using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnCounterAttack;

    public void CreateClone(Transform _clonePosition, int _facingDir, int _attackNumber = 0, Vector3 _offset = new Vector3())
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, _attackNumber, _facingDir, _offset);
    }

    public bool AttackConfirm => canAttack;

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            if (PlayerManager.instance.attackDash)
            {
                CreateClone(player.transform, player.facingDir, PlayerManager.instance.comboCounter);
            }
            else
            {
                CreateClone(player.transform, player.facingDir);
            }
        }
        PlayerManager.instance.attackDash = false;
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform, int _facingDir)
    {
        if (createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, _facingDir, _facingDir * Vector2.right));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, int _dir, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _dir * -1, 1, _offset);
    }
}
