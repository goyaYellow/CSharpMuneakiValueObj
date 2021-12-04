using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Support;

namespace Tools.ValueObject.SizeObj
{
    /// <summary> 幅 </summary>
    public record Width : IntValueObject<Width>
    {
        /// <summary> Initializes a new instance of the <see cref="Width"/> class. </summary>
        /// <param name="value">長さ</param>
        public Width(int value) : base(value)
        {
            if (value.IsNegative()) throw new ArgumentException($"nameof(value) は負の数を設定できません");
        }
    }
}
