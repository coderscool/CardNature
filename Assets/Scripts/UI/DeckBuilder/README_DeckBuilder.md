# Deck Builder — Hướng dẫn dựng trong Editor

Phần code đã xong. File này hướng dẫn bạn tự dựng scene, prefab và nối reference.
Toàn bộ script UI nằm trong namespace `CardGame.UI`, data trong `CardGame`.

## Yêu cầu
- **TextMeshPro**: Window → TextMeshPro → Import TMP Essential Resources.
- Đã có sẵn các thẻ (MonsterCard, SpellCard, TrapCard, FieldCard) dạng asset.
- Với FieldCard: nhớ điền cả `artwork` (hình lá bài) lẫn `fieldSprite`
  (hình hiện lên Ô Field trên bàn khi lá đó được chơi — xem
  `Scripts/UI/Duel/README_Duel.md` mục 3).

---

## 1. Tạo CardDatabase
1. Chuột phải trong Project → **Create → Cards → Card Database**.
2. Kéo TẤT CẢ thẻ vào list `cards`.
3. Nhớ: mỗi thẻ phải có `id` khác nhau (export/import dựa vào id).

## 2. Layout tổng thể (Canvas)
Tạo `Canvas` (Screen Space - Overlay) rồi dựng cây object như sau:

```
Canvas
└── DeckBuilder                (gắn: DeckBuilderController)
    ├── LeftPanel              (gắn: CardPreviewPanel)
    │   ├── Artwork            (Image)
    │   ├── NameLabel          (TMP Text)
    │   ├── TypeLabel          (TMP Text)
    │   ├── DescriptionLabel   (TMP Text)
    │   ├── StatsLabel         (TMP Text)
    │   ├── ExportButton       (Button)
    │   ├── ImportButton       (Button)
    │   └── BackButton         (Button)
    └── RightPanel
        ├── CollectionScroll   (Scroll View)
        │   └── Viewport/Content   (gắn: CardCollectionView vào Content)
        ├── SideDeck           (gắn: DeckSectionView)  -> Monster/Trap/Spell
        │   ├── TitleLabel     (TMP Text)
        │   ├── CountLabel     (TMP Text)
        │   └── Content        (container các thẻ)
        └── NatureDeck         (gắn: DeckSectionView)  -> Field
            ├── TitleLabel
            ├── CountLabel
            └── Content
```

Gợi ý dùng **Horizontal Layout Group** cho `DeckBuilder` (Left | Right),
và **Vertical Layout Group** cho `RightPanel` (Collection trên, 2 deck dưới).
Trong các `Content`, thêm **Grid Layout Group** để thẻ tự xếp lưới,
và **Content Size Fitter** cho content của Scroll View.

## 3. Prefab thẻ (CardItemView)
1. Tạo một UI object (ví dụ 1 `Button` có `Image` + `TMP Text` con + 1 nút "X" nhỏ để xóa).
2. Gắn script **CardItemView** lên root prefab.
3. Nối reference trong Inspector:
   - `artwork` → Image ảnh thẻ
   - `nameLabel` → TMP Text tên
   - `countLabel` → TMP Text nhỏ ở góc (hiện "x5" khi có thẻ trùng; tự ẩn khi chỉ 1 lá)
   - `selectButton` → Button chính (bấm để chọn)
   - `removeButton` → nút "X" (để trống cũng được; sẽ tự ẩn ở collection)
4. Kéo thành **Prefab**.

## 4. Nối reference các View
**CardCollectionView** (trên object Content của scroll):
- `content` → chính Transform Content đó
- `itemPrefab` → prefab CardItemView

**DeckSectionView** (SideDeck và NatureDeck, mỗi cái set riêng):
- `content` → Content của ngăn
- `itemPrefab` → prefab CardItemView
- `titleLabel`, `countLabel` → TMP Text tương ứng
- `title` → "Side Deck" hoặc "Nature Deck"

**CardPreviewPanel** (LeftPanel):
- Nối các label, artwork, và 3 button Export/Import/Back.

## 5. Nối DeckBuilderController
Trên object `DeckBuilder`, kéo vào các ô:
- `database` → CardDatabase asset
- `collectionView` → CardCollectionView
- `sideDeckSection` → SideDeck (DeckSectionView)
- `natureDeckSection` → NatureDeck (DeckSectionView)
- `previewPanel` → LeftPanel (CardPreviewPanel)
- `onBack` (UnityEvent) → xem mục 6 bên dưới

## 6. Nối nút Back về Home

Scene này được vào từ Home (nút "Deck Builder", xem `Scripts/UI/Home/README_Home.md`),
nên "Back" cần quay lại scene `Home`:
1. Đảm bảo `DeckBuilder` đã có trong Build Settings (xem README_Home.md mục 1).
2. Thêm 1 object trong scene, gắn script **SceneNavigator**
   (`Scripts/Services/SceneNavigator.cs`).
3. Trên `DeckBuilderController`, kéo object đó vào field `onBack` (UnityEvent),
   chọn hàm `SceneNavigator.GoHome`.

Nút **Back** trên `CardPreviewPanel` (đã nối `BackClicked` → `HandleBack` →
`onBack.Invoke()` sẵn trong code) giờ sẽ tự quay về Home, không cần sửa gì thêm.

---

## Luồng hoạt động (đã code sẵn)
- Bấm thẻ trong **Collection** → hiện ở preview trái + tự thêm vào deck đúng loại
  (Field → Nature Deck, còn lại → Side Deck).
- Bấm thẻ trong ngăn deck → hiện ở preview.
- Bấm nút X trên thẻ trong deck → gỡ khỏi deck.
- **Export** → lưu JSON vào `Application.persistentDataPath` (xem đường dẫn ở Console).
- **Import** → đọc lại file JSON cùng tên deck.
- **Back** → gọi UnityEvent `onBack`.

## Tùy chỉnh
- Giới hạn số thẻ: sửa `maxMainCards`, `maxFieldCards` trong `DeckModel.cs`.
- Đổi chỗ lưu file / hộp thoại chọn file: sửa `DeckSerializer.cs` (mặc định
  persistentDataPath; muốn có dialog chọn file cần thêm plugin, vd StandaloneFileBrowser).
- Đổi tên deck trước khi export: hiện `deckName` mặc định "New Deck" — bạn có thể
  thêm 1 InputField và set `_deck.deckName` (mở thêm API nếu cần).
