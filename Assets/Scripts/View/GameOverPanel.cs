using DG.Tweening;
using System;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    // ����������� ��� ���������� ���� ��������
    public static Action OnAllPanelsFilled;

    [SerializeField] private GameObject gameOverPanel;
    private RectTransform panelRect;

    private void Start()
    {
        panelRect = gameOverPanel.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        OnAllPanelsFilled += DoOnPanelFilled;
    }

    private void OnDisable()
    {
        OnAllPanelsFilled -= DoOnPanelFilled;
    }

    // ���������� ��� ������ OnAllPanelsFilled
    private void DoOnPanelFilled()
    {
        gameOverPanel.SetActive(true);
        
        // �������� ��������� ������ GameOver
        panelRect.DOAnchorPosY(panelRect.anchoredPosition.y - panelRect.rect.height - 20f, 1f).SetEase(Ease.InQuad).OnComplete(()=>
        {
            panelRect.DOAnchorPosY(panelRect.anchoredPosition.y + 20f, 0.5f).SetEase(Ease.OutQuad);
        });
    }
}
