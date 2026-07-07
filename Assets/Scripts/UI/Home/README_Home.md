# Home — Hướng dẫn dựng trong Editor

Phần code đã xong (`HomeController.cs`, `SceneNames.cs`, `SceneNavigator.cs`).
File này hướng dẫn dựng scene, Canvas và nối 3 scene lại với nhau kiểu Yu-Gi-Oh
(Home → Deck Builder / Duel, có nút Back quay lại Home).

## 1. Tạo 3 scene và thêm vào Build Settings

1. Tạo 3 scene, đặt tên **đúng chính xác** (khớp `SceneNames.cs`):
   - `Home`
   - `DeckBuilder`
   - `Duel`
2. **File → Build Profiles (hoặc Build Settings) → Scene List**, kéo cả 3 vào,
   theo thứ tự: `Home` (index 0) → `DeckBuilder` → `Duel`.
   - Nếu đổi tên scene khác, sửa lại các hằng số trong `Scripts/Services/SceneNames.cs`.

## 2. Scene Home

```
Canvas                      (Screen Space - Overlay)
└── Home                    (gắn: HomeController)
    ├── Title               (TMP Text, tên game)
    ├── DeckBuilderButton    (Button)
    └── PlayButton           (Button)
```

Nối reference trên `HomeController`:
- `deckBuilderButton` → DeckBuilderButton
- `playButton` → PlayButton
- `deckBuilderSceneName` = "DeckBuilder" (mặc định đã đúng, chỉ sửa nếu đổi tên scene)
- `duelSceneName` = "Duel" (tương tự)

Không cần nối OnClick thủ công trong Inspector — `HomeController.Awake()` tự
`AddListener` cho 2 nút.

## 3. Scene DeckBuilder — nối nút Back về Home

Scene này đã có sẵn cấu trúc trong `README_DeckBuilder.md`. Việc còn lại:
1. Thêm 1 object (vd đặt cạnh `DeckBuilder`), gắn script **SceneNavigator**.
2. Trên `DeckBuilderController`, kéo object đó vào field `onBack` (UnityEvent),
   chọn hàm `SceneNavigator.GoHome`.

## 4. Scene Duel — bàn 8x8

Xem `Scripts/UI/Duel/README_Duel.md` để dựng `BoardGrid` (bàn 8x8) và nút quay về Home.

## Luồng hoạt động (đã code sẵn)

- Home → bấm **Deck Builder** → load scene `DeckBuilder`.
- Home → bấm **Play** → load scene `Duel`, `BoardGrid` tự sinh bàn 8x8 lúc `Start()`.
- DeckBuilder/Duel → bấm **Back** → `SceneNavigator.GoHome()` → quay lại `Home`.

## Tùy chỉnh

- Muốn Deck Builder/Duel hiện dạng overlay (không đổi scene) thay vì load scene
  riêng: bỏ `SceneManager.LoadScene` trong `HomeController`, thay bằng
  `SetActive(true/false)` các Canvas con nằm chung 1 scene. Cách hiện tại (scene
  riêng) là cách các game bài kiểu Yu-Gi-Oh hay dùng, dễ quản lý bộ nhớ và tách
  biệt UI hơn.
