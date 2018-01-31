using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Tizen.Appium.Dbus;

namespace Tizen.Appium
{
    public class ResetEvent : IDbusMethod
    {
        // This method is provided for debugging.
        public string Name => MethodNames.ResetEvent;

        public string Signature => "";

        public string ReturnSignature => "b";

        public string[] Args => new string[] { Params.ElementId };

        public Arguments Run(Arguments args)
        {
            Log.Debug(TizenAppium.Tag, "#### ResetService");

            var ret = new Arguments();

            EventObject.RemoveAll();

            ret.SetArgument(Params.Return, true);

            return ret;
        }
    }
}