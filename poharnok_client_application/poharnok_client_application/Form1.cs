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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: " + ex.Message);
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://20.93.113.186/DesktopModules/Hotcakes/API/rest/v1/orders?key=1-ecf245b9-b5b3-4c71-a4d0-e1115fb1fa3d";
                var response = await client.GetFromJsonAsync<OrderRoot>(url);

                if (response?.Content != null)
                {
                    // SENIOR TRÜKK: Csak azokat szedjük ki, akik:
                    // 1. NEM adták le a rendelést (IsPlaced == false)
                    // 2. DE megadták az email címüket
                    var abandonedCarts = response.Content
                        .Where(o => o.IsPlaced == false && !string.IsNullOrEmpty(o.UserEmail))
                        .ToList();

                    if (abandonedCarts.Count == 0)
                    {
                        MessageBox.Show("Nincs elhagyott kosár, akinek küldhetnénk kupont.");
                        return;
                    }

                    foreach (var cart in abandonedCarts)
                    {
                        // Itt hívjuk meg a küldõ funkciót
                        SendCouponEmail(cart.UserEmail);
                        Console.WriteLine($"[MAIL] Kupon küldése a következõnek: {cart.UserEmail}. Kód: SAVE10");
                    }

                    MessageBox.Show($"{abandonedCarts.Count} db kupon kiküldve!");
                }
            }
        }

        // Egyszerûsített email küldõ (szimuláció)
        private void SendCouponEmail(string email)
        {
            // Egyetemi projektben elég, ha ide írsz egy Console.WriteLine-t 
            // vagy logolod egy fájlba, hogy "Kupon elküldve: email@cim.hu"
            // Ha nagyon profi akarsz lenni, ide jöhet egy SmtpClient hívás.
            Console.WriteLine($"[MAIL] Kupon küldése a következõnek: {email}. Kód: SAVE10");
        }
    }
}