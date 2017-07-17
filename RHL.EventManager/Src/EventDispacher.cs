using System;
using RHL.EventManager.MonoBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RHL.EventManager {

    public static class EventDispacher {

        private static DispatchController dispatchController;

        private static DispatchController DispatchController {
            get {
                if (dispatchController != null) {
                    return dispatchController;
                }
                GameObject gameObject = new GameObject(nameof(DispatchController));
                Object.DontDestroyOnLoad(gameObject);
                dispatchController = gameObject.AddComponent<DispatchController>();
                return dispatchController;
            }
        }

        public static uint AddListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            if (eventHandler == null) {
                throw new ArgumentNullException(nameof(eventHandler));
            }
            return DispatchController.AddListener(eventHandler);
        }

        public static bool RemoveListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            if (eventHandler == null) {
                throw new ArgumentNullException(nameof(eventHandler));
            }
            return DispatchController.RemoveListener(eventHandler);
        }

        public static bool RemoveListener(uint id) {
            AssertThatIdIsNotEqualsToZero(id);
            return DispatchController.RemoveListener(id);
        }

        public static bool ContainsListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            if (eventHandler == null) {
                throw new ArgumentNullException(nameof(eventHandler));
            }
            return DispatchController.ContainsListener(eventHandler);
        }

        public static bool ContainsListener(uint id) {
            AssertThatIdIsNotEqualsToZero(id);
            return DispatchController.ContainsListener(id);
        }

        public static void Dispatch<T>(T eventArgs) where T : EventArgs {
            Dispatch(null, eventArgs);
        }

        public static void Dispatch<T>(object sender, T eventArgs) where T : EventArgs {
            if (eventArgs == null) {
                throw new ArgumentNullException(nameof(eventArgs));
            }
            DispatchController.Dispatch(sender, eventArgs);
        }

        private static void AssertThatIdIsNotEqualsToZero(uint id) {
            if (id == 0) {
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be equal to zero.");
            }
        }

    }

}