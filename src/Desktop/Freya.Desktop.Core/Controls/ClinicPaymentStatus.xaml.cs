using Models.Core.Subscriptions;
using System.Windows.Controls;
using System.Windows.Media;

namespace Freya.Desktop.Core.Controls;

public partial class ClinicPaymentStatus : UserControl
{
    public SubscriptionPaymentResponse? SubscriptionPayment
    {
        get { return (SubscriptionPaymentResponse)GetValue(SubscriptionPaymentProperty); }
        set { SetValue(SubscriptionPaymentProperty, value); }
    }

    public bool ShowPlanName
    {
        get { return (bool)GetValue(ShowPlanNameProperty); }
        set { SetValue(ShowPlanNameProperty, value); }
    }

    public static readonly DependencyProperty ShowPlanNameProperty =
        DependencyProperty.Register("ShowPlanName", typeof(bool), typeof(ClinicPaymentStatus), new PropertyMetadata(true));



    public static readonly DependencyProperty SubscriptionPaymentProperty =
        DependencyProperty.Register("SubscriptionPayment", typeof(SubscriptionPaymentResponse), typeof(ClinicPaymentStatus), new PropertyMetadata(null, OnChangedState));

    private static void OnChangedState(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ClinicPaymentStatus)d;
        if (control != null)
        {
            control.UpdateState();
        }
    }

    private void UpdateState()
    {
        var text = "Sin subscripción";
        SolidColorBrush backgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#BCD684")!;
        SolidColorBrush textcolor = (SolidColorBrush)new BrushConverter().ConvertFrom("#2D2D33")!;

        switch (SubscriptionPayment?.State)
        {
            case SubscriptionStates.NONE:
                text = "Sin subscripción";
                backgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#BCD684")!;
                StateBorder.BorderThickness = new Thickness(0);
                break;
            case SubscriptionStates.ACTIVE:
                text = "Activa";
                backgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00C49D")!;
                StateBorder.BorderThickness = new Thickness(0);
                break;
            case SubscriptionStates.REJECTED_BY_BANK:
                text = "Rechazada por banco";
                backgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#19FF586E")!;
                textcolor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF586E")!;
                StateBorder.BorderBrush = textcolor;
                StateBorder.BorderThickness = new Thickness(2);
                break;
            case SubscriptionStates.REJECTED_BY_EMPLOYEE:
                text = "Cancelada";
                backgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#19FF586E")!;
                textcolor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF586E")!;
                StateBorder.BorderBrush = textcolor;
                StateBorder.BorderThickness = new Thickness(2);
                break;
        }

        StateText.Text = text;
        StateBorder.Background = backgroundColor;
        StateText.Foreground = textcolor;
        StateName.Text = SubscriptionPayment?.Name;
        StateName.Foreground = textcolor;
        StateSeparation.Foreground = textcolor;
        if (string.IsNullOrEmpty(SubscriptionPayment?.Name))
        {
            StateSeparation.Visibility = Visibility.Hidden;
        }
        else
        {
            StateSeparation.Visibility = ShowPlanName ? Visibility.Visible : Visibility.Hidden;
        }
    }

    public ClinicPaymentStatus()
    {
        InitializeComponent();
        UpdateState();
    }
}
