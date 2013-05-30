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
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Automation;
using WindowsPhoneTestFramework.Client.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Helpers
{
    using System.Collections;
    using System.Windows.Controls.Primitives;
    using Microsoft.Phone.Controls;

    public static class AutomationElementFinder
    {
        public readonly static List<string> StringPropertyNamesToTestForText = new List<string>()
                                                                       {
                                                                            "Text",
                                                                            "Password",
                                                                            "Message",
                                                                            "Source"
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

        public static UIElement FindElement(AutomationIdentifier controlIdentifier, int ordinal = 0, AutomationIdentifier parentIdentifier = null)
        {
            // get the parent element as uielement
            var rootVisual = GetRootVisual();
            var parent = parentIdentifier == null ? rootVisual :
                FindElementsNearestParentOfType<UIElement>(rootVisual, parentIdentifier);

            // get the control that is a child of parent as uielement
            var control = FindElementsNearestParentOfType<UIElement>(parent, controlIdentifier, ordinal);

            return control;
        }

        public static UIElement FindElementsNearestParentOfType<TParentType>(UIElement root, AutomationIdentifier identifier, int index = 0)
            where TParentType : UIElement
        {
            if (!string.IsNullOrEmpty(identifier.AutomationName))
            {
                var candidate = FindElementsNearestParentByAutomationProperty<TParentType>(root, identifier.AutomationName, index);
                if (candidate != null)
                    return candidate;
            } 
            
            if (!string.IsNullOrEmpty(identifier.AutomationName))
            {
                var candidate = FindElementsNearestParentByAutomationTag<TParentType>(root, identifier.AutomationName);
                if (candidate != null)
                    return candidate;
            }

            if (!string.IsNullOrEmpty(identifier.ElementName))
            {
                var candidate = FindElementsNearestParentByElementName<TParentType>(root, identifier.ElementName, index);
                if (candidate != null)
                    return candidate;
            }

            if (!string.IsNullOrEmpty(identifier.DisplayedText))
            {
                var candidate = FindElementsNearestParentByDisplayedText<TParentType>(root, identifier.DisplayedText);
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

        public static UIElement FindElementsNearestParentByAutomationProperty<TElementType>(UIElement root, string controlName, int index)
            where TElementType : UIElement
        {
            var count = -1;

            Func<UIElement, bool> elementTest = (element) =>
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement == null)
                    return false;

                var name = frameworkElement.GetValue(AutomationProperties.NameProperty);
                if (name == null)
                    return false;

                if (string.IsNullOrWhiteSpace(name.ToString()))
                    return false;

                if (name.ToString() != controlName)
                    return false;

                count++;
                return index < 0 || count == index;
            };

            var searchResult = SearchFrameworkElementTreeFor<TElementType>(root, elementTest);

            if (searchResult == null)
            {
                foreach (Popup popup in VisualTreeHelper.GetOpenPopups())
                {
                    searchResult = SearchFrameworkElementTreeFor<TElementType>(popup.Child, elementTest);
                    if (searchResult != null) break;
                }
            }


            if (searchResult == null)
                return null;
            return searchResult.NearestParent;
        }

        public static UIElement FindElementsNearestParentByAutomationTag<TElementType>(UIElement root, string automationName)
            where TElementType : UIElement
        {

            Func<UIElement, bool> elementTest = (element) =>
                {
                    var frameworkElement = element as FrameworkElement;
                    if (frameworkElement == null) return false;
                    var tag = frameworkElement.Tag;
                    if (tag == null) return false;

                    var tagString = tag.ToString();
                    if (tagString.Length <= "auto:".Length || !tagString.StartsWith("auto:")) return false;

                    if (tagString.Substring("auto:".Length) != automationName) return false;

                    return true;
                };

            var searchResult = SearchFrameworkElementTreeFor<TElementType>(root, elementTest);

            if (searchResult == null)
            {
                foreach (Popup popup in VisualTreeHelper.GetOpenPopups())
                {
                    searchResult = SearchFrameworkElementTreeFor<TElementType>(popup.Child, elementTest);
                    if (searchResult != null) break;
                }
            }

            if (searchResult == null)
                return null;
            return searchResult.NearestParent;
        }

        public static UIElement FindElementsNearestParentByElementName<TElementType>(UIElement root, string elementName, int index)
            where TElementType : UIElement
        {
            var count = -1;

            Func<UIElement, bool> elementTest = (element) =>
                {
                    var frameworkElement = element as FrameworkElement;
                    if (frameworkElement == null) return false;

                    var name = frameworkElement.Name;
                    if (string.IsNullOrEmpty(name)) return false;

                    if (name != elementName) return false;

                    count++;
                    return index < 0 || count == index;
                };

            var searchResult = SearchFrameworkElementTreeFor<TElementType>(root, elementTest);

            if (searchResult == null)
            {
                foreach (Popup popup in VisualTreeHelper.GetOpenPopups())
                {
                    searchResult = SearchFrameworkElementTreeFor<TElementType>(popup.Child, elementTest);
                    if (searchResult != null) break;
                }
            }

            if (searchResult == null)
                return null;

            return searchResult.NearestParent;
        }

        public static UIElement FindElementByDisplayedText(string displayedText)
        {
            return FindElementsNearestParentByDisplayedText<UIElement>(GetRootVisual(), displayedText);
        }

        public static UIElement FindElementsNearestParentByDisplayedText<TElementType>(UIElement root, string displayedText)
            where TElementType : UIElement
        {

            Func<UIElement, bool> elementTest = (element) =>
                {
                    var frameworkElement = element as FrameworkElement;
                    if (frameworkElement == null) return false;

                    if (frameworkElement.Visibility == Visibility.Collapsed) return false;

                    if (frameworkElement.Opacity == 0.0) return false;

                    foreach (var textName in StringPropertyNamesToTestForText)
                    {
                        var stringPropertyValue = GetElementProperty<string>(frameworkElement, textName);
                        if (!string.IsNullOrEmpty(stringPropertyValue)) if (stringPropertyValue.Contains(displayedText)) return true;
                    }

                    foreach (var objectName in ObjectPropertyNamesToTestForText)
                    {
                        var objectPropertyValue = GetElementProperty<object>(frameworkElement, objectName);
                        if (objectPropertyValue != null && objectPropertyValue is string
                            && !string.IsNullOrEmpty(objectPropertyValue.ToString())) if (objectPropertyValue.ToString().Contains(displayedText)) return true;
                    }

                    return false;
                };

            var searchResult = SearchFrameworkElementTreeFor<TElementType>(root, elementTest);

            if (searchResult == null)
            {
                foreach (Popup popup in VisualTreeHelper.GetOpenPopups())
                {
                    searchResult = SearchFrameworkElementTreeFor<TElementType>(popup.Child, elementTest);
                    if(searchResult != null) break;
                }
            }

            if (searchResult == null)
                return null;

            return searchResult.NearestParent;
        }
		
		public static UIElement FindElementsChildByType<TElementType>(UIElement parentElement)
            where TElementType : UIElement        
        {
            if (parentElement == null)
                return null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parentElement);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, i) as FrameworkElement;
                if (child is TElementType)
                {
                    return child;
                }
                else
                {
                    return FindElementsChildByType<TElementType>(child);
                }
            }

            return null; 
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

        public static FrameworkElement GetRootVisual(bool waitUntilEnabled = true)
        {
            var result = Application.Current.RootVisual as PhoneApplicationFrame;
            var i = 0;
            while (waitUntilEnabled && i++ < 5 && result != null && !result.IsEnabled)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            return result;
        }

        public static Rect Position(FrameworkElement element)
        {
            // Obtain transform information based off root element
            GeneralTransform gt = element.TransformToVisual(Application.Current.RootVisual);

            // Find the four corners of the element
            Point topLeft = gt.Transform(new Point(0, 0));
            Point topRight = gt.Transform(new Point(element.RenderSize.Width, 0));
            Point bottomLeft = gt.Transform(new Point(0, element.RenderSize.Height));
            Point bottomRight = gt.Transform(new Point(element.RenderSize.Width, element.RenderSize.Height));

            var left = Math.Min(Math.Min(Math.Min(topLeft.X, topRight.X), bottomLeft.X), bottomRight.X);
            var top = Math.Min(Math.Min(Math.Min(topLeft.Y, topRight.Y), bottomLeft.Y), bottomRight.Y);
            var position = new Rect(left, top, element.ActualWidth, element.ActualHeight);
            return position;
        }
    }
}
