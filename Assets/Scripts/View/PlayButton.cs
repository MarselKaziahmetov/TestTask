using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    private RectTransform panelRect;
    private Button playButton;

    void Start()
    {
        playButton = GetComponent<Button>();
        playButton.onClick.AddListener(HidePanel);

        panelRect = startPanel.GetComponent<RectTransform>();
    }

    // ���������� ��� ������� �� ������ Play
    private void HidePanel()
    {
        // ������� ������������ �����
        panelRect.DOAnchorPosY(panelRect.anchoredPosition.y + 20f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // ����� �������� �����, ������� ����
            panelRect.DOAnchorPosY(panelRect.anchoredPosition.y - panelRect.rect.height, 1f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                // ����� ���������� �������� ������ ������ ����������
                startPanel.SetActive(false);
            });
        });
    }
}
