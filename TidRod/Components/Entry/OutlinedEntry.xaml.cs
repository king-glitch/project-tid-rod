using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Components.Entry
{
    public partial class OutlinedEntry : Grid
    {
        public OutlinedEntry()
        {
            InitializeComponent();

            customEntry.Text = Text;

            customEntry.TextChanged += OnCustomEntryTextChanged;

            customEntry.Completed += OnCustomEntryCompleted;
        }

        private ImageSource tempIcon;

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(OutlinedEntry),
            default(string),
            BindingMode.TwoWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                OutlinedEntry view = (OutlinedEntry)bindable;

                view.customEntry.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(
            nameof(PlaceholderText),
            typeof(string),
            typeof(OutlinedEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                OutlinedEntry view = (OutlinedEntry)bindable;

                view.placeholderText.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty HelperTextProperty = BindableProperty.Create(
            nameof(HelperText),
            typeof(string),
            typeof(OutlinedEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                OutlinedEntry view = (OutlinedEntry)bindable;

                view.helperText.Text = (string)newValue;

                view.helperText.IsVisible = view.errorText.IsVisible ? false : !string.IsNullOrEmpty(view.helperText.Text);
            }
        );

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
            nameof(ErrorText),
            typeof(string),
            typeof(OutlinedEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedEntry)bindable;

                view.errorText.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty LeadingIconProperty = BindableProperty.Create(
            nameof(LeadingIcon),
            typeof(ImageSource),
            typeof(OutlinedEntry),
            default(ImageSource),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedEntry)bindable;

                view.leadingIcon.Source = (ImageSource)newValue;

                view.leadingIcon.IsVisible = !view.leadingIcon.Source.IsEmpty;
            }
        );

        public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create(
            nameof(TrailingIcon),
            typeof(ImageSource),
            typeof(OutlinedEntry),
            default(ImageSource),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedEntry)bindable;

                view.trailingIcon.Source = (ImageSource)newValue;

                view.trailingIcon.IsVisible = view.trailingIcon.Source != null;
            }
        );

        public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(
            nameof(HasError),
            typeof(bool),
            typeof(OutlinedEntry),
            default(bool),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedEntry)bindable;

                view.errorText.IsVisible = (bool)newValue;

                view.containerFrame.BorderColor = view.errorText.IsVisible ? Color.Red : Color.Black;

                view.helperText.IsVisible = !view.errorText.IsVisible;

                view.placeholderText.TextColor = view.errorText.IsVisible ? Color.Red : Color.Gray;

                view.PlaceholderText = view.errorText.IsVisible ? $"{view.PlaceholderText}*" : view.PlaceholderText;

                if (view.TrailingIcon != null && !view.TrailingIcon.IsEmpty)
                    view.tempIcon = view.TrailingIcon;

                view.TrailingIcon = view.errorText.IsVisible
                    ? ImageSource.FromFile("ic_error.png")
                    : view.tempIcon;

                view.trailingIcon.IsVisible = view.errorText.IsVisible;
            }
        );

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(OutlinedEntry),
            default(bool),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedEntry)bindable;

                view.customEntry.IsPassword = (bool)newValue;

                view.passwordIcon.IsVisible = (bool)newValue;
            }
        );

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            nameof(MaxLength),
            typeof(int),
            typeof(OutlinedEntry),
            default(int),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                OutlinedEntry view = (OutlinedEntry)bindable;

                view.customEntry.MaxLength = (int)newValue;

                view.charCounterText.IsVisible = view.customEntry.MaxLength > 0;

                view.charCounterText.Text = $"0 / {view.MaxLength}";
            }
        );

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(OutlinedEntry),
            Color.FromHex("1A73E9"),
            BindingMode.OneWay
        );

        public event EventHandler<EventArgs> EntryCompleted;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public string HelperText
        {
            get => (string)GetValue(HelperTextProperty);
            set => SetValue(HelperTextProperty, value);
        }

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public ImageSource LeadingIcon
        {
            get => (ImageSource)GetValue(LeadingIconProperty);
            set => SetValue(LeadingIconProperty, value);
        }

        public ImageSource TrailingIcon
        {
            get => (ImageSource)GetValue(TrailingIconProperty);
            set => SetValue(TrailingIconProperty, value);
        }

        public bool HasError
        {
            get => (bool)GetValue(HasErrorProperty);
            set => SetValue(HasErrorProperty, value);
        }

        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public int MaxLength
        {
            get
            {
                return (int)GetValue(MaxLengthProperty);
            }

            set
            {
                SetValue(MaxLengthProperty, value);
            }
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public Keyboard Keyboard
        {
            set => customEntry.Keyboard = value;
        }

        public ReturnType ReturnType
        {
            set
            {
                customEntry.ReturnType = value;
            }
        }

        public ICommand ReturnCommand
        {
            get => (ICommand)GetValue(ReturnCommandProperty);
            set => SetValue(ReturnCommandProperty, value);
        }

        public static BindableProperty ReturnCommandProperty { get; } = BindableProperty.Create(
            nameof(ReturnCommand),
            typeof(ICommand),
            typeof(OutlinedEntry),
            default(ICommand),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (OutlinedEntry)bindable;

                view.customEntry.ReturnCommand = (ICommand)newValue;
            }
        );

        private async Task ControlFocused()
        {
            if (string.IsNullOrEmpty(customEntry.Text) || customEntry.Text.Length > 0)
            {
                customEntry.Focus();

                containerFrame.BorderColor = HasError ? Color.Red : BorderColor;
                placeholderText.TextColor = HasError ? Color.Red : BorderColor;

                int y = DeviceInfo.Platform == DevicePlatform.UWP ? -25 : -20;

                _ = await placeholderContainer.TranslateTo(0, y, 100, Easing.Linear);

                placeholderContainer.HorizontalOptions = LayoutOptions.Start;
                placeholderText.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            }
            else
            {
                await ControlUnfocused();
            }
        }

        private async Task ControlUnfocused()
        {
            containerFrame.BorderColor = HasError ? Color.Red : Color.FromHex("B9B9B9");
            placeholderText.TextColor = HasError ? Color.Red : Color.Gray;

            customEntry.Unfocus();

            if (string.IsNullOrEmpty(customEntry.Text) || customEntry.MaxLength <= 0)
            {
                _ = await placeholderContainer.TranslateTo(0, 0, 100, Easing.Linear);

                placeholderContainer.HorizontalOptions = LayoutOptions.FillAndExpand;
                placeholderText.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            }
        }

        private void CustomEntryFocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                MainThread.BeginInvokeOnMainThread(async () => await ControlFocused());
            }
        }

        private void CustomEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                MainThread.BeginInvokeOnMainThread(async () => await ControlUnfocused());
            }
        }

        private void OutlinedEntryTapped(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => await ControlFocused());
        }

        private void PasswordEyeTapped(object sender, EventArgs e)
        {
            customEntry.IsPassword = !customEntry.IsPassword;
        }

        private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = e.NewTextValue;

            if (charCounterText.IsVisible)
            {
                charCounterText.Text = $"{customEntry.Text.Length} / {MaxLength}";
            }

            TextChanged?.Invoke(this, e);
        }

        private void OnCustomEntryCompleted(object sender, EventArgs e)
        {
            EntryCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}