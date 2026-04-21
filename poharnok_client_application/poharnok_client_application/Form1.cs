using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace poharnok_client_application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<OrderDisplayModel> _mindenAdat = new List<OrderDisplayModel>();
        private async void button1_Click(object sender, EventArgs e)
        {
            string apiUrl = "http://20.93.113.186/DesktopModules/Hotcakes/API/rest/v1/orders?key=1-ecf245b9-b5b3-4c71-a4d0-e1115fb1fa3d";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetFromJsonAsync<OrderRoot>(apiUrl);

                    if (response?.Content != null)
                    {
                        // Itt történik a varázslat: kiválogatjuk, mit akarunk látni
                        var megjelenitendoAdatok = response.Content.Where(o => o.IsPlaced == false && !string.IsNullOrEmpty(o.UserEmail))

                            .Select(o => new
                            {
                                Azonosito = o.Id,
                                Bvin = o.bvin,
                                RendelesSzam = o.OrderNumber,
                                Email = o.UserEmail,
                                Osszeg = o.TotalGrand,
                                // Itt "nyitjuk ki" a DTO-t:
                                Varos = o.BillingAddress?.City ?? "Nincs megadva",
                                Nev = $"{o.BillingAddress?.FirstName} {o.BillingAddress?.LastName}"
                            }).ToList();

                        dgvOrders.DataSource = megjelenitendoAdatok;
                        dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }


                    //uj resz TESZT
                    if (response?.Content != null)
                    {
                        _mindenAdat = response.Content
                            .Where(o => o.IsPlaced == false && !string.IsNullOrEmpty(o.UserEmail))
                            .Select(o => new OrderDisplayModel
                            {
                                Azonosito = o.Id,
                                Email = o.UserEmail,
                                Nev = $"{o.BillingAddress?.FirstName} {o.BillingAddress?.LastName}",
                                Osszeg = o.TotalGrand
                            }).ToList();

                        dgvOrders.DataSource = _mindenAdat;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: " + ex.Message);
                }
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            var lista = (List<OrderDisplayModel>)dgvOrders.DataSource;
            if (lista == null) return;

            int counter = 0;
            foreach (var item in lista)
            {
                if (item.Selected) // Csak ha be van pipálva
                {
                    SendCouponEmail(item.Email);
                    counter++;
                }
            }
            MessageBox.Show($"{counter} db kupon elküldve!");
        }

        private void SendCouponEmail(string email)
        {
            // Szimuláció a Debug ablakba
            System.Diagnostics.Debug.WriteLine($"[KÜLDÉS] Cél: {email} | Kód: SAVE10");
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            string szuro = textBoxFilter.Text.ToLower();

            // Ha üres a mezõ, mutassunk mindent, egyébként szûrjünk email alapján
            var szurtLista = _mindenAdat
                .Where(x => x.Email.ToLower().Contains(szuro))
                .ToList();

            dgvOrders.DataSource = szurtLista;
        }
    }
}