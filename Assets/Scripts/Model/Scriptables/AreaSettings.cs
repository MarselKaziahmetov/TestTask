using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaNameSettings", menuName = "Scriptables/AreaSettings")]
public class AreaSettings : ScriptableObject
{
    public int maxObjects;
    public AreaObjectInfo[] gameObjectList;
}

[CustomEditor(typeof(AreaSettings))]
public class AreaSettingsEditor : Editor
{
    SerializedProperty gameObjectList;
    SerializedProperty maxObjects;

    void OnEnable()
    {
        gameObjectList = serializedObject.FindProperty("gameObjectList");
        maxObjects = serializedObject.FindProperty("maxObjects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Отображение максимального кол-ва объектов в инспектор
        EditorGUILayout.LabelField("Макс. кол-во объектов в области", EditorStyles.boldLabel);
        SerializedProperty maxElement = maxObjects;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(maxObjects, GUIContent.none, GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Объекты Данной Области", EditorStyles.boldLabel);

        // Отображение заголовка таблицы
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab", EditorStyles.boldLabel, GUILayout.Width(130));
        EditorGUILayout.LabelField("Is Limited", EditorStyles.boldLabel, GUILayout.Width(80));
        EditorGUILayout.LabelField("Кол-во копий", EditorStyles.boldLabel, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        // Отображение линии между строками таблицы
        EditorGUILayout.Space();
        Rect horizontalLine = EditorGUILayout.GetControlRect(false, 1);
        horizontalLine.height = 1;
        EditorGUI.DrawRect(horizontalLine, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space();

        // Отображение строк таблицы
        for (int i = 0; i < gameObjectList.arraySize; i++)
        {
            SerializedProperty element = gameObjectList.GetArrayElementAtIndex(i);
            SerializedProperty prefabProp = element.FindPropertyRelative("prefab");
            SerializedProperty isLimitedProp = element.FindPropertyRelative("isLimited");
            SerializedProperty numberOfCopiesProp = element.FindPropertyRelative("numberOfCopies");

            EditorGUILayout.BeginHorizontal();

            // Отображение поля для префаба
            EditorGUILayout.PropertyField(prefabProp, GUIContent.none, GUILayout.Width(130));

            // Отображение поля для isLimited
            Rect isLimitedRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.toggle, GUILayout.Width(80));
            EditorGUI.LabelField(new Rect(isLimitedRect.x, isLimitedRect.y, 30, isLimitedRect.height),"");
            isLimitedProp.boolValue = EditorGUI.Toggle(new Rect(isLimitedRect.x + 30, isLimitedRect.y, 20, isLimitedRect.height), isLimitedProp.boolValue);
            EditorGUI.LabelField(new Rect(isLimitedRect.x + 50, isLimitedRect.y, 30, isLimitedRect.height), "", EditorStyles.miniLabel);

            // Отображение поля количства копий
            EditorGUI.BeginDisabledGroup(!isLimitedProp.boolValue);
            numberOfCopiesProp.intValue = EditorGUILayout.IntField(numberOfCopiesProp.intValue, GUILayout.Width(100));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            // Отображение линии между строками таблицы
            EditorGUILayout.Space();
            horizontalLine = EditorGUILayout.GetControlRect(false, 1);
            horizontalLine.height = 1;
            EditorGUI.DrawRect(horizontalLine, new Color(0.5f, 0.5f, 0.5f, 1));
            EditorGUILayout.Space();
        }

        // Отображение кнопки Добавить элемент
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Добавить элемент"))
        {
            gameObjectList.arraySize++;
        }

        // Отображение кнопки Удалить последний элемент
        if (GUILayout.Button("Удалить последний элемент") && gameObjectList.arraySize > 0)
        {
            gameObjectList.arraySize--;
        }

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}