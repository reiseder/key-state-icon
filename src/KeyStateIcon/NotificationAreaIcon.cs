using H.NotifyIcon.Core;

namespace KeyStateIcon;

internal sealed class NotificationAreaIcon : IDisposable
{
    private readonly TrayIcon _trayIcon;
    private readonly System.Timers.Timer _timer;

    public NotificationAreaIcon()
    {
        _trayIcon = new TrayIconWithContextMenu
        {
            Icon = Icons.all_on.Handle,
            ToolTip = "Showing the current state of Caps, Num and Scroll lock",
            ContextMenu = new()
            {
                Items =
                {
                    new PopupMenuItem("Exit", (_, _) =>
                    {
                        Dispose();
                        Environment.Exit(0);
                    }),
                }
            }
        };

        _timer = new System.Timers.Timer(200);
        _timer.Elapsed += (_, _) => UpdateIcon((int)GetCurrentKeyStates());
        _timer.Start();
    }

    public void Create()
    {
        _trayIcon.Create();
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
        _trayIcon.Dispose();
    }

    private static KeyStates GetCurrentKeyStates()
    {
        var state = KeyStates.None;

        if (Control.IsKeyLocked(Keys.Capital) || Control.IsKeyLocked(Keys.CapsLock))
            state |= KeyStates.CapsLock;

        if (Control.IsKeyLocked(Keys.NumLock))
            state |= KeyStates.NumLock;

        if (Control.IsKeyLocked(Keys.Scroll))
            state |= KeyStates.ScrollLock;

        return state;
    }

    private void UpdateIcon(int state)
    {
        _trayIcon.UpdateIcon(state switch
        {
            0 => Icons.all_off.Handle,
            1 => Icons.caps_on.Handle,
            2 => Icons.num_on.Handle,
            3 => Icons.caps_num_on.Handle,
            4 => Icons.scroll_on.Handle,
            5 => Icons.caps_scroll_on.Handle,
            6 => Icons.num_scroll_on.Handle,
            7 => Icons.all_on.Handle,
            _ => throw new NotSupportedException($"State '{state}' is not supported!")
        });
    }
}
