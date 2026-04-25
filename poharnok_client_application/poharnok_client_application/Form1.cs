using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;
using System.Net.Mail;

namespace poharnok_client_application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UpdateButtonState();
            cmbAmount.Items.AddRange(new object[] { 500, 1500, 3000, 5000 });
            cmbAmount.SelectedIndex = 1; // Alapértelmezett az 1500
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
                        
                        var megjelenitendoAdatok = response.Content.Where(o => o.IsPlaced == false && !string.IsNullOrEmpty(o.UserEmail))

                            .Select(o => new
                            {
                                Azonosito = o.Id,
                                Bvin = o.bvin,
                                RendelesSzam = o.OrderNumber,
                                Email = o.UserEmail,
                                Osszeg = o.TotalGrand,
                                
                                Varos = o.BillingAddress?.City ?? "Nincs megadva",
                                Nev = $"{o.BillingAddress?.FirstName} {o.BillingAddress?.LastName}"
                            }).ToList();

                        dgvOrders.DataSource = megjelenitendoAdatok;
                        UpdateButtonState();
                        dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }

                    if (response?.Content != null)
                    {
                        _mindenAdat = response.Content
                            .Where(o => o.IsPlaced == false && !string.IsNullOrEmpty(o.UserEmail))
                            .Select(o => new OrderDisplayModel
                            {

                                Nev = $"{o.BillingAddress?.FirstName} {o.BillingAddress?.LastName}",
                                Keresztnev = o.BillingAddress?.FirstName ?? "Vásárlónk",

                                Azonosito = o.Id,
                                Email = o.UserEmail,
                                Osszeg = o.TotalGrand,
                                Frissitve = ParseJsonDate(o.LastUpdatedUtc)

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
            ApplyFilters();


        }

        private void SendEmailWithGiftCard(string recipientEmail, string keresztnev, string cardCode, decimal amount)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("poharnok.contact@gmail.com", "zguh viqt wuea csam"),
                    EnableSsl = true,
                };
                //string koszones = string.IsNullOrWhiteSpace(recipientName) ? "Vásárlónk" : recipientName;
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sajat.email@gmail.com"),
                    Subject = "Különleges ajándék neked!",
                    Body = $"Kedves {keresztnev}!\n\n" +
               $"Szeretnénk megajándékozni egy {amount} Ft értékû ajándékkártyával, " +
               $"melyet a következõ vásárlásodnál használhatsz fel.\n\n" +
               $"Kódod: {cardCode}\n\n" +
               "Várunk vissza az áruházba!",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(recipientEmail);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EMAIL HIBA] {ex.Message}");
            }
        }


        private void UpdateButtonState()
        {
            var lista = dgvOrders.DataSource as List<OrderDisplayModel>;

            // Feltétel: Van adat ÉS van legalább egy kijelölt elem
            bool hasSelection = lista != null && lista.Any(x => x.Kijelolve);

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



        private async void button2_Click(object sender, EventArgs e)
        {
            if (cmbAmount.SelectedItem == null) return;
            decimal selectedAmount = decimal.Parse(cmbAmount.SelectedItem.ToString());

            var lista = (List<OrderDisplayModel>)dgvOrders.DataSource;
            if (lista == null) return;

            button2.Enabled = false;

            // 1. Összes létezõ kártya lekérése az API-tól
            var letezoKartyak = await GetExistingGiftCardsAsync();

            int elküldve = 0;
            int kihagyva = 0;

            foreach (var item in lista.Where(x => x.Kijelolve))
            {
                // 2. Ellenõrzés a letöltött listában
                bool marVanNeki = letezoKartyak.Any(c =>
                    c.RecipientEmail?.ToLower() == item.Email.ToLower());

                if (!checkBox1.Checked && marVanNeki)
                {
                    kihagyva++;
                    continue;
                }

                await CreateGiftCardAsync(item.Email, item.Keresztnev, selectedAmount);
                elküldve++;
            }

            MessageBox.Show($"{elküldve} kupon email elküldve, {kihagyva} duplikált kupon kihagyva.");
            button2.Enabled = true;
            UpdateButtonState();
        }

        private async Task<List<GiftCardDTO>> GetExistingGiftCardsAsync()
        {
            string url = "http://20.93.113.186/DesktopModules/Hotcakes/API/rest/v1/giftcards?key=1-ecf245b9-b5b3-4c71-a4d0-e1115fb1fa3d";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetFromJsonAsync<GiftCardRoot>(url);
                    return response?.Content ?? new List<GiftCardDTO>();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"JSON hiba: {ex.Message}");
                    return new List<GiftCardDTO>();
                }
            }
        }

        private string GenerateGiftCardNumber()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            string GenerateBlock() => new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"KOSAR-{GenerateBlock()}-{GenerateBlock()}-{GenerateBlock()}-{GenerateBlock()}";
        }

        private async Task CreateGiftCardAsync(string email, string nev, decimal amount)
        {
            string url = "http://20.93.113.186/DesktopModules/Hotcakes/API/rest/v1/giftcards?key=1-ecf245b9-b5b3-4c71-a4d0-e1115fb1fa3d";

            using (HttpClient client = new HttpClient())
            {
                string issueDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string expiryDate = DateTime.UtcNow.AddMonths(12).ToString("yyyy-MM-ddTHH:mm:ssZ");

                var newCard = new GiftCardDTO
                {
                    StoreId = 1,
                    CardNumber = GenerateGiftCardNumber(),
                    Amount = amount, // A választott érték
                    UsedAmount = 0,
                    IssueDateUtc = issueDate,
                    ExpirationDateUtc = expiryDate,
                    RecipientEmail = email,
                    RecipientName = nev,
                    Enabled = true
                };

                var response = await client.PostAsJsonAsync(url, newCard);
        
        if (response.IsSuccessStatusCode)
                {
                    // Az emailnek is átadjuk az összeget
                    SendEmailWithGiftCard(email, nev, newCard.CardNumber, amount);
                }
            }
        }
        private void ApplyFilters()
        {
            string emailSzuro = textBoxFilter.Text.ToLower();

            // Ár szûrés (ha nem szám, akkor 0)
            decimal.TryParse(textBoxPrice.Text, out decimal minAr);

            // Dátum szûrés (DateTimePicker használata ajánlott: dtpDate)
            DateTime minDatum = dateTimePicker1.Value.Date;
            DateTime maxDatum = dateTimePicker2.Value.Date;

            var szurtLista = _mindenAdat.Where(x =>
                x.Email.ToLower().Contains(emailSzuro) &&
                x.Osszeg >= minAr &&
                x.Frissitve.Date >= minDatum &&
                x.Frissitve.Date <= maxDatum
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
                item.Kijelolve = _allSelected;
            }

            // 4. Frissítjük a táblázat megjelenítését
            dgvOrders.Refresh();
            UpdateButtonState();

            // Opcionális: Gomb szövegének módosítása
            button3.Text = _allSelected ? "Mind eltávolítása" : "Mind kijelölése";
        }

        private async void RefreshGiftCardTable()
        {
            var kartyak = await GetExistingGiftCardsAsync(); // [cite: 216]

            dgvGiftCards.DataSource = null;
            dgvGiftCards.DataSource = kartyak;

            // Oszlopok elrejtése név alapján
            string[] elrejtendo = { "StoreId", "RecipientName", "Enabled" };

            foreach (string colName in elrejtendo)
            {
                if (dgvGiftCards.Columns[colName] != null)
                {
                    dgvGiftCards.Columns[colName].Visible = false;
                }
            }

            // Opcionális: A maradék oszlopok szebb elnevezése
            if (dgvGiftCards.Columns["CardNumber"] != null) dgvGiftCards.Columns["CardNumber"].HeaderText = "Kártyaszám";
            if (dgvGiftCards.Columns["Amount"] != null) dgvGiftCards.Columns["Amount"].HeaderText = "Összeg";
            if (dgvGiftCards.Columns["UsedAmount"] != null) dgvGiftCards.Columns["UsedAmount"].HeaderText = "Elhasznált";
            if (dgvGiftCards.Columns["RecipientEmail"] != null) dgvGiftCards.Columns["RecipientEmail"].HeaderText = "Vevõ Email";
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
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(-7);
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now.AddDays(-7);
            dateTimePicker4.Value = DateTime.Now;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshGiftCardTable();
        }

        private void dgvGiftCards_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Csak sorokra nézzük, a fejlécet hagyjuk békén
            if (e.RowIndex < 0) return;

            // Megkeressük az "UsedAmount" oszlopot 
            var row = dgvGiftCards.Rows[e.RowIndex];
            var usedAmountVal = row.Cells["UsedAmount"].Value;

            if (usedAmountVal != null && decimal.TryParse(usedAmountVal.ToString(), out decimal usedAmount))
            {
                // Ha elhasználtak belõle bármennyit (> 0) 
                if (usedAmount > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    row.DefaultCellStyle.SelectionBackColor = Color.Green; // Kijelölve is maradjon zöldes
                }
                else
                {
                    // Visszaállítás alaphelyzetbe (fontos a görgetés miatt)
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
    }
}