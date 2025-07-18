using System;
using UnityEngine;

using MagicRentalShop.Core;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 대장간 제작 진행 상황을 관리하는 런타임 인스턴스
    /// </summary>
    [System.Serializable]
    public class CraftingInstance
    {
        [Header("제작 정보")]
        [Tooltip("사용된 레시피 데이터")]
        public RecipeData recipe;
        
        [Tooltip("결과 무기")]
        public WeaponData resultWeapon;
        
        [Tooltip("제작 시작 날짜")]
        public int startDate;
        
        [Tooltip("제작 완료 예정 날짜")]
        public int completionDate;
        
        [Tooltip("제작이 완료되었는지 여부")]
        public bool isCompleted;
        
        [Header("고유 식별자")]
        [Tooltip("제작 인스턴스 고유 ID")]
        public string craftingID;

        /// <summary>
        /// 제작 시작 생성자
        /// </summary>
        public CraftingInstance(RecipeData recipe, int craftingDuration)
        {
            this.recipe = recipe;
            this.resultWeapon = DataManager.Instance.GetWeaponData(recipe.resultWeaponID);
            this.startDate = GameController.Instance != null ? GameController.Instance.CurrentDay : 0;
            this.completionDate = this.startDate + craftingDuration;
            this.isCompleted = false;
            this.craftingID = GenerateCraftingID();
        }

        /// <summary>
        /// 기본 생성자 (역직렬화용)
        /// </summary>
        public CraftingInstance()
        {
            craftingID = GenerateCraftingID();
            isCompleted = false;
        }

        /// <summary>
        /// 제작 완료까지 남은 일수 반환
        /// </summary>
        public int GetRemainingDays()
        {
            if (isCompleted) return 0;
            
            int currentDay = GameController.Instance != null ? GameController.Instance.CurrentDay : startDate;
            int remaining = completionDate - currentDay;
            return Mathf.Max(0, remaining);
        }

        /// <summary>
        /// 제작 진행률 반환 (0.0 ~ 1.0)
        /// </summary>
        public float GetProgress()
        {
            if (isCompleted) return 1.0f;
            
            int totalDuration = completionDate - startDate;
            if (totalDuration <= 0) return 1.0f;
            
            int currentDay = GameController.Instance != null ? GameController.Instance.CurrentDay : startDate;
            int elapsedDays = currentDay - startDate;
            
            return Mathf.Clamp01((float)elapsedDays / totalDuration);
        }

        /// <summary>
        /// 제작이 완료되었는지 확인
        /// </summary>
        public bool IsReadyToComplete()
        {
            if (isCompleted) return true;
            
            int currentDay = GameController.Instance != null ? GameController.Instance.CurrentDay : startDate;
            return currentDay >= completionDate;
        }

        /// <summary>
        /// 제작 완료 처리
        /// </summary>
        public WeaponInstance CompleteeCrafting()
        {
            if (!IsReadyToComplete()) return null;
            
            isCompleted = true;
            
            // 레시피에서 결과 무기 생성
            if (recipe?.resultWeaponID != null)
            {
                if (resultWeapon == null)
                {
                    Debug.LogError($"Weapon {resultWeapon} has not exist!");
                    return null;
                }
                var completedWeapon = new WeaponInstance(resultWeapon);
                Debug.Log($"Crafting completed: {completedWeapon.GetDisplayName()}");
                return completedWeapon;
            }
            
            Debug.LogError($"Recipe {recipe?.id} has no result weapon!");
            return null;
        }

        /// <summary>
        /// 제작될 무기 이름 반환
        /// </summary>
        public string GetResultWeaponName() => resultWeapon?.weaponName ?? "Unknown Weapon";

        /// <summary>
        /// 제작될 무기 아이콘 반환
        /// </summary>
        public Sprite GetResultWeaponIcon() => resultWeapon?.icon ?? null;

        /// <summary>
        /// 제작될 무기 등급 반환
        /// </summary>
        public Grade GetResultWeaponGrade() => resultWeapon?.grade ?? Grade.Common;

        /// <summary>
        /// 제작될 무기 속성 반환
        /// </summary>
        public Element GetResultWeaponElement() => resultWeapon?.element ?? Element.None;

        /// <summary>
        /// 레시피 이름 반환
        /// </summary>
        public string GetRecipeName() => recipe?.recipeName ?? "Unknown Recipe";

        /// <summary>
        /// 레시피 ID 반환 (저장/로드용)
        /// </summary>
        public string GetRecipeID() => recipe?.id ?? "unknown_recipe";

        /// <summary>
        /// 제작 상태 문자열 반환 (UI 표시용)
        /// </summary>
        public string GetStatusText()
        {
            if (isCompleted)
            {
                return "제작 완료!";
            }
            else
            {
                int remaining = GetRemainingDays();
                if (remaining <= 0)
                {
                    return "완료 대기중";
                }
                else
                {
                    return $"제작중... ({remaining}일 남음)";
                }
            }
        }

        /// <summary>
        /// 데이터 유효성 검사
        /// </summary>
        public bool IsValid()
        {
            return recipe != null && 
                   resultWeapon != null && 
                   !string.IsNullOrEmpty(craftingID) &&
                   completionDate >= startDate;
        }

        /// <summary>
        /// 고유 제작 ID 생성
        /// </summary>
        private string GenerateCraftingID()
        {
            return "crafting_" + Guid.NewGuid().ToString("N")[..8];
        }

        /// <summary>
        /// 디버그용 문자열 표현
        /// </summary>
        public override string ToString()
        {
            return $"CraftingInstance[{craftingID}] {GetRecipeName()} -> {GetResultWeaponName()} | {GetStatusText()}";
        }
    }

    /// <summary>
    /// 저장/로드용 CraftingInstance 데이터
    /// </summary>
    [System.Serializable]
    public class CraftingInstanceSaveData
    {
        [Tooltip("사용된 레시피 ID")]
        public string recipeID;
        
        [Tooltip("제작 시작 날짜")]
        public int startDate;
        
        [Tooltip("제작 완료 예정 날짜")]
        public int completionDate;
        
        [Tooltip("제작 완료 여부")]
        public bool isCompleted;
        
        [Tooltip("제작 인스턴스 고유 ID")]
        public string craftingID;

        /// <summary>
        /// CraftingInstance에서 저장 데이터 생성
        /// </summary>
        public CraftingInstanceSaveData(CraftingInstance crafting)
        {
            recipeID = crafting.GetRecipeID();
            startDate = crafting.startDate;
            completionDate = crafting.completionDate;
            isCompleted = crafting.isCompleted;
            craftingID = crafting.craftingID;
        }

        /// <summary>
        /// JSON 역직렬화용 기본 생성자
        /// </summary>
        public CraftingInstanceSaveData() { }

        /// <summary>
        /// 저장 데이터에서 CraftingInstance 복원
        /// </summary>
        public CraftingInstance ToCraftingInstance(RecipeData recipe)
        {
            var crafting = new CraftingInstance
            {
                recipe = recipe,
                startDate = startDate,
                completionDate = completionDate,
                isCompleted = isCompleted,
                craftingID = craftingID
            };

            return crafting;
        }
    }
}