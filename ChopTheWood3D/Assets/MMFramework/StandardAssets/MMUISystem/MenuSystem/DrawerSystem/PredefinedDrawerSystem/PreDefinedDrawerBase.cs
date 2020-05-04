using UnityEngine;

public abstract class PreDefinedDrawerBase : MonoBehaviour
{
    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public abstract void ActivateListeners();
    public abstract void DeactivateListeners();
}
