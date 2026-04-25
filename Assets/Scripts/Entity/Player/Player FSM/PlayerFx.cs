using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFx : MonoBehaviour
{
    [Header("Tears Info")]
    [SerializeField] private GameObject tears;
    private Animator animTears;



    private void Start()
    {
        animTears = tears.GetComponent<Animator>();
    }

    public void ShowTears()
    {

    }

    public void StartTearsAttack()
    {
        animTears.SetBool("TearsAttack", true);
    }

    public void StopTearsAttack()
    {
        animTears.SetBool("TearsAttack", false);
        Debug.Log("success stopTearsAttack");
    }

    public void HideTears()
    {

    }

}
