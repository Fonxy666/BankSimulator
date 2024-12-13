using BankSimulator.View;

namespace BankSimulator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Ui ui = GetUi();

            await ui.Run();
        }

        private static Ui GetUi()
        {
            return new Ui();
        }
    }
}
