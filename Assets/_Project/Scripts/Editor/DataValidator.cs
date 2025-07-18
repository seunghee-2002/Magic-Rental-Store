using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

using MagicRentalShop.Data;

namespace MagicRentalShop.Editor
{
    /// <summary>
    /// ScriptableObject 데이터 유효성 검증 인터페이스
    /// </summary>
    public interface IValidatable
    {
        bool IsValid();
        List<string> GetValidationErrors();
    }

    /// <summary>
    /// 모든 ScriptableObject 데이터의 유효성을 검증하는 에디터 툴
    /// </summary>
    public static class DataValidator
    {
        [MenuItem("Magic Rental Shop/Validate All Data", priority = 1)]
        public static void ValidateAllData()
        {
            Debug.Log("=== 데이터 검증 시작 ===");
            
            var validationResults = new List<ValidationResult>();
            
            // 각 데이터 타입별로 검증
            validationResults.AddRange(ValidateDataType<WeaponData>("무기"));
            validationResults.AddRange(ValidateDataType<CustomerData>("고객"));
            validationResults.AddRange(ValidateDataType<DungeonData>("던전"));
            validationResults.AddRange(ValidateDataType<MaterialData>("재료"));
            validationResults.AddRange(ValidateDataType<RecipeData>("레시피"));
            
            // 결과 출력
            DisplayValidationResults(validationResults);
        }

        [MenuItem("Magic Rental Shop/Validate Weapons Only", priority = 11)]
        public static void ValidateWeaponsOnly()
        {
            var results = ValidateDataType<WeaponData>("무기");
            DisplayValidationResults(results);
        }

        [MenuItem("Magic Rental Shop/Validate Customers Only", priority = 12)]
        public static void ValidateCustomersOnly()
        {
            var results = ValidateDataType<CustomerData>("고객");
            DisplayValidationResults(results);
        }

        [MenuItem("Magic Rental Shop/Validate Recipes Only", priority = 13)]
        public static void ValidateRecipesOnly()
        {
            var results = ValidateDataType<RecipeData>("레시피");
            DisplayValidationResults(results);
        }

        /// <summary>
        /// 특정 타입의 ScriptableObject들을 검증
        /// </summary>
        private static List<ValidationResult> ValidateDataType<T>(string typeName) where T : ScriptableObject
        {
            var results = new List<ValidationResult>();
            var assets = AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(asset => asset != null)
                .ToList();

            Debug.Log($"📋 {typeName} 데이터 검증 중... ({assets.Count}개)");

            foreach (var asset in assets)
            {
                var errors = ValidateAsset(asset);
                results.Add(new ValidationResult
                {
                    AssetName = asset.name,
                    AssetPath = AssetDatabase.GetAssetPath(asset),
                    TypeName = typeName,
                    IsValid = errors.Count == 0,
                    Errors = errors
                });
            }

            return results;
        }

        /// <summary>
        /// 개별 에셋 검증
        /// </summary>
        private static List<string> ValidateAsset(ScriptableObject asset)
        {
            var errors = new List<string>();

            // IValidatable 인터페이스 구현 확인
            if (asset is IValidatable validatable)
            {
                if (!validatable.IsValid())
                {
                    errors.AddRange(validatable.GetValidationErrors());
                }
                return errors;
            }

            // 기본 검증 (타입별)
            switch (asset)
            {
                case WeaponData weapon:
                    errors.AddRange(ValidateWeapon(weapon));
                    break;
                case CustomerData customer:
                    errors.AddRange(ValidateCustomer(customer));
                    break;
                case DungeonData dungeon:
                    errors.AddRange(ValidateDungeon(dungeon));
                    break;
                case MaterialData material:
                    errors.AddRange(ValidateMaterial(material));
                    break;
                case RecipeData recipe:
                    errors.AddRange(ValidateRecipe(recipe));
                    break;
            }

            return errors;
        }

        #region 타입별 검증 메서드

        private static List<string> ValidateWeapon(WeaponData weapon)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(weapon.id))
                errors.Add("ID가 비어있습니다");
            
            if (string.IsNullOrEmpty(weapon.weaponName))
                errors.Add("무기 이름이 비어있습니다");
            
            if (weapon.basePrice < 0)
                errors.Add("기본 가격이 음수입니다");
            
            if (weapon.icon == null)
                errors.Add("아이콘이 설정되지 않았습니다");

            // 전설 등급 검증
            if (weapon.grade == Grade.Legendary && weapon.secondaryElement == Element.None)
                errors.Add("전설 등급 무기는 보조 속성이 필요합니다");

            return errors;
        }

        private static List<string> ValidateCustomer(CustomerData customer)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(customer.id))
                errors.Add("ID가 비어있습니다");
            
            if (string.IsNullOrEmpty(customer.customerName))
                errors.Add("고객 이름이 비어있습니다");
            
            if (customer.customerIcon == null)
                errors.Add("고객 아이콘이 설정되지 않았습니다");

            return errors;
        }

        private static List<string> ValidateDungeon(DungeonData dungeon)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dungeon.id))
                errors.Add("ID가 비어있습니다");
            
            if (string.IsNullOrEmpty(dungeon.dungeonName))
                errors.Add("던전 이름이 비어있습니다");
            
            if (dungeon.baseGoldReward < 0)
                errors.Add("기본 골드 보상이 음수입니다");
            
            if (dungeon.icon == null)
                errors.Add("아이콘이 설정되지 않았습니다");

            return errors;
        }

        private static List<string> ValidateMaterial(MaterialData material)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(material.id))
                errors.Add("ID가 비어있습니다");
            
            if (string.IsNullOrEmpty(material.materialName))
                errors.Add("재료 이름이 비어있습니다");
            
            if (material.baseValue < 0)
                errors.Add("기본 가치가 음수입니다");
            
            if (material.icon == null)
                errors.Add("아이콘이 설정되지 않았습니다");

            if (material.dropRate <= 0 || material.dropRate > 1)
                errors.Add("드롭율은 0과 1 사이여야 합니다");

            return errors;
        }

        private static List<string> ValidateRecipe(RecipeData recipe)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(recipe.id))
                errors.Add("ID가 비어있습니다");
            
            if (string.IsNullOrEmpty(recipe.recipeName))
                errors.Add("레시피 이름이 비어있습니다");
            
            if (string.IsNullOrEmpty(recipe.resultWeaponID))
                errors.Add("결과 무기 ID가 비어있습니다");
            
            if (recipe.craftingCost < 0)
                errors.Add("제작 비용이 음수입니다");
            
            if (recipe.craftingDays <= 0)
                errors.Add("제작 일수는 1일 이상이어야 합니다");

            if (recipe.requiredMaterials == null || recipe.requiredMaterials.Count == 0)
                errors.Add("필요 재료가 설정되지 않았습니다");
            else
            {
                for (int i = 0; i < recipe.requiredMaterials.Count; i++)
                {
                    var material = recipe.requiredMaterials[i];
                    if (string.IsNullOrEmpty(material.materialID))
                        errors.Add($"재료 {i + 1}의 ID가 비어있습니다");
                    if (material.quantity <= 0)
                        errors.Add($"재료 {i + 1}의 수량이 0 이하입니다");
                }
            }

            return errors;
        }

        #endregion

        /// <summary>
        /// 검증 결과 출력
        /// </summary>
        private static void DisplayValidationResults(List<ValidationResult> results)
        {
            var validCount = results.Count(r => r.IsValid);
            var invalidCount = results.Count(r => !r.IsValid);

            Debug.Log($"✅ 검증 완료: 총 {results.Count}개 중 {validCount}개 유효, {invalidCount}개 오류");

            if (invalidCount > 0)
            {
                Debug.LogError("❌ 오류가 발견된 데이터들:");
                
                foreach (var result in results.Where(r => !r.IsValid))
                {
                    Debug.LogError($"🔴 [{result.TypeName}] {result.AssetName}:");
                    foreach (var error in result.Errors)
                    {
                        Debug.LogError($"  - {error}");
                    }
                    
                    // 에셋 선택 가능하도록 링크 제공
                    var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(result.AssetPath);
                    if (asset != null)
                    {
                        Debug.LogError($"  📁 경로: {result.AssetPath}", asset);
                    }
                }
            }
            else
            {
                Debug.Log("🎉 모든 데이터가 유효합니다!");
            }

            Debug.Log("=== 데이터 검증 완료 ===");
        }

        /// <summary>
        /// 검증 결과 데이터 구조
        /// </summary>
        private class ValidationResult
        {
            public string AssetName;
            public string AssetPath;
            public string TypeName;
            public bool IsValid;
            public List<string> Errors;
        }
    }
}