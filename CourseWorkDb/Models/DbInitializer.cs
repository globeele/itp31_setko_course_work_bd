using CourseWorkDb.Models.Tables;
using System;
using System.Linq;
using System.Text;

namespace CourseWorkDb.Models
{
    public static class DbInitializer
    {
        public static void Initialize(RationingDbContext db)
        {
            db.Database.EnsureCreated();

           // db = new RationingDbContext();

            if (db.Companies.Any())
            {
                return;
            }

            int onSideOne = 100;
            int onSideMany = 1000;

            Random random = new Random();

            Company con = new Company();

            con.FormOfOwnership = CreateRandomString(random);

            for (int i = 0; i < onSideOne; i++)
            {
                //db.Companies.Add(new Company
                //{
                //    CompanyName = CreateRandomString(random),
                //    FormOfOwnership = CreateRandomString(random),
                //    HeadName = CreateRandomString(random),
                //    ActivityType = CreateRandomString(random),
                //});
                db.ProductTypes.Add(new ProductType
                {
                    ProductionType = CreateRandomString(random)
                });
                //db.Products.Add(new Product
                //{
                //    ProductName = CreateRandomString(random),
                //    MeasureUnit = CreateRandomString(random),
                //    Features = CreateRandomString(random),
                //    Photo = null
                //});
            }
            //db.SaveChanges();

            for (int i = 0; i < onSideMany; i++)
            {
                int quarter = random.Next(1, 5);
                int year = random.Next(1970, 2050);
                int companyId = i % onSideOne + 1;
                int productId = i % onSideOne + 1;

                //db.Outputs.Add(new Output
                //{
                //    OutputPlan = random.Next(1, 1000),
                //    OutputFact = (int)(random.NextDouble() * random.Next(100, 1000000)),
                //    Quarter = (short)quarter,
                //    Year = (short)year,
                //    CompanyId = companyId,
                //    ProductId = productId
                //});
                //db.Releases.Add(new Release
                //{
                //    ReleasePlan = random.Next(1, 1000),
                //    ReleaseFact = (int)(random.NextDouble() * random.Next(100, 1000000)),
                //    Quarter = (short)quarter,
                //    Year = (short)year,
                //    CompanyId = companyId,
                //    ProductId = productId,

                //});
            }

            //db.SaveChanges();
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
