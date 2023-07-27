namespace FreyaManager.Features.Main
{
    public class MainViewModel : CoreViewModel
    {
        private bool isLogged;
        private LateralMenuViewModel lateralMenuViewModel;
        private HeaderControlViewModel headerControlViewModel;

        public MainViewModel(
            LateralMenuViewModel lateralMenuViewModel,
            HeaderControlViewModel headerControlViewModel)
        {
            this.lateralMenuViewModel = lateralMenuViewModel;
            this.LateralMenuViewModel.MainViewModel = this;
            this.headerControlViewModel = headerControlViewModel;
        }

        public LateralMenuViewModel LateralMenuViewModel
        {
            get { return this.lateralMenuViewModel; }
            set { SetAndRaisePropertyChanged(ref lateralMenuViewModel, value); }
        }

        public HeaderControlViewModel HeaderControlViewModel
        {
            get { return this.headerControlViewModel; }
            set { SetAndRaisePropertyChanged(ref headerControlViewModel, value); }
        }

        public bool IsLogged
        {
            get { return isLogged; }
            set
            {
                SetAndRaisePropertyChanged(ref isLogged, value);

            }
        }

        internal async Task LoadItems()
        {
            await LateralMenuViewModel.OnAppearingAsync();
            await HeaderControlViewModel.OnAppearingAsync();
        }
    }
}
