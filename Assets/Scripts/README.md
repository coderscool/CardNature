# Card System (Unity)

Bộ script data cho game thẻ bài kiểu Yu-Gi-Oh. Tất cả nằm trong namespace `CardGame`.

## Cấu trúc

```
Scripts/
├── Cards/        Data gốc của thẻ (ScriptableObject)
│   ├── CardData.cs      base + enum CardType
│   ├── MonsterCard.cs   atk/def/level/attribute
│   ├── SpellCard.cs
│   ├── TrapCard.cs
│   └── FieldCard.cs     thẻ môi trường (id + environment + effect)
├── Effects/      Hệ thống hiệu ứng (Command pattern)
│   ├── CardEffect.cs     base abstract, hàm Execute()
│   ├── DamageEffect.cs
│   ├── HealEffect.cs
│   ├── BuffAtkEffect.cs
│   └── BattleContext.cs  ngữ cảnh khi hiệu ứng chạy
├── Runtime/      Trạng thái trong trận (class thường, KHÔNG phải SO)
│   ├── CardInstance.cs   1 lá bài đang chơi (buff, úp/mở...)
│   ├── Player.cs         máu, tay bài, sân
│   └── Deck.cs           bộ bài
├── Board/        Bàn chơi dạng lưới (MonoBehaviour, dùng ở scene Duel)
│   ├── BoardCell.cs      1 ô: tọa độ + lá bài đang chiếm ô + sự kiện click
│   └── BoardGrid.cs      sinh bàn NxN (mặc định 8x8) lúc vào scene
├── Services/     Hạ tầng dùng chung
│   ├── SceneNames.cs     hằng số tên scene (Home/DeckBuilder/Duel)
│   ├── SceneNavigator.cs điều hướng scene, nối vào UnityEvent trên UI
│   └── DeckSerializer.cs export/import deck ra JSON
└── UI/           Màn hình (namespace CardGame.UI)
    ├── Home/         HomeController.cs - 2 nút: Deck Builder, Play
    ├── DeckBuilder/  xem README_DeckBuilder.md
    └── Duel/         DuelSceneController.cs - màn chơi, bàn 8x8
```

## Cách dùng

1. Chuột phải trong Project → **Create → Cards → Monster / Spell / Trap / Field**
   để tạo từng lá bài thành asset. Điền số liệu ngay trong Inspector.
2. Tạo hiệu ứng: **Create → Cards → Effects → Damage / Heal / BuffAtk**,
   rồi kéo asset hiệu ứng đó vào field `effect` của thẻ.
3. Tạo bộ bài: **Create → Cards → Deck**, kéo các thẻ vào list `cards`.
4. Vào trận: `deck.BuildRuntime()` trả về `List<CardInstance>` để chơi.

## Nguyên tắc quan trọng

- **Data gốc (ScriptableObject) không bao giờ sửa lúc chơi.** Mọi thay đổi
  (buff, trạng thái) nằm trong `CardInstance`.
- **Thêm hiệu ứng mới** = tạo class con của `CardEffect`, không đụng code thẻ.
- `Player` và `BattleContext` ở đây là ví dụ tối giản — mở rộng theo game của bạn.
