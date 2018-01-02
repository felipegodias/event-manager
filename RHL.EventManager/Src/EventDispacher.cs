using System;
using RHL.EventManager.MonoBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RHL.EventManager {

    /// <summary>
    ///     Provides event-related utility methods that register events.
    /// </summary>
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

        /// <summary>
        ///     Adds a new listener to the given event type.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="eventHandler">Represents the method that will handle the event.</param>
        /// <returns>The listener id if added with success; otherwise, 0</returns>
        public static uint AddListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            if (eventHandler == null) {
                throw new ArgumentNullException(nameof(eventHandler));
            }
            return DispatchController.AddListener(eventHandler);
        }

        /// <summary>
        ///     Removes the given event handler method from the listeners.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="eventHandler">Represents the method that handle the event.</param>
        /// <returns>True if removed with success; otherwise, false.</returns>
        public static bool RemoveListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            if (eventHandler == null) {
                throw new ArgumentNullException(nameof(eventHandler));
            }
            return DispatchController.RemoveListener(eventHandler);
        }

        /// <summary>
        ///     Removes the given event handler id from the listeners.
        /// </summary>
        /// <param name="id">The id of the event handler method.</param>
        /// <returns>True if removed with success; otherwise, false.</returns>
        public static bool RemoveListener(uint id) {
            AssertThatIdIsNotEqualsToZero(id);
            return DispatchController.RemoveListener(id);
        }

        /// <summary>
        ///     Remove all event handlers.
        /// </summary>
        public static void Clear() {
            DispatchController.Clear();
        }

        /// <summary>
        ///     Remove all event handlers from the given event type.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        public static void Clear<T>() where T : EventArgs {
            DispatchController.Clear<T>();
        }

        /// <summary>
        ///     Determines whether an event handler method is in the listeners.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="eventHandler">Represents the method that handle the event.</param>
        /// <returns>True if item is found in the listeners; otherwise, false.</returns>
        public static bool ContainsListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            if (eventHandler == null) {
                throw new ArgumentNullException(nameof(eventHandler));
            }
            return DispatchController.ContainsListener(eventHandler);
        }

        /// <summary>
        ///     Determines whether an event handler id is in the listeners.
        /// </summary>
        /// <param name="id">The id of the event handler method.</param>
        /// <returns>True if item is found in the listeners; otherwise, false.</returns>
        public static bool ContainsListener(uint id) {
            AssertThatIdIsNotEqualsToZero(id);
            return DispatchController.ContainsListener(id);
        }

        /// <summary>
        ///     Calls all methods that are registered to the given event data type.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="eventArgs">An object that contains the event data.</param>
        public static void Dispatch<T>(T eventArgs) where T : EventArgs {
            Dispatch(null, eventArgs);
        }

        /// <summary>
        ///     Calls all methods that are registered to the given event data type.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="eventArgs">An object that contains the event data.</param>
        /// <param name="delay">In how many seconds the methods will be called.</param>
        public static void Dispatch<T>(T eventArgs, float delay) where T : EventArgs {
            Dispatch(null, eventArgs, delay);
        }

        /// <summary>
        ///     Calls all methods that are registered to the given event data type.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">An object that contains the event data.</param>
        public static void Dispatch<T>(object sender, T eventArgs) where T : EventArgs {
            Dispatch(sender, eventArgs, 0);
        }

        /// <summary>
        ///     Calls all methods that are registered to the given event data type.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">An object that contains the event data.</param>
        /// <param name="delay">In how many seconds the methods will be called.</param>
        public static void Dispatch<T>(object sender, T eventArgs, float delay) where T : EventArgs {
            if (eventArgs == null) {
                throw new ArgumentNullException(nameof(eventArgs));
            }
            DispatchController.Dispatch(sender, eventArgs, delay);
        }

        private static void AssertThatIdIsNotEqualsToZero(uint id) {
            if (id == 0) {
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be equal to zero.");
            }
        }

    }

}