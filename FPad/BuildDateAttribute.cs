using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

[AttributeUsage(AttributeTargets.Assembly)]
public class BuildDateAttribute : Attribute
{
    public DateTime LocalValue { get; set; }

    public BuildDateAttribute(long utcTicks)
    {
        LocalValue = (new DateTime(utcTicks, DateTimeKind.Utc)).ToLocalTime();
    }
}
