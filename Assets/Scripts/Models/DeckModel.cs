using System;
using System.Collections.Generic;

namespace CardGame
{
    /// <summary>
    /// Bộ bài đang được chỉnh trong màn deck-builder (runtime, KHÔNG phải ScriptableObject).
    /// Giữ tham chiếu tới CardData gốc và bắn event Changed mỗi khi có thay đổi
    /// để UI tự cập nhật.
    /// </summary>
    public class DeckModel
    {
        // Giới hạn tùy chỉnh - đổi theo luật game của bạn.
        // Side deck (Monster/Trap/Spell) tối đa 50; Nature deck (Field) tối đa 40.
        public int maxMainCards = 50;
        public int maxFieldCards = 40;

        public string deckName = "New Deck";

        private readonly List<CardData> _mainCards = new List<CardData>();
        private readonly List<CardData> _fieldCards = new List<CardData>();

        public IReadOnlyList<CardData> MainCards => _mainCards;
        public IReadOnlyList<CardData> FieldCards => _fieldCards;

        /// <summary>Bắn ra mỗi khi deck thay đổi (thêm/xóa/load).</summary>
        public event Action Changed;

        /// <summary>
        /// Thêm thẻ vào đúng ngăn theo loại. Trả false nếu đã đầy.
        /// </summary>
        public bool AddCard(CardData card)
        {
            if (card == null) return false;

            if (card.type == CardType.Field)
            {
                if (_fieldCards.Count >= maxFieldCards) return false;
                _fieldCards.Add(card);
            }
            else
            {
                if (_mainCards.Count >= maxMainCards) return false;
                _mainCards.Add(card);
            }

            Changed?.Invoke();
            return true;
        }

        /// <summary>Xóa một lá (bản đầu tiên tìm thấy).</summary>
        public bool RemoveCard(CardData card)
        {
            if (card == null) return false;

            bool removed = card.type == CardType.Field
                ? _fieldCards.Remove(card)
                : _mainCards.Remove(card);

            if (removed) Changed?.Invoke();
            return removed;
        }

        public void Clear()
        {
            _mainCards.Clear();
            _fieldCards.Clear();
            Changed?.Invoke();
        }

        /// <summary>Xuất sang DTO để serialize.</summary>
        public DeckData ToData()
        {
            var data = new DeckData { deckName = deckName };
            foreach (var c in _mainCards) data.mainCardIds.Add(c.id);
            foreach (var c in _fieldCards) data.fieldCardIds.Add(c.id);
            return data;
        }

        /// <summary>
        /// Nạp deck từ DTO, tra thẻ qua database. Tự phân loại theo LOẠI THẺ
        /// (Field -> nature deck, còn lại -> side deck) và tôn trọng giới hạn.
        /// </summary>
        public void LoadFrom(DeckData data, CardDatabase database)
        {
            _mainCards.Clear();
            _fieldCards.Clear();
            deckName = data.deckName;

            // Gộp tất cả id rồi phân loại lại theo type để chắc chắn đúng ngăn.
            AddImported(data.fieldCardIds, database);
            AddImported(data.mainCardIds, database);

            Changed?.Invoke();
        }

        private void AddImported(List<string> ids, CardDatabase database)
        {
            if (ids == null) return;
            foreach (var id in ids)
            {
                var card = database.GetById(id);
                if (card == null) continue;

                if (card.type == CardType.Field)
                {
                    if (_fieldCards.Count < maxFieldCards) _fieldCards.Add(card);
                }
                else
                {
                    if (_mainCards.Count < maxMainCards) _mainCards.Add(card);
                }
            }
        }
    }
}
