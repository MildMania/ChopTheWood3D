using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridBasedFillBarScript : MonoBehaviour
{
    #region Events
    public Action<float> OnBarUpdate;
    private void FireOnBarUpdate(float clampedPassedTime)
    {
        OnBarUpdate?.Invoke(clampedPassedTime);
    }

    public Action OnBarUpdateComplete;
    private void FireOnBarUpdateComplete()
    {
        OnBarUpdateComplete?.Invoke();
    }

    public Action OnGridsUpdated;
    private void FireOnGridsUpdated()
    {
        OnGridsUpdated?.Invoke();
    }
    #endregion

    public Slider Slider;
    public HorizontalLayoutGroup HorLayoutGroup;
    public UISpawnController SpawnController;
    public RectTransform GridRectTrans;

    private List<RectTransform> _gridRectTransCollection = new List<RectTransform>();
    private int _gridPercValue;

    public void InitSlider()
    {
        Slider.wholeNumbers = true;
        Slider.value = 0;

        int roundedMaxValue = Mathf.FloorToInt(Slider.maxValue);

        Slider.maxValue = roundedMaxValue;

        _gridPercValue = 100 / roundedMaxValue;

        UpdateBarForGridCount();

        FillGridInstances();
    }

    public List<RectTransform> GetGrids()
    {
        return new List<RectTransform>(_gridRectTransCollection);
    }

    public void ResetAndSetGridCount(int gridCount)
    {
        Slider.maxValue = gridCount;

        UpdateBarForGridCount();

        FillGridInstances();
    }

    public void UpdateValueByPerc(int perc)
    {
        int gridCount = 0;

        if (perc == 100.0f)
            gridCount = Mathf.RoundToInt(Slider.maxValue);
        else if (perc > _gridPercValue)
            gridCount = Mathf.FloorToInt(perc * Slider.maxValue / 100.0f);
        else if (perc > 0.0f && perc < _gridPercValue)
            gridCount = 1;

        UpdateValueByGrid(gridCount);
    }

    public void UpdateValueByGrid(int activeGridCount)
    {
        Slider.value = activeGridCount;
    }

    private void UpdateBarForGridCount()
    {
        float barWidth = (GridRectTrans.rect.width * Slider.maxValue) + ((Slider.maxValue - 1) * HorLayoutGroup.spacing);

        ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
    }

    private void FillGridInstances()
    {
        if (Slider.maxValue < _gridRectTransCollection.Count)
            DestroyExcessGrids();
        else if (Slider.maxValue > _gridRectTransCollection.Count)
            CreateMissingGrids();

        FireOnGridsUpdated();
    }

    private void DestroyExcessGrids()
    {
        int countToDestroy = _gridRectTransCollection.Count - (int)Slider.maxValue;

        List<RectTransform> targetGridColl = _gridRectTransCollection.GetRange(_gridRectTransCollection.Count - countToDestroy, countToDestroy);
        for (int i = 0; i < countToDestroy; i++)
            Destroy(targetGridColl[i].gameObject);

        _gridRectTransCollection.RemoveRange(_gridRectTransCollection.Count - countToDestroy, countToDestroy);
    }

    private void CreateMissingGrids()
    {
        int countToCreate = (int)Slider.maxValue - _gridRectTransCollection.Count;

        List<RectTransform> newGridColl = SpawnController.LoadSpawnables<RectTransform>(countToCreate, true);

        _gridRectTransCollection.AddRange(newGridColl);
    }
}