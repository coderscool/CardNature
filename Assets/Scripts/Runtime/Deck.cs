using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Một bộ bài: chỉ là danh sách CardData. Tạo asset trong Editor,
    /// kéo thả các thẻ vào. Khi vào trận, chuyển thành List<CardInstance>.
    /// </summary>
    [CreateAssetMenu(fileName = "Deck", menuName = "Cards/Deck")]
    public class Deck : ScriptableObject
    {
        public string deckName;
        public List<CardData> cards = new List<CardData>();

        /// <summary>Tạo danh sách instance runtime từ data gốc (đã xáo).</summary>
        public List<CardInstance> BuildRuntime(bool shuffle = true)
        {
            var list = new List<CardInstance>(cards.Count);
            foreach (var c in cards)
                list.Add(new CardInstance(c));

            if (shuffle)
                Shuffle(list);
            return list;
        }

        private static void Shuffle(List<CardInstance> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
