# Duel — Hướng dẫn dựng bàn 8x8 trong Editor

Phần code đã xong (`BoardCell.cs`, `BoardGrid.cs`, `DuelSceneController.cs`).
File này hướng dẫn dựng prefab ô và scene `Duel`.

## 1. Prefab ô (BoardCell)

Tùy bạn dựng bàn kiểu world-space (3D/2D) hay UI, chọn 1 trong 2 cách - code đã
hỗ trợ cả hai, không cần sửa gì thêm:

**Cách A - world-space (khuyến nghị cho bàn 3D/2D kiểu bàn cờ):**
1. Tạo 1 `Quad` hoặc `Sprite` làm ô vuông.
2. Thêm **Collider** (BoxCollider hoặc BoxCollider2D) để bắt click (`OnMouseDown`).
3. Gắn script **BoardCell**.
4. Kéo thành **Prefab**.

**Cách B - UI (Canvas):**
1. Tạo 1 `Image` (ô vuông) bên trong 1 `Canvas` có `GraphicRaycaster`.
2. Gắn script **BoardCell** (đã implement `IPointerClickHandler`, tự bắt click UI).
3. Kéo thành **Prefab**.

## 2. Scene Duel

```
BoardRoot                   (gắn: BoardGrid)
Canvas
└── Duel                    (gắn: DuelSceneController)
    └── BackButton          (Button)
```

Nối reference trên **BoardGrid**:
- `rows` = 8, `columns` = 8 (mặc định đã đúng)
- `cellPrefab` → prefab BoardCell vừa tạo
- `cellSize`, `cellSpacing` → chỉnh theo kích thước ô thật của bạn
- `cellsParent` → để trống là được (mặc định dùng chính `BoardRoot`)
- `generateOnStart` = true → tự sinh bàn khi vào scene, không cần gọi tay

Nối reference trên **DuelSceneController**:
- `board` → BoardRoot (BoardGrid)
- `backButton` → BackButton
- `sceneNavigator` → thêm script **SceneNavigator** lên 1 object bất kỳ trong
  scene rồi kéo vào đây (dùng lại được cho cả DeckBuilder, xem README_Home.md)

## Luồng hoạt động (đã code sẵn)

- Vào scene `Duel` → `BoardGrid.Start()` tự sinh bàn 8x8, mỗi ô là 1
  `BoardCell` biết tọa độ `(row, column)` và lá bài đang chiếm ô (`occupant`).
- Click 1 ô → `BoardCell.Clicked` → `BoardGrid.CellClicked` →
  `DuelSceneController.HandleCellClicked` (hiện chỉ log ra Console).
- Bấm **Back** → `SceneNavigator.GoHome()` → quay lại scene `Home`.

## 3. Ô Field (Field Zone) — hiện sprite khi thả thẻ Field vào

Field card giờ có thêm field `fieldSprite` (khác `artwork` - hình lá bài).
Điền sprite này khi tạo asset FieldCard (**Create → Cards → Field**).

**Prefab thẻ trên tay (kéo được):**
1. Dùng lại prefab `CardItemView` đã có (từ Deck Builder) hoặc tạo bản riêng cho tay bài.
2. Trong Inspector của `CardItemView`, bật `draggable = true`.
   (Prefab dùng cho Deck Builder thì để `draggable = false` như cũ, không ảnh hưởng gì.)

**Ô Field trên bàn:**
1. Tạo 1 `Image` trong Canvas, đặt ở vị trí Field Zone trên bàn.
2. Gắn script **FieldSlotView**.
3. Nối `slotImage` → chính Image đó.
4. (Tùy chọn) Tạo thêm 1 `Image` viền mỏng đè lên, nối vào `highlightBorder`
   để đổi màu xanh/đỏ khi kéo thẻ ngang qua (xanh = hợp lệ, đỏ = sai loại thẻ).

**Luồng hoạt động (đã code sẵn):**
- Kéo 1 `CardItemView` đang chứa `FieldCard` thả vào ô Field →
  `FieldSlotView.OnDrop` nhận, hiện `fieldSprite` lên `slotImage`, bắn event
  `FieldPlaced`, rồi xóa thẻ đó khỏi tay (đã "chơi" lá bài).
- Thả nhầm thẻ không phải Field (Monster/Spell/Trap) → bị từ chối, thẻ tự bay
  về vị trí cũ trên tay (`CardItemView.OnEndDrag`).
- Muốn cập nhật `Player.activeEnvironment` khi field được đặt: subscribe
  `fieldSlotView.FieldPlaced += card => player.activeEnvironment = card;`
  (vd trong `DuelSceneController`).

## Tùy chỉnh / bước tiếp theo

- **Đặt bài lên ô**: gọi `cell.TryPlace(cardInstance)` / `cell.Clear()` từ logic
  game của bạn (vd trong `HandleCellClicked`, sau khi người chơi chọn 1 lá từ tay).
- **Đổi kích thước bàn**: sửa `rows`/`columns` trên `BoardGrid`, không cần sửa code.
- **Lấy ô theo tọa độ**: `board.GetCell(row, column)`.
- **Sinh lại bàn lúc runtime** (vd sang màn mới): gọi `board.Generate()`
  (tự xóa bàn cũ trước khi sinh bàn mới).
