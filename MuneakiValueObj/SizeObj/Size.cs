using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuneakiValueObject.SizeObj
{
    /// <summary> サイズ </summary>
    public record Size(Length Length, Width Width, Height Height);
}
