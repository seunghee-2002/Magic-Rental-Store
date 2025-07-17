using UnityEngine;
using System.Collections.Generic;

namespace MagicRentalShop.Data
{
    [CreateAssetMenu(fileName = "RecipeData", menuName = "Data/RecipeData")]
    public class RecipeData : ScriptableObject
    {
        [Header("기본 정보")]
        [Tooltip("고유 식별자")]
        public string id;
        
        [Tooltip("레시피 이름")]
        public string recipeName;
        
        [Tooltip("레시피 설명")]
        [TextArea(3, 5)]
        public string description;
        
        [Header("결과물")]
        [Tooltip("제작될 무기의 ID")]
        public string resultWeaponID;
        
        [Header("필요 재료")]
        [Tooltip("제작에 필요한 재료 목록")]
        public List<RequiredMaterial> requiredMaterials = new List<RequiredMaterial>();
        
        [Header("제작 조건")]
        [Tooltip("제작 비용 (골드)")]
        public int craftingCost;
        
        [Tooltip("제작 완료까지 소요 일수")]
        public int craftingDays = 1;
        
        [Tooltip("레시피 해금 조건 (일수)")]
        public int unlockDay = 1;
        
        [Header("시각적 요소")]
        [Tooltip("레시피 아이콘")]
        public Sprite icon;
        
        [Header("게임 밸런스")]
        [Tooltip("레시피 등급")]
        public Grade grade;
        
        [Tooltip("레시피 희귀도")]
        [Range(0.1f, 1.0f)]
        public float availability = 1.0f;
    }
    
    /// <summary>
    /// 레시피에 필요한 재료 정보
    /// </summary>
    [System.Serializable]
    public class RequiredMaterial
    {
        [Tooltip("필요한 재료의 ID")]
        public string materialID;
        
        [Tooltip("필요한 수량")]
        public int quantity = 1;
        
        /// <summary>
        /// 기본 생성자
        /// </summary>
        public RequiredMaterial() { }
        
        /// <summary>
        /// 재료 ID와 수량으로 생성
        /// </summary>
        /// <param name="materialID">재료 ID</param>
        /// <param name="quantity">필요 수량</param>
        public RequiredMaterial(string materialID, int quantity)
        {
            this.materialID = materialID;
            this.quantity = quantity;
        }
    }
}