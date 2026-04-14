using System;
using System.Drawing;
using System.Windows.Forms;

internal sealed class ToastNotifier : IDisposable
{
    private readonly NotifyIcon notifyIcon;

    public ToastNotifier(Icon icon)
    {
        notifyIcon = new NotifyIcon();
        notifyIcon.Icon = icon;
        notifyIcon.Visible = true;
    }

    public void ShowInfo(string title, string message)
    {
        notifyIcon.BalloonTipTitle = title;
        notifyIcon.BalloonTipText = message;
        notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
        notifyIcon.ShowBalloonTip(2500);
    }

    public void Dispose()
    {
        notifyIcon.Visible = false;
        notifyIcon.Dispose();
    }
}
