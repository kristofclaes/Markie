using System;

namespace Markie.ViewModels
{
    public class SetupViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool HasError { get; set; }

        public void Validate()
        {
            HasError = Login.IsNullOrWhiteSpace() || Password.IsNullOrWhiteSpace() || !Login.IsValidEmailAddress();
        }
    }
}