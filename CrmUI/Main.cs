using CrmBL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrmUI
{
    public partial class Main : Form
    {
        CrmContext db;
        Cart cart;
        Customer customer;
        CashDesk cashDesk;

        public Main()
        {
            InitializeComponent();
            db = new CrmContext();

            cart = new Cart(customer);
            cashDesk = new CashDesk(1, db.Sellers.FirstOrDefault(),db)
            {
                IsModel = false
            };
        }
        private void ProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var CatalogProduct = new Catalog<Product>(db.Products,db);
            CatalogProduct.Show();
        }

        private void SellerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var CatalogSeller = new Catalog<Seller>(db.Sellers,db);
            CatalogSeller.Show();
        }

        private void CustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var CatalogCustomer = new Catalog<Customer>(db.Customers,db);
            CatalogCustomer.Show();
        }

        private void CheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var CatalogCheck = new Catalog<Check>(db.Checks,db);
            CatalogCheck.Show();
        }

        private void customerAddToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var form = new CustomerForm();
            if(form.ShowDialog() == DialogResult.OK)
            {
                db.Customers.Add(form.Customer);
                db.SaveChanges();
            }
        }

        private void sellerAddToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new SellerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                db.Sellers.Add(form.Seller);
                db.SaveChanges();
            }
        }

        private void productAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ProductForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                db.Products.Add(form.Product);
                db.SaveChanges();
            }
        }

        private void modelləşdirməToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ModelForm();
            form.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {

            Task.Run(() =>
            {
                listBox1.Invoke((Action)delegate
                {
                    listBox1.Items.AddRange(db.Products.ToArray());
                    UpdateLists();
                });
            });

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem is Product product)
            {
                cart.Add(product);
                listBox2.Items.Add(product);
                UpdateLists();
            }
        }

        private void UpdateLists()
        {
            listBox2.Items.Clear();
            listBox2.Items.AddRange(cart.GetAll().ToArray());
            label1.Text = "Ümumi:" + cart.Price;

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = new Login();
            form.ShowDialog();
            if(form.DialogResult == DialogResult.OK)
            {
                var tempCustomer = db.Customers.FirstOrDefault(c => c.Name.Equals(form.Customer.Name));
                if (tempCustomer != null)
                {
                    customer = tempCustomer;
                    

                }
                else
                {
                    db.Customers.Add(form.Customer);
                    db.SaveChanges();
                    customer = form.Customer;
                }

                cart.Customer = customer;
            }
            linkLabel1.Text = $"Salam,{customer.Name}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(customer != null)
            {
                cashDesk.Enqueue(cart);
                var price = cashDesk.Dequeue();
                listBox2.Items.Clear();
                cart = new Cart(customer);

                MessageBox.Show("Alış uğurla başa çatıb. Mebleg:  " + price, "Alış tamamlandı",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Zəhmət olmasa daxil olun!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
