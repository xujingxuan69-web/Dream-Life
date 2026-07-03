using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    public void CreateClone(Transform _clonePosition, int _facingDir, int _attackNumber = 0, Vector3 _offset = new Vector3())
    {
        GameObject newClone = Instantiate(clonePrefab);

        if (_attackNumber == 0)
            newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, _attackNumber, _facingDir, _offset, player);
        else if (_offset != new Vector3())
            newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, _attackNumber, player.primaryAttackState.attackDir, _offset, player);
        else
            newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, _attackNumber, _facingDir, _facingDir * Vector2.left, player);
    }

    public IEnumerator CreateCloneWithDelay(Transform _transform, int _dir, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _dir * -1, 1, _offset);
    }
}
