using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Trạng thái người chơi trong trận: máu, bài trên tay, bài trên sân.
    /// Đây là ví dụ tối giản để hiệu ứng có chỗ tác động - bạn mở rộng theo game.
    /// </summary>
    public class Player
    {
        public string playerName;
        public int maxHp = 8000;
        public int hp = 8000;

        public readonly List<CardInstance> hand = new List<CardInstance>();
        public readonly List<CardInstance> field = new List<CardInstance>();

        public FieldCard activeEnvironment; // thẻ môi trường đang mở

        public void TakeDamage(int amount)
        {
            hp = Mathf.Max(0, hp - amount);
        }

        public void Heal(int amount)
        {
            hp = Mathf.Min(maxHp, hp + amount);
        }

        public bool IsDefeated => hp <= 0;
    }
}
