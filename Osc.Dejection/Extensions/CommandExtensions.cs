using Osc.Dejection.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Extensions
{
    public static class CommandExtensions
    {
        public static string ToReadableFormat(this CommandData commandData)
        {
            return $"ClassName: {Path.GetFileName(commandData.ClassName)} | MethodName: {commandData.MethodName} | LineNumber: {commandData.LineNumber}";
        }
    }
}
