namespace BankSimulator.View.UiComponents
{
    internal class LoginUi(Func<string[]> getNamesMethod)
    {
        public async Task<bool> Login()
        {
            string[] names = getNamesMethod();
            return true;
        }
    }
}
