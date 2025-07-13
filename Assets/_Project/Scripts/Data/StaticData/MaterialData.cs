using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    [CreateAssetMenu(fileName = "MaterialData", menuName = "Data/MaterialData")]
    public class MaterialData : ScriptableObject
    {
        [Header("기본 정보")]
        [Tooltip("고유 식별자")]
        public string id;
        
        [Tooltip("재료 이름")]
        public string materialName;
        
        [Tooltip("재료 설명")]
        [TextArea(3, 5)]
        public string description;
        
        [Header("속성")]
        [Tooltip("재료의 등급")]
        public Grade grade;
        
        [Header("시각적 요소")]
        [Tooltip("재료의 아이콘")]
        public Sprite icon;
        
        [Header("획득 정보")]
        [Tooltip("이 재료를 획득할 수 있는 던전 ID 목록")]
        public List<string> availableDungeonIDs = new List<string>();

        [Header("게임 밸런스")]
        [Tooltip("재료의 기본 가치")]
        public int baseValue = 1000;
        
        [Tooltip("재료의 희귀도 (낮을수록 희귀)")]
        [Range(0.01f, 1.0f)]
        public float dropRate = 0.5f;
    }
}
