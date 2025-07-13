# UI 흐름 & 이벤트 (UI Flow & Events) - 최신 완성본

**목적:** 최종적으로 확정된 아키텍처와 Hero 시스템에 맞춰, 사용자 행동에 따른 UI 전환 로직과 컨트롤러 간의 명확한 이벤트 처리 방식을 정의합니다.

---

## 1. UI 제어 아키텍처

### 1.1. 공통 UI 레이아웃
```
┌─────────────────────────────────────┐
│                                     │ ← 3/5 비율
│      현재 페이즈 패널 영역           │   (페이즈별 메인 컨텐츠)
│    (Morning/Day/Night View)        │
│                                     │
├─────────────────────────────────────┤
│ 골드: 1,500G | 12일차 | 아침 | 03:45 │ ← StatusBar (중간 구분선)
├─────────────────────────────────────┤
│        InventoryView (하단)         │ ← 2/5 비율 (항상 표시)
│      무기 탭  |  재료 탭            │
└─────────────────────────────────────┘
```

### 1.2. 핵심 관리자
- **UIManager (중앙 UI 통제실)**: 모든 UI 패널과 팝업의 생성, 표시, 소멸을 전담하는 싱글톤 관리자
- **페이즈 컨트롤러 (Morning/Day/NightController)**: 각 시간대의 사용자 상호작용과 게임 로직 흐름 제어
- **팝업 시스템**: AlertPopup, LoadingPopup만 사용 (ConfirmPopup은 각 패널에서 직접 처리)

---

## 2. 공통 UI 시스템

### 2.1. StatusBarView (상태 표시줄)
- **위치**: 페이즈 패널과 인벤토리 사이 (구분선 역할)
- **자동 업데이트**: GameController가 PlayerData 변경 시 실시간 반영
- **표시 정보**: 골드, 날짜, 페이즈, 남은 시간 (아침/낮만)

### 2.2. InventoryView (인벤토리)
- **항상 활성화**: 모든 페이즈에서 하단에 고정 표시
- **모드별 동작**:
  - **아침**: Management 모드 (무기 관리/정보 확인)
  - **낮**: Selection 모드 (무기 장착용 선택)
  - **밤**: Normal 모드 (정보 확인만)

### 2.3. 공통 팝업
- **AlertPopup**: 골드 부족, 재료 부족 등 알림용
- **LoadingPopup**: 저장/로드, 페이즈 전환 시 사용
- **확인/선택**: 각 패널에서 직접 "확인/취소" 버튼으로 처리

---

## 3. 아침 (Morning) UI 흐름

### 3.1. 페이즈 시작
```
GameController.StartMorning()
    → MorningController.Activate()
        → UIManager.SetPhasePanel(MorningView)
        → InventoryView.SetMode(Management)
```

### 3.2. 무기 상점 흐름
```
MorningView.weaponShopButton 클릭
    → MorningController.OnClickWeaponShop()
        → WeaponShopController.GetShopItems()
        → UIManager.OpenPanel<WeaponShopPanel>(itemList)
            → BuyWeaponButton 클릭
                → UIManager.OpenPopup<BuyWeaponPanel>(weaponData)
                    → [구매] 버튼 클릭 → 구매 처리
                    → [취소] 버튼 클릭 → 팝업 닫기
            → [새로고침] 버튼 클릭 (1000골드)
                → WeaponShopController.RefreshShop()
                → 상점 목록 재생성, 구매 상태 초기화
```

### 3.3. 대장간 흐름
```
MorningView.blacksmithButton 클릭
    → MorningController.OnClickBlacksmith()
        → BlacksmithController.CheckAvailability()
        → UIManager.OpenPanel<BlacksmithPanel>()
            → 레시피 영역 (상단 3/5): RecipeButton 리스트
            → 제작 현황 (하단 2/5): CraftingWeaponButton 리스트
                → RecipeButton 클릭 → RecipeInfoPanel
                    → [제작] 버튼 → 제작 시작
                → CraftingWeaponButton 클릭
                    → 제작중: CraftingInfoPanel (진행 상황)
                    → 완료: 무기 획득, 인벤토리로 이동
```

### 3.4. 이벤트 확인 흐름
```
MorningView.eventCheckButton 클릭
    → MorningController.OnClickEvent()
        → DailyEventManager.GetTodayEvent()
        → UIManager.OpenPopup<EventInfoPanel>(eventData)
```

### 3.5. 월세 경고 흐름
```
GameController.StartMorning()
→ MorningController.CheckAndShowRentWarning()
→ RentCalculator.IsRentWarningDay()
→ 경고일인 경우: UIManager.ShowRentWarning()
```

### 3.6. 무기 판매 흐름 (신규)
```
InventoryView.SetMode(Management)
→ 무기 아이템의 판매 버튼 표시
→ 판매 버튼 클릭 → UIManager.ShowSellWeaponPanel()
→ [판매] 확인 → InventoryController.SellWeapon()
→ 골드 추가 → StatusBarView.UpdateGold()
```

---

## 4. 낮 (Day) UI 흐름

### 4.1. 페이즈 시작
```
GameController.StartDay()
    → DayController.Activate()
        → CustomerManager.RequestCustomers()
        → UIManager.SetPhasePanel(DayView)
        → InventoryView.SetMode(Selection)
```

### 4.2. 고객 모험 준비 흐름
```
DayView.CustomerButton 클릭
    → DayController.SelectCustomer(customer)
        → UIManager.OpenPanel<CustomerInfoPanel>(customerData)
            → dungeonIcon 클릭 → CustomerDungeonInfoPanel (정보 확인)
            → weaponIcon 클릭 
                → InventoryView.HighlightForSelection(true)
                → 무기 선택 → RentWeaponPanel
                    → [확인] 버튼 → 모험 예약 완료
                    → CustomerInfoPanel.weaponIcon 업데이트
```

### 4.3. Hero 시스템 흐름
```
DayView.heroMenuButton 클릭
    → DayController.OnClickHeroMenu()
        → HeroManager.GetAvailableHeroesByDay(currentDay) ← 잠금해제 확인
            → UIManager.OpenPopup<HeroMenuPanel>()
                → [Hero 도감] 버튼 클릭
                    → UIManager.OpenPanel<HeroCollectionPanel>()
                        → 미획득: 그림자 아이콘 (클릭 불가)
                        → 획득: 실제 아이콘 → HeroCollectionInfoPanel
                → [Hero 모험] 버튼 클릭
                    → UIManager.OpenPanel<HeroListPanel>()
                        → 정상 Hero: CustomerListPanel과 동일한 흐름
                        → 부상 Hero: 회색 처리, "회복까지 X일 남음"
```

### 4.4. Hero 치료 시스템 흐름
```
### 4.4. Hero 치료 시스템 흐름 (신규)
HeroListPanel에서 부상 Hero 클릭
    → HeroManager.CanTreatHero() 확인
        → UIManager.OpenPopup<HeroTreatmentPanel>()
            → [치료] 버튼 클릭
                → HeroManager.TreatInjuredHero()
                → 골드 차감, 치료 시작
            → [취소] 버튼 클릭
                → 팝업 닫기, 부상 상태 유지
```

---

## 5. 밤 (Night) UI 흐름

### 5.1. 페이즈 시작
```
GameController.StartNight()
    → NightController.Activate()
        → AdventureController.GetOngoingAdventures()
        → AdventureController.GetCompletedAdventures()
        → UIManager.SetPhasePanel(NightView)
        → InventoryView.SetMode(Normal)
        → Customer/Hero 통합 정렬 및 표시
```

### 5.2. 진행중 모험 확인 흐름
```
정렬 순서: Customer 모험들 → Hero 모험들 (각 그룹 내 남은 일수 순)

AdventureInfoPanel 클릭
    → customerIcon 클릭
        → Customer: AdventureCustomerInfoPanel
        → Hero: AdventureHeroInfoPanel (특수능력 정보 추가)
    → weaponIcon 클릭 → AdventureWeaponInfoPanel
    → dungeonIcon 클릭 → AdventureDungeonInfoPanel
```

### 5.3. 완료된 모험 결과 확인 흐름
```
정렬 순서: Customer 결과들 → Hero 결과들

ResultButton 클릭
    → UIManager.OpenPopup<ResultInfoPanel>(resultData)
        → 성공/실패, 무기 회수 여부 표시
            → Hero 성공 시: 레벨업 표시 (25 → 26)
        → customerIcon 클릭
            → Customer: ResultCustomerInfoPanel
            → Hero: ResultHeroInfoPanel
        → Hero 전환 확률 계산 및 처리 (성공 시에만)
```

### 5.4. Hero 전환 처리
```
모험 성공 시:
    → AdventureController.CalculateHeroConversion()
        → 확률 계산: 기본 확률 × (무기 보정 + 던전 보정)
        → 전환 성공 시:
            → CustomerManager.RemoveFromPool(customer)
            → HeroManager.AddToHeroPool(newHero)
            → HeroCollectionPanel.UpdateCollection()
```

### 5.5. 다음 날 진행
```
NightView.nextDayButton 클릭
    → NightController.GoToNextDay()
        → PersistenceController.SaveGame() (자동저장)
        → AdventureController.ProcessDayChange()
        → GameController.StartMorning()
```

---

## 6. 상호작용 패턴

### 6.1. 인벤토리 모드 전환
```
페이즈 전환 시:
GameController → 각 페이즈 Controller → UIManager → InventoryView.SetMode()

무기 선택 필요 시:
DayController → UIManager → InventoryView.HighlightForSelection(true)
→ 무기 선택 → RentWeaponPanel → 확인 → InventoryView.HighlightForSelection(false)
```

### 6.2. 팝업 계층 관리
```
UIManager 팝업 계층:
Level 0: 페이즈 패널 (Morning/Day/Night View)
Level 1: 메인 팝업 (WeaponShopPanel, CustomerInfoPanel 등)
Level 2: 상세 정보 팝업 (BuyWeaponPanel, RecipeInfoPanel 등)
Level 3: 하위 정보 팝업 (MaterialInfoPanel, DungeonInfoPanel 등)
Level 4: 시스템 팝업 (AlertPopup, LoadingPopup)
```

### 6.3. 데이터 흐름
```
사용자 입력 → View → Controller → 로직 처리 → UIManager → View 업데이트

예시: 무기 구매
BuyWeaponPanel.[구매] 클릭 
    → MorningController.OnPurchaseWeapon()
        → WeaponShopController.PurchaseWeapon()
            → InventoryController.AddWeapon()
                → PlayerData 업데이트
                    → StatusBarView.UpdateGold() (자동)
                    → InventoryView.RefreshWeaponList()
```

---

## 7. Hero 시스템 특별 처리

### 7.1. Hero 상태 관리
- **활성 상태**: 정상적인 모험 가능
- **부상 상태**: 10일간 모험 불가, 회색 처리, 클릭 비활성화
- **도감 상태**: 그림자 (미획득) ↔ 실제 아이콘 (획득)

### 7.2. Hero 구분 표시
- **아이콘**: Hero 아이콘에 별표(★) 오버레이
- **이름**: Hero명★ 형태로 표시
- **정렬**: Customer 우선, Hero 나중 (각 그룹 내 정렬 유지)

### 7.3. Hero 전용 능력 표시
- **정보 패널**: Hero 정보 확인 시 특수능력 정보 추가 표시
- **성공률**: Hero 전용 등급보정 자동 적용
- **무기 보호**: Hero 전용 회수 확률 자동 적용

---

## 8. 구현 시 고려사항

### 8.1. 성능 최적화
- **StatusBar 업데이트**: 변경된 값만 선택적 업데이트
- **인벤토리 모드**: 모드 전환 시 필요한 UI만 갱신
- **리스트 페이지네이션**: GameConfig.uiPageSize에 따른 동적 분할

### 8.2. 사용자 경험
- **직관적 피드백**: 각 액션에 대한 명확한 시각적 반응
- **일관된 조작**: 모든 페이즈에서 동일한 상호작용 패턴
- **명확한 구분**: StatusBar를 통한 영역 구분

### 8.3. 데이터 무결성
- **자동저장**: 각 페이즈 전환 시 자동저장으로 데이터 보호
- **상태 동기화**: PlayerData 변경 시 모든 관련 UI 자동 업데이트
- **Hero 전환**: Customer 풀에서 제거, Hero 풀에 추가의 원자적 처리

### 8.4. 확장성
- **모듈화된 팝업**: 새로운 팝업 추가 시 UIManager 확장
- **Hero 특수능력**: TODO로 남겨둔 개별 Hero 특수능력 시스템
- **이벤트 시스템**: 일일 이벤트 확장을 위한 확장 가능한 구조