using System.Linq;
using Dapper;
using FluentAssertions;
using Npgsql;
using Xunit;

namespace MultiMappingTests
{
    public class MultiMappingTests
    {
        private const string connectionString =
            "User ID=postgres;Password=payment;Host=localhost;Port=5432;Database=invoicesManagementDev;";

        [Fact]
        public void Should_retrieve_invoices()
        {
            using NpgsqlConnection connection = new(connectionString);
            const string getInvoices = "SELECT * FROM invoices_management.invoices;";
            var invoices = connection.Query(getInvoices);
            invoices.Should().HaveCountGreaterThan(1);
        }
        
        [Fact]
        public void Should_retrieve_invoices_joined_payments()
        {
            using NpgsqlConnection connection = new(connectionString);
            const string getInvoices = "SELECT i.id, p.id, p.amount from invoices_management.invoices i LEFT JOIN invoices_management.payments p on i.id = p.invoice_id;";
            var invoicesJoined = connection.Query(getInvoices);
            invoicesJoined.Should().HaveCountGreaterThan(1);
        }
        
        [Fact]
        public void Should_retrieve_invoices_joined_payments_grouped()
        {
            using NpgsqlConnection connection = new(connectionString);
            const string getInvoices = "SELECT i.id as inv_id, p.id as pay_id, p.amount as amount from invoices_management.invoices i LEFT JOIN invoices_management.payments p on i.id = p.invoice_id;";
            var invoicesJoined = connection.Query(getInvoices).Where(r=>r.amount >0);
            invoicesJoined.Should().HaveCountGreaterThan(1);
            var invoicedGrouped=invoicesJoined.GroupBy(
                ij=>ij.inv_id,
                ij=>new {pay_id=ij.pay_id,amount=ij.amount},
                (key,group)=>new {inv_id=key, pays=group.ToList()}
                );
        }

    }
}