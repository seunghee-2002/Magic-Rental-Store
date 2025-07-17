using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    [CreateAssetMenu(fileName = "DungeonData", menuName = "Data/DungeonData")]
    public class DungeonData : ScriptableObject
    {
        [Header("기본 정보")]
        [Tooltip("고유 식별자")]
        public string id;
        
        [Tooltip("던전 이름")]
        public string dungeonName;

        [Tooltip("던전 설명")]
        [TextArea(3, 5)]
        public string description;
        
        [Header("속성")]
        [Tooltip("던전 등급")]
        public Grade grade;

        [Tooltip("던전 속성")]
        public Element element;
        
        [Header("시각적 요소")]
        [Tooltip("던전 아이콘")]
        public Sprite icon;

        [Header("경제")]
        [Tooltip("던전 기본 보상")]
        public int baseGoldReward;

        [Tooltip("이 던전에서 획득 가능한 재료 ID 목록")]
        public List<string> dropMaterialIDs = new List<string>();
    }
}