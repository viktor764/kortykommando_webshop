using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace poharnok_client_application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UpdateButtonState();
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
                        UpdateButtonState();
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
                        UpdateButtonState();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: " + ex.Message);
                }
            }



        }




        private void UpdateButtonState()
        {
            var lista = dgvOrders.DataSource as List<OrderDisplayModel>;

            // Feltétel: Van adat ÉS van legalább egy kijelölt elem
            bool hasSelection = lista != null && lista.Any(x => x.Selected);

            button2.Enabled = hasSelection;

            // Szín beállítása (opcionális, az Enabled = false alapból szürkít)
            button2.BackColor = hasSelection ? Color.White : Color.LightGray;
        }


        // Ez kényszeríti a táblázatot, hogy azonnal mentse a pipát, ne csak a cella elhagyásakor
        private void dgvOrders_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOrders.IsCurrentCellDirty)
            {
                dgvOrders.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        // Ez hívódik meg, amikor a pipa értéke ténylegesen megváltozott
        private void dgvOrders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Csak akkor frissítünk, ha nem a fejlécet nyomtuk meg (-1)
            if (e.RowIndex >= 0)
            {
                UpdateButtonState();
            }
        }

        private DateTime ParseJsonDate(string jsonDate)
        {
            if (string.IsNullOrEmpty(jsonDate)) return DateTime.MinValue;
            // Kiszûrjük a számokat a "/Date(1776794896403)/" szövegbõl
            string msStr = System.Text.RegularExpressions.Regex.Match(jsonDate, @"\d+").Value;
            long ms = long.Parse(msStr);
            return DateTimeOffset.FromUnixTimeMilliseconds(ms).DateTime.ToLocalTime();
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

        

        private void ApplyFilters()
        {
            string emailSzuro = textBoxFilter.Text.ToLower();

            // Ár szûrés (ha nem szám, akkor 0)
            decimal.TryParse(textBoxPrice.Text, out decimal minAr);

            // Dátum szûrés (DateTimePicker használata ajánlott: dtpDate)
            DateTime minDatum = dateTimePicker1.Value.Date;

            var szurtLista = _mindenAdat.Where(x =>
                x.Email.ToLower().Contains(emailSzuro) &&
                x.Osszeg >= minAr &&
                x.Frissitve.Date >= minDatum
            ).ToList();

            dgvOrders.DataSource = szurtLista;
            UpdateButtonState();
        }


        private bool _allSelected = false; // Segédváltozó az állapot követéséhez

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Lekérjük a JELENLEG megjelenített (szûrt) listát
            var aktualisLista = dgvOrders.DataSource as List<OrderDisplayModel>;

            if (aktualisLista == null || aktualisLista.Count == 0) return;

            // 2. Állapot megfordítása
            _allSelected = !_allSelected;

            // 3. Csak a szûrt elemeken megyünk végig
            foreach (var item in aktualisLista)
            {
                item.Selected = _allSelected;
            }

            // 4. Frissítjük a táblázat megjelenítését
            dgvOrders.Refresh();
            UpdateButtonState();

            // Opcionális: Gomb szövegének módosítása
            button3.Text = _allSelected ? "Select None" : "Select All";
        }

        private void textBoxPrice_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }


    }
}