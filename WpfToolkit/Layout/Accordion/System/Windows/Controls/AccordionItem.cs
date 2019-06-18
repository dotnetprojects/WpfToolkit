// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.Globalization;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Controls
{
    /// <summary>
    /// Represents a control that displays a header and has a collapsible 
    /// content window.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplateVisualState(Name = VisualStates.StateNormal, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateMouseOver, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StatePressed, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateDisabled, GroupName = VisualStates.GroupCommon)]

    [TemplateVisualState(Name = VisualStates.StateFocused, GroupName = VisualStates.GroupFocus)]
    [TemplateVisualState(Name = VisualStates.StateUnfocused, GroupName = VisualStates.GroupFocus)]

    [TemplateVisualState(Name = VisualStates.StateExpanded, GroupName = VisualStates.GroupExpansion)]
    [TemplateVisualState(Name = VisualStates.StateCollapsed, GroupName = VisualStates.GroupExpansion)]

    [TemplateVisualState(Name = VisualStates.StateLocked, GroupName = VisualStates.GroupLocked)]
    [TemplateVisualState(Name = VisualStates.StateUnlocked, GroupName = VisualStates.GroupLocked)]

    [TemplateVisualState(Name = VisualStates.StateExpandDown, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandUp, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandLeft, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandRight, GroupName = VisualStates.GroupExpandDirection)]

    [TemplatePart(Name = ElementExpandSiteName, Type = typeof(ExpandableContentControl))]
    [TemplatePart(Name = ElementExpanderButtonName, Type = typeof(AccordionButton))]

    [StyleTypedProperty(Property = "AccordionButtonStyle", StyleTargetType = typeof(AccordionButton))]
    [StyleTypedProperty(Property = "ExpandableContentControlStyle", StyleTargetType = typeof(ExpandableContentControl))]
    public class AccordionItem : HeaderedContentControl, IUpdateVisualState
    {
        #region Template Parts
        /// <summary>
        /// The name of the ExpanderButton template part.
        /// </summary>
        private const string ElementExpanderButtonName = "ExpanderButton";

        /// <summary>
        /// The name of the ExpandSite template part.
        /// </summary>
        private const string ElementExpandSiteName = "ExpandSite";

        /// <summary>
        /// The ExpanderButton template part is a templated ToggleButton that's 
        /// used to select and unselect this AccordionItem.
        /// </summary>
        private AccordionButton _expanderButton;

        /// <summary>
        /// Gets or sets the ExpanderButton template part.
        /// </summary>
        private AccordionButton ExpanderButton
        {
            get { return _expanderButton; }
            set
            {
                // Detach from old ExpanderButton
                if (_expanderButton != null)
                {
                    _expanderButton.Click -= OnExpanderButtonClicked;
                    _expanderButton.ParentAccordionItem = null;
                    _expanderButton.SizeChanged -= OnHeaderSizeChanged;
                }

                _expanderButton = value;

                if (_expanderButton != null)
                {
                    _expanderButton.IsChecked = IsSelected;
                    _expanderButton.Click += OnExpanderButtonClicked;
                    _expanderButton.ParentAccordionItem = this;
                    _expanderButton.SizeChanged += OnHeaderSizeChanged;
                }
            }
        }

        /// <summary>
        /// BackingField for the ExpandSite property.
        /// </summary>
        private ExpandableContentControl _expandSite;

        /// <summary>
        /// Gets or sets the expand site template part.
        /// </summary>
        private ExpandableContentControl ExpandSite
        {
            get
            {
                return _expandSite;
            }
            set
            {
                _expandSite = value;
            }
        }

        #endregion

        #region public ExpandDirection ExpandDirection
        /// <summary>
        /// Determines whether the ExpandDirection property may be written.
        /// </summary>
        private bool _allowedToWriteExpandDirection;

        /// <summary>
        /// Gets the direction in which the AccordionItem content window opens.
        /// </summary>
        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            protected internal set
            {
                _allowedToWriteExpandDirection = true;
                SetValue(ExpandDirectionProperty, value);
                _allowedToWriteExpandDirection = false;
            }
        }

        /// <summary>
        /// Identifies the ExpandDirection dependency property. 
        /// </summary>
        public static readonly DependencyProperty ExpandDirectionProperty =
                DependencyProperty.Register(
                        "ExpandDirection",
                        typeof(ExpandDirection),
                        typeof(AccordionItem),
                        new PropertyMetadata(ExpandDirection.Down, OnExpandDirectionPropertyChanged));

        /// <summary>
        /// ExpandDirectionProperty PropertyChangedCallback call back static 
        /// function.
        /// This function validates the new value before calling virtual function 
        /// OnExpandDirectionChanged.
        /// </summary>
        /// <param name="d">Expander object whose ExpandDirection property is 
        /// changed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs which contains 
        /// the old and new values.</param>
        private static void OnExpandDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem ctrl = (AccordionItem)d;
            ExpandDirection oldValue = (ExpandDirection)e.OldValue;
            ExpandDirection newValue = (ExpandDirection)e.NewValue;
            
            if (!ctrl._allowedToWriteExpandDirection)
            {
                // revert to old value
                ctrl.ExpandDirection = oldValue;

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    Properties.Resources.AccordionItem_InvalidWriteToExpandDirection,
                    newValue);
                throw new InvalidOperationException(message);
            }

            // invalid value. This check is not of great importance anymore since 
            // the previous check should catch all invalid sets.
            if (newValue != ExpandDirection.Down &&
                newValue != ExpandDirection.Left &&
                newValue != ExpandDirection.Right &&
                newValue != ExpandDirection.Up)
            {
                // revert to old value
                ctrl.ExpandDirection = oldValue;

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    Properties.Resources.Expander_OnExpandDirectionPropertyChanged_InvalidValue,
                    newValue);
                throw new ArgumentException(message, "e");
            }

            if (ctrl.ExpandSite != null)
            {
                // Jump to correct percentage after a direction change
#if SILVERLIGHT
                ctrl.ExpandSite.Percentage = ctrl.IsSelected ? 1 : 0;
#else
 //               ctrl.ExpandSite.RecalculatePercentage(ctrl.IsSelected ? 1 : 0);
#endif
            }

            ctrl.UpdateVisualState(true);
        }
        #endregion public ExpandDirection ExpandDirection

        #region public bool IsSelected
        /// <summary>
        /// Gets or sets a value indicating whether the AccordionItem is 
        /// selected and its content window is visible.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
                DependencyProperty.Register(
                        "IsSelected",
                        typeof(bool),
                        typeof(AccordionItem),
                        new PropertyMetadata(OnIsSelectedPropertyChanged));

        /// <summary>
        /// SelectedProperty PropertyChangedCallback static function.
        /// </summary>
        /// <param name="d">Expander object whose Expanded property is changed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs which contains the 
        /// old and new values.</param>
        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem ctrl = (AccordionItem) d;
            bool isSelected = (bool) e.NewValue;

            // Not allowed to change the IsSelected state when locked.
            if (ctrl.IsLocked && ctrl._isSelectedNestedLevel == 0)
            {
                ctrl._isSelectedNestedLevel++;
                ctrl.SetValue(IsSelectedProperty, e.OldValue);
                ctrl._isSelectedNestedLevel--;

                throw new InvalidOperationException(Properties.Resources.AccordionItem_OnIsSelectedPropertyChanged_InvalidChange);
            }

            if (ctrl._isSelectedNestedLevel == 0)
            {
                Accordion parent = ctrl.ParentAccordion;
                if (parent != null)
                {
                    if (isSelected)
                    {
                        parent.OnAccordionItemSelected(ctrl);
                    }
                    else
                    {
                        parent.OnAccordionItemUnselected(ctrl);
                    }
                }

                if (isSelected)
                {
                    ctrl.OnSelected();
                }
                else
                {
                    ctrl.OnUnselected();
                }
            }
        }

        /// <summary>
        /// Nested level for IsSelectedCoercion.
        /// </summary>
        private int _isSelectedNestedLevel;
        #endregion public bool IsSelected

        #region public bool IsLocked

        /// <summary>
        /// Gets a value indicating whether the AccordionItem cannot be 
        /// selected by the user.
        /// </summary>
        /// <value><c>True</c> if this instance is locked; otherwise, <c>false</c>.</value>
        /// <remarks>The IsSelected property may not be changed when the 
        /// AccordionItem is locked. Locking occurs when the item is the first 
        /// in the list, the SelectionMode of Accordion requires atleast one selected
        /// AccordionItem and the AccordionItem is currently selected.</remarks>
        public bool IsLocked
        {
            get { return _isLocked; }
            internal set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;

                    UpdateVisualState(true);
                }
            }
        }
        
        /// <summary>
        /// BackingField for IsLocked.
        /// </summary>
        private bool _isLocked;

        #endregion public bool IsLocked

        #region public Style AccordionButtonStyle
        /// <summary>
        /// Gets or sets the Style used by AccordionButton.
        /// </summary>
        public Style AccordionButtonStyle
        {
            get { return GetValue(AccordionButtonStyleProperty) as Style; }
            set { SetValue(AccordionButtonStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the AccordionButtonStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty AccordionButtonStyleProperty =
            DependencyProperty.Register(
                "AccordionButtonStyle",
                typeof(Style),
                typeof(AccordionItem),
                new PropertyMetadata(OnAccordionButtonStylePropertyChanged));

        /// <summary>
        /// AccordionButtonStyleProperty property changed handler.
        /// </summary>
        /// <param name="d">AccordionItem that changed its AccordionButtonStyle.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnAccordionButtonStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem source = (AccordionItem) d;
            source.OnAccordionButtonStyleChanged(e.OldValue as Style, e.NewValue as Style);
        }

        /// <summary>
        /// Called when AccordionButtonStyle is changed.
        /// </summary>
        /// <param name="oldStyle">The old style.</param>
        /// <param name="newStyle">The new style.</param>
        protected virtual void OnAccordionButtonStyleChanged(Style oldStyle, Style newStyle)
        {
        }
        #endregion public Style AccordionButtonStyle

        #region public Style ExpandableContentControlStyle
        /// <summary>
        /// Gets or sets the Style used by ExpandableContentControl.
        /// </summary>
        public Style ExpandableContentControlStyle
        {
            get { return GetValue(ExpandableContentControlStyleProperty) as Style; }
            set { SetValue(ExpandableContentControlStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the ExpandableContentControlStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpandableContentControlStyleProperty =
            DependencyProperty.Register(
                "ExpandableContentControlStyle",
                typeof(Style),
                typeof(AccordionItem),
                new PropertyMetadata(OnExpandableContentControlStylePropertyChanged));

        /// <summary>
        /// ExpandableContentControlStyleProperty property changed handler.
        /// </summary>
        /// <param name="d">AccordionItem that changed its ExpandableContentControlStyle.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnExpandableContentControlStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem source = (AccordionItem)d;
            source.OnExpandableContentControlStyleChanged(e.OldValue as Style, e.NewValue as Style);
        }

        /// <summary>
        /// Called when ExpandableContentControlStyle is changed.
        /// </summary>
        /// <param name="oldStyle">The old style.</param>
        /// <param name="newStyle">The new style.</param>
        protected virtual void OnExpandableContentControlStyleChanged(Style oldStyle, Style newStyle)
        {
        }
        #endregion public Style ExpandableContentControlStyle

        #region public Size ContentTargetSize
        /// <summary>
        /// Determines whether it is allowed to set the ContentTargetSize
        /// property.
        /// </summary>
        private bool _allowedToWriteContentTargetSize;

        /// <summary>
        /// Gets the Size that the content will animate to.
        /// </summary>
        public Size ContentTargetSize
        {
            get { return (Size)GetValue(ContentTargetSizeProperty); }
            internal set
            {
                _allowedToWriteContentTargetSize = true;
                SetValue(ContentTargetSizeProperty, value);
                _allowedToWriteContentTargetSize = false;
            }
        }

        /// <summary>
        /// Identifies the ContentTargetSize dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTargetSizeProperty =
            DependencyProperty.Register(
                "ContentTargetSize",
                typeof(Size),
                typeof(AccordionItem),
                new PropertyMetadata(new Size(double.NaN, double.NaN), OnContentTargetSizePropertyChanged));

        /// <summary>
        /// ContentTargetSizeProperty property changed handler.
        /// </summary>
        /// <param name="d">AccordionItem that changed its ContentTargetSize.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnContentTargetSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem source = (AccordionItem)d;
            Size targetSize = (Size)e.NewValue;

            if (!source._allowedToWriteContentTargetSize)
            {
                // revert to old value
                source.ContentTargetSize = (Size)e.OldValue;

                throw new InvalidOperationException(Properties.Resources.AccordionItem_InvalidWriteToContentTargetSize);
            }
        }
        #endregion public Size ContentTargetSize

        #region Accordion ParentAccordion
        /// <summary>
        /// Gets or sets a reference to the parent Accordion of an
        /// AccordionItem.
        /// </summary>
        internal Accordion ParentAccordion { get; set; }
        #endregion Accordion ParentAccordion

        /// <summary>
        /// Gets the scheduled action.
        /// </summary>
        /// <value>The scheduled action.</value>
        internal AccordionAction ScheduledAction { get; private set; }

        /// <summary>
        /// Occurs when the accordionItem is selected.
        /// </summary>
        public static RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AccordionItem));

        /// <summary>
        /// Occurs when the accordionItem is selected.
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        /// <summary>
        /// Occurs when the accordionItem is unselected.
        /// </summary>
        public static RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AccordionItem));

        /// <summary>
        /// Occurs when the accordionItem is unselected.
        /// </summary>
        public event RoutedEventHandler Unselected
        {
            add { AddHandler(UnselectedEvent, value); }
            remove { RemoveHandler(UnselectedEvent, value); }
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static AccordionItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccordionItem), new FrameworkPropertyMetadata(typeof(AccordionItem)));
        }

        /// <summary>
        /// Initializes a new instance of the AccordionItem class.
        /// </summary>
        public AccordionItem()
        {
            // initialize to no action.
            ScheduledAction = AccordionAction.None;

#if SILVERLIGHT
            DefaultStyleKey = typeof(AccordionItem);
#endif
            _interaction = new InteractionHelper(this);
        }

        #region Layout

        /// <summary>
        /// Called when the size of the control changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnHeaderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // allow the parent to reschedule a layout pass.
            if (ParentAccordion != null)
            {
                ParentAccordion.OnHeaderSizeChange(this);
            }
        }

        /// <summary>
        /// Gets the relevant size of the current content.
        /// </summary>
        internal Size RelevantContentSize
        {
            get { return ExpandSite == null ? new Size(0, 0) : ExpandSite.RelevantContentSize; }
        }

        /// <summary>
        /// Schedules the specified action.
        /// </summary>
        /// <param name="action">The action to be performed.</param>
        private void Schedule(AccordionAction action)
        {
#if SILVERLIGHT
            if (DesignerProperties.GetIsInDesignMode(this) && ExpandSite != null)
            {
                switch (action)
                {
                    case AccordionAction.None:
                        break;
                    case AccordionAction.Collapse:
                        ExpandSite.Percentage = 0;
                        break;
                    case AccordionAction.Expand:
                    case AccordionAction.Resize:
                        ExpandSite.Percentage = 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("action");
                }
                return;
            }
#endif

            ScheduledAction = action;

            if (ParentAccordion == null)
            {
                // no parentaccordion to notify, so just execute.
                StartAction();
            }
            else
            {
                bool directExecute = ParentAccordion.ScheduleAction(this, action);
                if (directExecute)
                {
                    StartAction();
                }
            }
        }

        /// <summary>
        /// Starts an action, such as resize, collapse or expand.
        /// </summary>
        internal virtual void StartAction()
        {
            if (ScheduledAction == AccordionAction.None)
            {
                throw new InvalidOperationException(Properties.Resources.AccordionItem_StartAction_InvalidCall);
            }
            Action layoutAction;

            switch (ScheduledAction)
            {
                case AccordionAction.Collapse:
                    layoutAction = () =>
                    {
                        VisualStateManager.GoToState(this, VisualStates.StateExpanded, false);
                        // We only want to notify the parent that this action is finished when we are done with
                        // the state transition. In SL this is done through OnStoryboardFinished, but the same code
                        // in WPF would result in a corrupted state because OnStoryboardFinished gets called more frequently than expected.
                        // In the WPF case, we make the scheduled action synchornous to ensure the transition is properly done.
#if SILVERLIGHT
                        VisualStateManager.GoToState(this, VisualStates.StateCollapsed, true);
#else
                        if (VisualStateManager.GoToState(this, VisualStates.StateCollapsed, true))
                        {
                            ParentAccordion.OnActionFinish(this);
                        }
#endif
                    };
                    break;
                case AccordionAction.Expand:
                    layoutAction = () =>
                    {
                        VisualStateManager.GoToState(this, VisualStates.StateCollapsed, false);
#if SILVERLIGHT
                        VisualStateManager.GoToState(this, VisualStates.StateExpanded, true);
#else
                        if (VisualStateManager.GoToState(this, VisualStates.StateExpanded, true))
                        {
                            ParentAccordion.OnActionFinish(this);
                        }
#endif
                    };
                    break;
                case AccordionAction.Resize:
                    layoutAction = () =>
                    {
                        // trigger ExpandedState to run again, by quickly moving to collapsed. 
                        // the effect is not noticeable because no layout pass is done.
                        VisualStateManager.GoToState(this, VisualStates.StateExpanded, false);
                        VisualStateManager.GoToState(this, VisualStates.StateCollapsed, false);
#if SILVERLIGHT
                        VisualStateManager.GoToState(this, VisualStates.StateExpanded, true);
#else
                        if (VisualStateManager.GoToState(this, VisualStates.StateExpanded, true))
                        {
                            ParentAccordion.OnActionFinish(this);
                        }
#endif
                    };
                    break;
                default:
                    {
                        string message = string.Format(
                            CultureInfo.InvariantCulture,
                            Properties.Resources.AccordionItem_StartAction_InvalidAction,
                            ScheduledAction);

                        throw new NotSupportedException(message);
                    }
            }

            ScheduledAction = AccordionAction.None;
            layoutAction();
        }

#if SILVERLIGHT
        /// <summary>
        /// Called when a storyboard finishes.
        /// </summary>
        /// <param name="sender">The AccordionItem that finished a storyboard.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing 
        /// the event data.</param>
        /// <remarks>AccordionItem is required to make this call.</remarks>
        private void OnStoryboardFinished(object sender, EventArgs e)
        {
            _isBusyWithAction = false;

            if (ParentAccordion != null)
            {
                ParentAccordion.OnActionFinish(this);
            }
        }
#endif

        /// <summary>
        /// Gets a value indicating whether the AccordionItem fills width.
        /// </summary>
        private bool ShouldFillWidth
        {
            get
            {
                return (ExpandDirection == ExpandDirection.Left || ExpandDirection == ExpandDirection.Right) &&
                       (!Double.IsNaN(ContentTargetSize.Width) || HorizontalAlignment == HorizontalAlignment.Stretch);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the AccordionItem fills height.
        /// </summary>
        private bool ShouldFillHeight
        {
            get
            {
                return (ExpandDirection == ExpandDirection.Down || ExpandDirection == ExpandDirection.Up) &&
                       (!Double.IsNaN(ContentTargetSize.Height) || VerticalAlignment == VerticalAlignment.Stretch);
            }
        }
        #endregion Layout

        /// <summary>
        /// Builds the visual tree for the AccordionItem control when a new 
        /// template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ExpanderButton = GetTemplateChild(ElementExpanderButtonName) as AccordionButton;
            ExpandSite = GetTemplateChild(ElementExpandSiteName) as ExpandableContentControl;

            if (VisualTreeHelper.GetChildrenCount(this) > 0)
            {
                FrameworkElement root = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;

#if SILVERLIGHT
                if (root != null)
                {
                    ExpandStoryboard = (from stategroup in
                                            (VisualStateManager.GetVisualStateGroups(root) as
                                             Collection<VisualStateGroup>)
                                        where stategroup.Name == VisualStates.GroupExpansion
                                        from state in (stategroup.States as Collection<VisualState>)
                                        where state.Name == VisualStates.StateExpanded
                                        select state.Storyboard).FirstOrDefault();

                    CollapseStoryboard = (from stategroup in
                                              (VisualStateManager.GetVisualStateGroups(root) as
                                               Collection<VisualStateGroup>)
                                          where stategroup.Name == VisualStates.GroupExpansion
                                          from state in (stategroup.States as Collection<VisualState>)
                                          where state.Name == VisualStates.StateCollapsed
                                          select state.Storyboard).FirstOrDefault();
                }
                else
                {
                    ExpandStoryboard = null;
                    CollapseStoryboard = null;
                }
#endif

            }

            _interaction.OnApplyTemplateBase();

            UpdateVisualState(false);

            // the UpdateVisualState will not set the expand or collapse state.
            if (IsSelected)
            {
                Schedule(AccordionAction.Expand);
            }
            else
            {
                Schedule(AccordionAction.Collapse);
            }
        }

        /// <summary>
        /// Returns a AccordionItemAutomationPeer for use by the Silverlight
        /// automation infrastructure.
        /// </summary>
        /// <returns>A AccordionItemAutomationPeer object for the AccordionItem.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
#if SILVERLIGHT
            return new AccordionItemAutomationPeer(this);
#else
            return new AccordionItemWrapperAutomationPeer(this);
#endif
        }

        /// <summary>
        /// Prepares the specified container to display the specified item.
        /// </summary>
        /// <param name="element">
        /// Container element used to display the specified item.
        /// </param>
        /// <param name="item">Specified item to display.</param>
        /// <param name="parent">The parent ItemsControl.</param>
        /// <param name="parentItemContainerStyle">
        /// The ItemContainerStyle for the parent ItemsControl.
        /// </param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parent", Justification = "Following ItemsControl signature.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parentItemContainerStyle", Justification = "Following ItemsControl signature.")]
        internal static void PreparePrepareHeaderedContentControlContainerForItemOverride(HeaderedContentControl element, object item, ItemsControl parent, Style parentItemContainerStyle)
        {
            if (element != item)
            {
                // We do not have proper access to Visual.
                // Nor do we keep track of the HeaderIsItem property.
                if (!(item is UIElement) && HasDefaultValue(element, HeaderProperty))
                {
                    element.Header = item;
                }
            }
        }

        /// <summary>
        /// Check whether a control has the default value for a property.
        /// </summary>
        /// <param name="control">The control to check.</param>
        /// <param name="property">The property to check.</param>
        /// <returns>
        /// True if the property has the default value; false otherwise.
        /// </returns>
        private static bool HasDefaultValue(Control control, DependencyProperty property)
        {
            Debug.Assert(control != null, "control should not be null!");
            Debug.Assert(property != null, "property should not be null!");
            return control.ReadLocalValue(property) == DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Provides handling for the KeyDown event.
        /// </summary>
        /// <param name="e">Key event args.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Handled || !IsEnabled || IsLocked)
            {
                return;
            }

            bool isSelected = IsSelected;
            switch (ExpandDirection)
            {
                case ExpandDirection.Down:
                    if ((isSelected && e.Key == Key.Up) || (!isSelected && e.Key == Key.Down))
                    {
                        IsSelected = !isSelected;
                    }
                    break;
                case ExpandDirection.Up:
                    if ((isSelected && e.Key == Key.Down) || (!isSelected && e.Key == Key.Up))
                    {
                        IsSelected = !isSelected;
                    }
                    break;
                case ExpandDirection.Left:
                    if ((isSelected && e.Key == Key.Right) || (!isSelected && e.Key == Key.Left))
                    {
                        IsSelected = !isSelected;
                    }
                    break;
                case ExpandDirection.Right:
                    if ((isSelected && e.Key == Key.Left) || (!isSelected && e.Key == Key.Right))
                    {
                        IsSelected = !isSelected;
                    }
                    break;
            }
        }

        /// <summary>
        /// Raises the Selected event when the IsSelected property changes 
        /// from false to true.
        /// </summary>
        protected virtual void OnSelected()
        {
#if SILVERLIGHT
            ToggleSelected(Selected, new RoutedEventArgs());
#else
            ToggleSelected(new RoutedEventArgs(SelectedEvent));
#endif
        }

        /// <summary>
        /// Raises the Unselected event when the IsSelected property changes 
        /// from true to false.
        /// </summary>
        protected virtual void OnUnselected()
        {
#if SILVERLIGHT
            ToggleSelected(Unselected, new RoutedEventArgs());
#else
            ToggleSelected(new RoutedEventArgs(UnselectedEvent));
#endif
        }

#if SILVERLIGHT
        /// <summary>
        /// Handle changes to the IsSelected property.
        /// </summary>
        /// <param name="handler">Event handler.</param>
        /// <param name="args">Event arguments.</param>
        private void ToggleSelected(RoutedEventHandler handler, RoutedEventArgs args)
        {
#else
        /// <summary>
        /// Handle changes to the IsSelected property.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        private void ToggleSelected(RoutedEventArgs args)
        {
#endif
            ToggleButton expander = ExpanderButton;
            if (expander != null)
            {
                expander.IsChecked = IsSelected;
            }

            if (IsSelected)
            {
                Schedule(AccordionAction.Expand);
            }
            else
            {
                Schedule(AccordionAction.Collapse);
            }

            UpdateVisualState(true);
#if SILVERLIGHT
            RaiseEvent(handler, args);
#else
            // WPF has a slightly different mechanism for raising routed events
            RaiseEvent(args);
#endif
        }

#if SILVERLIGHT
        /// <summary>
        /// Raise a RoutedEvent.
        /// </summary>
        /// <param name="handler">Event handler.</param>
        /// <param name="args">Event arguments.</param>
        private void RaiseEvent(RoutedEventHandler handler, RoutedEventArgs args)
        {
            if (handler != null)
            {
                handler(this, args);
            }
        }
#endif

        /// <summary>
        /// Handle ExpanderButton's click event.
        /// </summary>
        /// <param name="sender">The ExpanderButton in template.</param>
        /// <param name="e">Routed event arg.</param>
        private void OnExpanderButtonClicked(object sender, RoutedEventArgs e)
        {
            // If the item is locked, the item is not permitted to change its selection state
            if (!IsLocked)
            {
                IsSelected = !IsSelected;
            }
        }

        #region Visual state management

        /// <summary>
        /// Gets or sets the helper that provides all of the standard
        /// interaction functionality.
        /// </summary>
        private InteractionHelper _interaction;

        /// <summary>
        /// Provides handling for the GotFocus event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (_interaction.AllowGotFocus(e))
            {
                _interaction.OnGotFocusBase();
                base.OnGotFocus(e);
            }
        }

        /// <summary>
        /// Provides handling for the LostFocus event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (_interaction.AllowLostFocus(e))
            {
                _interaction.OnLostFocusBase();
                base.OnLostFocus(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseEnter event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (_interaction.AllowMouseEnter(e))
            {
                _interaction.OnMouseEnterBase();
                base.OnMouseEnter(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseLeave event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (_interaction.AllowMouseLeave(e))
            {
                _interaction.OnMouseLeaveBase();
                base.OnMouseLeave(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseLeftButtonDown event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (_interaction.AllowMouseLeftButtonDown(e))
            {
                _interaction.OnMouseLeftButtonDownBase();
                base.OnMouseLeftButtonDown(e);
            }
        }

        /// <summary>
        /// Called before the MouseLeftButtonUp event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_interaction.AllowMouseLeftButtonUp(e))
            {
                _interaction.OnMouseLeftButtonUpBase();
                base.OnMouseLeftButtonUp(e);
            }
        }

        /// <summary>
        /// Update the visual state of the control.
        /// </summary>
        /// <param name="useTransitions">
        /// A value indicating whether to automatically generate transitions to
        /// the new state, or instantly transition to the new state.
        /// </param>
        void IUpdateVisualState.UpdateVisualState(bool useTransitions)
        {
            UpdateVisualState(useTransitions);
        }

        /// <summary>
        /// Update the current visual state of the button.
        /// </summary>
        /// <param name="useTransitions">
        /// True to use transitions when updating the visual state, false to
        /// snap directly to the new visual state.
        /// </param>
        internal virtual void UpdateVisualState(bool useTransitions)
        {
            if (IsLocked)
            {
                VisualStates.GoToState(this, useTransitions, VisualStates.StateLocked);
            }
            else
            {
                VisualStates.GoToState(this, useTransitions, VisualStates.StateUnlocked);
            }

            switch (ExpandDirection)
            {
                case ExpandDirection.Down:
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateExpandDown);
                    break;

                case ExpandDirection.Up:
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateExpandUp);
                    break;

                case ExpandDirection.Left:
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateExpandLeft);
                    break;

                default:
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateExpandRight);
                    break;
            }

            // let the header know a change has possibly occured.
            if (ExpanderButton != null)
            {
                ExpanderButton.UpdateVisualState(useTransitions);
            }

            // Handle the Common and Focused states
            _interaction.UpdateVisualStateBase(useTransitions);
        }
        #endregion
    }
}
