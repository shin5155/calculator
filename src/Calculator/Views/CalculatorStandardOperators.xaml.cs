// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

//
// CalculatorStandardOperators.xaml.h
// Declaration of the CalculatorStandardOperators class
//

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CalculatorApp.Common;
using CalculatorApp.ViewModel.Common;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;
using Windows.UI.Xaml.Controls.Primitives;

namespace CalculatorApp
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public sealed partial class CalculatorStandardOperators
    {
        public CalculatorStandardOperators()
        {
            m_isErrorVisualState = false;
            InitializeComponent();

            // Ensure Multiply button is registered on initialization
            RegisterMultiplyButton();
        }

        public bool IsErrorVisualState
        {
            get => m_isErrorVisualState;
            set
            {
                if (m_isErrorVisualState != value)
                {
                    m_isErrorVisualState = value;
                    string newState = m_isErrorVisualState ? "ErrorLayout" : "NoErrorLayout";
                    VisualStateManager.GoToState(this, newState, false);
                    NumberPad.IsErrorVisualState = m_isErrorVisualState;
                }
            }
        }
        private bool m_isErrorVisualState;

        /// <summary>
        /// Registers the multiply button manually if not already mapped.
        /// </summary>
        private void RegisterMultiplyButton()
        {
            int viewId = Utilities.GetWindowId();

            // Ensure s_virtualKey has the Multiply key registered
            if (!KeyboardShortcutManager.s_virtualKey.ContainsKey(viewId))
            {
                Debug.WriteLine("🔴 ViewId not found in s_virtualKey, creating entry...");
                KeyboardShortcutManager.s_virtualKey[viewId] = new SortedDictionary<MyVirtualKey, List<WeakReference>>();
            }

            // Check if Multiply is already registered
            if (!KeyboardShortcutManager.s_virtualKey[viewId].ContainsKey(MyVirtualKey.Multiply))
            {
                Debug.WriteLine("🟡 Multiply key not registered, searching for button...");

                var multiplyButton = FindMultiplyButton();
                if (multiplyButton != null)
                {
                    Debug.WriteLine("✅ MultiplyButton found and registered!");
                    KeyboardShortcutManager.s_virtualKey[viewId].Add(MyVirtualKey.Multiply, new List<WeakReference> { new WeakReference(multiplyButton) });
                }
                else
                {
                    Debug.WriteLine("❌ MultiplyButton NOT found in UI. Check XAML.");
                }
            }
            else
            {
                Debug.WriteLine("✔️ Multiply key already registered.");
            }
        }

        /// <summary>
        /// Finds the multiply button in the UI.
        /// </summary>
        /// <returns>The multiply button if found, otherwise null.</returns>
        private ButtonBase FindMultiplyButton()
        {
            return FindButtonByName("MultiplyButton");
        }

        /// <summary>
        /// Searches for a button by name within the current UI elements.
        /// </summary>
        /// <param name="name">The name of the button to find.</param>
        /// <returns>The ButtonBase object if found, otherwise null.</returns>
        private ButtonBase FindButtonByName(string name)
        {
            // 🔍 Iterate over the outer dictionary entries (view IDs)
            foreach (var outerEntry in KeyboardShortcutManager.s_virtualKey)
            {
                // 🔍 Iterate over the inner dictionary entries (MyVirtualKey → List<WeakReference>)
                foreach (var innerEntry in outerEntry.Value)
                {
                    List<WeakReference> buttonList = innerEntry.Value; // ✅ Get the List<WeakReference>

                    // 🔍 Iterate over each WeakReference inside the list
                    foreach (WeakReference weakRef in buttonList)
                    {
                        if (weakRef.Target is ButtonBase btn && btn.Name == name)
                        {
                            return btn; // ✅ Found the button, return it
                        }
                    }
                }
            }

            return null; // ❌ Return null if button is not found
        }






    }
}
