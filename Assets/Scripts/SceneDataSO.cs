using UnityEditor;
using UnityEngine;

public class SceneDataSO : ScriptableObject
{
    [field: SerializeField] public SceneAsset SceneAsset { get; private set; }

    public string SceneName;
}
