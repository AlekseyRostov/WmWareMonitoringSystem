using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OwinWebApiClient.Model;

namespace OwinWebApiClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Wait for the async stuff to run...
                Run().Wait();

                // Then Write Done...
                Console.WriteLine("");
                Console.WriteLine("Done! Press the Enter key to Exit...");
                Console.ReadLine();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static async Task Run()
        {
            // Create an http client provider:
            string hostUriString = "http://localhost:8080";
            var provider = new ApiClientProvider(hostUriString);
            string _accessToken;
            Dictionary<string, string> _tokenDictionary;

            try
            {
                // Pass in the credentials and retrieve a token dictionary:
                _tokenDictionary =
                    await provider.GetTokenDictionary("john@example.com", "password");
                _accessToken = _tokenDictionary["access_token"];

                // Write the contents of the dictionary:
                foreach (var kvp in _tokenDictionary)
                {
                    Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                    Console.WriteLine("");
                }

                // Create a company client instance:
                var baseUri = new Uri(hostUriString);
                var companyClient = new CompanyClient(baseUri, _accessToken);

                // Read initial companies:
                Console.WriteLine("Read all the companies...");
                var companies = await companyClient.GetCompaniesAsync();
                WriteCompaniesList(companies);

                int nextId = (from c in companies select c.Id).Max() + 1;

                Console.WriteLine("Add a new company...");
                var result = await companyClient.AddCompanyAsync(
                    new Company { Name = string.Format("New Company #{0}", nextId) });
                WriteStatusCodeResult(result);

                Console.WriteLine("Updated List after Add:");
                companies = await companyClient.GetCompaniesAsync();
                WriteCompaniesList(companies);

                Console.WriteLine("Update a company...");
                var updateMe = await companyClient.GetCompanyAsync(nextId);
                updateMe.Name = string.Format("Updated company #{0}", updateMe.Id);
                result = await companyClient.UpdateCompanyAsync(updateMe);
                WriteStatusCodeResult(result);

                Console.WriteLine("Updated List after Update:");
                companies = await companyClient.GetCompaniesAsync();
                WriteCompaniesList(companies);

                Console.WriteLine("Delete a company...");
                result = await companyClient.DeleteCompanyAsync(nextId - 1);
                WriteStatusCodeResult(result);

                Console.WriteLine("Updated List after Delete:");
                companies = await companyClient.GetCompaniesAsync();
                WriteCompaniesList(companies);
            }
            catch (AggregateException ex)
            {
                // If it's an aggregate exception, an async error occurred:
                Console.WriteLine(ex.InnerExceptions[0].Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }
            catch (Exception ex)
            {
                // Something else happened:
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }
        }

        private static void WriteCompaniesList(IEnumerable<Company> companies)
        {
            foreach (var company in companies)
            {
                Console.WriteLine("Id: {0} Name: {1}", company.Id, company.Name);
            }
            Console.WriteLine("");
        }

        private static void WriteStatusCodeResult(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("Opreation Succeeded - status code {0}", statusCode);
            }
            else
            {
                Console.WriteLine("Opreation Failed - status code {0}", statusCode);
            }
            Console.WriteLine("");
        }
    }
}