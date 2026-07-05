using UnityEngine;

namespace CardGame
{
    /// <summary>Gây damage thẳng vào máu đối thủ.</summary>
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "Cards/Effects/Damage")]
    public class DamageEffect : CardEffect
    {
        public int amount = 500;

        public override void Execute(BattleContext ctx)
        {
            if (ctx.opponent == null) return;
            ctx.opponent.TakeDamage(amount);
        }
    }
}
