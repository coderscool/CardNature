using UnityEngine;

namespace CardGame
{
    /// <summary>Hồi máu cho người dùng thẻ.</summary>
    [CreateAssetMenu(fileName = "HealEffect", menuName = "Cards/Effects/Heal")]
    public class HealEffect : CardEffect
    {
        public int amount = 500;

        public override void Execute(BattleContext ctx)
        {
            if (ctx.source == null) return;
            ctx.source.Heal(amount);
        }
    }
}
