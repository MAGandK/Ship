using Scripts.UI.UIControllers;

namespace Scripts.UI.WindowsLogic
{
    public interface IWindowController
    {
        void Show();
        void Hide();
        void SetUIController(UIController uiController);
    }
}
