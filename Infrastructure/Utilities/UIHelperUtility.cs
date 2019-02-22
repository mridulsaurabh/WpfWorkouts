
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Controls;

namespace Infrastructure.Utility
{
    public static class UIHelperUtility
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent)
        where T : DependencyObject
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                var childType = child as T;
                if (childType == null)
                {
                    foreach (var other in FindVisualChildren<T>(child))
                    {
                        yield return other;
                    }
                }
                else
                {
                    yield return (T)child;
                }
            }
        }

        public static T FindVisualParent<T>(DependencyObject dependencyObject)
        where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(dependencyObject);
            if (parentObject == null)
            {
                return null;
            }

            //Check if the parent matches the type we are looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }

        public static List<FrameworkElement> FindTextFieldAndSelectionControls(DependencyObject parent)
        {
            List<FrameworkElement> matchItems = new List<FrameworkElement>();
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                var childType = child as FrameworkElement;
                if (childType != null)
                {
                    if ((childType is TextBox) || (childType is ComboBox))
                    {
                        matchItems.Add(childType);
                    }
                    else
                    {
                        foreach (var other in FindTextFieldAndSelectionControls(child))
                        {
                            matchItems.Add(other);
                        }
                    }
                }
            }
            return matchItems;
        }

        public static string GetEnumDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

    }
}
