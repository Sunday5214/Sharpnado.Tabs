﻿using Sharpnado.Tasks;

namespace Sharpnado.Tabs;

public class DelayedView<TView> : LazyView<TView>
    where TView : View, new()
{
    public int DelayInMilliseconds { get; set; } = 200;

    public override void LoadView()
    {
        TaskMonitor.Create(
            async () =>
                {
                    View? view = new TView
                    {
                        BindingContext = BindingContext,
                    };

                    await Task.Delay(DelayInMilliseconds);

                    IsLoaded = true;
                    Content = view;
                });
    }

    public override void LoadView(object parentContext)
    {
        TaskMonitor.Create(
           async () =>
           {
               View? view = new TView
               {
                   BindingContext = parentContext,
               };

               await Task.Delay(DelayInMilliseconds);

               IsLoaded = true;
               Content = view;
           });
    }
}

public class DelayedView : ALazyView
{
    public static readonly BindableProperty ViewProperty = BindableProperty.Create(
        nameof(View),
        typeof(View),
        typeof(DelayedView),
        default(View));

    public View View
    {
        get => (View)GetValue(ViewProperty);
        set => SetValue(ViewProperty, value);
    }

    public int DelayInMilliseconds { get; set; } = 200;

    public override void LoadView()
    {
        if (IsLoaded)
        {
            return;
        }

        TaskMonitor.Create(
            async () =>
                {
                    await Task.Delay(DelayInMilliseconds);
                    if (IsLoaded)
                    {
                        return;
                    }

                    IsLoaded = true;
                    Content = View;
                });
    }

    public override void LoadView(object parentContext)
    {
        if (IsLoaded)
        {
            return;
        }

        TaskMonitor.Create(
            async () =>
            {
                await Task.Delay(DelayInMilliseconds);
                if (IsLoaded)
                {
                    return;
                }

                IsLoaded = true;
                Content = View;
            });
    }
}