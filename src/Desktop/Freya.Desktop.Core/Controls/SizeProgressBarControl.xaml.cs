using System.Windows.Controls;

namespace Freya.Desktop.Core.Controls
{
    /// <summary>
    /// Interaction logic for SizeProgressBarControl.xaml
    /// </summary>
    public partial class SizeProgressBarControl : UserControl
    {
        public long Value
        {
            get { return (long)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long), typeof(SizeProgressBarControl), new PropertyMetadata(0L, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SizeProgressBarControl)d;
            long percentage = 0L;
            if (control.Value != 0 && control.MaxValue != 0)
            {
                percentage = control.Value * 100 / control.MaxValue;
            }

            control.UpdatePercentage(percentage);
        }

        public long MaxValue
        {
            get { return (long)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(long), typeof(SizeProgressBarControl), new PropertyMetadata(0L, OnValueChanged));


        public SizeProgressBarControl()
        {
            InitializeComponent();
            UpdatePercentage(0);
        }

        private void UpdatePercentage(long percentage)
        {
            Percentage.Text = percentage + "%";
        }
    }
}
