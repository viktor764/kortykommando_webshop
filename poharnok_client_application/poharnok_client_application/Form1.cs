using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private bool _allSelected = false; // Segédváltozó az állapot követéséhez

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
                // ITT VAN A JAVÍTÁS: Megvizsgáljuk, hogy a keresztnév üres-e, vagy csak szóköz
                string megszolitas = string.IsNullOrWhiteSpace(keresztnev) ? "Vásárlónk" : keresztnev;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    // FIGYELEM: A jelszavad élesítés elõtt érdemes környezeti változóba vagy config fájlba rejteni!
                    Credentials = new NetworkCredential("poharnok.contact@gmail.com", "zguh viqt wuea csam"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("poharnok.contact@gmail.com"),
                    Subject = "Ajándék vár a kosaradban!",
                    IsBodyHtml = true, // A sima Body tulajdonságot üresen hagyjuk, mert AlternateView-t használunk
                };

                mailMessage.To.Add(recipientEmail);

                // A HTML tartalom összeállítása
                string htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>

<style>
body {{
    margin:0;
    padding:0;
    background:#1a1a1a;
    font-family:Arial, sans-serif;
}}

.wrapper {{
    width:100%;
    padding:40px 0;
    background:#1a1a1a;
}}

.container {{
    width:600px;
    margin:0 auto;
    background:#1f1f1f;
    border:1px solid #6d141b;
    border-radius:12px;
    overflow:hidden;
}}

.header {{
    background:#6d141b;
    padding:35px 30px;
    text-align:center;
}}

.logo {{
    max-width:220px;
    margin-bottom:20px;
}}

.title {{
    color:#ffffff;
    font-size:30px;
    font-weight:bold;
    line-height:38px;
}}

.subtitle {{
    color:#f0d7da;
    font-size:14px;
    margin-top:10px;
}}

.content {{
    padding:35px 30px;
}}

.text {{
    color:#d6d6d6;
    font-size:15px;
    line-height:26px;
}}

.gift-box {{
    margin:30px 0;
    background:#171717;
    border:1px solid #6d141b;
    border-radius:10px;
    padding:25px;
    text-align:center;
}}

.gift-label {{
    color:#cccccc;
    font-size:13px;
    margin-bottom:12px;
}}

.gift-code {{
    color:#ffffff;
    font-size:32px;
    font-weight:bold;
    letter-spacing:3px;
}}

.button {{
    display:inline-block;
    margin-top:30px;
    background:#6d141b;
    color:#ffffff !important;
    text-decoration:none;
    padding:14px 30px;
    border-radius:8px;
    font-size:15px;
    font-weight:bold;
    text-transform:uppercase;
}}

.footer {{
    padding:28px;
    text-align:center;
    border-top:1px solid #333333;
    background:#151515;
    color:#999999;
    font-size:12px;
    line-height:20px;
}}

.divider {{
    width:120px;
    height:2px;
    background:#6d141b;
    margin:20px auto;
    border-radius:10px;
}}
</style>
</head>

<body>

<div class='wrapper'>

    <table class='container' cellpadding='0' cellspacing='0' border='0' align='center'>

        <tr>
            <td class='header'>

                <!-- Itt történik a varázslat: az src a 'PoharnokLogo' Content ID-ra (cid) hivatkozik -->
                <img class='logo' src='cid:PoharnokLogo' alt='Pohárnok'>

                <div class='title'>
                    Ajándék vár a kosaradban!
                </div>

                <div class='subtitle'>
                    Ne hagyd elveszni a kiválasztott termékeket.
                </div>

            </td>
        </tr>

        <tr>
            <td class='content'>

                <div class='text'>

                    <!-- ITT VAN A JAVÍTÁS: keresztnev helyett megszolitas -->
                    Kedves {megszolitas}!<br><br>

                    Észrevettük, hogy korábban nálunk hagytál egy kosarat.<br>
                    Szeretnénk segíteni a döntésben, ezért készítettünk neked egy
                    <strong>{amount} Ft-os ajándékkártyát</strong>.

                </div>

                <div class='gift-box'>

                    <div class='gift-label'>
                        A kuponkódod:
                    </div>

                    <div class='gift-code'>
                        {cardCode}
                    </div>

                </div>

                <div class='text'>

                    Használd fel a pénztárnál, és szerezd meg kedvenc italaid kedvezményesen.<br><br>

                    Az ajánlat korlátozott ideig érvényes.

                </div>

                <div align='center'>

                    <a href='http://20.93.113.186'
                       class='button'>

                        Vissza a kosárhoz

                    </a>

                </div>

            </td>
        </tr>

        <tr>
            <td class='footer'>

                Pohárnok

                <div class='divider'></div>

                Üdvözlettel a Pohárnok csapata 

            </td>
        </tr>

    </table>

</div>

</body>
</html>
";

                // 1. Létrehozzuk a HTML nézetet az e-mailhez
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                // 2. Kép betöltése a Solutionbõl (feltételezve, hogy csinálsz egy 'Images' mappát)
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "logo.jpg");

                if (File.Exists(imagePath))
                {
                    LinkedResource logo = new LinkedResource(imagePath, "image/jpg")
                    {
                        ContentId = "PoharnokLogo" // Ennek kell egyeznie az <img> src='cid:...' értékkel!
                    };

                    // 3. Hozzáadjuk a képet és a nézetet a levélhez
                    htmlView.LinkedResources.Add(logo);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[FIGYELMEZTETÉS] A kép nem található az alábbi útvonalon: {imagePath}");
                }

                mailMessage.AlternateViews.Add(htmlView);

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

            // Összes létezõ kártya lekérése az API-tól
            var letezoKartyak = await GetExistingGiftCardsAsync();

            int elküldve = 0;
            int kihagyva = 0;

            foreach (var item in lista.Where(x => x.Kijelolve))
            {
                // Ellenõrzés a listában
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

            dgvOrders.Columns["Kijelolve"].Width = 80;
            dgvOrders.Columns["Kijelolve"].HeaderText = "Kijelölve";

            dgvOrders.Columns["Nev"].Width = 120;
            dgvOrders.Columns["Nev"].HeaderText = "Név";

            dgvOrders.Columns["Azonosito"].Width = 65;
            dgvOrders.Columns["Azonosito"].HeaderText = "ID";

            dgvOrders.Columns["Keresztnev"].Visible = false;
            dgvOrders.Columns["Keresztnev"].HeaderText = "Keresztnév";

            dgvOrders.Columns["Osszeg"].Width = 90;
            dgvOrders.Columns["Osszeg"].HeaderText = "Összeg";
            dgvOrders.Columns["Osszeg"].DefaultCellStyle.Format = "N0";

            dgvOrders.Columns["Frissitve"].Width = 150;
            dgvOrders.Columns["Frissitve"].HeaderText = "Frissítve";

            dgvOrders.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvOrders.Columns["Email"].MinimumWidth = 200;

            UpdateButtonState();
        }

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

        private List<GiftCardDisplayModel> _osszesGiftCard = new List<GiftCardDisplayModel>();

        private async void RefreshGiftCardTable()
        {
            var apiValasz = await GetExistingGiftCardsAsync();

            // Konvertáljuk a csúnya stringeket szép objektumokká
            _osszesGiftCard = apiValasz.Select(c => new GiftCardDisplayModel
            {
                Kartyaszam = c.CardNumber,
                Osszeg = c.Amount,
                Elhasznalt = c.UsedAmount,
                Email = c.RecipientEmail,
                // Itt hívjuk meg a korábban megírt ParseJsonDate-et
                Datum = ParseJsonDate(c.IssueDateUtc)
            }).ToList();

            ApplyGiftCardFilters();
        }

        private void ApplyGiftCardFilters()
        {
            // A dateTimePicker3 és 4 használata a szûréshez
            DateTime mettol = dateTimePicker3.Value.Date;
            DateTime meddig = dateTimePicker4.Value.Date.AddDays(1).AddSeconds(-1); // A nap végéig

            var szurt = _osszesGiftCard
                .Where(x => x.Datum >= mettol && x.Datum <= meddig)
                .ToList();

            dgvGiftCards.DataSource = null;
            dgvGiftCards.DataSource = szurt;
            if (dgvGiftCards.Columns["Osszeg"] != null)
            {
                dgvGiftCards.Columns["Osszeg"].DefaultCellStyle.Format = "N0";
                dgvGiftCards.Columns["Osszeg"].Width = 90;
                dgvGiftCards.Columns["Osszeg"].HeaderText = "Összeg";
            }

            if (dgvGiftCards.Columns["Elhasznalt"] != null)
            {
                dgvGiftCards.Columns["Elhasznalt"].DefaultCellStyle.Format = "N0";
                dgvGiftCards.Columns["Elhasznalt"].Width = 80;
                dgvGiftCards.Columns["Elhasznalt"].HeaderText = "Elhasznált";
                dgvGiftCards.Columns["Kartyaszam"].HeaderText = "Kupon";
                dgvGiftCards.Columns["Kartyaszam"].Width = 260;

                dgvGiftCards.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvGiftCards.Columns["Email"].MinimumWidth = 180;
            }

            // Dátum formátum beállítása a táblázatban
            if (dgvGiftCards.Columns["Datum"] != null)
            {
                dgvGiftCards.Columns["Datum"].DefaultCellStyle.Format = "yyyy.MM.dd. HH:mm";
                dgvGiftCards.Columns["Datum"].HeaderText = "Létrehozva";
            }

            decimal osszesenElhasznalt = szurt.Sum(x => x.Elhasznalt);
            label8.Text = $"Összes kedvezmény:\n{osszesenElhasznalt:N0} Ft";
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
            // Csak a sorokra figyelünk, a fejlécet kihagyjuk
            if (e.RowIndex < 0) return;

            var row = dgvGiftCards.Rows[e.RowIndex];

            // FONTOS: Az oszlop neve most már "Elhasznalt", mert a GiftCardDisplayModel-ben így neveztük el!
            if (dgvGiftCards.Columns["Elhasznalt"] != null)
            {
                var usedAmountVal = row.Cells["Elhasznalt"].Value;

                if (usedAmountVal != null && decimal.TryParse(usedAmountVal.ToString(), out decimal usedAmount))
                {
                    // Ha az elhasznált összeg nagyobb, mint 0 
                    if (usedAmount > 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                        row.DefaultCellStyle.SelectionBackColor = Color.Green;
                    }
                    else
                    {
                        // Alaphelyzet visszaállítása (a virtuális megjelenítés miatt kötelezõ)
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    }
                }
            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            ApplyGiftCardFilters();
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            ApplyGiftCardFilters();
        }
    }
}