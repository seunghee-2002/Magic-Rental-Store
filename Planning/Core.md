# 핵심 시스템 설계 (Core Systems) - Hero 시스템 포함 최종본

**목적:** Hero 시스템을 포함한 Model, View, Controller별 파일/클래스와 책임을 최종적으로 확정하고, 개발에 바로 착수할 수 있는 상세한 시스템 동작 방식을 정의합니다.

---

## 1. 핵심 관리 시스템 (Core/Global Systems)

**설명**: 게임 전역에 걸쳐 항상 동작하며 다른 모든 시스템을 조율하는 핵심 관리자들입니다.

### 1.1. 게임 흐름 관리 (`GameController.cs`) - Hero 확장

**역할**: 게임의 전체 흐름(아침→낮→밤)과 시간, 날짜를 관리하며, Hero 시스템의 회복 처리를 담당합니다.

**주요 로직**:
- `StartMorning()` 호출 시: `DailyEventManager.TriggerDailyEvent()` 실행 및 `MorningController` 활성화
- `Night` → `Morning` 전환 시: `OnDayPassed()` 이벤트 발생으로 모든 시스템에 날짜 경과 알림
- **Hero 회복 처리**: `ProcessHeroRecovery()`를 통해 부상 중인 Hero들의 회복 상태 확인 및 처리
- **Hero 전환 조율**: Customer → Hero 전환 시 모든 관련 시스템(CustomerManager, HeroManager) 동기화

### 1.2. UI 관리 (`UIManager.cs`) - Hero 확장

**역할**: 모든 UI 패널과 팝업의 생성, 표시, 소멸을 전담하는 중앙 관리자입니다.

**주요 로직**:
- 기존: `OpenPanel<T>()`, `ShowPopup<T>()` 요청 처리
- **Hero UI 관리**: Hero 도감, Hero 모험 관련 UI 패널 전담 관리
- **통합 표시 관리**: Customer/Hero 구분 표시 UI 효과 관리
- **새로운 팝업 시스템**: AlertPopup, LoadingPopup만 사용 (ConfirmPopup 제거)

### 1.3. 데이터 관리 (`DataManager.cs`) - Hero 확장

**역할**: 게임 시작 시 모든 정적 데이터를 로드하여 Dictionary 형태로 보관하는 '데이터 사전'입니다.

**확장된 메서드**:
- 기존: `GetWeaponData()`, `GetCustomerData()` 등
- **Hero 도감용**: `GetAllCustomerData()` - Hero 도감 표시용 전체 Customer 데이터
- **재료-던전 연결**: `GetMaterialDungeonInfo()` - MaterialData의 availableDungeonIDs 정보 관리

---

## 2. Hero 시스템 전용 관리자 (신규)

### 2.1. Hero 관리 (`HeroManager.cs`)

**역할**: Hero 수집, 관리, 부상 시스템을 전담하는 관리자입니다.

**주요 책임**:
- **Hero 수집 관리**: Customer → Hero 전환 처리 및 도감 업데이트
- **부상 시스템**: Hero 실패 시 부상 처리, 10일 회복 관리
- **가용성 확인**: 모험 가능한 Hero 목록 제공
- **도감 상태**: 수집 상태 관리 및 UI 동기화

**핵심 메서드**:
```csharp
public void AddHero(CustomerInstance customer);           // Customer를 Hero로 전환
public List<HeroInstance> GetAvailableHeroes();          // 모험 가능한 Hero 목록
public void InjureHero(string heroID);                   // Hero 부상 처리
public void ProcessRecovery(int currentDay);             // 부상 회복 처리
public bool IsHeroAcquired(string customerID);           // Hero 획득 여부
public Dictionary<string, bool> GetHeroCollectionStatus(); // 도감 상태
---

## 3. 아침 (Morning): 공급 및 정비 시스템 - 확장

**핵심 페이즈**: 아침 (Morning)  
**주요 컨트롤러**: `MorningController`, `WeaponShopController`, `BlacksmithController`  
**핵심 View**: `MorningView.prefab` (무기상점, 대장간, 이벤트 버튼)

### 3.1. 상세 흐름 (확장)

1. **페이즈 시작**: `GameController`가 `MorningController.Activate()` 호출
2. **Hero 회복 처리**: `GameController.ProcessHeroRecovery()` - 부상 중인 Hero들 회복 확인
3. **이벤트 확인**: 일일 이벤트 정보 표시 (Hero 시스템에는 직접 영향 없음)
4. **무기 상점 이용 (새로고침 확장)**:
   - 기존 구매 흐름 + 새로고침 기능 (1000골드)
   - 새로고침 시 구매 상태 초기화, 새로운 무기 목록 생성
5. **대장간 이용 (제작 시스템 확장)**:
   - 레시피 영역 (상단 3/5) + 제작 현황 (하단 2/5)
   - 동시 제작 지원, 완료 시 체크 표시 → 클릭하여 획득

---

## 4. 낮 (Day): 모험 준비 및 Hero 시스템

**핵심 페이즈**: 낮 (Day)  
**주요 컨트롤러**: `DayController`, `CustomerManager`, `HeroManager`, `AdventureController`  
**핵심 View**: `DayView.prefab`, `CustomerInfoPanel.prefab`, `HeroMenuPanel.prefab`

### 4.1. 고객 모험 준비 시스템 (기존)

1. **페이즈 시작**: `DayController.Activate()` 호출
2. **고객 목록 표시**: `CustomerManager`로부터 방문 고객 리스트 받아 표시
3. **고객 선택**: 7:3 비율 `CustomerInfoPanel` (상단: 고객 정보, 하단: 던전/무기 선택)
4. **무기 장착**: 인벤토리 최상위 레이어 → 무기 선택 → `RentWeaponPanel` → 모험 예약
5. **모험 시작**: 밤 페이즈 전환 시 예약된 모험들 일괄 시작

### 4.2. Hero 시스템 (신규)

#### 4.2.1. Hero 메뉴 시스템
```
DayView.heroMenuButton 클릭
    → HeroMenuPanel 열기
        → [Hero 도감] 버튼 → HeroCollectionPanel
        → [Hero 모험] 버튼 → HeroListPanel
```

#### 4.2.2. Hero 도감 시스템
**구조**: `HeroCollectionPanel` (모든 Customer 풀 표시)
- **미획득**: 그림자 아이콘만 표시, 클릭 불가
- **획득**: 실제 아이콘 표시, 클릭 시 `HeroCollectionInfoPanel` 열기
- **수집 상태**: `HeroManager.GetHeroCollectionStatus()`로 관리

#### 4.2.3. Hero 모험 시스템
**구조**: `HeroListPanel` (CustomerListPanel과 완전 동일한 구조)
- **정상 Hero**: 일반 고객과 동일한 모험 준비 흐름
- **부상 Hero**: 회색 처리, 클릭 불가, "회복까지 X일 남음" 표시
- **Hero 전용 능력**: 자동으로 적용되는 강화된 성공률 및 무기 보호

---

## 5. 밤 (Night): 결과 정산 및 Hero 전환 시스템

**핵심 페이즈**: 밤 (Night)  
**주요 컨트롤러**: `NightController`, `AdventureController`, `HeroConversionCalculator`  
**핵심 View**: `NightView.prefab` (Customer/Hero 통합 표시)

### 5.1. Customer/Hero 통합 표시 시스템 (신규)

#### 5.1.1. 진행 중 모험 (`AdventureInfoPanel`)
**통합 표시 구조**:
```
┌─────────────────────────────────────┐
│ [Customer 아이콘] [무기] [던전] 3일   │ ← Customer 우선 표시
│ [Customer 아이콘] [무기] [던전] 1일   │
│ [Hero 아이콘★] [무기] [던전] 2일     │ ← Hero는 별표로 구분
│ [Hero 아이콘★] [무기] [던전] 5일     │
└─────────────────────────────────────┘
```

**정렬 규칙**:
1. Customer 모험들 먼저 표시
2. Hero 모험들 나중에 표시  
3. 각 그룹 내에서는 남은 일수 순 정렬

#### 5.1.2. 완료된 모험 (`ResultButton`)
**동일한 통합 표시 + 정렬 규칙**:
1. Customer 결과들 먼저 표시
2. Hero 결과들 나중에 표시
3. 각 그룹 내에서는 완료 순서대로 표시

### 5.2. Hero 전환 시스템 (신규)

#### 5.2.1. Hero 전환 흐름
```
1. 모험 완료 시 (AdventureController)
   → 성공 여부 계산 (SuccessRateCalculator - Hero 보정 적용)
   → 성공 시: Hero 전환 확률 계산 (HeroConversionCalculator)

2. Hero 전환 성공 시
   → CustomerManager.RemoveFromPool() - Customer 풀에서 제거
   → HeroManager.AddHero() - Hero 풀에 추가
   → PlayerData 업데이트
   → UI 동기화 (도감, 리스트 등)

3. Hero 실패 시
   → HeroManager.InjureHero() - 10일 부상 처리
   → InjuredHeroData 생성
   → UI 상태 변경 (회색 처리, 타이머 표시)
```

#### 5.2.2. Hero 전용 능력 처리
- **성공률 계산**: Hero 전용 등급보정 자동 적용
- **무기 보호**: Hero 전용 회수 확률 (+10%) 자동 적용
- **실패 패널티**: 일반 고객은 사망, Hero는 10일 부상

### 5.3. 상세 흐름 (Hero 확장)

1. **페이즈 시작**: `NightController.Activate()` 호출
2. **모험 결과 처리**: `AdventureController`로부터 진행중/완료 모험 목록 받기
3. **Customer/Hero 통합 정렬**: Customer 우선, Hero 구분 표시로 정렬
4. **진행 중 모험 확인**: Customer/Hero 구분된 `AdventureInfoPanel` 표시
5. **완료된 모험 확인**: `ResultInfoPanel`에서 Hero 전환 확률 계산 및 처리
6. **Hero 전환 처리**: 성공한 Customer 대상 확률 계산 → 전환 성공 시 시스템 전체 업데이트
7. **다음 날로**: 모든 처리 완료 후 새로운 하루 시작

---

## 6. 데이터 구조 및 모델 (Hero 확장)

### 6.1. 정적 데이터 (Static Data / ScriptableObjects)

**기존**: `GameConfig`, `WeaponData`, `CustomerData`, `DungeonData`, `RecipeData`, `MaterialData`, `DailyEventData`

**Hero 시스템 확장**:
- **GameConfig**: `heroInjuryDays`, `weaponShopRefreshCost` 등 Hero 관련 설정
- **MaterialData**: `availableDungeonIDs` 필드 추가 (재료 획득 던전 정보)

### 6.2. 동적 데이터 모델 (Hero 확장)

**PlayerData**: Hero 시스템 필드 추가
```csharp
public List<HeroInstance> ownedHeroes;          // 보유한 Hero 목록  
public List<InjuredHeroData> injuredHeroes;     // 부상 중인 Hero 목록
public Dictionary<string, bool> heroCollection; // Hero 도감 수집 상태
```

### 6.3. 런타임 인스턴스 (Hero 확장)

**신규 추가**:
- **HeroInstance**: Customer에서 전환된 Hero의 런타임 인스턴스
- **InjuredHeroData**: 부상당한 Hero의 회복 정보 관리
- **CraftingInstance**: 대장간 제작 진행 상황 관리

**기존 확장**:
- **AdventureInstance**: `isHero` 플래그 추가로 Customer/Hero 구분
- **AdventureResultData**: `isHero`, `heroConverted` 플래그 추가

---

## 7. 시스템 간 상호작용 (Hero 포함)

### 7.1. Hero 전환 데이터 흐름
```
모험 성공 → SuccessRateCalculator (Hero 보정 적용)
         → HeroConversionCalculator (전환 확률 계산)
         → CustomerManager.RemoveFromPool()
         → HeroManager.AddHero()
         → PlayerData 업데이트
         → UIManager (도감, 리스트 UI 동기화)
```

### 7.2. Hero 부상 관리 흐름
```
Hero 모험 실패 → HeroManager.InjureHero()
              → InjuredHeroData 생성 (회복날짜 = 현재날짜 + 10)
              → UI 비활성화 처리
              → 매일 GameController.ProcessHeroRecovery() 체크
              → 회복 완료 시 자동 복귀
```

### 7.3. Customer/Hero 통합 관리
```
낮 페이즈: CustomerManager (방문 고객) + HeroManager (보유 Hero) 분리 관리
밤 페이즈: AdventureController에서 Customer/Hero 통합 처리
UI 표시: Customer 우선, Hero 별표 구분하여 통합 정렬
```