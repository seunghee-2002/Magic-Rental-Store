using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterial", menuName = "Data/Material")]
public class MaterialData : ScriptableObject
{
    public string materialName; // 재료 이름
    public string desc; // 재료 설명
    public Sprite icon; // 재료 아이콘
}
