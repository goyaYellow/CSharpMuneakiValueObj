using System;
using MuneakiTools.CommonExtentions;

namespace Tools.ValueObject.SizeObj
{
    /// <summary> 高さ </summary>
    /// <remarks> イミュターブルな値オブジェクトです </remarks>
    public record Height : IntValueObject<Height>
    {
        /// <summary> Initializes a new instance of the <see cref="Height"/> class. </summary>
        /// <param name="value">高さ</param>
        public Height(int value) : base(value)
        {
            if (value.IsNegative()) throw new ArgumentException($"nameof(value) は負の数を設定できません");
        }
    }
}
