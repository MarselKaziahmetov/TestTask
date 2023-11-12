using System;
using UnityEngine;

public class AreasState : MonoBehaviour
{
    // ����������� ��� ���������� ����� �� ��������
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

    // ���������� ��� ������ OnAreaFilled
    private void DoOnPanelFilled()
    {
        filledAreaCount++;
        if (filledAreaCount == areaCount)
        {
            GameOverPanel.OnAllPanelsFilled?.Invoke();
        }
    }
}
