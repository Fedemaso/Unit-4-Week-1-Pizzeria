using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models
{
    public class CarrelloRiepilogo
    {
        public class Carrello
        {
            public int Id { get; set; }
            public string NomePizza { get; set; }
            public int Quantita { get; set; }
            public decimal Prezzo { get; set; }
            public decimal Totale { get { return Quantita * Prezzo; } }
        }

        public class Riepilogo
        {
            public List<Carrello> Items { get; set; }
            public string IndirizzoSpedizione { get; set; }
            public string Note { get; set; }

            public decimal TotaleOrdine
            {
                get
                {
                    return Items?.Sum(i => i.Totale) ?? 0;
                }
            }
        }

    }

}