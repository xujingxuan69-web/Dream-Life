using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    [Header("Flash Fx")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material originalMat;

    private List<SpriteRenderer> allSR = new List<SpriteRenderer>();


    [Header("Stun Vortex")]
    [SerializeField] private GameObject stunVortex;
    [SerializeField] private float heightOffset = 0.3f;

    private Collider2D entityCollider;
    private GameObject currentVortex;


    private void Start()
    {
        entityCollider = GetComponent<Collider2D>();
        GetComponentsInChildren(true, allSR);
        if (allSR.Count > 0)
        {
            originalMat = allSR[0].material;
        }
    }

    public void MakeTransparent(bool _Transparent)  //!瞬间隐形，未使用
    {
        if (_Transparent)
        {
            for (int i = 0; i < allSR.Count; i++)  //for的性能略优于foreach，所以在此使用
                allSR[i].color = Color.clear;
        }
        else
        {
            for (int i = 0; i < allSR.Count; i++)
                allSR[i].color = Color.white;
        }
    }

    private IEnumerator FlashFx()
    {
        foreach (var sr in allSR)
        {
            if (sr != null) sr.material = hitMat;
        }

        yield return new WaitForSeconds(flashDuration);

        foreach (var sr in allSR)
        {
            if (sr != null) sr.material = originalMat;
        }
    }


    private void ShowStunVortex()
    {
        if (stunVortex == null||entityCollider == null)
            return;

        if (currentVortex != null)
            Destroy(currentVortex);


        Vector2 vortexPos = new Vector2(entityCollider.bounds.center.x,entityCollider.bounds.max.y + heightOffset);

        currentVortex = Instantiate(stunVortex, vortexPos, Quaternion.identity, transform);
    }
    
    private void DestoryStunVortex()
    {
        if (currentVortex != null)
        {
            Destroy(currentVortex);
            currentVortex = null;
        }

        
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
