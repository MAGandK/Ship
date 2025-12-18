using Scripts.UI.UIControllers;
using Zenject;

namespace Scripts.UI
{
    public class UiInitializer : IInitializable
    {
        private IUIController _uiController;

        public UiInitializer(IUIController uiController)
        {
            _uiController = uiController;
        }

        public void Initialize()
        {
        }
    }
}