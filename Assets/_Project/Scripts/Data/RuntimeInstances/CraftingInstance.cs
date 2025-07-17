using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 제작 인스턴스 클래스
    /// </summary>
    [System.Serializable]
    public class CraftingInstance
    {
        public string recipeID;          // 레시피 ID
        public int startDate;            // 제작 시작 날짜
        public int completionDate;       // 완료 예정 날짜
        public bool isCompleted;         // 완료 여부
        public string resultWeaponID;    // 결과 무기 ID
        
        public CraftingInstance()
        {
            isCompleted = false;
        }
        
        public CraftingInstance(string recipe, int currentDay, int duration)
        {
            recipeID = recipe;
            startDate = currentDay;
            completionDate = currentDay + duration;
            isCompleted = false;
        }
        
        public bool IsReady(int currentDay)
        {
            return currentDay >= completionDate && !isCompleted;
        }
        
        public int GetRemainingDays(int currentDay)
        {
            return Mathf.Max(0, completionDate - currentDay);
        }
    }
}
