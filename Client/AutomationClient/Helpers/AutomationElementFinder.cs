// ----------------------------------------------------------------------
// <copyright file="AutomationElementFinder.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using WindowsPhoneTestFramework.Client.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Helpers
{
    public static class AutomationElementFinder
    {
        public readonly static List<string> StringPropertyNamesToTestForText = new List<string>()
                                                                       {
                                                                            "Text",
                                                                            "Password",
                                                                            "Message"
                                                                       };
        public readonly static List<string> ObjectPropertyNamesToTestForText = new List<string>()
                                                                       {
                                                                            "Content"
                                                                       };
        public readonly static List<string> PropertyNamesToTestForValue = new List<string>()
                                                                       {
                                                                            "Text",
                                                                            "Password",
                                                                            "IsChecked",
                                                                            "Value"
                                                                       };

        public static UIElement FindElement(AutomationIdentifier identifier)
        {
            return FindElementsNearestParentOfType<UIElement>(identifier);
        }

        public static UIElement FindElementsNearestParentOfType<TParentType>(AutomationIdentifier identifier)
            where TParentType : UIElement
        {
            if (!string.IsNullOrEmpty(identifier.AutomationName))
            {
                var candidate = FindElementsNearestParentByAutomationTag<TParentType>(identifier.AutomationName);
                if (candidate != null)
                    return candidate;
            }

            if (!string.IsNullOrEmpty(identifier.ElementName))
            {
                var candidate = FindElementsNearestParentByElementName<TParentType>(identifier.ElementName);
                if (candidate != null)
                    return candidate;
            }

            if (!string.IsNullOrEmpty(identifier.DisplayedText))
            {
                var candidate = FindElementsNearestParentByDisplayedText<TParentType>(identifier.DisplayedText);
                if (candidate != null)
                    return candidate;
            }
            // not found - return null :/
            return null;
        }

        public static T GetElementProperty<T>(object target, string name)
        {
            var propertyInfo = GetPropertyInfo(target, name);

            if (propertyInfo == null)
                return default(T);

            if (!typeof(T).IsAssignableFrom(propertyInfo.PropertyType))
                return default(T);

            return (T)propertyInfo.GetValue(target, new object[0]);
        }

        public static bool SetElementProperty<T>(object target, string name, T value)
        {
            var propertyInfo = GetPropertyInfo(target, name);

            if (propertyInfo == null)
                return false;

            if (!typeof(T).IsAssignableFrom(propertyInfo.PropertyType))
                return false;

            propertyInfo.SetValue(target, value, new object[0]);
            return true;
        }

        private static PropertyInfo GetPropertyInfo(object target, string name)
        {
            var targetType = target.GetType();
            return targetType.GetProperty(name,
                                          BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance |
                                          BindingFlags.FlattenHierarchy);
        }

        private class SearchResult<TElementType>
            where TElementType : UIElement
        {
            public UIElement IdentifiedElement { get; private set; }
            public TElementType NearestParent { get; private set; }

            public bool HasNearestParent { get { return NearestParent != null; } }
            public bool TryInsertNearestParent(UIElement nearestParentCandidate)
            {
                if (HasNearestParent)
                    return false;

                var cast = nearestParentCandidate as TElementType;
                if (cast == null)
                    return false;
                
                NearestParent = cast;
                return true;
            }

            public SearchResult(UIElement identifiedElement)
            {
                IdentifiedElement = identifiedElement;
                NearestParent = identifiedElement as TElementType;
            }
        }

        private static SearchResult<TElementType> SearchFrameworkElementTreeFor<TElementType>(UIElement parentElement, Func<UIElement, bool> elementTest)
            where TElementType : UIElement
        {
            if (parentElement == null)
                return null;

            if (elementTest(parentElement))
                return new SearchResult<TElementType>(parentElement);

            var childrenCount = VisualTreeHelper.GetChildrenCount(parentElement);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, i) as FrameworkElement;
                if (child != null)
                {
                    var candidate = SearchFrameworkElementTreeFor<TElementType>(child, elementTest);
                    if (candidate != null)
                    {
                        candidate.TryInsertNearestParent(parentElement);
                        return candidate;
                    }
                }
            }

            return null;
        }

        public static UIElement FindElementsNearestParentByAutomationTag<TElementType>(string automationName)
            where TElementType : UIElement
        {
            var rootVisual = Application.Current.RootVisual;
            var searchResult = SearchFrameworkElementTreeFor<TElementType>(rootVisual, (element) =>
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement == null)
                    return false;
                var tag = frameworkElement.Tag;
                if (tag == null)
                    return false;

                var tagString = tag.ToString();
                if (tagString.Length <= "auto:".Length || !tagString.StartsWith("auto:"))
                    return false;

                if (tagString.Substring("auto:".Length) != automationName)
                    return false;

                return true;
            });
            if (searchResult == null)
                return null;
            return searchResult.NearestParent;
        }

        public static UIElement FindElementsNearestParentByElementName<TElementType>(string elementName)
            where TElementType : UIElement
        {
            var rootVisual = Application.Current.RootVisual;
            var searchResult = SearchFrameworkElementTreeFor<TElementType>(rootVisual, (element) =>
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement == null)
                    return false;

                var name = frameworkElement.Name;
                if (string.IsNullOrEmpty(name))
                    return false;

                if (name != elementName)
                    return false;

                return true;
            });
            if (searchResult == null)
                return null;
            return searchResult.NearestParent;
        }

        public static UIElement FindElementByDisplayedText(string displayedText)
        {
            return FindElementsNearestParentByDisplayedText<UIElement>(displayedText);
        }

        public static UIElement FindElementsNearestParentByDisplayedText<TElementType>(string displayedText)
            where TElementType : UIElement
        {
            var rootVisual = Application.Current.RootVisual;
            var searchResult = SearchFrameworkElementTreeFor<TElementType>(rootVisual, (element) =>
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement == null)
                    return false;

                if (frameworkElement.Visibility == Visibility.Collapsed)
                    return false;

                if (frameworkElement.Opacity == 0.0)
                    return false;

                foreach (var textName in StringPropertyNamesToTestForText)
                {
                    var stringPropertyValue = GetElementProperty<string>(frameworkElement, textName);
                    if (!string.IsNullOrEmpty(stringPropertyValue))
                        if (stringPropertyValue == displayedText)
                            return true;
                }

                foreach (var objectName in ObjectPropertyNamesToTestForText)
                {
                    var objectPropertyValue = GetElementProperty<object>(frameworkElement, objectName);
                    if (objectPropertyValue != null
                        && objectPropertyValue is string
                        && !string.IsNullOrEmpty(objectPropertyValue.ToString()))
                        if (objectPropertyValue.ToString() == displayedText)
                            return true;
                }

                return false;
            });
            if (searchResult == null)
                return null;
            return searchResult.NearestParent;
        }

        public static string GetTextForFrameworkElement(FrameworkElement frameworkElement)
        {
            foreach (var textName in StringPropertyNamesToTestForText)
            {
                var stringPropertyValue = GetElementProperty<string>(frameworkElement, textName);
                if (stringPropertyValue != null)
                   return stringPropertyValue;
            }

            foreach (var objectName in ObjectPropertyNamesToTestForText)
            {
                var objectPropertyValue = GetElementProperty<object>(frameworkElement, objectName);
                if (objectPropertyValue != null
                    && objectPropertyValue is string)
                    return (string) objectPropertyValue;
            }

            return null;
        }

        public static string GetValueForFrameworkElement(FrameworkElement frameworkElement)
        {
            foreach (var objectName in PropertyNamesToTestForValue)
            {
                var objectPropertyValue = GetElementProperty<object>(frameworkElement, objectName);
                if (objectPropertyValue != null)
                {
                    return (string) objectPropertyValue.ToString();
                }
            }

            return null;
        }
    }
}
