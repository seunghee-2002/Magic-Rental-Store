using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MagicRentalShop.Data
{
    /// <summary>
    /// 게임의 모든 정적 데이터를 포함하는 컨테이너
    /// </summary>
    /// <remarks>
    /// 이 스크립트는 게임의 모든 정적 데이터를 관리합니다.
    /// 각 데이터는 별도의 ScriptableObject로 정의되어야 합니다.
    /// </remarks>
    [CreateAssetMenu(fileName = "GameDataContainer", menuName = "Game/GameDataContainer")]
    public class GameDataContainer : ScriptableObject
    {
        [Header("모든 게임 데이터")]
        public WeaponData[] weaponDatas;
        public CustomerData[] customerDatas;
        public DungeonData[] dungeonDatas;
        public MaterialData[] materialDatas;
        public RecipeData[] recipeDatas;
        public DailyEventData[] dailyEventDatas;

        [Header("자동 로드 설정")]
        [Tooltip("Assets/_Project/Data/StaticData 폴더 기준으로 데이터를 자동 로드합니다")]
        public string basePath = "Assets/_Project/Data/StaticData";

#if UNITY_EDITOR
        [Space(10)]
        [Header("에디터 도구")]
        [SerializeField] private bool showAutoLoadTools = true;
        
        /// <summary>
        /// 모든 데이터를 자동으로 로드하는 메서드 (인스펙터 버튼용)
        /// </summary>
        public void LoadAllData()
        {
            Debug.Log("=== GameDataContainer 자동 로드 시작 ===");
            
            weaponDatas = LoadDataFromFolder<WeaponData>("Weapons");
            customerDatas = LoadDataFromFolder<CustomerData>("Customers");
            dungeonDatas = LoadDataFromFolder<DungeonData>("Dungeons");
            materialDatas = LoadDataFromFolder<MaterialData>("Materials");
            recipeDatas = LoadDataFromFolder<RecipeData>("Recipes");
            dailyEventDatas = LoadDataFromFolder<DailyEventData>("Events");
            
            // 에디터에서 변경사항을 저장
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"=== 로드 완료 ===\n" +
                     $"- 무기: {weaponDatas?.Length ?? 0}개\n" +
                     $"- 고객: {customerDatas?.Length ?? 0}개\n" +
                     $"- 던전: {dungeonDatas?.Length ?? 0}개\n" +
                     $"- 재료: {materialDatas?.Length ?? 0}개\n" +
                     $"- 레시피: {recipeDatas?.Length ?? 0}개\n" +
                     $"- 이벤트: {dailyEventDatas?.Length ?? 0}개");
        }

        /// <summary>
        /// 특정 폴더에서 T 타입의 ScriptableObject들을 모두 로드
        /// </summary>
        private T[] LoadDataFromFolder<T>(string folderName) where T : ScriptableObject
        {
            string folderPath = $"{basePath}/{folderName}";
            
            // 폴더 존재 확인
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogWarning($"폴더를 찾을 수 없습니다: {folderPath}");
                return new T[0];
            }

            // 해당 폴더의 모든 하위 폴더에서 T 타입의 에셋을 찾음
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });
            
            List<T> dataList = new List<T>();
            
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T data = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                
                if (data != null)
                {
                    dataList.Add(data);
                    Debug.Log($"로드됨: {assetPath}");
                }
            }
            
            // ID 순으로 정렬 (ID가 있는 경우)
            if (typeof(T) == typeof(WeaponData))
                return dataList.Cast<WeaponData>().OrderBy(x => x.id).Cast<T>().ToArray();
            else if (typeof(T) == typeof(CustomerData))
                return dataList.Cast<CustomerData>().OrderBy(x => x.id).Cast<T>().ToArray();
            else if (typeof(T) == typeof(DungeonData))
                return dataList.Cast<DungeonData>().OrderBy(x => x.id).Cast<T>().ToArray();
            else if (typeof(T) == typeof(MaterialData))
                return dataList.Cast<MaterialData>().OrderBy(x => x.id).Cast<T>().ToArray();
            
            return dataList.ToArray();
        }

        /// <summary>
        /// 특정 타입의 데이터만 다시 로드
        /// </summary>
        public void ReloadWeapons() => weaponDatas = LoadDataFromFolder<WeaponData>("Weapons");
        public void ReloadCustomers() => customerDatas = LoadDataFromFolder<CustomerData>("Customers");
        public void ReloadDungeons() => dungeonDatas = LoadDataFromFolder<DungeonData>("Dungeons");
        public void ReloadMaterials() => materialDatas = LoadDataFromFolder<MaterialData>("Materials");
        public void ReloadRecipes() => recipeDatas = LoadDataFromFolder<RecipeData>("Recipes");
        public void ReloadEvents() => dailyEventDatas = LoadDataFromFolder<DailyEventData>("Events");

        /// <summary>
        /// 모든 데이터 배열을 초기화
        /// </summary>
        public void ClearAllData()
        {
            weaponDatas = new WeaponData[0];
            customerDatas = new CustomerData[0];
            dungeonDatas = new DungeonData[0];
            materialDatas = new MaterialData[0];
            recipeDatas = new RecipeData[0];
            dailyEventDatas = new DailyEventData[0];
            
            EditorUtility.SetDirty(this);
            Debug.Log("모든 데이터가 초기화되었습니다.");
        }

        /// <summary>
        /// 데이터 유효성 검사
        /// </summary>
        public void ValidateData()
        {
            Debug.Log("=== 데이터 유효성 검사 시작 ===");
            
            ValidateArray(weaponDatas, "WeaponData");
            ValidateArray(customerDatas, "CustomerData");
            ValidateArray(dungeonDatas, "DungeonData");
            ValidateArray(materialDatas, "MaterialData");
            ValidateArray(recipeDatas, "RecipeData");
            ValidateArray(dailyEventDatas, "DailyEventData");
            
            Debug.Log("=== 데이터 유효성 검사 완료 ===");
        }

        private void ValidateArray<T>(T[] array, string typeName) where T : ScriptableObject
        {
            if (array == null)
            {
                Debug.LogError($"{typeName} 배열이 null입니다!");
                return;
            }

            int nullCount = 0;
            int duplicateIdCount = 0;
            HashSet<string> ids = new HashSet<string>();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    nullCount++;
                    continue;
                }

                // ID 중복 검사 (ID 필드가 있는 타입들만)
                if (array[i] is WeaponData weapon)
                {
                    if (!string.IsNullOrEmpty(weapon.id))
                    {
                        if (!ids.Add(weapon.id))
                            duplicateIdCount++;
                    }
                }
                else if (array[i] is CustomerData customer)
                {
                    if (!string.IsNullOrEmpty(customer.id))
                    {
                        if (!ids.Add(customer.id))
                            duplicateIdCount++;
                    }
                }
                // 다른 타입들도 필요시 추가...
            }

            string message = $"{typeName}: {array.Length}개 (null: {nullCount}개";
            if (duplicateIdCount > 0)
                message += $", 중복 ID: {duplicateIdCount}개";
            message += ")";

            if (nullCount > 0 || duplicateIdCount > 0)
                Debug.LogWarning(message);
            else
                Debug.Log(message);
        }
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// GameDataContainer용 커스텀 에디터
    /// </summary>
    [CustomEditor(typeof(GameDataContainer))]
    public class GameDataContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("데이터 로드 도구", EditorStyles.boldLabel);
            
            GameDataContainer container = (GameDataContainer)target;
            
            EditorGUILayout.BeginVertical("box");
            
            // 전체 로드 버튼
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("모든 데이터 자동 로드", GUILayout.Height(30)))
            {
                container.LoadAllData();
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.Space(5);
            
            // 개별 로드 버튼들
            EditorGUILayout.LabelField("개별 데이터 로드:", EditorStyles.miniBoldLabel);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("무기")) container.ReloadWeapons();
            if (GUILayout.Button("고객")) container.ReloadCustomers();
            if (GUILayout.Button("던전")) container.ReloadDungeons();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("재료")) container.ReloadMaterials();
            if (GUILayout.Button("레시피")) container.ReloadRecipes();
            if (GUILayout.Button("이벤트")) container.ReloadEvents();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            
            // 유틸리티 버튼들
            EditorGUILayout.BeginHorizontal();
            
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("데이터 검증"))
            {
                container.ValidateData();
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("전체 초기화"))
            {
                if (EditorUtility.DisplayDialog("확인", "모든 데이터를 초기화하시겠습니까?", "확인", "취소"))
                {
                    container.ClearAllData();
                }
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            
            // 현재 로드된 데이터 개수 표시
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("현재 로드된 데이터:", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField($"무기: {container.weaponDatas?.Length ?? 0}개");
            EditorGUILayout.LabelField($"고객: {container.customerDatas?.Length ?? 0}개");
            EditorGUILayout.LabelField($"던전: {container.dungeonDatas?.Length ?? 0}개");
            EditorGUILayout.LabelField($"재료: {container.materialDatas?.Length ?? 0}개");
            EditorGUILayout.LabelField($"레시피: {container.recipeDatas?.Length ?? 0}개");
            EditorGUILayout.LabelField($"이벤트: {container.dailyEventDatas?.Length ?? 0}개");
            EditorGUI.indentLevel--;
        }
    }
#endif
}