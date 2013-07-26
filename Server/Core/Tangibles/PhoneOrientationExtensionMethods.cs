//  ----------------------------------------------------------------------
//  <copyright file="PhoneOrientationExtensionMethods.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Drawing;

namespace WindowsPhoneTestFramework.Server.Core.Tangibles
{
    // this might be a bad name - really this contains a RectangleF extension method
    public static class PhoneOrientationExtensionMethods
    {
        public static Point ScreenMiddle(this PhoneOrientation orientation)
        {
            var size = orientation.ScreenSize();
            return new Point(size.Width/2, size.Height/2);
        }

        public static Size ScreenSize(this PhoneOrientation orientation)
        {
            switch (orientation)
            {
                case PhoneOrientation.Landscape800By480:
                    return new Size(800, 480);

                case PhoneOrientation.Portrait480By800:
                    return new Size(480, 800);

                default:
                    throw new ArgumentException("unknown orientation " + orientation);
            }
        }

        public static void IsVisible(this RectangleF position, PhoneOrientation orientation)
        {
            if (position.IsEmpty)
            {
                throw new ArgumentOutOfRangeException("IsVisible position is Empty");
            }

            var height = 0.0;
            var width = 0.0;

            switch (orientation)
            {
                case PhoneOrientation.Landscape800By480:
                    height = 480.0;
                    width = 800.0;
                    break;
                case PhoneOrientation.Portrait480By800:
                    height = 800.0;
                    width = 480.0;
                    break;
                default:
                    throw new ArgumentException("unknown orientation " + orientation);
            }

            if (position.X + position.Width <= 0)
                throw new ArgumentOutOfRangeException("bottom",
                                                      string.Format(
                                                          "IsVisible position.X ({0}) + position.Width ({1}) <= 0 ",
                                                          position.X, position.Width));

            if (position.Y + position.Height <= 0)
                throw new ArgumentOutOfRangeException("right",
                                                      string.Format(
                                                          "IsVisible position.Y ({0}) + position.Height ({1}) <= 0 ",
                                                          position.Y, position.Height));

            if (position.X >= width)
                throw new ArgumentOutOfRangeException("left",
                                                      string.Format("IsVisible position.X ({0}) > width ({1})",
                                                                    position.X, width));

            if (position.Y >= height)
                throw new ArgumentOutOfRangeException("top",
                                                      string.Format("IsVisible position.Y ({0}) > height ({1})",
                                                                    position.Y, height));
        }
    }
}