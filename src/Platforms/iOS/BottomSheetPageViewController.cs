﻿using System;
using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace The49.Maui.BottomSheet;

internal class BottomSheetPageContainer : UIView
{
    BottomSheet _page;
    UIView _view;

    internal BottomSheetPageContainer(BottomSheet page, UIView view)
    {
        _page = page;
        _view = view;
        AddSubview(_view);
    }
    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        var r = _page.Measure(_page.Window.Width, _page.Window.Height);
        _view.Frame = new CGRect(0, 0, Bounds.Width, r.Request.Height);
    }
}
public class BottomSheetPageViewController : UIViewController
{
    IMauiContext _windowMauiContext;
    BottomSheet _sheet;

    public BottomSheetPageViewController(IMauiContext windowMauiContext, BottomSheet page) : base()
    {
        _windowMauiContext = windowMauiContext;
        _sheet = page;
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var container = _sheet.ToPlatform(_windowMauiContext);

        var cv = new BottomSheetPageContainer(_sheet, container);

        View = cv;
        if (_sheet.BackgroundBrush != null)
        {
            Paint paint = _sheet.BackgroundBrush;
            View.BackgroundColor = paint.ToColor().ToPlatform();
        }
        else
        {
            View.BackgroundColor = UIColor.SystemBackground;
        }
    }

    public override void ViewWillDisappear(bool animated)
    {
        base.ViewWillDisappear(animated);
        if (IsBeingDismissed)
        {
            _sheet.NotifyDismissed();
        }
    }
    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();
        SheetPresentationController.InvalidateDetents();
    }
}

