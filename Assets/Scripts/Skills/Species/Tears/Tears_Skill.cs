using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TearsType
{ 
    Regular,
    Bounce,
    Pierce,
}

public class Tears_Skill : Skill
{
    public TearsType tearsType = TearsType.Regular;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;

    [Header("Peirce Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Skill Info")]
    [SerializeField] private GameObject tearsPrefab;
    [SerializeField] private Vector2 prefabOffset;

    [SerializeField] private Vector2 launchForce;

    [SerializeField] private float tearsGravity;
    [SerializeField] private float tearsScale;
    [SerializeField] private float tearsScaleSpeed;

    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float tearsDestroyDuration;

    [Header("Aim Info")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;


    private GameObject[] dots;

    private Vector2 finalDir;

    private float gravityTime;

    public bool dotsActive { get; private set; }

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.M))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }
    }

    private void SetupGravity()
    {
        if (tearsType == TearsType.Bounce)
        {
            tearsGravity = bounceGravity;
        }
        else if (tearsType == TearsType.Pierce)
        {
            tearsGravity = pierceGravity;
        }
    }

    public void CreateTears(int _facingDir)
    {
        float angle = Mathf.Atan2(finalDir.y, finalDir.x) * Mathf.Rad2Deg;

        GameObject newTears = Instantiate(tearsPrefab, 
            new Vector2(player.transform.position.x + player.facingDir * prefabOffset.x, player.transform.position.y + prefabOffset.y), 
            Quaternion.Euler(0, 0, angle));

        Tears_Skill_Controller newTearsScript = newTears.GetComponent<Tears_Skill_Controller>();

        SetupGravity();

        if (tearsType == TearsType.Bounce)
        {
            newTearsScript.SetupBounce(true, bounceAmount);
        }
        if (tearsType == TearsType.Pierce)
        {
            newTearsScript.SetupPierce(true, pierceAmount);
        }
        newTearsScript.SetupTears(finalDir, tearsGravity, gravityTime, tearsScale, tearsScaleSpeed, freezeTimeDuration, tearsDestroyDuration);
    }

    #region AimRegion
    public Vector2 AimDirection()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        gravityTime = Mathf.Abs(xInput * 0.25f + player.facingDir * 0.25f);

        Vector2 direction = new Vector2(player.facingDir + 0.5f * xInput, 1.5f * yInput);
        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
        dotsActive = _isActive;
    }

    public void DotsAim()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab,
                new Vector2(player.transform.position.x + player.facingDir * prefabOffset.x, player.transform.position.y + prefabOffset.y),
                Quaternion.identity,
                dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 velocity = new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y
        );

        Vector2 position = (Vector2)player.transform.position
                          + new Vector2(player.facingDir * prefabOffset.x, prefabOffset.y);

        if (t < gravityTime)
        {
            position += velocity * t;   //v0t
        }
        else
        {
            float tAfterGravity = t - gravityTime;
            position += velocity * t;
            position += 0.5f * (Physics2D.gravity * tearsGravity) * (tAfterGravity * tAfterGravity);
        }//v0t + 0.5at²

        return position;
    }
    #endregion
}
