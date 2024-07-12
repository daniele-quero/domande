using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class DomandeParser
{
    private static List<string> FIGURE = new List<string>() { "la figura", "dalla figura", "della figua", "il diagramma", "quale diagramma", "nel diagramma",
        "in figura", "lungo le righe", "seguenti orologi", "Quale figura", "in tabella", "La tabella", "SV si trova", "La figura", "Nella figura", "Ogni riga",
    "simboli rimangono", "dei nove dischetti", "Il grafico mostra", "vendite di corsi", "proprio guardaroba", "raffigurato", "alle figure", "Il grafico illustra",
    "Il grafico riassume", "20 borse, allora:", "ragazzi che seguono", "Le tabelle", "seguenti tabelle", "Todd ha 23", "Blaine viene"};

    public static List<string> ExtractDomande(string parsedPdf)
    {
        var list = parsedPdf.Split("Domanda\nN° ").ToList();
        MantieniDomande(list);
        ScartaFigure(list);
        return list;
    }

    private static void MantieniDomande(List<string> list) => list.RemoveAll(d => !(d.Contains("A)") && d.Contains("E)")));
    private static void ScartaFigure(List<string> list) => list.RemoveAll(d => FIGURE.Select(f => d.Contains(f)).Where(p => p = true).First());

    public static List<Domanda> ExtractDomande(List<string> domandeStr)
    {
        return domandeStr
                    .Select(d => new Domanda(
                            ExtractNumero(d),
                            ExtractQuesito(d),
                            ExtractRisposte(d))
                        .Build())
                    .ToList();
    }

    private static string ExtractQuesito(string d) => d.Substring(d.IndexOf(ExtractNumero(d)) + ExtractNumero(d).Length, d.IndexOf("A)") - (d.IndexOf(ExtractNumero(d)) + ExtractNumero(d).Length))
                                                    //.Substring(2)
                                                    .Trim();

    private static string ExtractNumero(string d) => d.Split("\n")[0];

    private static Risposta[] ExtractRisposte(string d)
    {
        string pattern = @"\n[A-E]{1}\)";
        Regex regex = new Regex(pattern);
        var matches = regex.Matches(d);
        var risposte = new Risposta[matches.Count];

        for (int i = 0; i < matches.Count - 1; i++)
        {
            int length = d.IndexOf(matches[i + 1].Value) - d.IndexOf(matches[i].Value);
            var t_ = d.Substring(d.IndexOf(matches[i].Value), length);
            risposte[i] = new Risposta(
                    CleanText(t_),
                    t_.Contains("A)"));
        }

        var t = d.Substring(d.IndexOf(matches[matches.Count - 1].Value));

        risposte[matches.Count - 1] = new Risposta(
                    CleanText(t),
                    t.Contains("A)"));

        return risposte;
    }

    private static string CleanText(string text)
    {
        string pattern = @"Pagina\s[0-9]{1,3}\sdi\s[0-9]{3}";
        return Regex.Replace(text, pattern, "").Replace("\n", " ").Replace("Domande seconda sessione 2024", "");
    }
}
