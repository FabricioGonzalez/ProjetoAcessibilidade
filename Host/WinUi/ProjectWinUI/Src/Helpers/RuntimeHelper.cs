using System.Runtime.InteropServices;
using System.Text;

namespace ProjectWinUI.Src.Helpers;

public class RuntimeHelper
{
    public static bool IsMSIX
    {
        get
        {
            var length = 0;

            return GetCurrentPackageFullName(packageFullNameLength: ref length, packageFullName: null) != 15700L;
        }
    }

    [DllImport(dllName: "kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetCurrentPackageFullName(
        ref int packageFullNameLength
        , StringBuilder? packageFullName
    );
}