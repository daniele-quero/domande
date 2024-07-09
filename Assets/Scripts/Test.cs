using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] List<Domanda> domandeBiologia;
    [SerializeField] List<Domanda> domandeCompetenze;
    [SerializeField] List<Domanda> domandeLogica;
    void Start()
    {

        domandeBiologia = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\biologia.pdf")));

        domandeCompetenze = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\competenze.pdf")));

        domandeLogica = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\logica.pdf")));
        //EditorUtility.OpenFilePanel

    }


}
