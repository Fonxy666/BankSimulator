namespace BankSimulator.View.UiComponents
{
    internal class LoginUi(Func<string[]> getNamesMethod)
    {
        public void Login()
        {
            string[] names = getNamesMethod();
        }
    }
}
