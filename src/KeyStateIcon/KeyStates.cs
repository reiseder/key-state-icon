namespace KeyStateIcon
{
    [Flags]
    internal enum KeyStates
    {
        None = 0,
        CapsLock = 1,
        NumLock = 2,
        ScrollLock = 4,
        AllOn = CapsLock | NumLock | ScrollLock,
    }
}
