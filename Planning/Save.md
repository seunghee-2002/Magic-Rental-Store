# 데이터 저장 및 불러오기 (Persistence) - Hero 시스템 포함 최종본

**목적:** Hero 시스템을 포함한 플레이어의 모든 동적 상태를 단일 데이터 클래스(`PlayerData`)로 관리하고, 이를 JSON으로 변환하여 안전하게 저장 및 불러오는 구조를 확립합니다.

---

## 1. 데이터 관리의 핵심: `PlayerData.cs` (Hero 시스템 포함)

### 1.1. 역할 및 책임
- **역할**: 게임을 저장할 때 파일로 남겨야 하는 모든 동적 데이터를 담는 순수 C# 클래스
- **책임**: `PersistenceController`는 오직 이 `PlayerData` 객체 하나만 알면 게임 전체 상태를 저장/로드 가능
- **Hero 시스템**: Customer → Hero 전환, Hero 부상, Hero 도감 수집 상태 모두 포함

### 1.2. PlayerData 구조 (Hero 확장)

```csharp
[System.Serializable]
public class PlayerData
{
    [Header("기본 게임 정보")]
    public int gold;
    public int currentDay;
    public GamePhase currentPhase;
    public float phaseRemainingTime;
    
    [Header("진행 상황")]
    public List<string> unlockedRecipeIDs;
    public List<string> purchasedWeaponIDs; // 오늘 구매한 무기 (새로고침용)
    
    [Header("인벤토리")]
    public List<WeaponInstanceSaveData> ownedWeapons;
    public List<MaterialInstanceSaveData> ownedMaterials;
    
    [Header("고객 시스템")]
    public List<CustomerInstanceSaveData> visitingCustomers;
    
    [Header("Hero 시스템 (신규)")]
    public List<HeroInstanceSaveData> ownedHeroes;           // 보유한 Hero 목록
    public List<InjuredHeroSaveData> injuredHeroes;         // 부상 중인 Hero 목록
    public Dictionary<string, HeroCollectionSaveData> heroCollection; // Hero 도감 수집 상태 (수정)
    
    [Header("모험 시스템")]
    public List<AdventureInstanceSaveData> ongoingAdventures;
    public List<AdventureResultSaveData> completedAdventures;
    
    [Header("제작 시스템")]
    public List<CraftingInstanceSaveData> ongoingCrafting;   // 진행 중인 제작
}
```

---

## 2. Hero 시스템 저장 데이터 구조

### 2.1. HeroInstanceSaveData (수정)
```csharp
[System.Serializable]
public class HeroInstanceSaveData
{
    public string staticDataID;        // 원본 CustomerData ID
    public string instanceID;          // Hero 고유 식별자
    public int level;                  // Hero 현재 레벨 (성장 가능)
    public int acquiredLevel;          // Hero로 전환 당시 레벨 (기록용)
    public int convertedDate;          // Hero로 전환된 날짜
}
```

### 2.2. HeroCollectionSaveData (신규)
```csharp
[System.Serializable]
public class HeroCollectionSaveData
{
    public bool isAcquired;           // 획득 여부
    public int currentLevel;          // 현재 레벨 (획득한 경우만 유효)
    public int acquiredLevel;         // 획득 당시 레벨 (획득한 경우만 유효)
    public int acquiredDate;          // 획득한 날짜 (획득한 경우만 유효)
}
```

### 2.3. InjuredHeroSaveData
```csharp
[System.Serializable]
public class InjuredHeroSaveData
{
    public string heroInstanceID;     // 부상당한 Hero ID
    public int injuryDate;            // 부상 날짜
    public int recoveryDate;          // 회복 예정 날짜
    public int treatmentCost;         // 치료비
    public int injuryDuration;        // 부상 기간
}
```

### 2.4. 확장된 AdventureInstanceSaveData
```csharp
[System.Serializable]
public class AdventureInstanceSaveData
{
    public string customerInstanceID;  // Customer 또는 Hero Instance ID
    public string weaponInstanceID;
    public string dungeonID;
    public int remainingDays;
    public bool isHero;               // Hero 모험인지 구분 플래그
}
```

### 2.5. 확장된 AdventureResultSaveData
```csharp
[System.Serializable]
public class AdventureResultSaveData
{
    public string customerInstanceID;
    public string weaponInstanceID;
    public string dungeonID;
    public bool isSuccess;
    public bool isWeaponRecovered;
    public bool isHero;               // Hero 모험이었는지 구분
    public bool heroConverted;        // Customer가 Hero로 전환되었는지
}
```

---

## 3. 저장 파일 구조 (JSON 표현)

### 3.1. Hero 시스템 포함 완전한 JSON 예시

```json
{
  "gold": 2500,
  "currentDay": 25,
  "currentPhase": "Night",
  "phaseRemainingTime": 0.0,
  
  "unlockedRecipeIDs": [
    "Recipe_IronSword_01",
    "Recipe_SteelSword_02"
  ],
  
  "purchasedWeaponIDs": [
    "Weapon_BronzeDagger_01"
  ],
  
  "ownedWeapons": [
    {
      "staticDataID": "Weapon_SteelSword_01",
      "instanceID": "weapon-uuid-1234",
      "enhancementLevel": 2
    },
    {
      "staticDataID": "Weapon_LegendarySword_01",
      "instanceID": "weapon-uuid-5678",
      "enhancementLevel": 0
    }
  ],
  
  "ownedMaterials": [
    {
      "staticDataID": "Material_IronOre",
      "quantity": 15
    },
    {
      "staticDataID": "Material_DragonScale",
      "quantity": 3
    }
  ],
  
  "visitingCustomers": [
    {
      "staticDataID": "Customer_YoungWarrior_01",
      "instanceID": "customer-uuid-abcd",
      "level": 12,
      "assignedDungeonID": "Dungeon_GoblinCave_01"
    }
  ],
  
  "ownedHeroes": [
    {
      "staticDataID": "Customer_BraveKnight_01",
      "instanceID": "hero-uuid-def0",
      "level": 25,
      "acquiredLevel": 18,
      "convertedDate": 20
    },
    {
      "staticDataID": "Customer_WiseMage_01", 
      "instanceID": "hero-uuid-ghi1",
      "level": 22,
      "acquiredLevel": 22,
      "convertedDate": 15
    }
  ],
  
  "injuredHeroes": [
    {
      "heroInstanceID": "hero-uuid-ghi1",
      "injuryDate": 23,
      "recoveryDate": 33
    }
  ],
  
  "heroCollection": {
    "Customer_BraveKnight_01": {
      "isAcquired": true,
      "currentLevel": 25,
      "acquiredLevel": 18,
      "acquiredDate": 20
    },
    "Customer_WiseMage_01": {
      "isAcquired": true,
      "currentLevel": 22,
      "acquiredLevel": 22,
      "acquiredDate": 15
    },
    "Customer_DragonSlayer_01": {
      "isAcquired": false,
      "currentLevel": 0,
      "acquiredLevel": 0,
      "acquiredDate": 0
    },
    "Customer_ShadowAssassin_01": {
      "isAcquired": false,
      "currentLevel": 0,
      "acquiredLevel": 0,
      "acquiredDate": 0
    }
  },
  
  "ongoingAdventures": [
    {
      "customerInstanceID": "customer-uuid-abcd",
      "weaponInstanceID": "weapon-uuid-1234",
      "dungeonID": "Dungeon_GoblinCave_01",
      "remainingDays": 2,
      "isHero": false
    },
    {
      "customerInstanceID": "hero-uuid-def0",
      "weaponInstanceID": "weapon-uuid-5678", 
      "dungeonID": "Dungeon_DragonLair_01",
      "remainingDays": 5,
      "isHero": true
    }
  ],
  
  "completedAdventures": [
    {
      "customerInstanceID": "customer-uuid-xyz9",
      "weaponInstanceID": "weapon-uuid-old1",
      "dungeonID": "Dungeon_OrcStronghold_01",
      "isSuccess": true,
      "isWeaponRecovered": true,
      "isHero": false,
      "heroConverted": true
    }
  ],
  
  "ongoingCrafting": [
    {
      "recipeID": "Recipe_IronSword_01",
      "startDate": 24,
      "completionDate": 26,
      "isCompleted": false
    }
  ]
}
```

---

## 4. 저장 및 불러오기 흐름 (Hero 시스템 포함)

### 4.1. 게임 저장 (Save) - Hero 데이터 수집

```
1. GameController.SaveGame() 호출
   ↓
2. 각 시스템에서 데이터 수집:
   - InventoryController → 무기/재료 인벤토리
   - CustomerManager → 방문 고객 목록
   - HeroManager → 보유 Hero, 부상 Hero, 도감 상태 (레벨 정보 포함)
   - AdventureController → 진행중/완료 모험 (Customer/Hero 구분)
   - BlacksmithController → 제작 진행 상황
   ↓
3. PlayerData 객체 완성 (모든 Hero 데이터 포함)
   ↓
4. PersistenceController.SaveToFile(playerData)
   ↓
5. JSON 직렬화 → 파일 저장
```

### 4.2. 게임 불러오기 (Load) - Hero 데이터 복원

```
1. PersistenceController.LoadFromFile()
   ↓
2. JSON 역직렬화 → PlayerData 객체 생성
   ↓
3. GameController가 각 시스템에 데이터 배포:
   - InventoryController.RestoreInventory(playerData.ownedWeapons, playerData.ownedMaterials)
   - CustomerManager.RestoreCustomers(playerData.visitingCustomers)
   - HeroManager.RestoreHeroes(playerData.ownedHeroes, playerData.injuredHeroes, playerData.heroCollection)
   - AdventureController.RestoreAdventures(playerData.ongoingAdventures, playerData.completedAdventures)
   - BlacksmithController.RestoreCrafting(playerData.ongoingCrafting)
   ↓
4. 각 시스템에서 ID → 실제 데이터 변환
   ↓
5. 게임 상태 완전 복원
```

---

## 5. Hero 시스템 ID 해석 및 복원 과정

### 5.1. HeroManager의 Hero 복원 과정 (수정)

```csharp
public void RestoreHeroes(List<HeroInstanceSaveData> heroSaveData, 
                         List<InjuredHeroSaveData> injuredSaveData,
                         Dictionary<string, HeroCollectionSaveData> collectionData)
{
    // 1. 보유 Hero 복원
    foreach(var saveData in heroSaveData)
    {
        // DataManager를 통해 원본 Customer 데이터 가져오기
        CustomerData originalData = DataManager.Instance.GetCustomerData(saveData.staticDataID);
        
        // HeroInstance 생성
        HeroInstance hero = new HeroInstance
        {
            data = originalData,
            instanceID = saveData.instanceID,
            level = saveData.level,               // 현재 레벨
            acquiredLevel = saveData.acquiredLevel, // 획득 당시 레벨
            convertedDate = saveData.convertedDate
        };
        
        ownedHeroes.Add(hero);
    }
    
    // 2. 부상 Hero 복원
    foreach(var injuredData in injuredSaveData)
    {
        InjuredHeroData injury = new InjuredHeroData
        {
            heroInstanceID = injuredData.heroInstanceID,
            injuryDate = injuredData.injuryDate,
            recoveryDate = injuredData.recoveryDate
        };
        
        injuredHeroes.Add(injury);
    }
    
    // 3. Hero 도감 복원 (새로운 구조)
    heroCollection = new Dictionary<string, HeroCollectionData>();
    foreach(var kvp in collectionData)
    {
        heroCollection[kvp.Key] = new HeroCollectionData
        {
            isAcquired = kvp.Value.isAcquired,
            currentLevel = kvp.Value.currentLevel,
            acquiredLevel = kvp.Value.acquiredLevel,
            acquiredDate = kvp.Value.acquiredDate
        };
    }
}
```

### 5.2. AdventureController의 Customer/Hero 모험 복원

```csharp
public void RestoreAdventures(List<AdventureInstanceSaveData> ongoing,
                             List<AdventureResultSaveData> completed)
{
    foreach(var saveData in ongoing)
    {
        AdventureInstance adventure = new AdventureInstance();
        
        // Hero/Customer 구분하여 복원
        if(saveData.isHero)
        {
            adventure.hero = HeroManager.Instance.GetHeroByInstanceID(saveData.customerInstanceID);
            adventure.isHero = true;
        }
        else
        {
            adventure.customer = CustomerManager.Instance.GetCustomerByInstanceID(saveData.customerInstanceID);
            adventure.isHero = false;
        }
        
        // 무기 및 던전 데이터 복원
        adventure.weapon = InventoryController.Instance.GetWeaponByInstanceID(saveData.weaponInstanceID);
        adventure.dungeon = DataManager.Instance.GetDungeonData(saveData.dungeonID);
        adventure.remainingDays = saveData.remainingDays;
        
        ongoingAdventures.Add(adventure);
    }
}
```

---

## 6. 게임오버 후 Hero 승계 시스템

### 6.1. 승계 데이터 처리
```csharp
public class HeroInheritanceData
{
    public Dictionary<string, HeroCollectionSaveData> inheritedHeroCollection;
    public List<InjuredHeroSaveData> inheritedInjuredHeroes;
    
    // 레벨 초기화 처리
    public void ResetHeroLevels()
    {
        foreach(var collection in inheritedHeroCollection.Values)
        {
            if(collection.isAcquired)
            {
                collection.currentLevel = 1; // 레벨 1로 초기화
                // acquiredLevel과 acquiredDate는 유지
            }
        }
    }
}
```

### 6.2. 새 게임 시작 시 Hero 데이터 로드
```
게임오버 시:
1. 현재 Hero 도감 상태 → HeroInheritanceData에 저장
2. Hero 부상 상태 → HeroInheritanceData에 저장
3. Hero 레벨 초기화 (1레벨로)

새 게임 시작 시:
1. HeroInheritanceData 로드
2. Hero 도감 상태 복원
3. Hero 부상 상태 복원
4. Hero 잠금해제 조건 적용 (등급별 일수 제한)
```

---

## 7. 자동 저장 타이밍

### 7.1. 자동 저장 시점
- **페이즈 전환 시**: 아침→낮, 낮→밤, 밤→아침 각 전환마다 자동 저장
- **Hero 전환 시**: Customer가 Hero로 전환되는 순간 즉시 저장
- **Hero 레벨업 시**: Hero가 레벨업하는 순간 즉시 저장
- **중요 액션 후**: 무기 구매/판매, 제작 완료, 모험 시작/완료 시 저장

### 7.2. 저장 실패 대응
- **백업 파일**: 이전 저장 파일을 `.backup` 확장자로 보관
- **저장 검증**: 저장 후 즉시 로드하여 데이터 무결성 확인
- **복구 시스템**: 저장 실패 시 백업 파일로 자동 복구

---

## 8. 성능 최적화 고려사항

### 8.1. Hero 데이터 최적화
- **지연 로딩**: Hero 도감은 실제 열람 시에만 로드
- **압축 저장**: 부상 Hero 목록은 회복된 Hero 자동 제거
- **캐싱**: 자주 접근하는 Hero 데이터는 메모리 캐시 활용

### 8.2. 저장 파일 관리
- **파일 크기 최적화**: 불필요한 완료 모험 데이터 주기적 정리
- **버전 관리**: 세이브 파일 버전을 명시하여 호환성 보장
- **암호화**: 중요한 진행 상황은 간단한 암호화 적용

### 8.3. Hero 승계 시스템 최적화
- **선택적 저장**: 획득한 Hero만 HeroInheritanceData에 저장
- **압축**: 미획득 Hero 정보는 bool 값만 저장
- **캐시**: 게임오버 시에만 승계 데이터 생성, 메모리에서 관리