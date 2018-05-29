using FsCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// arbitrary for uppercase strings, if no shrinker is specified shrinking is disabled
/// </summary>
public class MyArbs
{
    public static Arbitrary<string> StringUpperCase()
    {
        return Arb.From(Arb.Default.String().Generator
            .Where(s => s != null && Regex.IsMatch(s, "^[A-Z]+$"))
            , Arb.Default.String().Shrinker);
    }
}