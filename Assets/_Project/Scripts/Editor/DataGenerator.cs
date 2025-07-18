using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

using MagicRentalShop;
using MagicRentalShop.Data;

public class DataGenerator : EditorWindow
{
    [MenuItem("Tools/Data Generator")]
    public static void ShowWindow()
    {
        GetWindow<DataGenerator>("Data Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("더미 데이터 생성기", EditorStyles.boldLabel);
        
        // 경고 메시지
        EditorGUILayout.HelpBox("주의: 기존 데이터가 있으면 삭제 후 새로 생성됩니다.", MessageType.Warning);
        
        if (GUILayout.Button("무기 데이터 생성"))
        {
            if (EditorUtility.DisplayDialog("확인", "기존 무기 데이터를 모두 삭제하고 새로 생성하시겠습니까?", "생성", "취소"))
            {
                GenerateWeaponData();
            }
        }
        
        if (GUILayout.Button("고객 데이터 생성"))
        {
            if (EditorUtility.DisplayDialog("확인", "기존 고객 데이터를 모두 삭제하고 새로 생성하시겠습니까?", "생성", "취소"))
            {
                GenerateCustomerData();
            }
        }
        
        if (GUILayout.Button("던전 데이터 생성"))
        {
            if (EditorUtility.DisplayDialog("확인", "기존 던전 데이터를 모두 삭제하고 새로 생성하시겠습니까?", "생성", "취소"))
            {
                GenerateDungeonData();
            }
        }
        
        if (GUILayout.Button("재료 데이터 생성"))
        {
            if (EditorUtility.DisplayDialog("확인", "기존 재료 데이터를 모두 삭제하고 새로 생성하시겠습니까?", "생성", "취소"))
            {
                GenerateMaterialData();
            }
        }

        if (GUILayout.Button("레시피 데이터 생성"))
        {
            if (EditorUtility.DisplayDialog("확인", "기존 레시피 데이터를 모두 삭제하고 새로 생성하시겠습니까?", "생성", "취소"))
            {
                GenerateRecipeData();
            }
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("재료-던전 연결"))
        {
            LinkMaterialsToDungeons();
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("모든 데이터 생성"))
        {
            if (EditorUtility.DisplayDialog("확인", "모든 기존 데이터를 삭제하고 새로 생성하시겠습니까?", "생성", "취소"))
            {
                GenerateAllData();
            }
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("모든 데이터 삭제"))
        {
            if (EditorUtility.DisplayDialog("경고", "모든 더미 데이터를 삭제하시겠습니까? 되돌릴 수 없습니다!", "삭제", "취소"))
            {
                DeleteAllData();
            }
        }
    }

    private void GenerateAllData()
    {
        GenerateWeaponData();
        GenerateCustomerData();
        GenerateDungeonData();
        GenerateMaterialData();
        // 무기와 재료 데이터 생성 완료 후 레시피 생성
        EditorApplication.delayCall += () => {
            GenerateRecipeData();
            
            // 레시피 생성 완료 후 재료-던전 연결
            EditorApplication.delayCall += () => {
                LinkMaterialsToDungeons();
                Debug.Log("모든 더미 데이터 생성 및 연결 완료!");
            };
        };
    }

    private void DeleteAllData()
    {
        ClearExistingData("WeaponData");
        ClearExistingData("CustomerData");
        ClearExistingData("DungeonData");
        ClearExistingData("MaterialData");
        ClearExistingData("RecipeData");
        
        Debug.Log("모든 더미 데이터 삭제 완료!");
    }

    // 기존 데이터 삭제 메서드
    private void ClearExistingData(string dataType)
    {
        string[] guids = AssetDatabase.FindAssets($"t:{dataType}");
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("Assets/_Project/Data/StaticData/"))
            {
                AssetDatabase.DeleteAsset(path);
            }
        }
        
        AssetDatabase.Refresh();
    }

    private void GenerateWeaponData()
    {
        // 기존 무기 데이터 삭제
        ClearExistingData("WeaponData");
        
        string[] weaponNames = {
            "나무검", "철검", "강철검", "미스릴검", "드래곤 킬러",
            "화염검", "얼음검", "번개검", "대지검", "바람검",
            "빛의 검", "어둠의 검", "성검", "마검", "전설의 검",
            "단검", "장검", "도끼", "망치", "창",
            "활", "석궁", "지팡이", "완드", "오브"
        };

        string[] weaponTypes = {
            "WoodSword", "IronSword", "SteelSword", "MithrilSword", "DragonKiller",
            "FireSword", "IceSword", "ThunderSword", "EarthSword", "WindSword",
            "LightSword", "DarkSword", "HolySword", "DemonSword", "LegendarySword",
            "Dagger", "LongSword", "Axe", "Hammer", "Spear",
            "Bow", "Crossbow", "Staff", "Wand", "Orb"
        };

        Sprite[] weaponIcons = LoadIconsFromFolder("Assets/_Project/Sprites/Weapons");
        Grade[] grades = { Grade.Common, Grade.Uncommon, Grade.Rare, Grade.Epic, Grade.Legendary };
        Element[] elements = { Element.Fire, Element.Water, Element.Thunder, Element.Earth, Element.Air, Element.Ice, Element.Light, Element.Dark };

        int weaponIndex = 0;
        
        foreach (Grade grade in grades)
        {
            string gradePath = $"Assets/_Project/Data/StaticData/Weapons/{grade}/";
            CreateDirectoryIfNotExists(gradePath);
            
            int weaponsPerGrade = grade switch
            {
                Grade.Common => 20,
                Grade.Uncommon => 15,
                Grade.Rare => 12,
                Grade.Epic => 8,
                Grade.Legendary => 5,
                _ => 5
            };

            for (int i = 0; i < weaponsPerGrade; i++)
            {
                WeaponData weapon = CreateInstance<WeaponData>();
                
                // ID는 영어로만 구성
                weapon.id = $"Weapon_{grade}_{weaponTypes[weaponIndex % weaponTypes.Length]}";
                weapon.weaponName = $"{weaponNames[weaponIndex % weaponNames.Length]} +{(int)grade}";
                weapon.description = $"{grade} 등급의 {elements[i % elements.Length]} 속성 무기입니다.";
                weapon.grade = grade;
                weapon.element = elements[i % elements.Length];
                weapon.basePrice = (int)grade * 500 + 500;

                // 아이콘 설정 (랜덤 선택)
                if (weaponIcons.Length > 0)
                {
                    weapon.icon = weaponIcons[Random.Range(0, weaponIcons.Length)];
                }
                
                // 전설 무기는 2번째 속성 추가
                if (grade == Grade.Legendary)
                {
                    weapon.secondaryElement = elements[(i + 1) % elements.Length];
                }

                string assetPath = $"{gradePath}Weapon_{grade}_{i:00}.asset";
                AssetDatabase.CreateAsset(weapon, assetPath);
                
                weaponIndex++;
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("무기 데이터 생성 완료! (기존 데이터 삭제됨)");
    }

    private void GenerateCustomerData()
    {
        // 기존 고객 데이터 삭제
        ClearExistingData("CustomerData");
        
        string[] customerNames = {
            "용감한 전사", "지혜로운 마법사", "민첩한 도적", "성스러운 기사", "어둠의 암살자",
            "화염의 술사", "얼음의 마녀", "번개의 전사", "대지의 수호자", "바람의 궁수",
            "빛의 성직자", "어둠의 술사", "드래곤 헌터", "마족 킬러", "전설의 영웅",
            "늑대 기수", "매 조련사", "곰 사냥꾼", "독수리 전사", "사자 기사",
            "현명한 현자", "고대의 수호자", "별의 관찰자", "달의 무희", "태양의 전사",
            "바다의 지배자", "산의 왕", "숲의 수호자", "사막의 유랑자", "얼음의 여왕"
        };

        string[] customerTypes = {
            "BraveWarrior", "WiseMage", "AgileThief", "HolyKnight", "DarkAssassin",
            "FireMage", "IceWitch", "ThunderWarrior", "EarthGuardian", "WindArcher",
            "LightPriest", "DarkSorcerer", "DragonHunter", "DemonSlayer", "LegendaryHero",
            "WolfRider", "HawkTamer", "BearHunter", "EagleWarrior", "LionKnight",
            "WiseSage", "AncientGuardian", "StarObserver", "MoonDancer", "SunWarrior",
            "SeaRuler", "MountainKing", "ForestGuardian", "DesertWanderer", "IceQueen"
        };

        Sprite[] customerIcons = LoadIconsFromFolder("Assets/_Project/Sprites/Customers");
        Grade[] grades = { Grade.Common, Grade.Uncommon, Grade.Rare, Grade.Epic, Grade.Legendary };
        Element[] elements = { Element.Fire, Element.Water, Element.Thunder, Element.Earth, Element.Air, Element.Ice, Element.Light, Element.Dark };

        int customerIndex = 0;
        
        foreach (Grade grade in grades)
        {
            string gradePath = $"Assets/_Project/Data/StaticData/Customers/{grade}/";
            CreateDirectoryIfNotExists(gradePath);
            
            int customersPerGrade = grade switch
            {
                Grade.Common => 25,
                Grade.Uncommon => 20,
                Grade.Rare => 15,
                Grade.Epic => 10,
                Grade.Legendary => 5,
                _ => 5
            };

            for (int i = 0; i < customersPerGrade; i++)
            {
                CustomerData customer = CreateInstance<CustomerData>();
                
                // ID는 영어로만 구성
                customer.id = $"Customer_{grade}_{customerTypes[customerIndex % customerTypes.Length]}";
                customer.customerName = $"{customerNames[customerIndex % customerNames.Length]}";
                customer.description = $"{grade} 등급의 {elements[i % elements.Length]} 속성 모험가입니다.";
                customer.grade = grade;
                customer.element = elements[i % elements.Length];
                customer.heroName = $"Hero {customer.customerName}";
                customer.heroDescription = $"Hero로 각성한 {customer.customerName}입니다.";

                // 아이콘 설정 (랜덤 선택)
                if (customerIcons.Length > 0)
                {
                    customer.customerIcon = customerIcons[Random.Range(0, customerIcons.Length)];
                    // Hero 아이콘이 비어있으면 customerIcon 사용
                    customer.heroIcon = customer.customerIcon;
                }

                string assetPath = $"{gradePath}Customer_{grade}_{i:00}.asset";
                AssetDatabase.CreateAsset(customer, assetPath);
                
                customerIndex++;
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("고객 데이터 생성 완료! (기존 데이터 삭제됨)");
    }

    private void GenerateDungeonData()
    {
        // 기존 던전 데이터 삭제
        ClearExistingData("DungeonData");
        
        string[] dungeonNames = {
            "고블린 동굴", "오크 요새", "드래곤 둥지", "마왕성", "신들의 전당",
            "화염 지옥", "얼음 동굴", "번개 신전", "대지의 심장", "바람의 탑",
            "빛의 성역", "어둠의 미궁", "시간의 틈", "공간의 균열", "영원의 방",
            "고대 유적", "잊혀진 신전", "마법의 숲", "용암 동굴", "크리스탈 광산",
            "유령의 성", "악마의 탑", "천사의 정원", "죽음의 늪", "생명의 샘"
        };

        string[] dungeonTypes = {
            "GoblinCave", "OrcFortress", "DragonNest", "DemonCastle", "GodsTemple",
            "FireHell", "IceCavern", "ThunderShrine", "EarthCore", "WindTower",
            "LightSanctuary", "DarkMaze", "TimeRift", "SpaceCrack", "EternalChamber",
            "AncientRuins", "ForgottenTemple", "MagicForest", "LavaCave", "CrystalMine",
            "GhostCastle", "DemonTower", "AngelGarden", "DeathSwamp", "LifeSpring"
        };

        string[] dungeonDescriptions = {
            "초보 모험가들을 위한 기본 던전입니다.",
            "중급 모험가들이 도전하는 던전입니다.",
            "숙련된 모험가만이 클리어할 수 있는 던전입니다.",
            "전설적인 실력을 가진 모험가를 위한 던전입니다.",
            "신화급 영웅만이 도전할 수 있는 최고 난이도 던전입니다."
        };

        Sprite[] DungeonIcons = LoadIconsFromFolder("Assets/_Project/Sprites/Dungeons");
        Grade[] grades = { Grade.Common, Grade.Uncommon, Grade.Rare, Grade.Epic, Grade.Legendary };
        Element[] elements = { Element.Fire, Element.Water, Element.Thunder, Element.Earth, Element.Air, Element.Ice, Element.Light, Element.Dark };

        int dungeonIndex = 0;
        
        foreach (Grade grade in grades)
        {
            string gradePath = $"Assets/_Project/Data/StaticData/Dungeons/{grade}/";
            CreateDirectoryIfNotExists(gradePath);
            
            int dungeonsPerGrade = grade switch
            {
                Grade.Common => 15,
                Grade.Uncommon => 12,
                Grade.Rare => 10,
                Grade.Epic => 8,
                Grade.Legendary => 5,
                _ => 5
            };

            for (int i = 0; i < dungeonsPerGrade; i++)
            {
                DungeonData dungeon = CreateInstance<DungeonData>();
                
                Element dungeonElement = elements[i % elements.Length];
            
                dungeon.id = $"Dungeon_{grade}_{dungeonTypes[dungeonIndex % dungeonTypes.Length]}";
                dungeon.dungeonName = $"{dungeonNames[dungeonIndex % dungeonNames.Length]} ({dungeonElement.ToString()})";
                dungeon.description = dungeonDescriptions[(int)grade];
                dungeon.grade = grade;
                dungeon.element = dungeonElement;

                // 아이콘 설정 (랜덤 선택)
                if (DungeonIcons.Length > 0)
                {
                    dungeon.icon = DungeonIcons[Random.Range(0, DungeonIcons.Length)];
                }
                
                // 등급별 기본 골드 보상
                dungeon.baseGoldReward = grade switch
                {
                    Grade.Common => Random.Range(100, 300),
                    Grade.Uncommon => Random.Range(250, 500),
                    Grade.Rare => Random.Range(400, 800),
                    Grade.Epic => Random.Range(700, 1200),
                    Grade.Legendary => Random.Range(1000, 2000),
                    _ => 200
                };

                // 재료 ID 목록 초기화
                dungeon.dropMaterialIDs = new System.Collections.Generic.List<string>();

                string assetPath = $"{gradePath}Dungeon_{grade}_{i:00}.asset";
                AssetDatabase.CreateAsset(dungeon, assetPath);
                
                dungeonIndex++;
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("던전 데이터 생성 완료! (기존 데이터 삭제됨)");
    }

    private void GenerateMaterialData()
    {
        // 기존 재료 데이터 삭제
        ClearExistingData("MaterialData");
        
        string[] materialNames = {
            "철광석", "구리", "나무", "가죽", "천",
            "은광석", "금광석", "보석", "마법석", "크리스탈",
            "미스릴", "아다만타이트", "용의 비늘", "별의 조각", "시간의 파편",
            "불의 정수", "물의 정수", "번개의 정수", "대지의 정수", "바람의 정수",
            "얼음의 정수", "빛의 정수", "어둠의 정수", "신의 눈물", "악마의 심장",
            "고대 룬", "마법 가루", "영혼석", "생명의 이슬", "죽음의 재"
        };

        string[] materialTypes = {
            "IronOre", "Copper", "Wood", "Leather", "Cloth",
            "SilverOre", "GoldOre", "Gem", "MagicStone", "Crystal",
            "Mithril", "Adamantite", "DragonScale", "StarFragment", "TimeFragment",
            "FireEssence", "WaterEssence", "ThunderEssence", "EarthEssence", "AirEssence",
            "IceEssence", "LightEssence", "DarkEssence", "GodTear", "DemonHeart",
            "AncientRune", "MagicPowder", "SoulStone", "LifeDew", "DeathAsh"
        };

        string[] materialDescriptions = {
            "가장 기본적인 재료입니다.",
            "조금 더 나은 품질의 재료입니다.",
            "희귀한 재료로 특별한 무기 제작에 사용됩니다.",
            "매우 희귀한 재료로 강력한 무기 제작에 필요합니다.",
            "전설급 재료로 최고의 무기를 만들 때 사용됩니다."
        };

        Sprite[] MaterialIcons = LoadIconsFromFolder("Assets/_Project/Sprites/Materials");
        Grade[] grades = { Grade.Common, Grade.Uncommon, Grade.Rare, Grade.Epic, Grade.Legendary };
        
        int materialIndex = 0;
        
        foreach (Grade grade in grades)
        {
            string gradePath = $"Assets/_Project/Data/StaticData/Materials/{grade}/";
            CreateDirectoryIfNotExists(gradePath);
            
            int materialsPerGrade = grade switch
            {
                Grade.Common => 8,
                Grade.Uncommon => 6,
                Grade.Rare => 5,
                Grade.Epic => 4,
                Grade.Legendary => 3,
                _ => 5
            };

            for (int i = 0; i < materialsPerGrade; i++)
            {
                MaterialData material = CreateInstance<MaterialData>();
                
                // ID는 영어로만 구성
                material.id = $"Material_{grade}_{materialTypes[materialIndex % materialTypes.Length]}";
                material.materialName = materialNames[materialIndex % materialNames.Length];
                material.description = materialDescriptions[(int)grade];
                material.grade = grade;

                // 아이콘 설정 (랜덤 선택)
                if (MaterialIcons.Length > 0)
                {
                    material.icon = MaterialIcons[Random.Range(0, MaterialIcons.Length)];
                }
                
                // 등급별 기본 가치
                material.baseValue = grade switch
                {
                    Grade.Common => Random.Range(50, 150),
                    Grade.Uncommon => Random.Range(100, 300),
                    Grade.Rare => Random.Range(250, 500),
                    Grade.Epic => Random.Range(400, 800),
                    Grade.Legendary => Random.Range(700, 1500),
                    _ => 100
                };
                
                // 등급별 드롭률 (높은 등급일수록 낮은 확률)
                material.dropRate = grade switch
                {
                    Grade.Common => Random.Range(0.6f, 0.9f),
                    Grade.Uncommon => Random.Range(0.4f, 0.7f),
                    Grade.Rare => Random.Range(0.2f, 0.5f),
                    Grade.Epic => Random.Range(0.1f, 0.3f),
                    Grade.Legendary => Random.Range(0.05f, 0.15f),
                    _ => 0.5f
                };

                // availableDungeonIDs는 나중에 던전 데이터 생성 후 연결
                material.availableDungeonIDs = new System.Collections.Generic.List<string>();

                string assetPath = $"{gradePath}Material_{grade}_{i:00}.asset";
                AssetDatabase.CreateAsset(material, assetPath);
                
                materialIndex++;
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("재료 데이터 생성 완료! (기존 데이터 삭제됨)");
    }

    private void GenerateRecipeData()
    {
        // 기존 레시피 데이터 삭제
        ClearExistingData("RecipeData");
        
        // 무기와 재료 데이터 로드
        Dictionary<Grade, List<WeaponData>> weaponsByGrade = LoadWeaponsByGrade();
        Dictionary<Grade, List<MaterialData>> materialsByGrade = LoadMaterialsByGrade();
        
        if (weaponsByGrade.Count == 0 || materialsByGrade.Count == 0)
        {
            Debug.LogError("무기 또는 재료 데이터가 없습니다. 먼저 무기와 재료 데이터를 생성해주세요.");
            return;
        }
        
        Sprite[] RecipeIcons = LoadIconsFromFolder("Assets/_Project/Sprites/Recipes");
        Grade[] grades = { Grade.Common, Grade.Uncommon, Grade.Rare, Grade.Epic, Grade.Legendary };
        
        int recipeIndex = 0;
        
        foreach (Grade grade in grades)
        {
            string gradePath = $"Assets/_Project/Data/StaticData/Recipes/{grade}/";
            CreateDirectoryIfNotExists(gradePath);
            
            // 등급별 레시피 개수
            int recipesPerGrade = grade switch
            {
                Grade.Common => 10,
                Grade.Uncommon => 8,
                Grade.Rare => 6,
                Grade.Epic => 4,
                Grade.Legendary => 3,
                _ => 5
            };
            
            // 해당 등급의 무기가 있는지 확인
            if (!weaponsByGrade.ContainsKey(grade) || weaponsByGrade[grade].Count == 0)
            {
                Debug.LogWarning($"{grade} 등급 무기가 없어서 레시피를 생성할 수 없습니다.");
                continue;
            }
            
            var availableWeapons = weaponsByGrade[grade];
            
            for (int i = 0; i < recipesPerGrade && i < availableWeapons.Count; i++)
            {
                RecipeData recipe = CreateInstance<RecipeData>();
                
                // 결과물 무기 선택 (순환하며 선택)
                WeaponData resultWeapon = availableWeapons[i % availableWeapons.Count];
                
                // 기본 정보 설정
                recipe.id = $"Recipe_{resultWeapon.weaponName.Replace(" ", "").Replace("+", "")}_{grade}_{i:00}";
                recipe.recipeName = $"{resultWeapon.weaponName} 제작법";
                recipe.description = $"{resultWeapon.weaponName}을(를) 제작하는 레시피입니다.";
                recipe.resultWeaponID = resultWeapon.id;
                recipe.grade = grade;

                // 아이콘 설정 (랜덤 선택)
                if (RecipeIcons.Length > 0)
                {
                    recipe.icon = RecipeIcons[Random.Range(0, RecipeIcons.Length)];
                }
                
                // 필요 재료 설정
                recipe.requiredMaterials = GenerateRequiredMaterials(grade, materialsByGrade);
                
                // 제작 조건 설정
                recipe.craftingCost = grade switch
                {
                    Grade.Common => Random.Range(100, 300),
                    Grade.Uncommon => Random.Range(200, 500),
                    Grade.Rare => Random.Range(400, 800),
                    Grade.Epic => Random.Range(700, 1200),
                    Grade.Legendary => Random.Range(1000, 2000),
                    _ => 200
                };
                
                recipe.craftingDays = grade switch
                {
                    Grade.Common => Random.Range(1, 2),
                    Grade.Uncommon => Random.Range(1, 3),
                    Grade.Rare => Random.Range(2, 4),
                    Grade.Epic => Random.Range(3, 5),
                    Grade.Legendary => Random.Range(4, 7),
                    _ => 1
                };
                
                recipe.unlockDay = grade switch
                {
                    Grade.Common => Random.Range(1, 5),
                    Grade.Uncommon => Random.Range(3, 8),
                    Grade.Rare => Random.Range(7, 15),
                    Grade.Epic => Random.Range(15, 25),
                    Grade.Legendary => Random.Range(25, 40),
                    _ => 1
                };
                
                // 등급별 희귀도
                recipe.availability = grade switch
                {
                    Grade.Common => Random.Range(0.8f, 1.0f),
                    Grade.Uncommon => Random.Range(0.6f, 0.9f),
                    Grade.Rare => Random.Range(0.4f, 0.7f),
                    Grade.Epic => Random.Range(0.2f, 0.5f),
                    Grade.Legendary => Random.Range(0.1f, 0.3f),
                    _ => 1.0f
                };
                
                string assetPath = $"{gradePath}Recipe_{grade}_{i:00}.asset";
                AssetDatabase.CreateAsset(recipe, assetPath);
                
                recipeIndex++;
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("레시피 데이터 생성 완료! (기존 데이터 삭제됨)");
    }

    // 재료와 던전 연결하는 메서드 (완전 ID 기반 양방향 연결)
    private void LinkMaterialsToDungeons()
    {
        // 모든 던전 데이터 로드
        string[] dungeonGuids = AssetDatabase.FindAssets("t:DungeonData");
        var dungeonsByGrade = new System.Collections.Generic.Dictionary<Grade, System.Collections.Generic.List<DungeonData>>();
        var allDungeons = new System.Collections.Generic.Dictionary<string, DungeonData>();
        
        foreach (string guid in dungeonGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            DungeonData dungeon = AssetDatabase.LoadAssetAtPath<DungeonData>(path);
            
            if (!dungeonsByGrade.ContainsKey(dungeon.grade))
            {
                dungeonsByGrade[dungeon.grade] = new System.Collections.Generic.List<DungeonData>();
            }
            dungeonsByGrade[dungeon.grade].Add(dungeon);
            allDungeons[dungeon.id] = dungeon;
            
            // 던전의 재료 ID 목록 초기화
            dungeon.dropMaterialIDs.Clear();
        }

        // 모든 재료 데이터 로드하고 던전 연결
        string[] materialGuids = AssetDatabase.FindAssets("t:MaterialData");
        
        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MaterialData material = AssetDatabase.LoadAssetAtPath<MaterialData>(path);
            
            material.availableDungeonIDs.Clear();
            
            // 같은 등급의 던전들에 랜덤하게 연결
            if (dungeonsByGrade.ContainsKey(material.grade))
            {
                var availableDungeons = dungeonsByGrade[material.grade];
                int dungeonCount = Random.Range(1, Mathf.Min(4, availableDungeons.Count + 1));
                
                var selectedDungeons = new System.Collections.Generic.HashSet<string>();
                while (selectedDungeons.Count < dungeonCount && selectedDungeons.Count < availableDungeons.Count)
                {
                    int randomIndex = Random.Range(0, availableDungeons.Count);
                    selectedDungeons.Add(availableDungeons[randomIndex].id);
                }
                
                material.availableDungeonIDs.AddRange(selectedDungeons);
            }
            
            // 낮은 등급 던전에도 일정 확률로 연결
            if (material.grade > Grade.Common)
            {
                Grade lowerGrade = (Grade)((int)material.grade - 1);
                if (dungeonsByGrade.ContainsKey(lowerGrade) && Random.Range(0f, 1f) < 0.3f)
                {
                    var lowerDungeons = dungeonsByGrade[lowerGrade];
                    int randomIndex = Random.Range(0, lowerDungeons.Count);
                    material.availableDungeonIDs.Add(lowerDungeons[randomIndex].id);
                }
            }
            
            // 양방향 연결: 던전의 dropMaterialIDs에도 재료 ID 추가
            foreach (string dungeonId in material.availableDungeonIDs)
            {
                if (allDungeons.ContainsKey(dungeonId))
                {
                    allDungeons[dungeonId].dropMaterialIDs.Add(material.id);
                }
            }
            
            EditorUtility.SetDirty(material);
        }
        
        // 모든 던전 Dirty 처리
        foreach (var dungeon in allDungeons.Values)
        {
            EditorUtility.SetDirty(dungeon);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("재료-던전 ID 기반 양방향 연결 완료!");
    }

    // 무기 데이터를 등급별로 로드하는 헬퍼 메서드(GenerateRecipeData)
    private Dictionary<Grade, List<WeaponData>> LoadWeaponsByGrade()
    {
        var weaponsByGrade = new Dictionary<Grade, List<WeaponData>>();
        
        string[] weaponGuids = AssetDatabase.FindAssets("t:WeaponData");
        
        foreach (string guid in weaponGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            WeaponData weapon = AssetDatabase.LoadAssetAtPath<WeaponData>(path);
            
            if (!weaponsByGrade.ContainsKey(weapon.grade))
            {
                weaponsByGrade[weapon.grade] = new List<WeaponData>();
            }
            weaponsByGrade[weapon.grade].Add(weapon);
        }
        
        return weaponsByGrade;
    }

    // 재료 데이터를 등급별로 로드하는 헬퍼 메서드(GenerateRecipeData)
    private Dictionary<Grade, List<MaterialData>> LoadMaterialsByGrade()
    {
        var materialsByGrade = new Dictionary<Grade, List<MaterialData>>();
        
        string[] materialGuids = AssetDatabase.FindAssets("t:MaterialData");
        
        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MaterialData material = AssetDatabase.LoadAssetAtPath<MaterialData>(path);
            
            if (!materialsByGrade.ContainsKey(material.grade))
            {
                materialsByGrade[material.grade] = new List<MaterialData>();
            }
            materialsByGrade[material.grade].Add(material);
        }
        
        return materialsByGrade;
    }

    // 등급에 맞는 필요 재료 목록 생성(GenerateRecipeData)
    private List<RequiredMaterial> GenerateRequiredMaterials(Grade recipeGrade, Dictionary<Grade, List<MaterialData>> materialsByGrade)
    {
        var requiredMaterials = new List<RequiredMaterial>();
        
        // 등급별 필요 재료 개수
        int materialCount = recipeGrade switch
        {
            Grade.Common => Random.Range(2, 4),      // 2-3개
            Grade.Uncommon => Random.Range(3, 5),    // 3-4개
            Grade.Rare => Random.Range(3, 6),        // 3-5개
            Grade.Epic => Random.Range(4, 6),        // 4-5개
            Grade.Legendary => Random.Range(5, 8),   // 5-7개
            _ => 3
        };
        
        // 사용할 수 있는 재료 등급 범위 설정
        var availableGrades = new List<Grade>();
        
        // 현재 등급과 그보다 낮은 등급의 재료 사용 가능
        for (int i = 0; i <= (int)recipeGrade; i++)
        {
            Grade grade = (Grade)i;
            if (materialsByGrade.ContainsKey(grade) && materialsByGrade[grade].Count > 0)
            {
                availableGrades.Add(grade);
            }
        }
        
        if (availableGrades.Count == 0)
        {
            Debug.LogWarning($"{recipeGrade} 등급 레시피에 사용할 재료가 없습니다.");
            return requiredMaterials;
        }
        
        // 재료 선택 및 수량 설정
        var selectedMaterials = new HashSet<string>(); // 중복 방지
        
        for (int i = 0; i < materialCount; i++)
        {
            // 등급별 가중치 (높은 등급일수록 높은 등급 재료 선호)
            Grade selectedGrade = SelectMaterialGrade(recipeGrade, availableGrades);
            
            var availableMaterials = materialsByGrade[selectedGrade];
            MaterialData selectedMaterial = availableMaterials[Random.Range(0, availableMaterials.Count)];
            
            // 중복 체크
            if (selectedMaterials.Contains(selectedMaterial.id))
            {
                i--; // 다시 선택
                continue;
            }
            
            selectedMaterials.Add(selectedMaterial.id);
            
            // 수량 설정 (등급이 낮을수록 많이 필요)
            int quantity = selectedGrade switch
            {
                Grade.Common => Random.Range(3, 8),
                Grade.Uncommon => Random.Range(2, 6),
                Grade.Rare => Random.Range(1, 4),
                Grade.Epic => Random.Range(1, 3),
                Grade.Legendary => Random.Range(1, 2),
                _ => 1
            };
            
            requiredMaterials.Add(new RequiredMaterial(selectedMaterial.id, quantity));
        }
        
        return requiredMaterials;
    }

    // 레시피 등급에 따른 재료 등급 선택 (가중치 적용)(GenerateRecipeData)
    private Grade SelectMaterialGrade(Grade recipeGrade, List<Grade> availableGrades)
    {
        // 높은 등급 레시피일수록 높은 등급 재료를 더 많이 사용
        float[] weights = new float[availableGrades.Count];
        
        for (int i = 0; i < availableGrades.Count; i++)
        {
            Grade materialGrade = availableGrades[i];
            int gradeDiff = (int)recipeGrade - (int)materialGrade;
            
            // 등급 차이에 따른 가중치 (같은 등급이 가장 높음)
            weights[i] = gradeDiff switch
            {
                0 => 0.4f,  // 같은 등급
                1 => 0.3f,  // 1등급 낮음
                2 => 0.2f,  // 2등급 낮음
                3 => 0.1f,  // 3등급 낮음
                _ => 0.05f  // 그 이상
            };
        }
        
        // 가중치 기반 랜덤 선택
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }
        
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        
        for (int i = 0; i < weights.Length; i++)
        {
            currentWeight += weights[i];
            if (randomValue <= currentWeight)
            {
                return availableGrades[i];
            }
        }
        
        // 기본값 (마지막 등급)
        return availableGrades[availableGrades.Count - 1];
    }

    // 디렉토리가 없으면 생성하는 메서드
    private void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    // 지정된 폴더에서 스프라이트 아이콘을 로드하는 메서드
    private Sprite[] LoadIconsFromFolder(string folderPath)
    {
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
        Sprite[] icons = new Sprite[guids.Length];
        
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            icons[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
        
        return icons;
    }
}