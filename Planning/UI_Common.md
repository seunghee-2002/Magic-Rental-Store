# 공통 UI 상세 설계 (Common UI Detailed Design)

**목적:** 모든 페이즈에서 공통으로 사용되는 UI 컴포넌트와 레이아웃을 상세히 정의하여 일관된 사용자 경험을 제공합니다.

---

## 1. 전체 화면 레이아웃 구조

### 1.1. 기본 레이아웃 (3:2 비율)
```
┌─────────────────────────────────────┐
│                                     │
│                                     │ ← 3/5 비율
│      현재 페이즈 패널 영역           │   (메인 컨텐츠)
│    (Morning/Day/Night View)        │
│                                     │
│                                     │
├─────────────────────────────────────┤
│ 골드: 1,500G | 12일차 | 아침 | 03:45 │ ← StatusBarView (얇은 구분선)
├─────────────────────────────────────┤
│        InventoryView (하단)         │ ← 2/5 비율
│      무기 탭  |  재료 탭            │   (항상 표시)
└─────────────────────────────────────┘
```

### 1.2. 컴포넌트 정의
- **phaseContentArea: Transform** (페이즈별 패널이 들어갈 영역)
- **statusBarView: StatusBarView** (중간 구분선 역할)
- **inventoryView: InventoryView** (하단 고정)

---

## 2. StatusBarView (상태 표시줄)

### 2.1. 위치 및 역할
- **위치**: 페이즈 패널과 인벤토리 사이 (구분선 역할)
- **스타일**: 최대한 얇은 바 형태
- **기능**: 현재 게임 상태 정보 표시

### 2.2. 표시 정보
- **goldText: TMP_Text** (현재 보유 골드)
- **dayText: TMP_Text** (현재 게임 날짜)
- **phaseText: TMP_Text** (현재 페이즈)
- **timeText: TMP_Text** (페이즈 남은 시간, 아침/낮만)

### 2.3. UI 레이아웃 (얇은 바 형태)
```
┌─────────────────────────────────────┐
│ 골드: 1,500G | 12일차 | 아침 | 03:45 │ ← 높이 최소화
└─────────────────────────────────────┘
```

### 2.4. 자동 업데이트 로직
- **PlayerData 변경 감지**: GameController가 데이터 변경 시 이벤트 발생
- **실시간 반영**: 골드 증감, 날짜 변경, 페이즈 전환 즉시 표시
- **타이머 표시**: 아침/낮 페이즈에서만 남은 시간 카운트다운

### 2.5. 컴포넌트 세부사항
```csharp
public class StatusBarView : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text goldText;
    public TMP_Text dayText;
    public TMP_Text phaseText;
    public TMP_Text timeText;
    
    [Header("Layout Settings")]
    public float barHeight = 30f; // 얇은 바 높이
    
    // PlayerData 변경 시 호출되는 메서드들
    public void UpdateGold(int gold);
    public void UpdateDay(int day);
    public void UpdatePhase(GamePhase phase);
    public void UpdateTime(float remainingTime);
}
```

---

## 3. InventoryView (인벤토리 패널)

### 3.1. 탭 구조
- **weaponTabButton: Button** (무기 탭 버튼)
- **materialTabButton: Button** (재료 탭 버튼)
- **weaponContentRoot: Transform** (무기 리스트 부모)
- **materialContentRoot: Transform** (재료 리스트 부모)

### 3.2. UI 레이아웃 (넓은 영역 활용)
```
┌─────────────────────────────────────┐
│  [무기]    [재료]     < 1/3 >       │ ← 탭 + 페이지네이션
├─────────────────────────────────────┤
│                                    │
│ [무기1] [무기2] [무기3] [무기4]      │ ← 넓어진 인벤토리 영역
│ [무기5] [무기6] [무기7] [무기8]      │   더 많은 아이템 표시 가능
│                                    │
│ 또는                                │
│                                    │
│ [재료1] [재료2] [재료3] [재료4]      │ ← 재료 탭 활성화
│ x15     x7      x23     x3         │
│ [재료5] [재료6] [재료7] [재료8]      │
└─────────────────────────────────────┘
```

### 3.3. 페이지네이션
- **weaponPageNavigation: UI Component** (무기용 페이지네이션)
- **materialPageNavigation: UI Component** (재료용 페이지네이션)
- **GameConfig.uiPageSize**에 따라 한 페이지당 표시 개수 결정

### 3.4. 상호작용 모드
```csharp
public enum InventoryMode
{
    Normal,      // 기본 모드 (정보 확인만)
    Selection,   // 선택 모드 (무기 장착용)
    Management   // 관리 모드 (무기 판매용)
}
```

### 3.5. 페이즈별 동작
- **아침 페이즈**: Management 모드 (무기 판매, 재료 확인)
- **낮 페이즈**: Selection 모드 (무기 장착용 선택)
- **밤 페이즈**: Normal 모드 (정보 확인만)

### 3.6. Management 모드 (무기 판매)

#### 3.6.1. UI 변화
- **무기 아이템**: 각 무기에 "판매" 버튼 추가 표시
- **재료 아이템**: 판매 버튼 없음 (판매 불가)
- **시각적 구분**: Management 모드임을 알리는 UI 표시

#### 3.6.2. 판매 버튼 레이아웃
```
┌─────────────────────┐
│  [무기 아이콘]       │
│                    │
│   [판매] 버튼        │ ← Management 모드에서만 표시
│ 가격: XXX골드       │ ← 판매가 표시 (구매가의 50%)
└─────────────────────┘
```

### 3.7. 컴포넌트 세부사항
```csharp
public class InventoryView : MonoBehaviour
{
    [Header("Tab System")]
    public Button weaponTabButton;
    public Button materialTabButton;
    public Transform weaponContentRoot;
    public Transform materialContentRoot;
    
    [Header("Pagination")]
    public UI_Pagination weaponPageNavigation;
    public UI_Pagination materialPageNavigation;
    
    [Header("Mode System")]
    public GameObject managementModeIndicator; // Management 모드 표시
    
    private InventoryMode currentMode = InventoryMode.Normal;
    
    // 모드 변경 메서드
    public void SetMode(InventoryMode mode);
    public void HighlightForSelection(bool enable);
    
    // 아이템 클릭 이벤트
    public UnityEvent<WeaponInstance> OnWeaponClicked;
    public UnityEvent<MaterialInstance> OnMaterialClicked;
    public UnityEvent<WeaponInstance> OnWeaponSellClicked; // 판매 클릭 이벤트
}
```

---

## 4. 공통 팝업 시스템

### 4.1. AlertPopup (알림 팝업)

#### 표시 정보
- **titleText: TMP_Text** (알림 제목)
- **messageText: TMP_Text** (알림 내용)
- **okButton: Button** (확인 버튼)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│              알림                   │
├─────────────────────────────────────┤
│                                    │
│         골드가 부족합니다!           │
│                                    │
├─────────────────────────────────────┤
│              [확인]                 │
└─────────────────────────────────────┘
```

#### 사용 예시
- 골드 부족 알림
- 재료 부족 알림
- 일반적인 안내 메시지
- 에러 메시지
- **월세 납부 경고** (신규)

#### 월세 경고 전용 사용
```
┌─────────────────────────────────────┐
│             월세 경고               │
├─────────────────────────────────────┤
│                                    │
│      내일 월세 1,400골드를           │
│      납부해야 합니다.               │
│                                    │
│      현재 보유 골드: 2,100골드       │
│                                    │
├─────────────────────────────────────┤
│              [확인]                 │
└─────────────────────────────────────┘
```

### 4.2. LoadingPopup (로딩 팝업)

#### 표시 정보
- **loadingIcon: Image** (회전하는 로딩 아이콘)
- **messageText: TMP_Text** (로딩 메시지)
- **backgroundPanel: Image** (반투명 배경)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│         [회전 아이콘]               │
│                                    │
│           저장 중...               │
└─────────────────────────────────────┘
```

#### 사용 예시
- 게임 저장/로드 시
- 페이즈 전환 시
- 데이터 처리 시

---

## 5. 무기 판매 시스템

### 5.1. SellWeaponPanel (무기 판매 확인 팝업)

#### 표시 정보
- **weaponName: string** (무기 이름)
- **description: string** (무기 설명)
- **grade: Grade** (무기 등급)
- **element: Element** (무기 속성)
- **originalPrice: int** (원래 구매가)
- **sellPrice: int** (판매가, 구매가의 50%)
- **sellButton: Button** (판매 확인)
- **cancelButton: Button** (판매 취소)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           무기 판매                  │
├─────────────────────────────────────┤
│ 무기명: 강철검                      │
│ 등급: Uncommon                     │
│ 속성: Fire                         │
├─────────────────────────────────────┤
│ 원래 가격: 1,000골드                │
│ 판매 가격: 500골드                  │
├─────────────────────────────────────┤
│ 설명: 튼튼한 강철로 만든 검         │
├─────────────────────────────────────┤
│        [판매]      [취소]           │
└─────────────────────────────────────┘
```

#### 컴포넌트 세부사항
```csharp
public class SellWeaponPanel : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text weaponNameText;
    public TMP_Text gradeText;
    public TMP_Text elementText;
    public TMP_Text descriptionText;
    public TMP_Text originalPriceText;
    public TMP_Text sellPriceText;
    
    [Header("Buttons")]
    public Button sellButton;
    public Button cancelButton;
    
    private WeaponInstance currentWeapon;
    
    // 무기 판매 확인
    public void SetWeapon(WeaponInstance weapon);
    public void OnSellConfirm();
    public void OnCancel();
}
```

---

## 6. 확인/선택 처리 방식

### 6.1. 각 패널 내 직접 처리
**ConfirmPopup 대신 각 상황별 패널에서 직접 확인/취소 버튼 제공**

#### 예시: BuyWeaponPanel
```
┌─────────────────────────────────────┐
│           무기 구매                  │
├─────────────────────────────────────┤
│ 무기명: 강철검                      │
│ 비용: 1,500골드                    │
├─────────────────────────────────────┤
│        [구매]      [취소]           │ ← 패널 내에서 직접 처리
└─────────────────────────────────────┘
```

#### 예시: SellWeaponPanel
```
┌─────────────────────────────────────┐
│           무기 판매                  │
├─────────────────────────────────────┤
│ 무기명: 강철검                      │
│ 판매가: 500골드                    │
├─────────────────────────────────────┤
│        [판매]      [취소]           │ ← 패널 내에서 직접 처리
└─────────────────────────────────────┘
```

#### 예시: RentWeaponPanel
```
┌─────────────────────────────────────┐
│           무기 대여                  │
├─────────────────────────────────────┤
│ 이 무기를 대여하시겠습니까?          │
├─────────────────────────────────────┤
│        [확인]      [취소]           │ ← 패널 내에서 직접 처리
└─────────────────────────────────────┘
```

### 6.2. 처리 원칙
- **중요한 선택**: 각 패널에 확인/취소 버튼 포함
- **단순 알림**: AlertPopup 사용
- **로딩 상태**: LoadingPopup 사용

---

## 7. 공통 UI 관리 시스템

### 7.1. UIManager의 공통 UI 관리
```csharp
public class UIManager : MonoBehaviour
{
    [Header("Common UI")]
    public StatusBarView statusBar;
    public InventoryView inventory;
    
    [Header("Layout Settings")]
    public Transform phaseContentArea;
    
    [Header("Common Popups")]
    public AlertPopup alertPopupPrefab;
    public LoadingPopup loadingPopupPrefab;
    public SellWeaponPanel sellWeaponPanelPrefab; // 판매 팝업 추가
    
    // 공통 팝업 호출 메서드
    public void ShowAlert(string title, string message, System.Action onOk = null);
    public void ShowLoading(string message);
    public void HideLoading();
    public void ShowRentWarning(int rentAmount, int currentGold); // 월세 경고 전용
    public void ShowSellWeaponPanel(WeaponInstance weapon); // 무기 판매 팝업
    
    // 레이아웃 관리
    public void SetPhasePanel(GameObject panelPrefab);
    public void UpdateStatusBar(PlayerData playerData);
}
```

### 7.2. 인벤토리 모드 전환 흐름
```
페이즈 전환 시:
GameController → 각 페이즈 Controller → UIManager → InventoryView.SetMode()

무기 선택 필요 시:
DayController → UIManager → InventoryView.HighlightForSelection(true)

무기 판매 시:
MorningController → InventoryView.OnWeaponSellClicked → UIManager.ShowSellWeaponPanel()
```

### 7.3. StatusBar 업데이트 흐름
```
데이터 변경 시:
GameController → PlayerData 수정 → StatusBarView 이벤트 → UI 자동 업데이트
```

### 7.4. 월세 경고 흐름
```
아침 페이즈 시작 시:
GameController.StartMorning() → 월세 납부일 체크 → UIManager.ShowRentWarning()
```

---

## 8. 구현 시 고려사항

### 8.1. 레이아웃 최적화
- **StatusBar 높이 최소화**: 인벤토리 영역 최대 활용
- **인벤토리 확장**: 더 많은 아이템을 한 번에 표시
- **반응형 디자인**: 다양한 해상도에서 일관된 비율 유지

### 8.2. 성능 최적화
- **StatusBar 업데이트**: 변경된 값만 업데이트
- **인벤토리 아이템**: 풀링 시스템 활용
- **팝업 관리**: 필요할 때만 생성/활성화

### 8.3. 접근성
- **명확한 구분**: StatusBar를 통한 영역 구분
- **일관된 상호작용**: 각 모드별 명확한 피드백
- **직관적 조작**: 팝업 없이도 명확한 선택 인터페이스

### 8.4. 판매 시스템 고려사항
- **판매가 계산**: 구매가의 50% 정확한 계산
- **재료 판매 방지**: UI에서 재료에는 판매 버튼 미표시
- **Management 모드 표시**: 사용자가 현재 모드를 명확히 인지할 수 있도록

### 8.5. 월세 경고 시스템
- **타이밍 정확성**: 납부일 하루 전 아침에만 표시
- **골드 부족 경고**: 현재 골드 < 필요 월세일 때 특별 경고
- **사용자 친화적**: 명확하고 이해하기 쉬운 메시지