namespace CardGame
{
    /// <summary>
    /// Trạng thái RUNTIME của một lá bài trong trận. KHÔNG phải ScriptableObject.
    /// Tham chiếu tới CardData gốc, nhưng giữ mọi giá trị thay đổi ở đây
    /// (buff, úp/mở, đã tấn công chưa...) để không ghi đè lên data gốc.
    /// </summary>
    public class CardInstance
    {
        public readonly CardData data;

        // Trạng thái thay đổi trong trận
        public bool isFaceDown;
        public bool hasAttacked;
        public int atkModifier; // cộng dồn buff/debuff

        public CardInstance(CardData data)
        {
            this.data = data;
        }

        /// <summary>ATK hiện tại = gốc + modifier (chỉ có với quái).</summary>
        public int CurrentAtk
        {
            get
            {
                if (data is MonsterCard m)
                    return m.atk + atkModifier;
                return 0;
            }
        }

        public int CurrentDef
        {
            get
            {
                if (data is MonsterCard m)
                    return m.def;
                return 0;
            }
        }

        /// <summary>Kích hoạt hiệu ứng của lá này nếu có.</summary>
        public void ActivateEffect(BattleContext ctx)
        {
            ctx.sourceCard = this;
            CardEffect effect = GetEffect();
            if (effect != null)
                effect.Execute(ctx);
        }

        private CardEffect GetEffect()
        {
            switch (data)
            {
                case MonsterCard m: return m.effect;
                case SpellCard s:   return s.effect;
                case TrapCard t:    return t.effect;
                case FieldCard f:   return f.effect;
                default:            return null;
            }
        }
    }
}
