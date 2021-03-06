using System;
using Tizen.NUI;

namespace Tizen.Appium
{
    public class NUIAdapter : BaseAdapter
    {
        static bool _isInitialized = false;

        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        public static void Initialize()
        {
            if (!_isInitialized)
            {
                ScreenWidth = Utils.GetScreeenWidth();
                ScreenHeight = Utils.GetScreenHeight();

                TizenAppium.Start(new NUIAdapter());
                _isInitialized = true;
            }
        }

        public static void Terminate()
        {
            if (_isInitialized)
            {
                TizenAppium.Stop();
                _isInitialized = false;
            }
        }

        protected override IObjectList CreateObjectList()
        {
            return new NUIViewList();
        }

        protected override void AdaptApp()
        {
            Window.Instance.ViewAdded += (s, e) =>
            {
                ObjectList.Add(s);
            };
        }
    }
}
