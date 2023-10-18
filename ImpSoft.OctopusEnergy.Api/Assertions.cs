using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ImpSoft.OctopusEnergy.Api.Properties;

namespace ImpSoft.OctopusEnergy.Api;

internal static partial class Assertions
{
#if NET7_0_OR_GREATER
    [GeneratedRegex(@"^_[A-N]$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex GspRegEx();
#endif

#if NET7_0
    public static void AssertValidGsp(string gsp)
    {
        if (!GspRegEx().IsMatch(gsp))
        {
            throw new GspException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidGsp, gsp));
        }
    }
#elif NET8_0_OR_GREATER
    private static readonly CompositeFormat _cfForGsp = CompositeFormat.Parse(Resources.InvalidGsp);

    public static void AssertValidGsp(string gsp)
    {
        if (!GspRegEx().IsMatch(gsp))
        {
            throw new GspException(string.Format(CultureInfo.CurrentCulture, _cfForGsp, gsp));
        }
    }
#else
    public static void AssertValidGsp(string gsp)
    {
        if (!Regex.IsMatch(gsp, @"^_[A-N]$", RegexOptions.Compiled | RegexOptions.Singleline))
        {
            throw new GspException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidGsp, gsp));
        }
    }
#endif
}