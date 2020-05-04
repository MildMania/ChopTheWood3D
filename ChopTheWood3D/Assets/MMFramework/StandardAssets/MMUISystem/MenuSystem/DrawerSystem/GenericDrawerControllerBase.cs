using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISpawnController))]
public abstract class GenericDrawerControllerBase<T> : DrawerControllerBase<T>
    where T : IPLDBase
{
    private UISpawnController _spawnController;
    protected UISpawnController SpawnController
    {
        get
        {
            if(_spawnController == null)
                _spawnController = GetComponent<UISpawnController>();

            return _spawnController;
        }
    }

    public override void ActivateListeners()
    {
    }

    public override void DeactivateListeners()
    {
    }

    public virtual void GenerateDrawers(int count, bool activateAll = true)
    {
        DestroyDrawers();

        _drawerList = new List<DrawerBase<T>>(SpawnController.LoadSpawnables<DrawerBase<T>>(count, activateAll));

        ResetDrawers();

        ActivateDrawerListeners();
    }

    public virtual void AddDrawers(int count, bool activateAll = true)
    {
        List<DrawerBase<T>> addDrawers = new List<DrawerBase<T>>(SpawnController.LoadSpawnables<DrawerBase<T>>(count, activateAll));

        if (_drawerList == null)
            _drawerList = new List<DrawerBase<T>>();

        _drawerList.AddRange(addDrawers);

        addDrawers.ForEach(d => d.ResetDrawer());

        addDrawers.ForEach(d => d.ActivateListeners());
    }

    public virtual void DestroyDrawers()
    {
        if(_drawerList != null)
        {
            for(int i = _drawerList.Count - 1; i >= 0; i--)
                DestroyDrawer(_drawerList[i]);
        }
    }

    public void DestroyDrawer(DrawerBase<T> drawer)
    {
        DeactivateDrawerListeners(drawer);

        _drawerList.Remove(drawer);

        Destroy(drawer.gameObject);
    }
}
