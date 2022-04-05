using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace MultiMappingTests
{
    public class LinqTests
    {
        private List<Invoice> invoices = new();
        private List<Payment> payments = new();
        public LinqTests()
        {
            SeedList();
        }

        private void SeedList()
        {
            invoices.Add(new Invoice(1, "uno"));
            invoices.Add(new Invoice(2, "due"));
            invoices.Add(new Invoice(3, "tre"));
            payments.Add(new (1,101,"1-1"));
            payments.Add(new (1,102,"1-2"));
            payments.Add(new (2,201,"2-1"));
        }

        [Fact]
        public void Shoul_collect_all_payments_for_each_invoice()
        {
            var inner = invoices.Join(
                payments,
                i => i.Id,
                p => p.InvoiceId,
                (i, p) => new
                {
                    InvoiceId=i.Id,
                    InvoiceTitle=i.Title,
                    PaymntAmount=p.Amount,
                    PaymentDescription=p.Description
                });
            inner.Should().HaveCount(3);

            var outerLeft = invoices.GroupJoin(
                payments,
                i => i.Id,
                p => p.InvoiceId,
                (i, p) => new
                {
                    Invoice=i,
                    Payments=p
                });
            outerLeft.Should().HaveCount(3);

            var exploded = outerLeft.SelectMany(
                ol => ol.Payments,
                (ol, p) => new {ol.Invoice.Id, p.Amount, p.Description}
            );

            var explodedSum = outerLeft.Select(
                ol => new{Invoiceid= ol.Invoice.Id, TotalAmount = ol.Payments.Sum(p=>p.Amount)}
                );
                

            
        }
    }
}