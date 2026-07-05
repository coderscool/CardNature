using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Base cho mọi hiệu ứng. Mỗi hiệu ứng là một ScriptableObject riêng,
    /// bạn tạo asset và gắn vào thẻ. Thêm hiệu ứng mới = tạo class con,
    /// không đụng code thẻ (Command pattern).
    /// </summary>
    public abstract class CardEffect : ScriptableObject
    {
        [TextArea] public string effectText;

        /// <summary>Thực thi hiệu ứng trong ngữ cảnh trận đấu.</summary>
        public abstract void Execute(BattleContext ctx);
    }
}
