using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tears_Skill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private GameObject tearsPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float tearsGravity;

    public void CreateTears(Vector2 rotationDir)
    {
        
        float angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg + 90f;

        GameObject newTears = Instantiate(tearsPrefab, player.transform.position, Quaternion.Euler(0, 0, angle));
        Tears_Skill_Controller newTearsScript = newTears.GetComponent<Tears_Skill_Controller>();

        newTearsScript.SetupTears(launchDir, tearsGravity);
    }
}
