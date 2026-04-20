using System;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.Net.Http.Json;

namespace poharnok_client_application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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
                        var megjelenitendoAdatok = response.Content.Select(o => new
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: " + ex.Message);
                }
            }
        }
    }
}