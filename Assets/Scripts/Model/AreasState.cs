using System;
using UnityEngine;

public class AreasState : MonoBehaviour
{
    // Срабатывает при заполнении одной из областей
    public static Action OnAreaFilled;

    [SerializeField] private int areaCount;
    private int filledAreaCount = 0;

    private void OnEnable()
    {
        OnAreaFilled += DoOnPanelFilled;
    }

    private void OnDisable()
    {
        OnAreaFilled -= DoOnPanelFilled;
    }

    // Вызывается при инвоке OnAreaFilled
    private void DoOnPanelFilled()
    {
        filledAreaCount++;
        if (filledAreaCount == areaCount)
        {
            GameOverPanel.OnAllPanelsFilled?.Invoke();
        }
    }
}
