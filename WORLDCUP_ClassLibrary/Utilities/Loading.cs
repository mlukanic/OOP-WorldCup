using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Interfaces;

namespace WORLDCUP_ClassLibrary.Utilities
{
    public static class Loading
    {
        public static ILoading? LoadingScreen { get; private set; }

        public static void SetLoading(ILoading loadingScreen)
        {
            LoadingScreen = loadingScreen;
        }
    }
}
