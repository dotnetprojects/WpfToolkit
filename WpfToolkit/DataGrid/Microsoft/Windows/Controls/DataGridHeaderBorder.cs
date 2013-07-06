//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Windows.Controls
{
    /// <summary>
    ///     A Border used to provide the default look of headers.
    ///     When Background or BorderBrush are set, the rendering will
    ///     revert back to the default Border implementation.
    /// </summary>
    public class DataGridHeaderBorder : Border
    {
        static DataGridHeaderBorder()
        {
            // This allows the control to re-render correctly when there is a theme change. It relies
            // on a string resource in the theme-specific resource dictionaries.
            DataGridHelper.HookThemeChange(typeof(DataGridHeaderBorder), new PropertyChangedCallback(OnThemeChange));

            // We always set this to true on these borders, so just default it to true here.
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(true));
        }

        #region Header Appearance Properties

        /// <summary>
        ///     Whether the hover look should be applied.
        /// </summary>
        public bool IsHovered
        {
            get { return (bool)GetValue(IsHoveredProperty); }
            set { SetValue(IsHoveredProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for IsHovered.
        /// </summary>
        public static readonly DependencyProperty IsHoveredProperty =
            DependencyProperty.Register("IsHovered", typeof(bool), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        ///     Whether the pressed look should be applied.
        /// </summary>
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for IsPressed.
        /// </summary>
        public static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.Register("IsPressed", typeof(bool), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        ///     When false, will not apply the hover look even when IsHovered is true.
        /// </summary>
        public bool IsClickable
        {
            get { return (bool)GetValue(IsClickableProperty); }
            set { SetValue(IsClickableProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for IsClickable.
        /// </summary>
        public static readonly DependencyProperty IsClickableProperty =
            DependencyProperty.Register("IsClickable", typeof(bool), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        ///     Whether to appear sorted.
        /// </summary>
        public ListSortDirection? SortDirection
        {
            get { return (ListSortDirection?)GetValue(SortDirectionProperty); }
            set { SetValue(SortDirectionProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for SortDirection.
        /// </summary>
        public static readonly DependencyProperty SortDirectionProperty =
            DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        ///     Whether to appear selected.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for IsSelected.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        ///     Vertical = column header
        ///     Horizontal = row header
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for Orientation.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        ///     When there is a Background or BorderBrush, revert to the Border implementation.
        /// </summary>
        private bool UsingBorderImplementation
        {
            get
            {
                return (Background != null) || (BorderBrush != null);
            }
        }

        /// <summary>
        ///     Property that indicates the brush to use when drawing seperators between headers.
        /// </summary>
        public Brush SeparatorBrush
        {
            get { return (Brush)GetValue(SeparatorBrushProperty); }
            set { SetValue(SeparatorBrushProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for SeparatorBrush.
        /// </summary>
        public static readonly DependencyProperty SeparatorBrushProperty =
            DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(null));

        /// <summary>
        ///     Property that indicates the Visibility for the header seperators.
        /// </summary>
        public Visibility SeparatorVisibility
        {
            get { return (Visibility)GetValue(SeparatorVisibilityProperty); }
            set { SetValue(SeparatorVisibilityProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for SeperatorBrush.
        /// </summary>
        public static readonly DependencyProperty SeparatorVisibilityProperty =
            DependencyProperty.Register("SeparatorVisibility", typeof(Visibility), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(Visibility.Visible));
        
        #endregion

        #region Theme Information

        // Theme names
        private const string ClassicThemeName = "Classic";
        private const string AeroNormalColorName = "Aero.NormalColor";
        private const string LunaNormalColorName = "Luna.NormalColor";
        private const string LunaHomeSteadName = "Luna.HomeStead";
        private const string LunaMetallicName = "Luna.Metallic";
        private const string RoyaleNormalColorName = "Royale.NormalColor";

        /// <summary>
        ///     The name of the current theme.
        ///     Accessing this property finishes hooking up
        ///     the theme change notification.
        /// </summary>
        private string Theme
        {
            get
            {
                // Get the current theme string. Also finishes hooking up the theme change notification.
                string theme = DataGridHelper.GetTheme(this);
                if (String.IsNullOrEmpty(theme))
                {
                    // Use Classic as the default if an unknown theme is encountered
                    theme = ClassicThemeName;
                }

                return theme;
            }
        }

        /// <summary>
        ///     Called when the theme changes.
        /// </summary>
        private static void OnThemeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Release all the old resources.
            ReleaseCache();

            DataGridHeaderBorder border = (DataGridHeaderBorder)d;

            // Everything needs to be re-done.
            border.InvalidateMeasure();
            border.InvalidateArrange();
            border.InvalidateVisual();
        }

        #endregion

        #region Layout

        /// <summary>
        ///     Calculates the desired size of the element given the constraint.
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            if (UsingBorderImplementation)
            {
                // Revert to the Border implementation
                return base.MeasureOverride(constraint);
            }

            UIElement child = Child;
            if (child != null)
            {
                // Use the public Padding property if it's set
                Thickness padding = Padding;
                if (padding.Equals(new Thickness()))
                {
                    padding = DefaultPadding;
                }

                double childWidth = constraint.Width;
                double childHeight = constraint.Height;

                // If there is an actual constraint, then reserve space for the chrome
                if (!Double.IsInfinity(childWidth))
                {
                    childWidth = Math.Max(0.0, childWidth - padding.Left - padding.Right);
                }

                if (!Double.IsInfinity(childHeight))
                {
                    childHeight = Math.Max(0.0, childHeight - padding.Top - padding.Bottom);
                }

                child.Measure(new Size(childWidth, childHeight));
                Size desiredSize = child.DesiredSize;

                // Add on the reserved space for the chrome
                return new Size(desiredSize.Width + padding.Left + padding.Right, desiredSize.Height + padding.Top + padding.Bottom);
            }

            return new Size();
        }

        /// <summary>
        ///     Positions children and returns the final size of the element.
        /// </summary>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (UsingBorderImplementation)
            {
                // Revert to the Border implementation
                return base.ArrangeOverride(arrangeSize);
            }

            UIElement child = Child;
            if (child != null)
            {
                // Use the public Padding property if it's set
                Thickness padding = Padding;
                if (padding.Equals(new Thickness()))
                {
                    padding = DefaultPadding;
                }

                // Reserve space for the chrome
                double childWidth = Math.Max(0.0, arrangeSize.Width - padding.Left - padding.Right);
                double childHeight = Math.Max(0.0, arrangeSize.Height - padding.Top - padding.Bottom);

                child.Arrange(new Rect(padding.Left, padding.Top, childWidth, childHeight));
            }

            return arrangeSize;
        }

        #endregion

        #region Rendering

        /// <summary>
        ///     Called when this element should re-render.
        /// </summary>
        protected override void OnRender(DrawingContext dc)
        {
            if (UsingBorderImplementation)
            {
                // Revert to the Border implementation
                base.OnRender(dc);
            }
            else
            {
                // Choose the appropriate rendering based on the current theme
                switch (Theme)
                {
                    case ClassicThemeName:
                        RenderClassic(dc);
                        break;

                    case LunaNormalColorName:
                        RenderLuna(dc, Luna.NormalColor);
                        break;

                    case LunaHomeSteadName:
                        RenderLuna(dc, Luna.HomeStead);
                        break;

                    case LunaMetallicName:
                        RenderLuna(dc, Luna.Metallic);
                        break;

                    case RoyaleNormalColorName:
                        RenderLuna(dc, Luna.Metallic);
                        break;

                    case AeroNormalColorName:
                        RenderAeroNormalColor(dc);
                        break;
                }
            }
        }

        /// <summary>
        ///     Returns a default padding for the various themes for use
        ///     by measure and arrange.
        /// </summary>
        private Thickness DefaultPadding
        {
            get
            {
                Thickness padding = new Thickness(3.0); // The default padding
                if (Orientation == Orientation.Vertical)
                {
                    if (Theme == AeroNormalColorName)
                    {
                        // Reserve space above for the arrow
                        padding = new Thickness(5.0, 4.0, 5.0, 4.0);
                    }
                    else
                    {
                        // Reserve space to the right for the arrow
                        padding.Right = 15.0;
                    }
                }

                // When pressed, offset the child
                if (IsPressed && IsClickable)
                {
                    padding.Left += 1.0;
                    padding.Top += 1.0;
                    padding.Right -= 1.0;
                    padding.Bottom -= 1.0;
                }

                return padding;
            }
        }

        private static double Max0(double d)
        {
            return Math.Max(0.0, d);
        }

        #endregion

        #region Aero

        private void RenderAeroNormalColor(DrawingContext dc)
        {
            Size size = RenderSize;
            bool horizontal = Orientation == Orientation.Horizontal;
            bool isClickable = IsClickable && IsEnabled;
            bool isHovered = isClickable && IsHovered;
            bool isPressed = isClickable && IsPressed;
            ListSortDirection? sortDirection = SortDirection;
            bool isSorted = sortDirection != null;
            bool isSelected = IsSelected;
            bool hasBevel = (!isHovered && !isPressed && !isSorted && !isSelected);

            EnsureCache((int)AeroFreezables.NumFreezables);

            if (horizontal)
            {
                // When horizontal, rotate the rendering by -90 degrees
                Matrix m1 = new Matrix();
                m1.RotateAt(-90.0, 0.0, 0.0);
                Matrix m2 = new Matrix();
                m2.Translate(0.0, size.Height);

                MatrixTransform horizontalRotate = new MatrixTransform(m1 * m2);
                horizontalRotate.Freeze();
                dc.PushTransform(horizontalRotate);

                double temp = size.Width;
                size.Width = size.Height;
                size.Height = temp;
            }

            if (hasBevel)
            {
                // This is a highlight that can be drawn by just filling the background with the color.
                // It will be seen through the gab between the border and the background.
                LinearGradientBrush bevel = (LinearGradientBrush)GetCachedFreezable((int)AeroFreezables.NormalBevel);
                if (bevel == null)
                {
                    bevel = new LinearGradientBrush();
                    bevel.StartPoint = new Point();
                    bevel.EndPoint = new Point(0.0, 1.0);
                    bevel.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 0.0));
                    bevel.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 0.4));
                    bevel.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFC, 0xFC, 0xFD), 0.4));
                    bevel.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFB, 0xFC, 0xFC), 1.0));
                    bevel.Freeze();

                    CacheFreezable(bevel, (int)AeroFreezables.NormalBevel);
                }

                dc.DrawRectangle(bevel, null, new Rect(0.0, 0.0, size.Width, size.Height));
            }

            // Fill the background
            AeroFreezables backgroundType = AeroFreezables.NormalBackground;
            if (isPressed)
            {
                backgroundType = AeroFreezables.PressedBackground;
            }
            else if (isHovered)
            {
                backgroundType = AeroFreezables.HoveredBackground;
            }
            else if (isSorted || isSelected)
            {
                backgroundType = AeroFreezables.SortedBackground;
            }

            LinearGradientBrush background = (LinearGradientBrush)GetCachedFreezable((int)backgroundType);
            if (background == null)
            {
                background = new LinearGradientBrush();
                background.StartPoint = new Point();
                background.EndPoint = new Point(0.0, 1.0);

                switch (backgroundType)
                {
                    case AeroFreezables.NormalBackground:
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 0.4));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF7, 0xF8, 0xFA), 0.4));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF1, 0xF2, 0xF4), 1.0));
                        break;

                    case AeroFreezables.PressedBackground:
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xBC, 0xE4, 0xF9), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xBC, 0xE4, 0xF9), 0.4));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x8D, 0xD6, 0xF7), 0.4));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x8A, 0xD1, 0xF5), 1.0));
                        break;

                    case AeroFreezables.HoveredBackground:
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xE3, 0xF7, 0xFF), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xE3, 0xF7, 0xFF), 0.4));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xBD, 0xED, 0xFF), 0.4));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xB7, 0xE7, 0xFB), 1.0));
                        break;

                    case AeroFreezables.SortedBackground:
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF2, 0xF9, 0xFC), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF2, 0xF9, 0xFC), 0.4));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xE1, 0xF1, 0xF9), 0.4));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xD8, 0xEC, 0xF6), 1.0));
                        break;
                }

                background.Freeze();

                CacheFreezable(background, (int)backgroundType);
            }

            dc.DrawRectangle(background, null, new Rect(0.0, 0.0, size.Width, size.Height));

            if (size.Width >= 2.0)
            {
                // Draw the borders on the sides
                AeroFreezables sideType = AeroFreezables.NormalSides;
                if (isPressed)
                {
                    sideType = AeroFreezables.PressedSides;
                }
                else if (isHovered)
                {
                    sideType = AeroFreezables.HoveredSides;
                }
                else if (isSorted || isSelected)
                {
                    sideType = AeroFreezables.SortedSides;
                }

                if (SeparatorVisibility == Visibility.Visible)
                {
                    Brush sideBrush;
                    if (SeparatorBrush != null)
                    {
                        sideBrush = SeparatorBrush;
                    }
                    else
                    {
                        sideBrush = (Brush)GetCachedFreezable((int)sideType);
                        if (sideBrush == null)
                        {
                            LinearGradientBrush lgBrush = null;
                            if (sideType != AeroFreezables.SortedSides)
                            {
                                lgBrush = new LinearGradientBrush();
                                lgBrush.StartPoint = new Point();
                                lgBrush.EndPoint = new Point(0.0, 1.0);
                                sideBrush = lgBrush;
                            }

                            switch (sideType)
                            {
                                case AeroFreezables.NormalSides:
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF2, 0xF2, 0xF2), 0.0));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xEF, 0xEF, 0xEF), 0.4));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xE7, 0xE8, 0xEA), 0.4));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDE, 0xDF, 0xE1), 1.0));
                                    break;

                                case AeroFreezables.PressedSides:
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x7A, 0x9E, 0xB1), 0.0));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x7A, 0x9E, 0xB1), 0.4));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x50, 0x91, 0xAF), 0.4));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x4D, 0x8D, 0xAD), 1.0));
                                    break;

                                case AeroFreezables.HoveredSides:
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x88, 0xCB, 0xEB), 0.0));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x88, 0xCB, 0xEB), 0.4));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x69, 0xBB, 0xE3), 0.4));
                                    lgBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x69, 0xBB, 0xE3), 1.0));
                                    break;

                                case AeroFreezables.SortedSides:
                                    sideBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x96, 0xD9, 0xF9));
                                    break;
                            }

                            sideBrush.Freeze();

                            CacheFreezable(sideBrush, (int)sideType);
                        }
                    }

                    dc.DrawRectangle(sideBrush, null, new Rect(0.0, 0.0, 1.0, Max0(size.Height - 0.95)));
                    dc.DrawRectangle(sideBrush, null, new Rect(size.Width - 1.0, 0.0, 1.0, Max0(size.Height - 0.95)));
                }
            }

            if (isPressed && (size.Width >= 4.0) && (size.Height >= 4.0))
            {
                // When pressed, there are added borders on the left and top
                LinearGradientBrush topBrush = (LinearGradientBrush)GetCachedFreezable((int)AeroFreezables.PressedTop);
                if (topBrush == null)
                {
                    topBrush = new LinearGradientBrush();
                    topBrush.StartPoint = new Point();
                    topBrush.EndPoint = new Point(0.0, 1.0);
                    topBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x86, 0xA3, 0xB2), 0.0));
                    topBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x86, 0xA3, 0xB2), 0.1));
                    topBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xAA, 0xCE, 0xE1), 0.9));
                    topBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xAA, 0xCE, 0xE1), 1.0));
                    topBrush.Freeze();

                    CacheFreezable(topBrush, (int)AeroFreezables.PressedTop);
                }

                dc.DrawRectangle(topBrush, null, new Rect(0.0, 0.0, size.Width, 2.0));

                LinearGradientBrush pressedBevel = (LinearGradientBrush)GetCachedFreezable((int)AeroFreezables.PressedBevel);
                if (pressedBevel == null)
                {
                    pressedBevel = new LinearGradientBrush();
                    pressedBevel.StartPoint = new Point();
                    pressedBevel.EndPoint = new Point(0.0, 1.0);
                    pressedBevel.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xA2, 0xCB, 0xE0), 0.0));
                    pressedBevel.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xA2, 0xCB, 0xE0), 0.4));
                    pressedBevel.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x72, 0xBC, 0xDF), 0.4));
                    pressedBevel.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x6E, 0xB8, 0xDC), 1.0));
                    pressedBevel.Freeze();

                    CacheFreezable(pressedBevel, (int)AeroFreezables.PressedBevel);
                }

                dc.DrawRectangle(pressedBevel, null, new Rect(1.0, 0.0, 1.0, size.Height - 0.95));
                dc.DrawRectangle(pressedBevel, null, new Rect(size.Width - 2.0, 0.0, 1.0, size.Height - 0.95));
            }

            if (size.Height >= 2.0)
            {
                // Draw the bottom border
                AeroFreezables bottomType = AeroFreezables.NormalBottom;
                if (isPressed)
                {
                    bottomType = AeroFreezables.PressedOrHoveredBottom;
                }
                else if (isHovered)
                {
                    bottomType = AeroFreezables.PressedOrHoveredBottom;
                }
                else if (isSorted || isSelected)
                {
                    bottomType = AeroFreezables.SortedBottom;
                }

                SolidColorBrush bottomBrush = (SolidColorBrush)GetCachedFreezable((int)bottomType);
                if (bottomBrush == null)
                {
                    switch (bottomType)
                    {
                        case AeroFreezables.NormalBottom:
                            bottomBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xD5, 0xD5, 0xD5));
                            break;

                        case AeroFreezables.PressedOrHoveredBottom:
                            bottomBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x93, 0xC9, 0xE3));
                            break;

                        case AeroFreezables.SortedBottom:
                            bottomBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x96, 0xD9, 0xF9));
                            break;
                    }

                    bottomBrush.Freeze();

                    CacheFreezable(bottomBrush, (int)bottomType);
                }

                dc.DrawRectangle(bottomBrush, null, new Rect(0.0, size.Height - 1.0, size.Width, 1.0));
            }

            if (isSorted && (size.Width > 14.0) && (size.Height > 10.0))
            {
                // Draw the sort arrow
                TranslateTransform positionTransform = new TranslateTransform((size.Width - 8.0) * 0.5, 1.0);
                positionTransform.Freeze();
                dc.PushTransform(positionTransform);

                bool ascending = (sortDirection == ListSortDirection.Ascending);
                PathGeometry arrowGeometry = (PathGeometry)GetCachedFreezable(ascending ? (int)AeroFreezables.ArrowUpGeometry : (int)AeroFreezables.ArrowDownGeometry);
                if (arrowGeometry == null)
                {
                    arrowGeometry = new PathGeometry();
                    PathFigure arrowFigure = new PathFigure();

                    if (ascending)
                    {
                        arrowFigure.StartPoint = new Point(0.0, 4.0);

                        LineSegment line = new LineSegment(new Point(4.0, 0.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);

                        line = new LineSegment(new Point(8.0, 4.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);
                    }
                    else
                    {
                        arrowFigure.StartPoint = new Point(0.0, 0.0);

                        LineSegment line = new LineSegment(new Point(8.0, 0.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);

                        line = new LineSegment(new Point(4.0, 4.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);
                    }

                    arrowFigure.IsClosed = true;
                    arrowFigure.Freeze();

                    arrowGeometry.Figures.Add(arrowFigure);
                    arrowGeometry.Freeze();

                    CacheFreezable(arrowGeometry, ascending ? (int)AeroFreezables.ArrowUpGeometry : (int)AeroFreezables.ArrowDownGeometry);
                }

                // Draw two arrows, one inset in the other. This is to achieve a double gradient over both the border and the fill.
                LinearGradientBrush arrowBorder = (LinearGradientBrush)GetCachedFreezable((int)AeroFreezables.ArrowBorder);
                if (arrowBorder == null)
                {
                    arrowBorder = new LinearGradientBrush();
                    arrowBorder.StartPoint = new Point();
                    arrowBorder.EndPoint = new Point(1.0, 1.0);
                    arrowBorder.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x3C, 0x5E, 0x72), 0.0));
                    arrowBorder.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x3C, 0x5E, 0x72), 0.1));
                    arrowBorder.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xC3, 0xE4, 0xF5), 1.0));
                    arrowBorder.Freeze();
                    CacheFreezable(arrowBorder, (int)AeroFreezables.ArrowBorder);
                }

                dc.DrawGeometry(arrowBorder, null, arrowGeometry);

                LinearGradientBrush arrowFill = (LinearGradientBrush)GetCachedFreezable((int)AeroFreezables.ArrowFill);
                if (arrowFill == null)
                {
                    arrowFill = new LinearGradientBrush();
                    arrowFill.StartPoint = new Point();
                    arrowFill.EndPoint = new Point(1.0, 1.0);
                    arrowFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x61, 0x96, 0xB6), 0.0));
                    arrowFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x61, 0x96, 0xB6), 0.1));
                    arrowFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xCA, 0xE6, 0xF5), 1.0));
                    arrowFill.Freeze();
                    CacheFreezable(arrowFill, (int)AeroFreezables.ArrowFill);
                }

                // Inset the fill arrow inside the border arrow
                ScaleTransform arrowScale = (ScaleTransform)GetCachedFreezable((int)AeroFreezables.ArrowFillScale);
                if (arrowScale == null)
                {
                    arrowScale = new ScaleTransform(0.75, 0.75, 3.5, 4.0);
                    arrowScale.Freeze();
                    CacheFreezable(arrowScale, (int)AeroFreezables.ArrowFillScale);
                }

                dc.PushTransform(arrowScale);

                dc.DrawGeometry(arrowFill, null, arrowGeometry);

                dc.Pop(); // Scale Transform
                dc.Pop(); // Position Transform
            }

            if (horizontal)
            {
                dc.Pop(); // Horizontal Rotate
            }
        }

        private enum AeroFreezables : int
        {
            NormalBevel,
            NormalBackground,
            PressedBackground,
            HoveredBackground,
            SortedBackground,
            PressedTop,
            NormalSides,
            PressedSides,
            HoveredSides,
            SortedSides,
            PressedBevel,
            NormalBottom,
            PressedOrHoveredBottom,
            SortedBottom,
            ArrowBorder,
            ArrowFill,
            ArrowFillScale,
            ArrowUpGeometry,
            ArrowDownGeometry,
            NumFreezables
        }

        #endregion

        #region Luna

        private void RenderLuna(DrawingContext dc, Luna colorVariant)
        {
            Size size = RenderSize;
            bool horizontal = Orientation == Orientation.Horizontal;
            bool isClickable = IsClickable && IsEnabled;
            bool isHovered = isClickable && IsHovered;
            bool isPressed = isClickable && IsPressed;
            ListSortDirection? sortDirection = SortDirection;
            bool isSorted = sortDirection != null;
            bool isSelected = IsSelected;

            EnsureCache((int)LunaFreezables.NumFreezables);

            if (horizontal)
            {
                // When horizontal, rotate the rendering by -90 degrees
                Matrix m1 = new Matrix();
                m1.RotateAt(-90.0, 0.0, 0.0);
                Matrix m2 = new Matrix();
                m2.Translate(0.0, size.Height);

                MatrixTransform horizontalRotate = new MatrixTransform(m1 * m2);
                horizontalRotate.Freeze();
                dc.PushTransform(horizontalRotate);

                double temp = size.Width;
                size.Width = size.Height;
                size.Height = temp;
            }

            // Draw the background
            LunaFreezables backgroundType = isPressed ? LunaFreezables.PressedBackground : isHovered ? LunaFreezables.HoveredBackground : LunaFreezables.NormalBackground;
            LinearGradientBrush background = (LinearGradientBrush)GetCachedFreezable((int)backgroundType);
            if (background == null)
            {
                background = new LinearGradientBrush();
                background.StartPoint = new Point();
                background.EndPoint = new Point(0.0, 1.0);

                if (isPressed)
                {
                    if (colorVariant == Luna.Metallic)
                    {
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xB9, 0xB9, 0xC8), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xEC, 0xEC, 0xF3), 0.1));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xEC, 0xEC, 0xF3), 1.0));
                    }
                    else
                    {
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xC1, 0xC2, 0xB8), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDE, 0xDF, 0xD8), 0.1));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDE, 0xDF, 0xD8), 1.0));
                    }
                }
                else if (isHovered || isSelected)
                {
                    if (colorVariant == Luna.Metallic)
                    {
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFE, 0xFE, 0xFE), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFE, 0xFE, 0xFE), 0.85));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xBD, 0xBE, 0xCE), 1.0));
                    }
                    else
                    {
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFA, 0xF9, 0xF4), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFA, 0xF9, 0xF4), 0.85));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xEC, 0xE9, 0xD8), 1.0));
                    }
                }
                else
                {
                    if (colorVariant == Luna.Metallic)
                    {
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF9, 0xFA, 0xFD), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF9, 0xFA, 0xFD), 0.85));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xBD, 0xBE, 0xCE), 1.0));
                    }
                    else
                    {
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xEB, 0xEA, 0xDB), 0.0));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xEB, 0xEA, 0xDB), 0.85));
                        background.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xCB, 0xC7, 0xB8), 1.0));
                    }
                }

                background.Freeze();
                CacheFreezable(background, (int)backgroundType);
            }

            dc.DrawRectangle(background, null, new Rect(0.0, 0.0, size.Width, size.Height));

            if (isHovered && !isPressed && (size.Width >= 6.0) && (size.Height >= 4.0))
            {
                // When hovered, there is a colored tab at the bottom
                TranslateTransform positionTransform = new TranslateTransform(0.0, size.Height - 3.0);
                positionTransform.Freeze();
                dc.PushTransform(positionTransform);

                PathGeometry tabGeometry = new PathGeometry();
                PathFigure tabFigure = new PathFigure();

                tabFigure.StartPoint = new Point(0.5, 0.5);

                LineSegment line = new LineSegment(new Point(size.Width - 0.5, 0.5), true);
                line.Freeze();
                tabFigure.Segments.Add(line);

                ArcSegment arc = new ArcSegment(new Point(size.Width - 2.5, 2.5), new Size(2.0, 2.0), 90.0, false, SweepDirection.Clockwise, true);
                arc.Freeze();
                tabFigure.Segments.Add(arc);

                line = new LineSegment(new Point(2.5, 2.5), true);
                line.Freeze();
                tabFigure.Segments.Add(line);

                arc = new ArcSegment(new Point(0.5, 0.5), new Size(2.0, 2.0), 90.0, false, SweepDirection.Clockwise, true);
                arc.Freeze();
                tabFigure.Segments.Add(arc);

                tabFigure.IsClosed = true;
                tabFigure.Freeze();

                tabGeometry.Figures.Add(tabFigure);
                tabGeometry.Freeze();

                Pen tabStroke = (Pen)GetCachedFreezable((int)LunaFreezables.TabStroke);
                if (tabStroke == null)
                {
                    SolidColorBrush tabStrokeBrush = new SolidColorBrush((colorVariant == Luna.HomeStead) ? Color.FromArgb(0xFF, 0xCF, 0x72, 0x25) : Color.FromArgb(0xFF, 0xF8, 0xA9, 0x00));
                    tabStrokeBrush.Freeze();

                    tabStroke = new Pen(tabStrokeBrush, 1.0);
                    tabStroke.Freeze();

                    CacheFreezable(tabStroke, (int)LunaFreezables.TabStroke);
                }

                LinearGradientBrush tabFill = (LinearGradientBrush)GetCachedFreezable((int)LunaFreezables.TabFill);
                if (tabFill == null)
                {
                    tabFill = new LinearGradientBrush();
                    tabFill.StartPoint = new Point();
                    tabFill.EndPoint = new Point(1.0, 0.0);
                    if (colorVariant == Luna.HomeStead)
                    {
                        tabFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xE3, 0x91, 0x4F), 0.0));
                        tabFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xE3, 0x91, 0x4F), 1.0));
                    }
                    else
                    {
                        tabFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFC, 0xE0, 0xA6), 0.0));
                        tabFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF6, 0xC4, 0x56), 0.1));
                        tabFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF6, 0xC4, 0x56), 0.9));
                        tabFill.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDF, 0x97, 0x00), 1.0));
                    }

                    tabFill.Freeze();
                    CacheFreezable(tabFill, (int)LunaFreezables.TabFill);
                }

                dc.DrawGeometry(tabFill, tabStroke, tabGeometry);
                
                dc.Pop(); // Translate Transform
            }

            if (isPressed && (size.Width >= 2.0) && (size.Height >= 2.0))
            {
                // When pressed, there is a border on the left and bottom
                SolidColorBrush border = (SolidColorBrush)GetCachedFreezable((int)LunaFreezables.PressedBorder);
                if (border == null)
                {
                    border = new SolidColorBrush((colorVariant == Luna.Metallic) ? Color.FromArgb(0xFF, 0x80, 0x80, 0x99) : Color.FromArgb(0xFF, 0xA5, 0xA5, 0x97));
                    border.Freeze();
                    CacheFreezable(border, (int)LunaFreezables.PressedBorder);
                }

                dc.DrawRectangle(border, null, new Rect(0.0, 0.0, 1.0, size.Height));
                dc.DrawRectangle(border, null, new Rect(0.0, Max0(size.Height - 1.0), size.Width, 1.0));
            }

            if (!isPressed && !isHovered && (size.Width >= 4.0))
            {
                if (SeparatorVisibility == Visibility.Visible)
                {
                    Brush sideBrush;
                    if (SeparatorBrush != null)
                    {
                        sideBrush = SeparatorBrush;
                    }
                    else
                    {
                        // When not pressed or hovered, draw the resize gripper
                        LinearGradientBrush gripper = (LinearGradientBrush)GetCachedFreezable((int)(horizontal ? LunaFreezables.HorizontalGripper : LunaFreezables.VerticalGripper));
                        if (gripper == null)
                        {
                            gripper = new LinearGradientBrush();
                            gripper.StartPoint = new Point();
                            gripper.EndPoint = new Point(1.0, 0.0);

                            Color highlight = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                            Color shadow = Color.FromArgb(0xFF, 0xC7, 0xC5, 0xB2);

                            if (horizontal)
                            {
                                gripper.GradientStops.Add(new GradientStop(highlight, 0.0));
                                gripper.GradientStops.Add(new GradientStop(highlight, 0.25));
                                gripper.GradientStops.Add(new GradientStop(shadow, 0.75));
                                gripper.GradientStops.Add(new GradientStop(shadow, 1.0));
                            }
                            else
                            {
                                gripper.GradientStops.Add(new GradientStop(shadow, 0.0));
                                gripper.GradientStops.Add(new GradientStop(shadow, 0.25));
                                gripper.GradientStops.Add(new GradientStop(highlight, 0.75));
                                gripper.GradientStops.Add(new GradientStop(highlight, 1.0));
                            }

                            gripper.Freeze();
                            CacheFreezable(gripper, (int)(horizontal ? LunaFreezables.HorizontalGripper : LunaFreezables.VerticalGripper));
                        }

                        sideBrush = gripper;
                    }

                    dc.DrawRectangle(sideBrush, null, new Rect(horizontal ? 0.0 : Max0(size.Width - 2.0), 4.0, 2.0, Max0(size.Height - 8.0)));
                }
            }

            if (isSorted && (size.Width > 14.0) && (size.Height > 10.0))
            {
                // When sorted, draw an arrow on the right
                TranslateTransform positionTransform = new TranslateTransform(size.Width - 15.0, (size.Height - 5.0) * 0.5);
                positionTransform.Freeze();
                dc.PushTransform(positionTransform);

                bool ascending = (sortDirection == ListSortDirection.Ascending);
                PathGeometry arrowGeometry = (PathGeometry)GetCachedFreezable(ascending ? (int)LunaFreezables.ArrowUpGeometry : (int)LunaFreezables.ArrowDownGeometry);
                if (arrowGeometry == null)
                {
                    arrowGeometry = new PathGeometry();
                    PathFigure arrowFigure = new PathFigure();

                    if (ascending)
                    {
                        arrowFigure.StartPoint = new Point(0.0, 5.0);

                        LineSegment line = new LineSegment(new Point(5.0, 0.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);

                        line = new LineSegment(new Point(10.0, 5.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);
                    }
                    else
                    {
                        arrowFigure.StartPoint = new Point(0.0, 0.0);

                        LineSegment line = new LineSegment(new Point(10.0, 0.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);

                        line = new LineSegment(new Point(5.0, 5.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);
                    }

                    arrowFigure.IsClosed = true;
                    arrowFigure.Freeze();

                    arrowGeometry.Figures.Add(arrowFigure);
                    arrowGeometry.Freeze();

                    CacheFreezable(arrowGeometry, ascending ? (int)LunaFreezables.ArrowUpGeometry : (int)LunaFreezables.ArrowDownGeometry);
                }

                SolidColorBrush arrowFill = (SolidColorBrush)GetCachedFreezable((int)LunaFreezables.ArrowFill);
                if (arrowFill == null)
                {
                    arrowFill = new SolidColorBrush(Color.FromArgb(0xFF, 0xAC, 0xA8, 0x99));
                    arrowFill.Freeze();
                    CacheFreezable(arrowFill, (int)LunaFreezables.ArrowFill);
                }

                dc.DrawGeometry(arrowFill, null, arrowGeometry);

                dc.Pop(); // Position Transform
            }

            if (horizontal)
            {
                dc.Pop(); // Horizontal Rotate
            }
        }

        private enum Luna
        {
            NormalColor,
            HomeStead,
            Metallic,
        }

        private enum LunaFreezables
        {
            NormalBackground,
            HoveredBackground,
            PressedBackground,
            HorizontalGripper,
            VerticalGripper,
            PressedBorder,
            TabGeometry,
            TabStroke,
            TabFill,
            ArrowFill,
            ArrowUpGeometry,
            ArrowDownGeometry,
            NumFreezables
        }

        #endregion

        #region Classic

        private static readonly DependencyProperty ControlBrushProperty =
            DependencyProperty.Register("ControlBrush", typeof(Brush), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        private Brush EnsureControlBrush()
        {
            if (ReadLocalValue(ControlBrushProperty) == DependencyProperty.UnsetValue)
            {
                SetResourceReference(ControlBrushProperty, SystemColors.ControlBrushKey);
            }

            return (Brush)GetValue(ControlBrushProperty);
        }

        private void RenderClassic(DrawingContext dc)
        {
            Size size = RenderSize;
            bool isClickable = IsClickable && IsEnabled;
            bool isPressed = isClickable && IsPressed;
            ListSortDirection? sortDirection = SortDirection;
            bool isSorted = sortDirection != null;
            bool horizontal = Orientation == Orientation.Horizontal;
            Brush background = EnsureControlBrush();
            Brush light = SystemColors.ControlLightBrush;
            Brush dark = SystemColors.ControlDarkBrush;
            bool shouldDrawRight = true;
            bool shouldDrawBottom = true;
            bool usingSeparatorBrush = false;

            Brush darkDarkRight = null;
            if (!horizontal)
            {
                if (SeparatorVisibility == Visibility.Visible && SeparatorBrush != null)
                {
                    darkDarkRight = SeparatorBrush;
                    usingSeparatorBrush = true;
                }
                else
                {
                    shouldDrawRight = false;
                }
            }
            else
            {
                darkDarkRight = SystemColors.ControlDarkDarkBrush;
            }

            Brush darkDarkBottom = null;
            if (horizontal)
            {
                if (SeparatorVisibility == Visibility.Visible && SeparatorBrush != null)
                {
                    darkDarkBottom = SeparatorBrush;
                    usingSeparatorBrush = true;
                }
                else
                {
                    shouldDrawBottom = false;
                }
            }
            else
            {
                darkDarkBottom = SystemColors.ControlDarkDarkBrush;
            }

            EnsureCache((int)ClassicFreezables.NumFreezables);

            // Draw the background
            dc.DrawRectangle(background, null, new Rect(0.0, 0.0, size.Width, size.Height));

            if ((size.Width > 3.0) && (size.Height > 3.0))
            {
                // Draw the border
                if (isPressed)
                {
                    dc.DrawRectangle(dark, null, new Rect(0.0, 0.0, size.Width, 1.0));
                    dc.DrawRectangle(dark, null, new Rect(0.0, 0.0, 1.0, size.Height));
                    dc.DrawRectangle(dark, null, new Rect(0.0, Max0(size.Height - 1.0), size.Width, 1.0));
                    dc.DrawRectangle(dark, null, new Rect(Max0(size.Width - 1.0), 0.0, 1.0, size.Height));
                }
                else
                {
                    dc.DrawRectangle(light, null, new Rect(0.0, 0.0, 1.0, Max0(size.Height - 1.0)));
                    dc.DrawRectangle(light, null, new Rect(0.0, 0.0, Max0(size.Width - 1.0), 1.0));
                    
                    if (shouldDrawRight)
                    {
                        if (!usingSeparatorBrush)
                        {
                            dc.DrawRectangle(dark, null, new Rect(Max0(size.Width - 2.0), 1.0, 1.0, Max0(size.Height - 2.0)));
                        }

                        dc.DrawRectangle(darkDarkRight, null, new Rect(Max0(size.Width - 1.0), 0.0, 1.0, size.Height));
                    }

                    if (shouldDrawBottom)
                    {
                        if (!usingSeparatorBrush)
                        {
                            dc.DrawRectangle(dark, null, new Rect(1.0, Max0(size.Height - 2.0), Max0(size.Width - 2.0), 1.0));
                        }

                        dc.DrawRectangle(darkDarkBottom, null, new Rect(0.0, Max0(size.Height - 1.0), size.Width, 1.0));
                    }
                }
            }

            if (isSorted && (size.Width > 14.0) && (size.Height > 10.0))
            {
                // If sorted, draw an arrow on the right
                TranslateTransform positionTransform = new TranslateTransform(size.Width - 15.0, (size.Height - 5.0) * 0.5);
                positionTransform.Freeze();
                dc.PushTransform(positionTransform);

                bool ascending = (sortDirection == ListSortDirection.Ascending);
                PathGeometry arrowGeometry = (PathGeometry)GetCachedFreezable(ascending ? (int)ClassicFreezables.ArrowUpGeometry : (int)ClassicFreezables.ArrowDownGeometry);
                if (arrowGeometry == null)
                {
                    arrowGeometry = new PathGeometry();
                    PathFigure arrowFigure = new PathFigure();

                    if (ascending)
                    {
                        arrowFigure.StartPoint = new Point(0.0, 5.0);

                        LineSegment line = new LineSegment(new Point(5.0, 0.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);

                        line = new LineSegment(new Point(10.0, 5.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);
                    }
                    else
                    {
                        arrowFigure.StartPoint = new Point(0.0, 0.0);

                        LineSegment line = new LineSegment(new Point(10.0, 0.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);

                        line = new LineSegment(new Point(5.0, 5.0), false);
                        line.Freeze();
                        arrowFigure.Segments.Add(line);
                    }

                    arrowFigure.IsClosed = true;
                    arrowFigure.Freeze();

                    arrowGeometry.Figures.Add(arrowFigure);
                    arrowGeometry.Freeze();

                    CacheFreezable(arrowGeometry, ascending ? (int)ClassicFreezables.ArrowUpGeometry : (int)ClassicFreezables.ArrowDownGeometry);
                }

                dc.DrawGeometry(SystemColors.GrayTextBrush, null, arrowGeometry);

                dc.Pop(); // Position Transform
            }
        }

        private enum ClassicFreezables
        {
            ArrowUpGeometry,
            ArrowDownGeometry,
            NumFreezables
        }

        #endregion

        #region Freezable Cache

        /// <summary>
        ///     Creates a cache of frozen Freezable resources for use 
        ///     across all instances of the border.
        /// </summary>
        private static void EnsureCache(int size)
        {
            // Quick check to avoid locking
            if (_freezableCache == null) 
            {
                lock (_cacheAccess)
                {
                    // Re-check in case another thread created the cache
                    if (_freezableCache == null) 
                    {
                        _freezableCache = new List<Freezable>(size);
                        for (int i = 0; i < size; i++)
                        {
                            _freezableCache.Add(null);
                        }
                    }
                }
            }

            Debug.Assert(_freezableCache.Count == size, "The cache size does not match the requested amount.");
        }

        /// <summary>
        ///     Releases all resources in the cache.
        /// </summary>
        private static void ReleaseCache()
        {
            // Avoid locking if necessary
            if (_freezableCache != null) 
            {
                lock (_cacheAccess)
                {
                    // No need to re-check if non-null since it's OK to set it to null multiple times
                    _freezableCache = null;
                }
            }
        }

        /// <summary>
        ///     Retrieves a cached resource.
        /// </summary>
        private static Freezable GetCachedFreezable(int index)
        {
            lock (_cacheAccess)
            {
                Freezable freezable = _freezableCache[index];
                Debug.Assert((freezable == null) || freezable.IsFrozen, "Cached Freezables should have been frozen.");
                return freezable;
            }
        }

        /// <summary>
        ///     Caches a resources.
        /// </summary>
        private static void CacheFreezable(Freezable freezable, int index)
        {
            Debug.Assert(freezable.IsFrozen, "Cached Freezables should be frozen.");

            lock (_cacheAccess)
            {
                if (_freezableCache[index] != null)
                {
                    _freezableCache[index] = freezable;
                }
            }
        }

        private static List<Freezable> _freezableCache;
        private static object _cacheAccess = new object();

        #endregion
    }
}
