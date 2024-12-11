using BankSimulator.FileSaverFolder;
using BankSimulator.View;

namespace BankSimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileSaver filesaver = GetFileSaver();
            Ui ui = GetUi(filesaver);

            ui.Run();
        }

        private static Ui GetUi(FileSaver fileSaver)
        {
            return new Ui(fileSaver);
        }

        private static FileSaver GetFileSaver()
        {
            return new FileSaver();
        }
    }
}
