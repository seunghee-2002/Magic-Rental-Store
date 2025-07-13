# 아키텍처 & 설정 (Architecture & Configuration) - Hero 시스템 포함 최종본

**목적:** Hero 시스템을 포함한 프로젝트의 완전한 골격을 잡고, 최종적으로 확정된 폴더 구조, 코딩 규칙, 핵심 관리자의 역할을 상세히 정의합니다.

---

## 1. 프로젝트 폴더 구조 (완전 확장)

**설명:** Hero 시스템과 모든 게임 메커니즘을 포함한 시스템의 역할을 기반으로 폴더를 명확히 분리하여, 유지보수성과 팀원 간의 협업 효율을 극대화합니다.

```
Assets/
└── _Project/
    ├── Data/                           # ScriptableObject 에셋 (.asset)
    │   ├── GameConfig.asset            # 전역 게임 설정
    │   └── StaticData/                 # 모든 정적 데이터 에셋
    │       ├── Weapons/                # 무기 데이터 모음
    │       │   ├── Common/
    │       │   ├── Uncommon/
    │       │   ├── Rare/
    │       │   ├── Epic/
    │       │   └── Legendary/
    │       ├── Customers/              # 고객 데이터 모음 (Hero 후보)
    │       │   ├── Common/
    │       │   ├── Uncommon/
    │       │   ├── Rare/
    │       │   ├── Epic/
    │       │   └── Legendary/
    │       ├── Dungeons/               # 던전 데이터 모음
    │       │   ├── Common/
    │       │   ├── Uncommon/
    │       │   ├── Rare/
    │       │   ├── Epic/
    │       │   └── Legendary/
    │       ├── Recipes/                # 제작 레시피 데이터
    │       │   └── Blacksmith/
    │       ├── Materials/              # 재료 데이터 모음
    │       │   ├── Common/
    │       │   ├── Uncommon/
    │       │   ├── Rare/
    │       │   ├── Epic/
    │       │   └── Legendary/
    │       └── Events/                 # 일일 이벤트 데이터
    │           └── DailyEvents/
    │
    ├── Prefabs/                        # 모든 프리팹
    │   └── UI/
    │       ├── Common/                 # 공통 UI 컴포넌트
    │       │   ├── StatusBar.prefab    # 상태 표시줄 (중간 위치)
    │       │   ├── Inventory.prefab    # 인벤토리 패널 (하단)
    │       │   ├── AlertPopup.prefab   # 알림 팝업
    │       │   ├── LoadingPopup.prefab # 로딩 팝업
    │       │   └── SellWeaponPanel.prefab # 무기 판매 팝업 (신규)
    │       ├── Panels/                 # 메인 패널 (페이즈별)
    │       │   ├── Morning/            # 아침 페이즈 UI
    │       │   │   ├── MorningView.prefab
    │       │   │   ├── WeaponShopPanel.prefab
    │       │   │   ├── BlacksmithPanel.prefab
    │       │   │   └── EventInfoPanel.prefab
    │       │   ├── Day/                # 낮 페이즈 UI
    │       │   │   ├── DayView.prefab
    │       │   │   ├── CustomerInfoPanel.prefab
    │       │   │   ├── HeroMenuPanel.prefab
    │       │   │   ├── HeroCollectionPanel.prefab
    │       │   │   └── HeroListPanel.prefab
    │       │   └── Night/              # 밤 페이즈 UI
    │       │       ├── NightView.prefab
    │       │       ├── AdventureInfoPanel.prefab
    │       │       └── ResultInfoPanel.prefab
    │       └── ListItems/              # 동적 생성 아이템
    │           ├── CustomerButton.prefab
    │           ├── HeroButton.prefab
    │           ├── HeroCollectionButton.prefab
    │           ├── WeaponButton.prefab
    │           ├── RecipeButton.prefab
    │           ├── CraftingWeaponButton.prefab
    │           └── MaterialButton.prefab
    │
    └── Scripts/                        # 모든 C# 스크립트
        ├── Core/                       # 핵심 관리자 (싱글톤)
        │   ├── GameController.cs       # 게임 전체 흐름 관리
        │   ├── UIManager.cs            # UI 중앙 관리
        │   └── DataManager.cs          # 정적 데이터 관리
        │
        ├── Data/                       # 데이터 클래스 정의
        │   ├── PlayerData.cs           # 저장/로드될 플레이어 데이터 구조
        │   ├── Enums.cs                # 게임 전역 열거형 (GamePhase, Grade, Element 등)
        │   ├── RuntimeInstances/       # 런타임 인스턴스 클래스
        │   │   ├── WeaponInstance.cs
        │   │   ├── MaterialInstance.cs
        │   │   ├── CustomerInstance.cs
        │   │   ├── HeroInstance.cs     # Hero 인스턴스
        │   │   ├── HeroCollectionData.cs # Hero 도감 데이터 (신규)
        │   │   ├── InjuredHeroData.cs  # Hero 부상 데이터
        │   │   ├── AdventureInstance.cs
        │   │   ├── CraftingInstance.cs # 제작 진행 데이터
        │   │   └── AdventureResultData.cs
        │   └── StaticData/             # ScriptableObject 클래스 정의
        │       ├── GameConfig.cs       # 전역 게임 설정
        │       ├── WeaponData.cs
        │       ├── CustomerData.cs
        │       ├── DungeonData.cs
        │       ├── MaterialData.cs     # 확장: availableDungeonIDs 포함
        │       ├── RecipeData.cs
        │       └── DailyEventData.cs
        │
        └── Systems/                    # 시스템 컨트롤러
            ├── Common/                 # 백그라운드/공통 시스템
            │   ├── AdventureController.cs      # 모험 관리 (Hero 지원)
            │   ├── InventoryController.cs      # 인벤토리 관리
            │   ├── CustomerManager.cs          # 고객 생성 및 관리
            │   ├── HeroManager.cs              # Hero 전담 관리자 (신규)
            │   ├── WeaponShopController.cs     # 무기 상점 + 새로고침
            │   ├── BlacksmithController.cs     # 대장간 + 제작 시스템
            │   ├── DailyEventManager.cs        # 일일 이벤트
            │   ├── PersistenceController.cs    # 저장/로드 (Hero 데이터 포함)
            │   ├── SuccessRateCalculator.cs    # 성공률 계산 (Hero 보정 포함)
            │   ├── HeroConversionCalculator.cs # Hero 전환 확률 계산 (신규)
            │   └── RentCalculator.cs           # 월세 계산 시스템 (신규)
            │
            └── Phases/                 # 각 페이즈의 UI 흐름 제어
                ├── MorningController.cs        # 아침 페이즈 제어
                ├── DayController.cs            # 낮 페이즈 제어 (Hero 시스템 포함)
                └── NightController.cs          # 밤 페이즈 제어 (Customer/Hero 통합)
```

---

## 2. 네이밍 & 코드 스타일

### 2.1. 기본 네이밍 규칙
- **클래스**: `PascalCase` (예: `HeroManager`, `SuccessRateCalculator`, `RentCalculator`)
- **메서드**: `PascalCase` (예: `CalculateConversionRate()`, `ProcessHeroRecovery()`, `LevelUpHero()`)
- **변수**: `camelCase` (예: `ownedHeroes`, `heroCollection`, `currentLevel`)
- **상수**: `UPPER_SNAKE_CASE` (예: `HERO_INJURY_DAYS`, `MAX_HERO_LEVEL`, `RENT_MULTIPLIER`)
- **열거형**: `PascalCase` (예: `GamePhase.Morning`, `Grade.Epic`)

### 2.2. Hero 시스템 네이밍 규칙
- **Hero 관련 클래스**: `Hero` 접두사 사용 (예: `HeroInstance`, `HeroManager`, `HeroCollectionData`)
- **Hero 전용 메서드**: `Hero` 포함 (예: `ConvertToHero()`, `IsHeroAvailable()`, `LevelUpHero()`)
- **Hero 구분 변수**: `isHero` 플래그 사용 (예: `isHero: bool`)
- **Hero 레벨 구분**: `currentLevel` (현재), `acquiredLevel` (획득 당시)

### 2.3. 파일 및 폴더 규칙
- **스크립트 파일명**: 클래스명과 반드시 일치
- **데이터 에셋 네이밍**: `ID_Name_Grade` 형식 (예: `Customer_BraveWarrior_Epic`)
- **프리팹 네이밍**: 기능명 + 타입 (예: `HeroCollectionPanel`, `SellWeaponPanel`)

---

## 3. MVC 패턴 및 아키텍처 상세

### 3.1. Model Layer (데이터 모델)
**위치**: `Scripts/Data/` 폴더
**역할**: 게임의 모든 상태와 정보를 담는 순수 데이터 클래스들

#### 정적 데이터 (ScriptableObjects)
- **GameConfig**: 전역 게임 설정 (Hero 시스템 + 월세 시스템 설정 포함)
- **WeaponData, CustomerData, DungeonData**: 게임 원본 데이터
- **MaterialData**: 확장되어 `availableDungeonIDs` 포함
- **RecipeData, DailyEventData**: 제작 및 이벤트 데이터

#### 동적 데이터 (Runtime Instances)
- **PlayerData**: 모든 동적 상태의 최상위 컨테이너
- **Customer/Hero Instances**: 고객과 Hero의 런타임 인스턴스 (레벨 관리 분리)
- **HeroCollectionData**: Hero 도감 수집 상태 전용 데이터
- **Adventure/Crafting Instances**: 진행 상황 관리 인스턴스
- **InjuredHeroData**: Hero 부상 관리 전용 데이터

### 3.2. View Layer (UI 컴포넌트)
**위치**: `Prefabs/UI/` 폴더
**역할**: 사용자에게 정보를 시각적으로 보여주고, 사용자 입력을 감지

#### 공통 UI (Common)
- **StatusBarView**: 페이즈 패널과 인벤토리 사이 구분선 역할
- **InventoryView**: 모드별 상호작용 (Normal/Selection/Management)
- **SellWeaponPanel**: 무기 판매 확인 팝업 (신규)
- **AlertPopup, LoadingPopup**: 시스템 알림 및 로딩 표시

#### 페이즈별 UI (Panels)
- **Morning**: 무기상점, 대장간, 이벤트 관련 UI + 월세 경고
- **Day**: 고객 관리 + Hero 시스템 UI (도감, 모험, 잠금해제)
- **Night**: Customer/Hero 통합 모험 결과 UI + 레벨업 표시

#### 동적 생성 UI (ListItems)
- **CustomerButton, HeroButton**: 고객/Hero 개별 아이템 (잠금 상태 표시)
- **HeroCollectionButton**: 도감의 그림자/실제 아이콘 토글
- **CraftingWeaponButton**: 제작 진행/완료 상태 표시

### 3.3. Controller Layer (로직 제어)
**위치**: `Scripts/Core/`, `Scripts/Systems/` 폴더
**역할**: 게임 로직 처리 및 Model-View 간 중재

#### 핵심 관리자 (Core)
- **GameController**: 전체 게임 흐름 + Hero 회복 처리 + 월세 관리
- **UIManager**: UI 중앙 관리 + 새로운 팝업 시스템 + 판매 UI
- **DataManager**: 정적 데이터 + Hero 도감 데이터 관리

#### 백그라운드 시스템 (Common)
- **기존 확장**: Customer, Adventure, Inventory 관리자들의 Hero 지원
- **신규 추가**: HeroManager, HeroConversionCalculator, RentCalculator 등
- **계산 시스템**: SuccessRateCalculator의 Hero 보정 로직

#### 페이즈 컨트롤러 (Phases)
- **MorningController**: 무기상점 새로고침, 대장간 제작 시스템, 월세 경고
- **DayController**: Hero 도감/모험 시스템 통합 + 잠금해제
- **NightController**: Customer/Hero 통합 표시 및 전환 처리 + 레벨업

---

## 4. 핵심 관리자 역할 정의 (확장)

### 4.1. 기존 핵심 관리자 확장

#### GameController.cs (확장)
**기존 역할**: 게임의 전체 흐름(아침→낮→밤)과 시간, 날짜 관리
**Hero 시스템 추가 역할**:
- **Hero 회복 처리**: 매일 부상당한 Hero들의 회복 상태 확인
- **Hero 전환 조율**: Customer → Hero 전환 시 모든 관련 시스템 동기화
- **Hero 레벨업 관리**: Hero 성공 시 레벨업 처리 및 UI 업데이트
- **월세 관리**: 납부일 확인, 경고 표시, 게임오버 처리
- **데이터 통합 관리**: PlayerData의 Hero 관련 필드들 중앙 관리

#### UIManager.cs (확장)
**기존 역할**: 모든 UI 패널과 팝업의 생성, 표시, 소멸 전담
**Hero 시스템 추가 역할**:
- **새로운 팝업 시스템**: AlertPopup, LoadingPopup만 사용 (ConfirmPopup 제거)
- **Hero UI 관리**: Hero 도감, Hero 모험 관련 UI 패널 관리
- **통합 표시 관리**: Customer/Hero 구분 표시 UI 효과 관리
- **판매 UI 관리**: 무기 판매 팝업 및 Management 모드 UI
- **월세 경고**: 납부일 하루 전 AlertPopup 표시

#### DataManager.cs (확장)
**기존 역할**: 모든 정적 데이터를 Dictionary로 보관하는 데이터 사전
**Hero 시스템 추가 역할**:
- **Hero 도감 데이터**: 전체 Customer 데이터를 Hero 도감용으로 제공
- **재료-던전 연결**: MaterialData의 availableDungeonIDs 정보 관리
- **Hero 데이터 조회**: Customer 데이터를 Hero 데이터로도 활용

### 4.2. 새로운 핵심 관리자

#### HeroManager.cs (신규)
**역할**: Hero 수집, 관리, 부상 시스템을 전담하는 관리자
**주요 책임**:
- **Hero 수집 관리**: Customer → Hero 전환 처리 및 도감 업데이트
- **Hero 레벨 시스템**: 성공 시 레벨업, 현재/획득 레벨 분리 관리
- **부상 시스템**: Hero 실패 시 부상 처리, 10일 회복 관리
- **잠금해제 관리**: 등급별 일수 제한 확인 및 UI 상태 관리
- **가용성 확인**: 모험 가능한 Hero 목록 제공 (잠금/부상 제외)
- **도감 상태**: 수집 상태 관리 및 UI 동기화

#### SuccessRateCalculator.cs (신규)
**역할**: Customer와 Hero를 구분한 성공률 계산 전문 시스템
**주요 책임**:
- **기본 성공률**: 고객/Hero 레벨 vs 던전 레벨 계산
- **속성 보정**: 고객-무기-던전 간 속성 상성 계산
- **등급 보정**: Customer와 Hero의 서로 다른 등급 보정 적용
- **특수 보정**: Light/Dark 속성의 1.3배 보정

#### HeroConversionCalculator.cs (신규)
**역할**: 모험 성공 시 Customer의 Hero 전환 확률 계산
**주요 책임**:
- **기본 확률**: Customer 등급별 기본 전환 확률 (5% ~ 1%)
- **무기 보정**: 무기 등급에 따른 보정값 (-30% ~ +100%)
- **던전 보정**: 던전 등급에 따른 보정값 (-20% ~ +100%)
- **최종 계산**: 기본 확률 × (1 + 보정값들) 공식 적용

#### RentCalculator.cs (신규)
**역할**: 월세 계산 및 경고 시스템 전담 관리자
**주요 책임**:
- **월세 계산**: 현재 일수 × 100골드 공식 적용
- **납부일 관리**: 7일 주기 계산 및 다음 납부일 예측
- **경고 시스템**: 납부일 하루 전 경고 타이밍 확인
- **게임오버 판정**: 골드 부족 시 게임오버 조건 확인

---

## 5. 공통 설정 관리 (GameConfig.cs 확장)

### 5.1. 기존 설정 변수
```csharp
[Header("기본 게임 설정")]
public int startingGold = 5000;              // 게임 시작 시 초기 자본금
public float dayDuration = 300f;             // 아침/낮 페이즈 지속 시간(초)
public int blacksmithUnlockDay = 3;          // 대장간 해금 일자
public int weaponShopItemCount = 8;          // 무기 상점 기본 진열 개수
```

### 5.2. Hero 시스템 추가 설정
```csharp
[Header("Hero 시스템 설정")]
public int heroInjuryDays = 10;              // Hero 부상 회복 기간
public int heroMaxLevel = 100;               // Hero 최대 레벨
public int[] heroUnlockDays = {10, 20, 30, 40, 50}; // 등급별 잠금해제 일수

[Header("Hero 보상 시스템")]
public float heroGoldMultiplier = 1.5f;      // Hero 골드 보정 배율
public int[][] heroLevelUpTables = {          // 던전별 레벨업 확률 테이블
    {40, 40, 20, 0, 0},    // Common
    {30, 40, 25, 5, 0},    // Uncommon  
    {20, 30, 40, 9, 1},    // Rare
    {0, 20, 40, 25, 15},   // Epic
    {0, 10, 30, 30, 30}    // Legendary
};

[Header("Hero 페널티 시스템")]
public int heroGoldPenaltyPerDay = 50;       // 일수당 골드 페널티
public int heroBaseInjuryDays = 5;           // 기본 부상 기간
public int heroMinInjuryDays = 1;            // 최소 부상 기간  
public int heroMaxInjuryDays = 15;           // 최대 부상 기간

public int weaponShopRefreshCost = 1000;     // 상점 새로고침 비용
public int maxHeroCount = 50;                // 최대 보유 가능 Hero 수

[Header("경제 시스템 설정")]
public int rentPaymentDay = 7;               // 월세 납부 주기 (일)
public int rentMultiplier = 100;             // 일수당 월세 배율
public bool showRentWarning = true;          // 월세 납부일 임박 경고 표시
public float weaponSellRate = 0.5f;          // 무기 판매 비율 (구매가의 50%)

[Header("UI 및 인벤토리 설정")]
public int inventoryMaxSize = 100;           // 인벤토리 최대 크기
public int dailyCustomerCount = 6;           // 일일 방문 고객 수
public int uiPageSize = 8;                   // 리스트 한 페이지당 표시 개수

[Header("경제 밸런스 설정")]
public int[] dailyEventRewards = {500, -1000, 0, 0, 0}; // 일일 이벤트 골드 보정
public float[] gradeSpawnRates = {0.75f, 0.14f, 0.07f, 0.03f, 0.01f}; // 등급별 생성 확률

[Header("제작 시스템 설정")]
public int maxSimultaneousCrafting = 5;      // 동시 제작 가능 개수
public float craftingSpeedMultiplier = 1.0f; // 제작 속도 배율
```

### 5.3. Hero 전환 확률 설정
```csharp
[Header("Hero 전환 시스템")]
[Range(0f, 10f)]
public float[] heroBaseConversionRates = {5f, 4f, 3f, 2f, 1f}; // 등급별 기본 확률(%)

[Range(-50f, 200f)]
public float[] weaponGradeBonuses = {-30f, 0f, 20f, 50f, 100f}; // 무기 등급 보정(%)

[Range(-50f, 200f)]
public float[] dungeonGradeBonuses = {-20f, 0f, 20f, 50f, 100f}; // 던전 등급 보정(%)
```

---

## 6. 데이터 흐름 아키텍처

### 6.1. 게임 시작 시 초기화 순서
```
1. DataManager.Init() 
   → 모든 ScriptableObject 로드
   → Dictionary 구성 완료

2. GameController.StartGame()
   → PlayerData 초기화 또는 로드
   → 각 시스템 관리자들에게 데이터 배포

3. HeroManager.Initialize()
   → Hero 도감 상태 복원
   → 부상 Hero 회복 상태 확인
   → Hero 잠금해제 상태 확인

4. RentCalculator.Initialize()
   → 월세 납부 상태 확인
   → 경고 필요 여부 판단

5. UIManager.SetupCommonUI()
   → StatusBar, Inventory 등 공통 UI 활성화
   → 첫 번째 페이즈 UI 표시
```

### 6.2. Hero 전환 및 레벨업 데이터 흐름
```
1. 모험 완료 시 (AdventureController)
   → 성공 여부 계산 (SuccessRateCalculator)
   → 성공 시: Hero 전환 확률 계산 (HeroConversionCalculator)
   → Hero인 경우: 레벨업 처리 (HeroManager.LevelUpHero)

2. Hero 전환 성공 시
   → CustomerManager.RemoveFromPool()
   → HeroManager.AddHero()
   → PlayerData 업데이트
   → UI 동기화 (도감, 리스트 등)

3. Hero 실패 시
   → HeroManager.InjureHero()
   → InjuredHeroData 생성
   → UI 상태 변경 (회색 처리, 타이머 표시)
```

### 6.3. 월세 및 판매 시스템 데이터 흐름
```
월세 시스템:
아침 페이즈 시작 → RentCalculator.IsRentWarningDay()
→ 경고일인 경우: UIManager.ShowRentWarning()
→ 납부일인 경우: 골드 확인 → 부족 시 GameController.ProcessGameOver()

판매 시스템:
Management 모드 → 무기 판매 버튼 클릭
→ UIManager.ShowSellWeaponPanel()
→ 판매 확인 → InventoryController.SellWeapon()
→ 골드 추가 → StatusBar 업데이트
```

### 6.4. 저장/로드 데이터 흐름
```
저장 시:
GameController → 각 시스템에서 현재 상태 수집
→ PlayerData 객체 완성 → PersistenceController
→ JSON 직렬화 → 파일 저장

로드 시:
PersistenceController → JSON 역직렬화 
→ PlayerData 객체 생성 → GameController
→ 각 시스템에 데이터 배포 → 상태 복원
```

---

## 7. 확장성 고려사항

### 7.1. Hero 시스템 확장 준비
- **개별 Hero 특수능력**: 현재 TODO로 남겨둔 Hero별 고유 능력 시스템
- **Hero 등급 업그레이드**: Hero 자체의 성장 시스템
- **Hero 장비 시스템**: Hero 전용 장비나 액세서리
- **Hero 스킬 트리**: 레벨업 시 스킬 포인트 할당 시스템

### 7.2. UI 시스템 확장성
- **새로운 페이즈**: 추가 시간대 (새벽, 저녁 등)
- **고급 인벤토리**: 카테고리별 필터링, 검색 기능
- **Hero 관리 UI**: Hero별 상세 관리 패널
- **상점 확장**: 재료 상점, 장비 상점 등

### 7.3. 데이터 시스템 확장성
- **클라우드 저장**: 로컬 저장 외 클라우드 동기화
- **데이터 압축**: 대용량 Hero 데이터 효율적 저장
- **버전 관리**: 세이브 파일 호환성 관리
- **랭킹 시스템**: 최고 생존 일수 기록 관리

---

## 8. 성능 최적화 고려사항

### 8.1. Hero 시스템 최적화
- **Hero 도감**: 필요할 때만 로드하는 지연 로딩
- **부상 Hero**: 매일 전체 순회 대신 이벤트 기반 처리
- **Hero 리스트**: 페이지네이션과 오브젝트 풀링 활용
- **레벨업 계산**: 캐싱을 통한 반복 계산 최소화

### 8.2. UI 최적화
- **StatusBar**: 변경된 값만 선택적 업데이트
- **인벤토리**: 모드 전환 시 필요한 부분만 갱신
- **팝업 관리**: 미리 생성된 팝업 재사용
- **판매 UI**: 가격 계산 결과 캐싱

### 8.3. 메모리 관리
- **인스턴스 풀링**: 자주 생성/삭제되는 UI 요소들
- **가비지 컬렉션**: 불필요한 객체 생성 최소화
- **텍스처 메모리**: Hero 아이콘 등 이미지 자원 최적화
- **데이터 구조**: Dictionary vs List 성능 비교 및 최적화