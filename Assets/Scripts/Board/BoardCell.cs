using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardGame.Board
{
    /// <summary>
    /// Một ô trên bàn chơi. Lưu tọa độ lưới (hàng/cột) và lá bài đang chiếm ô
    /// (nếu có). Gắn lên prefab ô, do <see cref="BoardGrid"/> instantiate.
    ///
    /// Bắt click theo 2 kiểu, dùng kiểu nào tùy prefab của bạn (không xung đột):
    ///  - Bàn 3D/2D world-space: prefab cần có Collider  -> OnMouseDown.
    ///  - Bàn dựng bằng UI (Canvas): prefab cần có Graphic + Canvas GraphicRaycaster
    ///    -> IPointerClickHandler.
    /// </summary>
    public class BoardCell : MonoBehaviour, IPointerClickHandler
    {
        public int row;
        public int column;

        /// <summary>Bắn khi ô được click, theo cả 2 kiểu ở trên.</summary>
        public event Action<BoardCell> Clicked;

        /// <summary>Lá bài đang đứng trên ô này, null nếu ô trống.</summary>
        public CardInstance occupant;

        public bool IsEmpty => occupant == null;

        /// <summary>BoardGrid gọi hàm này ngay sau khi Instantiate.</summary>
        public void Init(int r, int c)
        {
            row = r;
            column = c;
            occupant = null;
            name = $"Cell_{r}_{c}";
        }

        public bool TryPlace(CardInstance card)
        {
            if (!IsEmpty) return false;
            occupant = card;
            return true;
        }

        public CardInstance Clear()
        {
            var card = occupant;
            occupant = null;
            return card;
        }

        public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(this);

        private void OnMouseDown() => Clicked?.Invoke(this);
    }
}
