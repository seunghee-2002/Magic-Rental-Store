# 스크립트별 구현 체크리스트 (Hero 시스템 포함 완성본)

_이 체크리스트는 MVC 4계층(Data, Model, View, Controller)에 속한 모든 스크립트를 구현 전 한눈에 점검할 수 있도록 변수·메서드를 정리한 목록입니다._

---

## 1. Data Layer (데이터 정의)

### 1.1. 정적 데이터 (ScriptableObjects)

**GameConfig.cs**
- **변수**: `startingGold: int`, `dayDuration: float`, `blacksmithUnlockDay: int`, `weaponShopItemCount: int`, `weaponShopRefreshCost: int`, `heroInjuryDays: int`, `heroMaxLevel: int`, `heroUnlockDays: int[]`, `inventoryMaxSize: int`, `dailyCustomerCount: int`, `uiPageSize: int`, `rentPaymentDay: int`, `rentMultiplier: int`

**WeaponData.cs, CustomerData.cs, DungeonData.cs, RecipeData.cs, DailyEventData.cs**
- **공통 변수**: `id: string`
- **주요 변수**: 각 데이터의 이름, 설명, 등급, 아이콘 등 고유 메타데이터

**MaterialData.cs (확장)**
- **공통 변수**: `id: string`
- **확장 변수**: `availableDungeonIDs: List<string>` (구할 수 있는 던전 ID 목록)

### 1.2. 동적 데이터 모델 (PlayerData)

**PlayerData.cs**
- **기본 정보**: `gold: int`, `currentDay: int`, `currentPhase: GamePhase`, `phaseRemainingTime: float`
- **제작/상점**: `unlockedRecipeIDs: List<string>`, `purchasedWeaponIDs: List<string>` (상점 구매 상태)
- **인벤토리**: `ownedWeapons: List<WeaponInstance>`, `ownedMaterials: List<MaterialInstance>`
- **고객/Hero**: `visitingCustomers: List<CustomerInstance>`, `ownedHeroes: List<HeroInstance>`, `injuredHeroes: List<InjuredHeroData>`
- **모험**: `ongoingAdventures: List<AdventureInstance>`, `completedAdventures: List<AdventureResultData>`
- **제작**: `ongoingCrafting: List<CraftingInstance>`

---

## 2. Model Layer (런타임 인스턴스)

**WeaponInstance.cs**: `data: WeaponData`, `uniqueID: string`, `enhancementLevel: int`

**MaterialInstance.cs**: `data: MaterialData`, `quantity: int`

**CustomerInstance.cs**: `data: CustomerData`, `uniqueID: string`, `level: int`, `assignedDungeon: DungeonData`

**HeroInstance.cs**: `data: CustomerData`, `uniqueID: string`, `level: int`, `acquiredLevel: int`, `convertedDate: int`

**HeroCollectionData.cs**: `isAcquired: bool`, `currentLevel: int`, `acquiredLevel: int`, `acquiredDate: int`

**InjuredHeroData.cs**: `heroID: string`, `injuryDate: int`, `recoveryDate: int` (부상 관리용)

**AdventureInstance.cs**: `customer: CustomerInstance OR HeroInstance`, `weapon: WeaponInstance`, `dungeon: DungeonData`, `remainingDays: int`, `isHero: bool`

**CraftingInstance.cs**: `recipe: RecipeData`, `startDate: int`, `completionDate: int`, `isCompleted: bool`

**AdventureResultData.cs**: `customer: CustomerInstance OR HeroInstance`, `weapon: WeaponInstance`, `dungeon: DungeonData`, `isSuccess: bool`, `isWeaponRecovered: bool`, `isHero: bool`, `heroConverted: bool`, `heroLevelUp: bool`

---

## 3. View Layer (UI MonoBehaviours)

### 3.1. 공통 뷰
**StatusBarView.cs**: `goldText`, `dayText`, `phaseText`, `timeText`

**InventoryView.cs**: `weaponTabButton`, `materialTabButton`, `weaponContentRoot`, `materialContentRoot`, `currentMode: InventoryMode`, `managementModeIndicator`

**SellWeaponPanel.cs**: `weaponNameText`, `gradeText`, `elementText`, `descriptionText`, `originalPriceText`, `sellPriceText`, `sellButton`, `cancelButton`

**AlertPopup.cs**: `titleText`, `messageText`, `okButton`

**LoadingPopup.cs**: `loadingIcon`, `messageText`

### 3.2. 아침(Morning) 뷰
**MorningView.cs**: `weaponShopButton`, `blacksmithButton`, `eventButton`

**WeaponShopPanel.cs**: `weaponShopPanelContent`, `refreshButton`, `refreshCostText`, `pageNavigation`, `closeButton`

**BuyWeaponButton.cs**: `weaponIcon`, `costText`, `gradeSprite`, `isPurchased: bool`

**BuyWeaponPanel.cs**: `weaponName`, `description`, `grade`, `element`, `buyButton`, `cancelButton`

**BlacksmithPanel.cs**: `recipePanelContent`, `blacksmithDisplayContent`, `recipePageNavigation`, `craftingPageNavigation`

**RecipeButton.cs**: `recipeIcon`

**RecipeInfoPanel.cs**: `recipeName`, `description`, `cost`, `resultWeaponButton`, `materialButtons`, `craftButton`, `cancelButton`

**CraftingWeaponButton.cs**: `resultWeaponIcon`, `completionStatus: bool`, `checkIcon`

**CraftingInfoPanel.cs**: `resultWeaponIcon`, `weaponName`, `remainingDays`

**EventInfoPanel.cs**: `eventIcon`, `eventName`, `eventEffect`

### 3.3. 낮(Day) 뷰
**DayView.cs**: `customerListRoot`, `heroMenuButton`

**CustomerButton.cs**: `customerIcon`, `level`

**CustomerInfoPanel.cs**: `customerName`, `description`, `level`, `grade`, `element`, `dungeonIcon`, `weaponIcon`

**CustomerDungeonInfoPanel.cs**: `dungeonName`, `description`, `level`, `grade`, `element`

**RentWeaponPanel.cs**: `weaponName`, `description`, `grade`, `element`, `confirmButton`, `cancelButton`

**HeroMenuPanel.cs**: `heroCollectionButton`, `heroAdventureButton`, `closeButton`

**HeroCollectionPanel.cs**: `heroCollectionContent`, `pageNavigation`

**HeroCollectionButton.cs**: `heroIcon`, `isAcquired: bool`, `level`, `shadowOverlay`, `lockOverlay`

**HeroCollectionInfoPanel.cs**: `heroName`, `description`, `grade`, `element`, `currentLevelText`, `acquiredLevelText`, `specialAbility`

**HeroListPanel.cs**: `heroListContent`, `pageNavigation` (CustomerListPanel과 동일한 구조)

**HeroButton.cs**: `heroIcon`, `level`, `isInjured: bool`, `recoveryDays: int`, `isLocked: bool`, `lockOverlay`

**HeroTreatmentPanel.cs**: `heroNameText`, `injuryInfoText`, `treatmentCostText`, `treatmentDaysText`, `treatButton`, `cancelButton`

### 3.4. 밤(Night) 뷰
**NightView.cs**: `adventurePanelRoot`, `resultPanelRoot`, `nextDayButton`

**AdventureInfoPanel.cs**: `customerIcon`, `weaponIcon`, `dungeonIcon`, `remainingDay`, `isHero: bool`, `heroIndicator` (별표)

**AdventureCustomerInfoPanel.cs**: `customerName`, `description`, `level`, `element`, `grade`, `icon`

**AdventureHeroInfoPanel.cs**: `heroName`, `description`, `level`, `element`, `grade`, `icon`, `specialAbilityText`

**AdventureWeaponInfoPanel.cs**: `weaponName`, `description`, `element`, `grade`

**AdventureDungeonInfoPanel.cs**: `dungeonName`, `description`, `level`, `element`, `grade`

**ResultButton.cs**: `customerName`, `weaponIcon`, `isHero: bool`, `heroIndicator`, `heroLevelUpIndicator`

**ResultInfoPanel.cs**: `successIcon`, `weaponRecoveryIcon`, `customerIcon`, `weaponIcon`, `dungeonIcon`, `isHero: bool`, `heroConvertedText`, `heroLevelUpText`

**ResultCustomerInfoPanel.cs**: `customerName`, `description`, `level`, `element`, `grade`, `icon`

**ResultHeroInfoPanel.cs**: `heroName`, `description`, `level`, `element`, `grade`, `icon`, `specialAbilityText`

---

## 4. Controller Layer (관리자 및 시스템)

### 4.1. 핵심 관리자 (Core)
**GameController.cs**:
- **변수**: `playerData: PlayerData`, `gameConfig: GameConfig`
- **메서드**: `StartGame()`, `StartMorning()`, `StartDay()`, `StartNight()`, `OnDayPassed()`, `ProcessHeroRecovery()`, `CheckRentWarning()`, `ProcessGameOver()`

**UIManager.cs**:
- **변수**: 각종 패널/팝업 프리팹 참조
- **메서드**: `OpenPanel<T>()`, `ShowPopup<T>()`, `CloseCurrentPopup()`, `ShowAlert()`, `ShowLoading()`, `HideLoading()`, `SetPhasePanel()`, `ShowRentWarning()`, `ShowSellWeaponPanel()`

**DataManager.cs**:
- **변수**: `weaponDatas: Dictionary<string, WeaponData>`, `customerDatas: Dictionary<string, CustomerData>`, `heroDatas: Dictionary<string, CustomerData>`
- **메서드**: `Init()`, `GetWeaponData(string id)`, `GetCustomerData(string id)`, `GetAllCustomerData()`

### 4.2. 페이즈별 UI 흐름 제어 (Phases)
**MorningController.cs**:
- **메서드**: `Activate()`, `OnClickWeaponShop()`, `OnClickBlacksmith()`, `OnClickEvent()`, `CheckAndShowRentWarning()`

**DayController.cs**:
- **변수**: `selectedCustomer: CustomerInstance OR HeroInstance`, `selectedWeapon: WeaponInstance`
- **메서드**: `Activate()`, `SelectCustomer()`, `SelectHero()`, `EquipWeapon()`, `StartAdventure()`, `OnClickHeroMenu()`, `OpenHeroCollection()`, `OpenHeroAdventure()`

**NightController.cs**:
- **메서드**: `Activate()`, `ShowAdventureDetail()`, `ShowResultDetail()`, `ProcessHeroConversion()`, `ProcessHeroLevelUp()`, `GoToNextDay()`

### 4.3. 백그라운드 시스템 (Common)
**AdventureController.cs**:
- **메서드**: `ProcessDayChange()`, `StartAdventure()`, `CalculateSuccessRate()`, `ProcessAdventureResult()`, `CalculateHeroConversion()`

**InventoryController.cs**:
- **메서드**: `AddWeapon()`, `RemoveWeapon()`, `AddMaterial()`, `RemoveMaterial()`, `GetAvailableWeapons()`, `SellWeapon()`, `CalculateSellPrice()`

**CustomerManager.cs**:
- **메서드**: `RequestCustomers()`, `GenerateCustomer()`, `RemoveFromPool()` (Hero 전환 시)

**HeroManager.cs**:
- **변수**: `ownedHeroes: List<HeroInstance>`, `injuredHeroes: List<InjuredHeroData>`, `heroCollection: Dictionary<string, HeroCollectionData>`
- **메서드**: `AddHero()`, `GetAvailableHeroes()`, `GetAvailableHeroesByDay()`, `InjureHero()`, `ProcessRecovery()`, `IsHeroAcquired()`, `GetHeroCollectionStatus()`, `LevelUpHero()`, `IsHeroUnlocked()`

**WeaponShopController.cs**:
- **변수**: `shopItemPool: List<WeaponData>`, `currentShopItems: List<WeaponData>`, `purchasedItems: List<string>`
- **메서드**: `RequestShopItems()`, `PurchaseWeapon()`, `RefreshShop()`, `IsPurchased()`

**BlacksmithController.cs**:
- **변수**: `ongoingCrafting: List<CraftingInstance>`
- **메서드**: `CheckAvailability()`, `Craft()`, `ProcessCraftingCompletion()`, `GetCraftingStatus()`

**DailyEventManager.cs**:
- **메서드**: `TriggerDailyEvent()`, `GetTodayEvent()`, `ApplyEventEffect()`

**PersistenceController.cs**:
- **메서드**: `SaveGame()`, `LoadGame()`, `SaveHeroData()`, `LoadHeroData()`, `SaveHeroInheritance()`, `LoadHeroInheritance()`

### 4.4. 계산 시스템 (새로 추가)
**SuccessRateCalculator.cs**:
- **메서드**: `CalculateBaseRate()`, `CalculateElementBonus()`, `CalculateGradeBonus()` (Customer/Hero 분기), `CalculateFinalRate()`

**HeroConversionCalculator.cs**:
- **메서드**: `CalculateConversionRate()`, `GetWeaponBonus()`, `GetDungeonBonus()`, `RollConversion()`

**RentCalculator.cs**:
- **메서드**: `CalculateRent()`, `GetNextRentDay()`, `IsRentWarningDay()`, `CanPayRent()`

**HeroRewardCalculator.cs**:
- **메서드**: `CalculateHeroGoldReward()`, `CalculateAdditionalLevelUp()`, `RollLevelUpBonus()`

**HeroPenaltyCalculator.cs**:
- **메서드**: `CalculateInjuryDuration()`, `CalculateTreatmentCost()`

**HeroManager.cs**:
- **메서드**: `TreatInjuredHero()`, `CanTreatHero()`, `GetTreatmentCost()`, `IsHeroPermanentlyInjured()`

---

## 5. 구현 우선순위

### 5.1. 1단계: 핵심 시스템
- **PlayerData 확장** (Hero 관련 필드 + 월세 관련 필드 추가)
- **HeroManager, HeroInstance, HeroCollectionData 구현**
- **RentCalculator 구현** (월세 계산 시스템)
- **SuccessRateCalculator 구현** (Hero 보정 포함)

### 5.2. 2단계: Hero UI 시스템
- **Hero 도감 UI** (HeroCollectionPanel 등)
- **Hero 모험 UI** (HeroListPanel 등)
- **Hero 잠금해제 UI** (잠금 상태 표시)
- **밤 UI Hero 통합 표시** (Customer/Hero 구분)

### 5.3. 3단계: Hero 특화 기능
- **Hero 전환 시스템** (HeroConversionCalculator)
- **Hero 부상 시스템** (InjuredHeroData 관리)
- **Hero 레벨업 시스템** (성공 시 자동 레벨업)
- **Hero 전용 능력** (성공률, 회수율 보정)

### 5.4. 4단계: 추가 기능
- **무기 판매 시스템** (SellWeaponPanel)
- **무기 상점 새로고침** (구매 상태 관리)
- **대장간 제작 시스템** (CraftingInstance 관리)
- **월세 경고 시스템** (납부일 하루 전 경고)
- **MaterialData 확장** (던전 정보 연결)

---

## 6. 테스트 체크포인트

### 6.1. Hero 시스템 테스트
- [ ] 고객 → Hero 전환 확률 계산
- [ ] Hero 전용 성공률 보정 적용
- [ ] Hero 부상 시스템 (10일 회복)
- [ ] Hero 도감 수집 상태 관리
- [ ] Hero 레벨업 시스템 정상 동작
- [ ] Hero 잠금해제 조건 확인

### 6.2. UI 통합 테스트
- [ ] Customer/Hero 통합 표시 (밤 페이즈)
- [ ] 인벤토리 모드 전환 (페이즈별)
- [ ] StatusBar 실시간 업데이트
- [ ] 페이지네이션 동작
- [ ] 판매 UI 정상 동작

### 6.3. 데이터 무결성 테스트
- [ ] Customer 풀에서 Hero 풀로 이동
- [ ] Hero 부상 상태 저장/로드
- [ ] Hero 레벨 정보 정확한 저장/로드
- [ ] 무기 상점 구매 상태 유지
- [ ] 제작 진행 상황 저장/로드

### 6.4. 월세 시스템 테스트
- [ ] 월세 계산 정확성 확인
- [ ] 월세 경고 타이밍 확인 (납부일 하루 전)
- [ ] 게임오버 조건 정확성 확인

### 6.5. 판매 시스템 테스트
- [ ] 무기 판매가 계산 정확성 (구매가의 50%)
- [ ] 재료 판매 방지 확인
- [ ] 판매 확인 팝업 정상 동작