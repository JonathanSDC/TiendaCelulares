namespace Tienda_Celulares.Web.Services
{
    public class AuthStateService
    {
        public event Action? OnChange;
        private string? _currentUser;

        public string? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
