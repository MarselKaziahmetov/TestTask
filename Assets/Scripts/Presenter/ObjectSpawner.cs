using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawner : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AreaSettings areaSettings;

    private int remainingObjectsCount;
    private AreaObjectInfo[] localAreaInfo;
    private bool isFilled;

    private void Start()
    {
        InitializeLocalAreaInfo();

        isFilled = false;
    }

    /// <summary>
    /// ����������� ��� ����� �� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        SpawnObject(eventData);
    }

    /// <summary>
    /// �������������� ��������� ���������� ����������� �� Scriptable
    /// </summary>
    private void InitializeLocalAreaInfo()
    {
        localAreaInfo = new AreaObjectInfo[areaSettings.gameObjectList.Length];

        remainingObjectsCount = areaSettings.maxObjects;
        for (int i = 0; i < areaSettings.gameObjectList.Length; i++)
        {
            localAreaInfo[i] = new AreaObjectInfo
            {
                prefab = areaSettings.gameObjectList[i].prefab,
                isLimited = areaSettings.gameObjectList[i].isLimited,
                numberOfCopies = areaSettings.gameObjectList[i].numberOfCopies
            };
        }
    }

    /// <summary>
    /// ������� ��������� ������ �� ����������
    /// </summary>
    /// <param name="eventData"></param>
    private void SpawnObject(PointerEventData eventData)
    {
        // ��������� �������� ���������� � ���������
        RectTransform parentRectTransform = transform as RectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, null, out localPoint);

        //��������� ��������� �� ����. � ��������� ������ �������� ����� �� ���������� ����� �� ���
        if (remainingObjectsCount > 0 && !isFilled)
        {
            GameObject objectToSpawn = SelectRandomObject();

            if (objectToSpawn)
            {
                remainingObjectsCount--;

                // ������� ������ � ����� �������
                GameObject spawnedObject = Instantiate(objectToSpawn, transform);
                spawnedObject.transform.localPosition = localPoint;
                
                // ������ ��������� ������� �������, � ����� ��������� ��������� �������
                spawnedObject.transform.localScale = Vector3.zero;
                spawnedObject.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
            }
        }
        else if (remainingObjectsCount <= 0 && !isFilled)
        {
            isFilled = true;
            AreasState.OnAreaFilled?.Invoke();
        }
    }

    /// <summary>
    /// �������� ��������� ������ ����
    /// </summary>
    /// <returns>Prefab �������</returns>
    private GameObject SelectRandomObject()
    {
        // ���� � ���������� ������ � �������������� �����������
        for (int i = 0; i < localAreaInfo.Length; i++)
        {
            if (localAreaInfo[i].isLimited)
            {
                if (localAreaInfo[i].numberOfCopies > 0)
                {
                    localAreaInfo[i].numberOfCopies--;
                    return localAreaInfo[i].prefab;
                }
            }
        }

        int index = 0;
        int tempIndex = 0;
        bool isFinded = false;
        // ���� ����� �� ��������������
        for (int i = 0; i < localAreaInfo.Length; i++)
        {
            index = Random.Range(0, localAreaInfo.Length);

            // ���� �� �������, �� ������ ���� �������� �� ������ 
            if (!localAreaInfo[i].isLimited)
            {
                isFinded = true;
                tempIndex = i;
            }

            // ���� ������ ��������, ��������� ���
            if (!localAreaInfo[index].isLimited)
            {
                return localAreaInfo[index].prefab;
            }
        }
        
        // ���� �������� �� �����, �� ����� ���, ������� �������� �� �������
        if (isFinded)
        {
            return localAreaInfo[tempIndex].prefab;
        }

        // ���� �� ������ ������� �� �������, �� ���������� null
        // � �������� ����� �� ���������� ����� �� ���
        isFilled = true;
        AreasState.OnAreaFilled?.Invoke();
        return null;
    }
}
