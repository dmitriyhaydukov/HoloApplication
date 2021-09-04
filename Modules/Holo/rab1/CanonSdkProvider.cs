using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDSDKLib;

namespace rab1
{
    public static class CanonSdkProvider
    {
        private static SDKHandler sdkHandler = new SDKHandler();

        public static SDKHandler GetSDKHandler()
        {
            return sdkHandler;
        }
    }
}
