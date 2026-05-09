using System;
using System.Collections.Generic;
using System.Timers;

namespace ThanhCaV2.Blazor.Services;

public enum ToastType
{
    Success,
    Info,
    Warning,
    Error
}

public class ToastMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = string.Empty;
    public ToastType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class ToastService
{
    public event Action? OnChange;
    private readonly List<ToastMessage> _toasts = new();
    private const int DefaultTimeout = 3000;

    public List<ToastMessage> GetToasts() => _toasts;

    public void ShowSuccess(string message) => Show(message, ToastType.Success);
    public void ShowInfo(string message) => Show(message, ToastType.Info);
    public void ShowWarning(string message) => Show(message, ToastType.Warning);
    public void ShowError(string message) => Show(message, ToastType.Error);

    public void Show(string message, ToastType type)
    {
        var toast = new ToastMessage
        {
            Message = message,
            Type = type
        };

        _toasts.Add(toast);
        NotifyStateChanged();

        var timer = new System.Timers.Timer(DefaultTimeout);
        timer.Elapsed += (s, e) => {
            RemoveToast(toast.Id);
            timer.Stop();
            timer.Dispose();
        };
        timer.Start();
    }

    public void RemoveToast(Guid id)
    {
        var toast = _toasts.Find(x => x.Id == id);
        if (toast != null)
        {
            _toasts.Remove(toast);
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
