namespace Statystyka.Generowanie
{
    public class Wydruk
    {
        private static byte[] StwórzPdfZHtml(string html)
        {
            GlobalConfig konfiguracja = new GlobalConfig();

            konfiguracja.SetPaperSize(PaperKind.A4);

            IPechkin pechkin = new SynchronizedPechkin(konfiguracja);
            byte[] bajty = pechkin.Convert(html);

        }
    }
}
