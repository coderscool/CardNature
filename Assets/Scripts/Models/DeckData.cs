using System;
using System.Collections.Generic;

namespace CardGame
{
    /// <summary>
    /// Dạng serialize được của một bộ bài (chỉ lưu id thẻ).
    /// Dùng cho export/import JSON. JsonUtility của Unity đọc được class này.
    /// </summary>
    [Serializable]
    public class DeckData
    {
        public string deckName = "New Deck";

        /// <summary>Id các thẻ Monster/Spell/Trap.</summary>
        public List<string> mainCardIds = new List<string>();

        /// <summary>Id các thẻ Field (nature deck).</summary>
        public List<string> fieldCardIds = new List<string>();
    }
}
