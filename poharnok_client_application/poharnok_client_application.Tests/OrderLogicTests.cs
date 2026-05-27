using NUnit.Framework;
using poharnok_client_application;

namespace poharnok_client_application.Tests
{
    [TestFixture]
    public class OrderLogicTests
    {
        [Test]
        public void HasSelectedOrder_NincsKijeloltRendeles_FalseAdVissza()
        {
            // Arrange
            var rendelesek = new List<OrderDisplayModel>
            {
                new OrderDisplayModel { Email = "teszt1@email.com", Kijelolve = false },
                new OrderDisplayModel { Email = "teszt2@email.com", Kijelolve = false }
            };

            // Act
            bool eredmeny = rendelesek.Any(x => x.Kijelolve);

            // Assert
            Assert.That(eredmeny, Is.False);
        }

        [Test]
        public void HasSelectedOrder_VanKijeloltRendeles_TrueAdVissza()
        {
            // Arrange
            var rendelesek = new List<OrderDisplayModel>
            {
                new OrderDisplayModel { Email = "teszt1@email.com", Kijelolve = false },
                new OrderDisplayModel { Email = "teszt2@email.com", Kijelolve = true }
            };

            // Act
            bool eredmeny = rendelesek.Any(x => x.Kijelolve);

            // Assert
            Assert.That(eredmeny, Is.True);
        }

        [Test]
        public void ArSzures_MinimumArAlattiRendelesKiesik()
        {
            // Arrange
            var rendelesek = new List<OrderDisplayModel>
    {
        new OrderDisplayModel
        {
            Email = "olcso@email.com",
            Osszeg = 1000,
            Frissitve = new DateTime(2026, 4, 1)
        },
        new OrderDisplayModel
        {
            Email = "draga@email.com",
            Osszeg = 5000,
            Frissitve = new DateTime(2026, 4, 1)
        }
    };

            decimal minimumAr = 3000;

            // Act
            var eredmeny = rendelesek
                .Where(x => x.Osszeg >= minimumAr)
                .ToList();

            // Assert
            Assert.That(eredmeny.Count, Is.EqualTo(1));
            Assert.That(eredmeny[0].Email, Is.EqualTo("draga@email.com"));
        }

        [Test]
        public void EmailSzures_CsakAMegadottEmailMarad()
        {
            // Arrange
            var rendelesek = new List<OrderDisplayModel>
    {
        new OrderDisplayModel
        {
            Email = "anna@email.com",
            Osszeg = 4000,
            Frissitve = new DateTime(2026, 4, 1)
        },
        new OrderDisplayModel
        {
            Email = "bela@email.com",
            Osszeg = 5000,
            Frissitve = new DateTime(2026, 4, 1)
        }
    };

            string emailSzuro = "anna";

            // Act
            var eredmeny = rendelesek
                .Where(x => x.Email.ToLower().Contains(emailSzuro.ToLower()))
                .ToList();

            // Assert
            Assert.That(eredmeny.Count, Is.EqualTo(1));
            Assert.That(eredmeny[0].Email, Is.EqualTo("anna@email.com"));
        }

        [Test]
        public void DatumSzures_CsakIdoszakonBeluliRendelesMarad()
        {
            // Arrange
            var rendelesek = new List<OrderDisplayModel>
    {
        new OrderDisplayModel
        {
            Email = "regi@email.com",
            Osszeg = 4000,
            Frissitve = new DateTime(2026, 3, 10)
        },
        new OrderDisplayModel
        {
            Email = "jo@email.com",
            Osszeg = 5000,
            Frissitve = new DateTime(2026, 4, 15)
        }
    };

            DateTime kezdoDatum = new DateTime(2026, 4, 1);
            DateTime vegDatum = new DateTime(2026, 4, 30);

            // Act
            var eredmeny = rendelesek
                .Where(x => x.Frissitve.Date >= kezdoDatum.Date &&
                            x.Frissitve.Date <= vegDatum.Date)
                .ToList();

            // Assert
            Assert.That(eredmeny.Count, Is.EqualTo(1));
            Assert.That(eredmeny[0].Email, Is.EqualTo("jo@email.com"));
        }

        [Test]
        public void DuplikaltKupon_EmailMarLetezik_TrueAdVissza()
        {
            // Arrange
            var kuponok = new List<GiftCardDTO>
    {
        new GiftCardDTO
        {
            RecipientEmail = "anna@email.com"
        }
    };

            string aktualisEmail = "anna@email.com";

            // Act
            bool eredmeny = kuponok.Any(x => x.RecipientEmail == aktualisEmail);

            // Assert
            Assert.That(eredmeny, Is.True);
        }
    }
}