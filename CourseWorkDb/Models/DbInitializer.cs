using System;
using System.Linq;
using System.Text;

namespace CourseWorkDb.Models
{
    public class DbInitializer
    {
        public static void Initialize(RationingDbContext db)
        {
            db.Database.EnsureCreated();

            if (db.Companies.Any())
            {
                return;
            }

            int onSideOne = 100;
            int onSideMany = 1000;

            Random random = new Random();

            for (int i = 0; i < onSideOne; i++)
            {
                db.Companies.Add(new Tables.Company
                {
                    CompanyName = CreateRandomString(random),
                    FormOfOwnership = CreateRandomString(random),
                    HeadName = CreateRandomString(random),
                    ActivityType = CreateRandomString(random),
                });
                db.ProductTypes.Add(new Tables.ProductType
                {
                    ProductionType = CreateRandomString(random)
                });
                db.Products.Add(new Tables.Product
                {
                    ProductName = CreateRandomString(random),
                    MeasureUnit = CreateRandomString(random),
                    Features = CreateRandomString(random),
                    //Photo = 
                });
            }
            db.SaveChanges();

            for (int i = 0; i < onSideMany; i++)
            {
                int quarter = random.Next(1, 5);
                int year = random.Next(1970, 2050);
                int companyId = i % onSideOne + 1;
                int productId = i % onSideOne + 1;
                db.Outputs.Add(new Tables.Output
                {
                    OutputPlan = random.Next(1, 1000),
                    OutputFact = (int)(random.NextDouble() * random.Next(100, 1000000)),
                    Quarter = (short)quarter,
                    Year = (short)year,
                    CompanyId = companyId,
                    ProductId = productId
                });
                db.Releases.Add(new Tables.Release
                {
                    ReleasePlan = random.Next(1, 1000),
                    ReleaseFact = (int)(random.NextDouble() * random.Next(100, 1000000)),
                    Quarter = (short)quarter,
                    Year = (short)year,
                    CompanyId = companyId,
                    ProductId = productId,

                });
            }

            db.SaveChanges();
        }

        private static string CreateRandomString(Random random)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзиёклмнопрстуфхцчшщъыьэюяabcdefghijklmnopqrstuvwxyz";

            StringBuilder builder = new StringBuilder();
            for (int i = 5; i <= 30; i++)
            {
                builder.Append(letters[random.Next(0, letters.Length - 1)]);
            }
            return builder.ToString();
        }

    }
}
