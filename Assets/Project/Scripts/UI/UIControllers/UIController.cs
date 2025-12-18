using System.Collections.Generic;
using System.Linq;
using Scripts.UI.WindowsLogic;
using UnityEngine;
using Zenject;

namespace Scripts.UI.UIControllers
{
    public class UIController : IUIController, IInitializable
    {
        private readonly IEnumerable<IWindowController> _controllers;
        private readonly List<IWindowController> _openedWindows = new();

        private int _orderIndex;

        public UIController(IEnumerable<IWindowController> controllers)
        {
            _controllers = controllers;
        }

        public void Initialize()
        {
            foreach (var windowController in _controllers)
            {
                Debug.Log($"[UI] Найден контроллер окна: {windowController.GetType().Name}");
                windowController.SetUIController(this);
                windowController.Hide();
            }
        }

        public void ShowWindow<T>() where T : IWindowController
        {
            var window = _controllers.FirstOrDefault(x => x is T);
            if (window == null)
            {
                Debug.LogError($"Окно {typeof(T).Name} не найдено!");
                return;
            }

            if (window is not IPopupController popController)
            {
                foreach (var openedWindow in _openedWindows)
                    openedWindow.Hide();
                _openedWindows.Clear();
                _orderIndex = 0;
            }
            else
            {
                if (!_openedWindows.Contains(window))
                    _openedWindows.Add(window);

                popController.SetOrderInLayer(++_orderIndex);
            }

            window.Show();
        }

        public T GetWindow<T>() where T : class, IWindowController
        {
            return _controllers.FirstOrDefault(x => x is T) as T;
        }

        public void CloseLastOpenPopup()
        {
            if (_openedWindows.Count == 0)
                if (_openedWindows.Count == 0)
                {
                    Debug.Log("Список открытых окон пуст");
                    return;
                }

            var windowController = _openedWindows[^1];
            if (windowController is not IPopupController)
            {
              
                return;
            }

            windowController.Hide();
            _openedWindows.RemoveAt(_openedWindows.Count - 1);
            _orderIndex--;
        }
    }
}