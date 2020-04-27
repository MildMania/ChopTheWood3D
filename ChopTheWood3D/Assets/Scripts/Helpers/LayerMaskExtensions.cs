using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool IsInLayerMask(this LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));

    }
}
