# 밤(Night) UI 상세 설계 (Night UI Detailed Design)

**목적:** 밤 페이즈의 모든 UI 컴포넌트와 상호작용을 상세히 정의하여 구현 시 참고할 수 있도록 합니다.

---

## 1. NightView (메인 화면)

### 1.1. 화면 구성 (새로운 공통 레이아웃 적용)
```
┌─────────────────────────────────────┐
│           Night Phase UI            │ ← 페이즈 패널 영역 (3/5)
├─────────────────┬───────────────────┤
│  진행중인 모험    │    완료된 모험     │
│                │                   │
│ Adventure       │     Result        │
│ InfoPanel       │     Panel         │
│ Content         │     Content       │
│ (Customer→Hero) │  (Customer→Hero)  │ ← Customer 우선, Hero 구분 표시
├─────────────────┴───────────────────┤
│           [다음 날로]               │
├─────────────────────────────────────┤
│ 골드: 1,500G | 12일차 | 밤 | --:--   │ ← StatusBar (중간)
├─────────────────────────────────────┤
│        InventoryView (하단)         │ ← 인벤토리 영역 (2/5)
│      무기 탭  |  재료 탭            │
└─────────────────────────────────────┘
```

### 1.2. 컴포넌트 정의
- **AdventureInfoPanelContent: Transform** (진행중 모험 리스트의 부모 객체)
- **ResultPanelContent: Transform** (완료된 모험 리스트의 부모 객체)  
- **nextDayButton: Button** (다음 날로 넘어가는 버튼)

---

## 2. 진행중 모험 시스템

### 2.1. AdventureInfoPanel (진행중 모험 아이템)

#### 표시 정보
- **customerName: string** (고객 이름, 클릭 가능)
- **weaponIcon: Sprite** (무기 아이콘, 클릭 가능)
- **remainingDay: int** (남은 일수)
- **dungeonIcon: Sprite** (던전 아이콘, 클릭 가능)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│ [고객명]    [무기아이콘]  [던전아이콘] │
│ CustomerName  WeaponIcon  DungeonIcon│
│           남은 일수: X일             │
└─────────────────────────────────────┘
```

#### 클릭 이벤트
- **customerName 클릭** → AdventureCustomerInfoPanel 호출
- **weaponIcon 클릭** → AdventureWeaponInfoPanel 호출
- **dungeonIcon 클릭** → AdventureDungeonInfoPanel 호출

### 2.2. AdventureCustomerInfoPanel & AdventureHeroInfoPanel

#### Customer/Hero 공통 표시 정보
- **name: string** (고객/Hero 이름)
- **description: string** (고객/Hero 설명)
- **level: int** (고객/Hero 레벨)
- **element: Element** (고객/Hero 속성)
- **grade: Grade** (고객/Hero 등급)
- **icon: Sprite** (고객/Hero 아이콘)
- **isHero: bool** (Hero 여부 - 타이틀 구분용)

#### UI 레이아웃
```
Customer용:
┌─────────────────────────────────────┐
│           고객 정보                  │
├─────────────────────────────────────┤
│ [고객 아이콘]    고객명: XXX         │
│                등급: XXX           │
│                레벨: XXX           │
│                속성: XXX           │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘

Hero용:
┌─────────────────────────────────────┐
│           Hero 정보                 │ ← 타이틀만 변경
├─────────────────────────────────────┤
│ [Hero 아이콘★]   Hero명: XXX        │
│                등급: XXX           │
│                레벨: XXX           │
│                속성: XXX           │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│ 특수능력: 강화된 성공률, 무기 보호   │ ← Hero만 추가 정보
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

### 2.3. AdventureWeaponInfoPanel (무기 상세 정보 팝업)

#### 표시 정보
- **weaponName: string** (무기 이름)
- **description: string** (무기 설명)
- **element: Element** (무기 속성)
- **grade: Grade** (무기 등급)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           무기 정보                  │
├─────────────────────────────────────┤
│ 무기명: XXX                        │
│ 등급: XXX                          │
│ 속성: XXX                          │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

### 2.4. AdventureDungeonInfoPanel (던전 상세 정보 팝업)

#### 표시 정보
- **dungeonName: string** (던전 이름)
- **description: string** (던전 설명)
- **level: int** (던전 레벨)
- **element: Element** (던전 속성)
- **grade: Grade** (던전 등급)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           던전 정보                  │
├─────────────────────────────────────┤
│ 던전명: XXX                        │
│ 등급: XXX                          │
│ 레벨: XXX                          │
│ 속성: XXX                          │
├─────────────────────────────────────┤
│ 설명: XXXXXXXXXXXXXXXXXXXXX        │
├─────────────────────────────────────┤
│              [닫기]                 │
└─────────────────────────────────────┘
```

---

## 3. 완료된 모험 시스템

### 3.1. ResultButton (완료된 모험 아이템 - Customer/Hero 통합)

#### 표시 정보
- **name: string** (고객/Hero 이름)
- **weaponIcon: Sprite** (무기 아이콘)
- **isHero: bool** (Hero 여부 - 별표 표시용)

#### UI 레이아웃 (Customer 우선, Hero 구분 표시)
```
Customer 결과:
┌─────────────────────────────────────┐
│ [고객명]        [무기아이콘]        │
│ CustomerName    WeaponIcon          │
│           [결과 확인]               │
└─────────────────────────────────────┘

Hero 결과:
┌─────────────────────────────────────┐
│ [Hero명★] [↑LV] [무기아이콘]        │ ← 별표로 Hero 구분
│ HeroName(★)     WeaponIcon         │
│           [결과 확인]               │
└─────────────────────────────────────┘

정렬 순서: Customer 결과들 → Hero 결과들
```

#### 클릭 이벤트
- **ResultButton 클릭** → ResultInfoPanel 호출

### 3.2. ResultInfoPanel (결과 상세 정보 팝업)

#### 표시 정보
- **성공여부: bool** (O/X 아이콘으로 표시)
- **무기회수여부: bool** (O/X 아이콘으로 표시)
- **레벨업수치: int** (이전 레벨 -> 올라간 레벨로 표시)
- **customerIcon: Sprite** (고객 아이콘, 클릭 가능)
- **weaponIcon: Sprite** (무기 아이콘, 클릭 가능)
- **dungeonIcon: Sprite** (던전 아이콘, 클릭 가능)

#### UI 레이아웃
```
┌─────────────────────────────────────┐
│           모험 결과                  │
├─────────────────────────────────────┤
│ 성공 여부: [O] 또는 [X]             │
│ 무기 회수: [O] 또는 [X]             │
│ Hero 레벨업: 25 → 26                │
├─────────────────────────────────────┤
│ [고객아이콘] [무기아이콘] [던전아이콘] │
│ CustomerIcon WeaponIcon DungeonIcon │
├─────────────────────────────────────┤
│              [확인]                 │
└─────────────────────────────────────┘
```

#### 클릭 이벤트
- **customerIcon 클릭** → Customer인 경우: ResultCustomerInfoPanel, Hero인 경우: ResultHeroInfoPanel 호출
- **weaponIcon 클릭** → ResultWeaponInfoPanel 호출
- **dungeonIcon 클릭** → ResultDungeonInfoPanel 호출

### 3.3. ResultCustomerInfoPanel & ResultHeroInfoPanel

#### 표시 정보
**Customer/Hero 공통:**
- **name: string**
- **description: string**
- **level: int**
- **element: Element**
- **grade: Grade**
- **icon: Sprite**
- **isHero: bool** (Hero 여부)

**※ AdventureCustomerInfoPanel & AdventureHeroInfoPanel과 동일한 구성**
- Customer용: "고객 정보" 타이틀
- Hero용: "Hero 정보" 타이틀 + 특수능력 정보 추가

### 3.4. ResultWeaponInfoPanel (무기 상세 정보 팝업)

#### 표시 정보
- **weaponName: string**
- **description: string**
- **element: Element**
- **grade: Grade**

**※ AdventureWeaponInfoPanel과 동일한 구성**

### 3.5. ResultDungeonInfoPanel (던전 상세 정보 팝업)

#### 표시 정보
- **dungeonName: string**
- **description: string**
- **level: int**
- **element: Element**
- **grade: Grade**

**※ AdventureDungeonInfoPanel과 동일한 구성**

---

## 4. UI 상호작용 플로우

### 4.1. 진행중 모험 확인 플로우
```
NightView 
    → AdventureInfoPanel 클릭 
        → customerName 클릭 → AdventureCustomerInfoPanel
        → weaponIcon 클릭 → AdventureWeaponInfoPanel  
        → dungeonIcon 클릭 → AdventureDungeonInfoPanel
```

### 4.2. 완료된 모험 확인 플로우
```
NightView 
    → ResultButton 클릭 
        → ResultInfoPanel 
            → customerIcon 클릭 → ResultCustomerInfoPanel
            → weaponIcon 클릭 → ResultWeaponInfoPanel
            → dungeonIcon 클릭 → ResultDungeonInfoPanel
```

### 4.3. 다음 날 진행 플로우
```
NightView → nextDayButton 클릭 → 자동저장 → MorningPhase 시작
```

---

## 5. 구현 시 고려사항

### 5.1. 데이터 정렬 및 표시
- **Customer/Hero 통합 리스트**: 각 패널에서 Customer와 Hero를 함께 표시
- **정렬 규칙**: 
  1. Customer 모험/결과 먼저 표시
  2. Hero 모험/결과 나중에 표시
  3. 각 그룹 내에서는 남은 일수 순 정렬 (진행중) 또는 완료 순서 (결과)
- **Hero 구분 표시**: 아이콘에 별표(★) 추가, 이름에 별표 표시
- 각 패널은 해당하는 데이터 객체(AdventureInstance, AdventureResultData)를 받아서 UI에 바인딩
- 아이콘과 텍스트는 ScriptableObject에서 가져온 데이터로 설정

### 5.2. 데이터 바인딩
- 각 패널은 해당하는 데이터 객체(AdventureInstance, AdventureResultData)를 받아서 UI에 바인딩
- 아이콘과 텍스트는 ScriptableObject에서 가져온 데이터로 설정
- Hero 여부에 따라 적절한 정보 패널 호출 및 별표 표시 처리
- UIManager를 통해 팝업 계층 관리
- 뒤로 가기나 ESC 키로 팝업 닫기 기능

### 5.3. 팝업 관리
- UIManager를 통해 팝업 계층 관리 (AlertPopup, LoadingPopup 사용)
- 뒤로 가기나 ESC 키로 팝업 닫기 기능
- Customer/Hero에 따른 적절한 정보 패널 표시
- 진행중/완료된 모험이 많을 경우 페이지 분할 처리
- GameConfig.uiPageSize에 따라 한 페이지당 표시 개수 결정

### 5.4. 페이지네이션
- 진행중/완료된 모험이 많을 경우 페이지 분할 처리
- GameConfig.uiPageSize에 따라 한 페이지당 표시 개수 결정
- Customer/Hero 통합 리스트에서 정렬 순서 유지하며 페이지 분할

### 5.6. Hero 잠금해제 상태 표시
- **잠금된 Hero**: 진행중/완료 모험에서 제외 (아직 해금되지 않은 Hero)
- **잠금 해제**: 해당 일수 도달 시 자동으로 모험 목록에 표시