using DevExpress.ExpressApp.Utils;
using System;

namespace SBT.Apps.Base.Module
{
    /// <summary>
    /// Implementar mecanismo para convertir cantidades numericas al texto equivalente, la cual es necesaria en varios 
    /// escenarios como impresion de facturas, recibos, cheques, etc.
    /// </summary>
    /// <remarks>
    /// Para implementar esta clase se uso como base la que se encontro en la siguiente direccion
    /// https://hdeleon.net/clase-en-c-net-para-convertir-una-cantidad-monetaria-en-letra/
    /// </remarks>
    public class NumeroALetras
    {

        private string[] fUnidades = { string.Empty, CaptionHelper.GetLocalizedText(@"Numeros", "1", "un") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "2", "dos") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "3", "tres") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "4", "cuatro") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "5", "cinco") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "6", "seis") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "7", "siete") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "8", "ocho") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "9", "nueve") + " "};
        private string[] fDecenas = {CaptionHelper.GetLocalizedText(@"Numeros", "10", "diez") + " ",
             CaptionHelper.GetLocalizedText(@"Numeros", "11", "once") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "12", "doce") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "13", "trece") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "14" ,"catorce") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "15", "quince") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "16", "dieciseis") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "17", "diecisiete") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "18", "dieciocho") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "19", "diecinueve") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "20", "veinte") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "30", "treinta") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "40", "cuarenta") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "50", "cincuenta") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "60", "sesenta") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "70", "setenta") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "80", "ochenta") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "90", "noventa") + " "};
        private string[] fCentenas = { string.Empty, CaptionHelper.GetLocalizedText(@"Numeros", "100", "ciento") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "200", "doscientos") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "300", "trescientos") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "400", "cuatrocientos") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "500", "quinientos") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "600", "seicientos") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "700", "setecientos") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "800", "ochocientos") + " ",
            CaptionHelper.GetLocalizedText(@"Numeros", "900", "novecientos ") + " "};

        public NumeroALetras()
        {

        }

        /// <summary>
        /// Invocar la conversión de una cantidad numerica a letras
        /// </summary>
        /// <param name="numero">El numero a convertir en letras</param>
        /// <param name="moneda">El plural de la moneda</param>
        /// <returns></returns>
        public string Convertir(decimal numero, string moneda = "")
        {

            string literal = string.Empty;
            string parte_decimal;
            //si el numero utiliza (,) en lugar de (.) -> se reemplaza
            string sNumero = numero.ToString("f2");
            sNumero = sNumero.Replace(",", ".");

            //se divide el numero 0000000.00 -> entero y decimal
            string[] Num = sNumero.Split('.');

            parte_decimal = Num[1] + "/100 " + moneda;
            int numEntero = int.Parse(Num[0]);
            //se convierte el numero a literal
            if (numEntero == 0)
                literal = $" {CaptionHelper.GetLocalizedText(@"Numeros", "0", "cero")} ";
            else if (numEntero > 999999)
                literal = ObtenerMillones(numEntero);
            else if (numEntero > 999)
                literal = ObtenerMiles(numEntero);
            else if (numEntero > 99)
                literal = ObtenerCentenas(numEntero);
            else if (numEntero > 9)
                literal = ObtenerDecenas(numEntero);
            else
                literal = ObtenerUnidades(numEntero);
            return (literal + parte_decimal);
        }

        /// <summary>
        ///  Invocar la conversión de una cantidad numerica a letras
        /// </summary>
        /// <param name="numero">El numero a convertir en letras</param>
        /// <param name="mayusculas">Indica si la cantidad se debe retornar convertida en mayusculas</param>
        /// <param name="moneda">El plural de la moneda</param>
        /// <returns></returns>
        public string Convertir(decimal numero, bool mayusculas, string moneda = "")
        {
            if (mayusculas)
                return Convertir(numero, moneda).ToUpper();
            else
                return Convertir(numero, moneda);
        }

        /// <summary>
        /// Convertir la cantidad que corresponde a las unidades a letras, desde 0 - 9
        /// </summary>
        /// <param name="numero">La parte del numero que corresponde a las unidades</param>
        /// <returns>La cantidad de unidades en su equivalente en letras</returns>
        private string ObtenerUnidades(int numero)
        {
            string sNum = Convert.ToString(numero);
            return fUnidades[int.Parse(sNum.Substring(sNum.Length - 1))];
        }

        /// <summary>
        /// Convertir la cantidad que corresponde a las decenas a letras, desde 10 hasta 99
        /// </summary>
        /// <param name="numero">La parte del numero que corresponde a las decenas</param>
        /// <returns>La cantidad de decenas en su equivalente en letras</returns>
        private string ObtenerDecenas(int numero)
        {
            if (numero < 10)
                return ObtenerUnidades(numero); // 01 - 09
            else if (numero > 19)
            {
                string u = ObtenerUnidades(numero);
                if (u.Equals(string.Empty))
                    return fDecenas[int.Parse(Convert.ToString(numero).Substring(0, 1)) + 8]; // 20 - 90
                else
                    return fDecenas[int.Parse(Convert.ToString(numero).Substring(0, 1)) + 8] + "y " + u;
            }
            else
            {
                return fDecenas[numero - 10]; //entre 11 - 19
            }
        }

        /// <summary>
        /// Convertir las cantidad que corresponde a las centenas a letras, desde 100 hasta 999
        /// </summary>
        /// <param name="numero">la parte del numero que corresponde a las centenas</param>
        /// <returns>La cantidad de centenas en su equivalente en letras</returns>
        private string ObtenerCentenas(int numero)
        {
            if (numero > 99)
            {
                if (numero == 100)
                    return $" {CaptionHelper.GetLocalizedText(@"Numeros", "hundred", "cien")} ";
                else
                {
                    string num = Convert.ToString(numero);
                    return fCentenas[int.Parse(num.Substring(0, 1))] + ObtenerDecenas(int.Parse(num.Substring(1)));
                }
            }
            else
                return ObtenerDecenas(numero);
        }

        /// <summary>
        /// Convertir la cantidad que corresponde a los miles a letras desde 1,000 hasta 999,999
        /// </summary>
        /// <param name="numero">la parte del numero que corresponde a los miles</param>
        /// <returns>La cantidad de miles en su equivalente en letras</returns>
        private string ObtenerMiles(int numero)
        {
            //obtiene las centenas
            string sNum = string.Format("{0:0000}", numero);
            int c = int.Parse(sNum.Substring(sNum.Length - 3));
            //obtiene los miles
            int m = int.Parse(sNum.Substring(0, sNum.Length - 3));
            string n = string.Empty;
            if (m > 0)
            {
                n = ObtenerCentenas(m);
                return n + $"{CaptionHelper.GetLocalizedText(@"Numeros", "thousand", "mil")} " + ObtenerCentenas(c);
            }
            else
                return string.Empty + ObtenerCentenas(c);
        }

        /// <summary>
        /// Convertir la cantidad de millones a letras, desde 1,000,000 hasta 999,999,999
        /// </summary>
        /// <param name="numero">La parte entera del numero a convertir en letras</param>
        /// <returns>El numero convertido en letras, sin la parte decimal</returns>
        private string ObtenerMillones(int numero)
        {
            //se obtiene los miles
            string sNum = Convert.ToString(numero);
            int miles = int.Parse(sNum.Substring(sNum.Length - 6));
            //se obtiene los millones
            int millon = int.Parse(sNum.Substring(0, sNum.Length - 6));
            String n = string.Empty;
            if (millon > 1)
                n = ObtenerCentenas(millon) + $"{CaptionHelper.GetLocalizedText(@"Numeros", "millions", "millones")} ";
            else
                n = ObtenerUnidades(millon) + $"{CaptionHelper.GetLocalizedText(@"Numeros", "million", "millón")} ";
            return n + ObtenerMiles(miles);
        }
    }
}
