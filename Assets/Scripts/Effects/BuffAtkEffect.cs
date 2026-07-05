using UnityEngine;

namespace CardGame
{
    /// <summary>Tăng (hoặc giảm nếu số âm) ATK cho lá mục tiêu.</summary>
    [CreateAssetMenu(fileName = "BuffAtkEffect", menuName = "Cards/Effects/BuffAtk")]
    public class BuffAtkEffect : CardEffect
    {
        public int amount = 500;

        public override void Execute(BattleContext ctx)
        {
            if (ctx.target == null) return;
            ctx.target.atkModifier += amount;
        }
    }
}
