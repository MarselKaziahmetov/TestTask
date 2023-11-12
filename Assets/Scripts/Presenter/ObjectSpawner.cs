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
    /// Срабатывает при клике на зону
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        SpawnObject(eventData);
    }

    /// <summary>
    /// Инициализирует локальные переменные информацией из Scriptable
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
    /// Спавнит случайный объект из оставшихся
    /// </summary>
    /// <param name="eventData"></param>
    private void SpawnObject(PointerEventData eventData)
    {
        // Переводим экранные координаты в локальные
        RectTransform parentRectTransform = transform as RectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, null, out localPoint);

        //Проверяем заполнена ли зона. В противном случае инвокает ивент на заполнение одной из зон
        if (remainingObjectsCount > 0 && !isFilled)
        {
            GameObject objectToSpawn = SelectRandomObject();

            if (objectToSpawn)
            {
                remainingObjectsCount--;

                // Спавним префаб в месте нажатия
                GameObject spawnedObject = Instantiate(objectToSpawn, transform);
                spawnedObject.transform.localPosition = localPoint;
                
                // Задаем начальный масштаб объекта, а потом анимируем появление объекта
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
    /// Выбирает случайный объект зоны
    /// </summary>
    /// <returns>Prefab объекта</returns>
    private GameObject SelectRandomObject()
    {
        // Ищет и возвращает объект с лимитированным количеством
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
        // Ищет среди не лимитированных
        for (int i = 0; i < localAreaInfo.Length; i++)
        {
            index = Random.Range(0, localAreaInfo.Length);

            // Ищем по порядку, на случай если рандомно не найден 
            if (!localAreaInfo[i].isLimited)
            {
                isFinded = true;
                tempIndex = i;
            }

            // Если найден рандомно, возвращем его
            if (!localAreaInfo[index].isLimited)
            {
                return localAreaInfo[index].prefab;
            }
        }
        
        // Если рандомно не найде, то берет тот, который запомнил по порядку
        if (isFinded)
        {
            return localAreaInfo[tempIndex].prefab;
        }

        // Если ни одного объекта не найдено, то возвращаем null
        // И инвокаем ивент на заполнение одной из зон
        isFilled = true;
        AreasState.OnAreaFilled?.Invoke();
        return null;
    }
}
