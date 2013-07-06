using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Diagnostics;


namespace ControlsLibrary
{
    [TemplatePart(Name = "Core", Type = typeof(FrameworkElement))]

    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]

    [TemplateVisualState(Name = "Sunny", GroupName = "WeatherStates")]
    [TemplateVisualState(Name = "PartlyCloudy", GroupName = "WeatherStates")]
    [TemplateVisualState(Name = "Cloudy", GroupName = "WeatherStates")]
    [TemplateVisualState(Name = "Rainy", GroupName = "WeatherStates")]
    public class WeatherControl : Control
    {

        #region Constructors

        /// <summary>
        ///     Initializes global state.
        /// </summary>
        static WeatherControl()
        {
            // Sets the type key used to find the default style for this control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WeatherControl), new FrameworkPropertyMetadata(typeof(WeatherControl)));
        }

        /// <summary>
        ///     Instantiates and instance of this class.
        /// </summary>
        public WeatherControl()
        {
        }

        #endregion

        #region States

        private void GoToState(bool useTransitions)
        {
            //  Go to states in NormalStates state group
            if (IsPressed)
            {
                VisualStateManager.GoToState(this, "Pressed", useTransitions);
            }
            else if (_isMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }

            //  Go to states in WeatherStates state group
            switch (Condition)
            {
                case WeatherCondition.Sunny:
                    VisualStateManager.GoToState(this, "Sunny", useTransitions);
                    break;

                case WeatherCondition.PartlyCloudy:
                    VisualStateManager.GoToState(this, "PartlyCloudy", useTransitions);
                    break;

                case WeatherCondition.Cloudy:
                    VisualStateManager.GoToState(this, "Cloudy", useTransitions);
                    break;

                case WeatherCondition.Rainy:
                    VisualStateManager.GoToState(this, "Rainy", useTransitions);
                    break;
            }
        }

        #endregion

        #region Parts

        /// <summary>
        ///     Called when a template is applied to this control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CorePart = (FrameworkElement)GetTemplateChild("Core");

            GoToState(/* useTransitions = */ false);
        }

        private FrameworkElement CorePart
        {
            get
            {
                return _corePart;
            }

            set
            {
                FrameworkElement oldCorePart = _corePart;

                if (oldCorePart != null)
                {
                    oldCorePart.MouseEnter -= new MouseEventHandler(CorePart_MouseEnter);
                    oldCorePart.MouseLeave -= new MouseEventHandler(CorePart_MouseLeave);
                    oldCorePart.MouseLeftButtonDown -= new MouseButtonEventHandler(CorePart_MouseLeftButtonDown);
                    oldCorePart.MouseLeftButtonUp -= new MouseButtonEventHandler(CorePart_MouseLeftButtonUp);
                    oldCorePart.LostMouseCapture -= new MouseEventHandler(CorePart_LostMouseCapture);
                }

                _corePart = value;

                if (_corePart != null)
                {
                    _corePart.MouseEnter += new MouseEventHandler(CorePart_MouseEnter);
                    _corePart.MouseLeave += new MouseEventHandler(CorePart_MouseLeave);
                    _corePart.MouseLeftButtonDown += new MouseButtonEventHandler(CorePart_MouseLeftButtonDown);
                    _corePart.MouseLeftButtonUp += new MouseButtonEventHandler(CorePart_MouseLeftButtonUp);
                    _corePart.LostMouseCapture += new MouseEventHandler(CorePart_LostMouseCapture);
                }
            }
        }

        #endregion

        #region Input

        private void CorePart_MouseEnter(object sender, MouseEventArgs e)
        {
            _isMouseOver = true;
            GoToState(/* useTransitions = */ true);
        }

        private void CorePart_MouseLeave(object sender, MouseEventArgs e)
        {
            _isMouseOver = false;
            GoToState(/* useTransitions = */ true);
        }

        private void CorePart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPressed = true;
        }

        private void CorePart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPressed = false;
        }

        private void CorePart_LostMouseCapture(object sender, MouseEventArgs e)
        {
            IsPressed = false;
        }

        private bool IsPressed
        {
            get { return _isPressed; }

            set
            {
                Debug.Assert(_corePart != null, "IsPressed was set when _corePart was null.");

                if (_isPressed != value)
                {
                    _isPressed = value;

                    if (_isPressed)
                    {
                        Mouse.Capture(_corePart);
                    }
                    else
                    {
                        if (Mouse.Captured == _corePart)
                        {
                            Mouse.Capture(null);
                        }
                    }

                    GoToState(/* useTransitions = */ true);
                }
            }
        }

        #endregion

        #region Weather Properties

        /// <summary>
        ///     DependencyProperty for the Temperature property.
        /// </summary>
        public static readonly DependencyProperty TemperatureProperty = DependencyProperty.Register("Temperature", typeof(string), typeof(WeatherControl), new PropertyMetadata(new PropertyChangedCallback(OnWeatherChange)));

        /// <summary>
        ///     The current temperature.
        /// </summary>
        public string Temperature
        {
            get
            {
                return (string)GetValue(TemperatureProperty);
            }

            set
            {
                SetValue(TemperatureProperty, value);
            }
        }

        /// <summary>
        ///     DependencyProperty for the Condition property.
        /// </summary>
        public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register("Condition", typeof(WeatherCondition), typeof(WeatherControl), new PropertyMetadata(new PropertyChangedCallback(OnWeatherChange)));

        /// <summary>
        ///     The current weather condition.
        /// </summary>
        public WeatherCondition Condition
        {
            get
            {
                return (WeatherCondition)GetValue(ConditionProperty);
            }

            set
            {
                SetValue(ConditionProperty, value);
            }
        }

        /// <summary>
        ///     DependencyProperty for the ConditionDescription property.
        /// </summary>
        public static readonly DependencyProperty ConditionDescriptionProperty = DependencyProperty.Register("ConditionDescription", typeof(string), typeof(WeatherControl), new PropertyMetadata(new PropertyChangedCallback(OnWeatherChange)));

        /// <summary>
        ///     A description of the current condition.
        /// </summary>
        public string ConditionDescription
        {
            get
            {
                return (string)GetValue(ConditionDescriptionProperty);
            }

            set
            {
                SetValue(ConditionDescriptionProperty, value);
            }
        }

        /// <summary>
        ///     Called when a weather property changes.
        /// </summary>
        /// <param name="d">The WeatherControl on which the property changed.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnWeatherChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeatherControl)d).OnWeatherChange(e);
        }

        /// <summary>
        ///     Called when a weather property changed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Base implementation invokes GoToState.
        /// </remarks>
        protected virtual void OnWeatherChange(DependencyPropertyChangedEventArgs e)
        {
            GoToState(/* useTransitions = */ true);
        }

        #endregion

        #region Data

        private FrameworkElement _corePart;
        private bool _isMouseOver;
        private bool _isPressed;

        #endregion
    }
}
