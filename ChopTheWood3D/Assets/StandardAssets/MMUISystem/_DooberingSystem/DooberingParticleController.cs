using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DooberingArgs
{
    public Vector3 SpawnPos { get; private set; }
    public Transform TargetTransform { get; private set; }
    public Sprite Sprite { get; private set; }
    public int DooberCount { get; private set; }

    public DooberingArgs(Vector3 spawnPos, Transform targetTransform, Sprite sprite, int dooberCount)
    {
        SpawnPos = spawnPos;
        TargetTransform = targetTransform;
        Sprite = sprite;
        DooberCount = dooberCount;
    }
}

public class DooberingParticleController : MonoBehaviour
{
    public UISpawnController SpawnController;

    public List<DooberingParticle> DeactiveDooberingList { get; private set; }
    public List<DooberingParticle> ActiveDooberingList { get; private set; }

    private void Awake()
    {
        ActiveDooberingList = new List<DooberingParticle>();
        DeactiveDooberingList = SpawnController.LoadSpawnables<DooberingParticle>();

        DeactiveDooberingList.ForEach(p => p.transform.localScale = new Vector3(1f, 1f, 1f));
    }

    private void Update()
    {
        foreach (var dooberParticle in ActiveDooberingList.ToList())
        {
            if (dooberParticle.IsReachedToTarget)
                DeactivateDoobering(dooberParticle);
        }
    }

    private void DeactivateDoobering(DooberingParticle activeParticle)
    {
        activeParticle.Deactivate();

        AddToDeactiveList(activeParticle);
    }

    public void ActivateDoobering(DooberingArgs args)
    {
        if (DeactiveDooberingList.Count == 0)
            return;

        DooberingParticle newDoobering = DeactiveDooberingList[0];

        newDoobering.Activate(args);

        AddToActiveList(newDoobering);
    }

    private void AddToActiveList(DooberingParticle newDoobering)
    {
        DeactiveDooberingList.Remove(newDoobering);
        ActiveDooberingList.Add(newDoobering);
    }

    private void AddToDeactiveList(DooberingParticle newDoobering)
    {
        ActiveDooberingList.Remove(newDoobering);
        DeactiveDooberingList.Add(newDoobering);
    }
}
