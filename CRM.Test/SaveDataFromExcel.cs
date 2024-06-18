using CRM.Module.BusinessObjects.Models.Excel;
using CRM.Utils;
using OfficeOpenXml;
using System.ComponentModel;


namespace CRM.Test
{
    public class SaveDataFromExcel
    {
        private const string TestFilePath = "C:\\Job\\Test\\CompanyTest.xlsx";

        public SaveDataFromExcel()
        {
            //ExcelPackage.LicenseContext = System.ComponentModel.LicenseContext.NonCommercial;
        }

        [Fact]
        public void TestHeaderIndexes()
        {
            // Arrange
            var headerNames = new List<string> { "CompanyName", "WebSite", "PhoneNumber", "BillingAddress", "ShippingAddress", "ShipToBilling" };

            CreateTestExcelFile(headerNames);
            var expectedHeaders = new Dictionary<string, int>
                {
                    { "CompanyName", 1 },
                    { "WebSite", 2 },
                    { "PhoneNumber", 3 },
                    { "BillingAddress", 4 },
                    { "ShippingAddress", 5 },
                    { "ShipToBilling", 6 }
                };

            // Act
            var headerIndexes = ExcelReader<ExcelCompany>.GetHeaderIndexes(TestFilePath);

            // Assert
            Assert.Equal(expectedHeaders, headerIndexes);
        }

        [Fact]
        public void TestReadFromExcel()
        {
            // Arrange
            var headerNames = new List<string> { "CompanyName", "WebSite", "PhoneNumber", "BillingAddress", "ShippingAddress", "ShipToBilling" };

            var expectedCompanies = new List<ExcelCompany>
                {
                    new ExcelCompany { CompanyName = "Company A", WebSite = "www.companya.com", PhoneNumber = "123456", BillingAddress = "Address A", ShippingAddress = "Address A", ShipToBilling = "true" },
                    new ExcelCompany { CompanyName = "Company B", WebSite = "www.companyb.com", PhoneNumber = "654321", BillingAddress = "Address B", ShippingAddress = "Address B", ShipToBilling = "flase" }
                };

            CreateTestExcelFile(headerNames, expectedCompanies);

            var headerIndexes = ExcelReader<ExcelCompany>.GetHeaderIndexes(TestFilePath);

            // Act
            var actualCompanies = ExcelReader<ExcelCompany>.ReadFromExcel(TestFilePath, (worksheet, row) =>
            {
                return new ExcelCompany
                {
                    CompanyName = worksheet.Cells[row, headerIndexes["CompanyName"]].Text,
                    WebSite = worksheet.Cells[row, headerIndexes["WebSite"]].Text,
                    PhoneNumber = worksheet.Cells[row, headerIndexes["PhoneNumber"]].Text,
                    BillingAddress = worksheet.Cells[row, headerIndexes["BillingAddress"]].Text,
                    ShippingAddress = worksheet.Cells[row, headerIndexes["ShippingAddress"]].Text,
                    ShipToBilling = worksheet.Cells[row, headerIndexes["ShipToBilling"]].Text
                };
            });

            // Assert
            Assert.Equal(expectedCompanies.Count, actualCompanies.Count);
            for (int i = 0; i < expectedCompanies.Count; i++)
            {
                Assert.Equal(expectedCompanies[i].CompanyName, actualCompanies[i].CompanyName);
                Assert.Equal(expectedCompanies[i].WebSite, actualCompanies[i].WebSite);
                Assert.Equal(expectedCompanies[i].PhoneNumber, actualCompanies[i].PhoneNumber);
                Assert.Equal(expectedCompanies[i].BillingAddress, actualCompanies[i].BillingAddress);
                Assert.Equal(expectedCompanies[i].ShippingAddress, actualCompanies[i].ShippingAddress);
                Assert.Equal(expectedCompanies[i].ShipToBilling, actualCompanies[i].ShipToBilling);
            }
        }

/*        [Fact]
        public void CreateCompanyFromExcel_Success()
        {
            // Arrange
            var excelCompanies = new List<ExcelCompany>
        {
            new ExcelCompany { CompanyName = "Company A", WebSite = "www.companya.com", PhoneNumber = "123456" },
            new ExcelCompany { CompanyName = "Company B", WebSite = "www.companyb.com", PhoneNumber = "654321" }
        };

            var mockObjectSpace = new Mock<IObjectSpace>();
            var mockCompany = new Mock<Company>();

            mockObjectSpace.Setup(os => os.CreateObject<Company>()).Returns(mockCompany.Object);

            var controller = new SaveDataFromExcelController();

            // Act
            controller.CreateCompanyFromExcel(excelCompanies);

            // Assert
            mockObjectSpace.Verify(os => os.CreateObject<Company>(), Times.Exactly(2)); // Verify that CreateObject was called twice
        }

        [Fact]
        public void CreateCompanyFromExcel_Exception()
        {
            // Arrange
            var excelCompanies = new List<ExcelCompany>
                {
                    new ExcelCompany { CompanyName = "Company A", WebSite = "www.companya.com", PhoneNumber = "123456" }
                };

            var mockObjectSpace = new Mock<IObjectSpace>();
            var mockCompany = new Mock<Company>();

            mockObjectSpace.Setup(os => os.CreateObject<Company>()).Returns(mockCompany.Object);

            var controller = new SaveDataFromExcelController();

            // Act
            void Action() => controller.CreateCompanyFromExcel(excelCompanies);

            // Assert
            Assert.Throws<AutoMapper.AutoMapperMappingException>(Action); // Verify that an exception is thrown
        }*/


        private void CreateTestExcelFile(List<string> headerNames, List<ExcelCompany> companies = null)
        {
            using (var package = new ExcelPackage(new FileInfo(TestFilePath)))
            {
                var workbook = package.Workbook;

                var existingWorksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Sheet1");
                if (existingWorksheet != null)
                {
                    workbook.Worksheets.Delete(existingWorksheet);
                }

                var worksheet = workbook.Worksheets.Add("Sheet1");

                for (int i = 0; i < headerNames.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerNames[i];
                }

                int row = 2;
                if (companies != null)
                {
                    foreach (var company in companies)
                    {
                        worksheet.Cells[row, 1].Value = company.CompanyName;
                        worksheet.Cells[row, 2].Value = company.WebSite;
                        worksheet.Cells[row, 3].Value = company.PhoneNumber;
                        worksheet.Cells[row, 4].Value = company.BillingAddress;
                        worksheet.Cells[row, 5].Value = company.ShippingAddress;
                        worksheet.Cells[row, 6].Value = company.ShipToBilling;
                        row++;
                    }
                }



                package.Save();
            }
        }
    }
}