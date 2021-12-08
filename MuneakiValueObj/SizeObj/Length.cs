using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuneakiTools;
using MuneakiTools.CommonExtentions;

namespace MuneakiValueObject.SizeObj
{
    /// <summary> 長さ </summary>
    /// <remarks> イミュターブルな値オブジェクトです </remarks>
    public record Length : IntValueObject<Length>
    {
        /// <summary> Initializes a new instance of the <see cref="Length"/> class. </summary>
        /// <param name="value">長さ</param>
        public Length(int value) : base(value)
        {
            if (value.IsNegative()) throw new ArgumentException($"nameof(value) は負の数を設定できません");
        }
    }
}
