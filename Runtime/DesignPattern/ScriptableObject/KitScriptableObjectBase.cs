using UnityEngine;

namespace Kit.Data
{
    [CreateAssetMenu(menuName = "Kit/Data", fileName = "KitDataObject")]
    public abstract class KitScriptableObjectBase : ScriptableObject 
    {
        public abstract bool IsEditorFile { get; }
    }
}