// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Behaviors;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Xaml.Interactivity;

using Taskbar_Customizer.Core.Contracts.Services.Navigation;

/// <summary>
/// Behavior that enhances a <see cref="NavigationView"/> by managing the header based on attached properties of <see cref="Page"/> elements.
/// </summary>
public class NavigationViewHeaderBehavior : Behavior<NavigationView>
{
    private static NavigationViewHeaderBehavior? current;

    private Page? currentPage;

    /// <summary>
    /// Gets or sets the default data template for the header.
    /// </summary>
    public DataTemplate? DefaultHeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the default header object.
    /// </summary>
    public object DefaultHeader
    {
        get => this.GetValue(DefaultHeaderProperty);
        set => this.SetValue(DefaultHeaderProperty, value);
    }

    /// <summary>
    /// Default Header Property.
    /// </summary>
    public static readonly DependencyProperty DefaultHeaderProperty =
        DependencyProperty.Register("DefaultHeader", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => current!.UpdateHeader()));

    /// <summary>
    /// Gets the header mode from the attached property of a <see cref="Page"/>.
    /// </summary>
    /// <param name="item">Page.</param>
    /// <returns>Header Mode.</returns>
    public static NavigationViewHeaderMode GetHeaderMode(Page item) => (NavigationViewHeaderMode)item.GetValue(HeaderModeProperty);

    /// <summary>
    /// Sets the header mode to the attached property of a <see cref="Page"/>.
    /// </summary>
    /// <param name="item">Page.</param>
    /// <param name="value">Header Mode.</param>
    public static void SetHeaderMode(Page item, NavigationViewHeaderMode value) => item.SetValue(HeaderModeProperty, value);

    /// <summary>
    /// Header Mode Property.
    /// </summary>
    public static readonly DependencyProperty HeaderModeProperty =
        DependencyProperty.RegisterAttached("HeaderMode", typeof(bool), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(NavigationViewHeaderMode.Always, (d, e) => current!.UpdateHeader()));

    /// <summary>
    /// Gets the header context from the attached property of a <see cref="Page"/>.
    /// </summary>
    /// <param name="item">Page.</param>
    /// <returns>Header context.</returns>
    public static object GetHeaderContext(Page item) => item.GetValue(HeaderContextProperty);

    /// <summary>
    /// Sets the header context to the attached property of a <see cref="Page"/>.
    /// </summary>
    /// <param name="item">Page.</param>
    /// <param name="value">Header context.</param>
    public static void SetHeaderContext(Page item, object value) => item.SetValue(HeaderContextProperty, value);

    /// <summary>
    /// Header Context Property.
    /// </summary>
    public static readonly DependencyProperty HeaderContextProperty =
        DependencyProperty.RegisterAttached("HeaderContext", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => current!.UpdateHeader()));

    /// <summary>
    /// Gets the header template from the attached property of a <see cref="Page"/>.
    /// </summary>
    /// <param name="item">Page.</param>
    /// <returns>Header template.</returns>
    public static DataTemplate GetHeaderTemplate(Page item) => (DataTemplate)item.GetValue(HeaderTemplateProperty);

    /// <summary>
    /// Sets the header template to the attached property of a <see cref="Page"/>.
    /// </summary>
    /// <param name="item">Page.</param>
    /// <param name="value">Header template.</param>
    public static void SetHeaderTemplate(Page item, DataTemplate value) => item.SetValue(HeaderTemplateProperty, value);

    /// <summary>
    /// Header Template Property.
    /// </summary>
    public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => current!.UpdateHeaderTemplate()));

    protected override void OnAttached()
    {
        base.OnAttached();

        var navigationService = App.GetService<INavigationService>();
        navigationService.Navigated += this.OnNavigated;

        current = this;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        var navigationService = App.GetService<INavigationService>();
        navigationService.Navigated -= this.OnNavigated;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame && frame.Content is Page page)
        {
            this.currentPage = page;
            this.UpdateHeader();
            this.UpdateHeaderTemplate();
        }
    }

    private void UpdateHeader()
    {
        if (this.currentPage is not null)
        {
            var headerMode = GetHeaderMode(this.currentPage);
            if (headerMode == NavigationViewHeaderMode.Never)
            {
                this.AssociatedObject.Header = null;
                this.AssociatedObject.AlwaysShowHeader = false;
            }
            else
            {
                var headerFromPage = GetHeaderContext(this.currentPage);
                this.AssociatedObject.Header = headerFromPage ?? this.DefaultHeader;

                this.AssociatedObject.AlwaysShowHeader = headerMode == NavigationViewHeaderMode.Always;
            }
        }
    }

    private void UpdateHeaderTemplate()
    {
        if (this.currentPage is not null)
        {
            var headerTemplate = GetHeaderTemplate(this.currentPage);
            this.AssociatedObject.HeaderTemplate = headerTemplate ?? this.DefaultHeaderTemplate;
        }
    }
}