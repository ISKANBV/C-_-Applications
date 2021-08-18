using CrmBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrmUI
{
    class CashBoxView
    {
        CashDesk cashDesk;

        public Label CashDeskName { get; set; }
        public NumericUpDown Price { get; set; }
        public ProgressBar QueueLenght { get; set; }
        public Label LeaveCustomersCount { get; set; }

        public CashBoxView(CashDesk cashDesk,int number,int x,int y)
        {
            this.cashDesk = cashDesk;

            CashDeskName = new Label();
            Price = new NumericUpDown();
            QueueLenght = new ProgressBar();
            LeaveCustomersCount = new Label();


            CashDeskName.AutoSize = true;
            CashDeskName.Location = new System.Drawing.Point(x , y);
            CashDeskName.Name = "label" + number;
            CashDeskName.Size = new System.Drawing.Size(46, 17);
            CashDeskName.TabIndex = number;
            CashDeskName.Text =cashDesk.ToString();
       
            Price.Location = new System.Drawing.Point(x + 70, y);
            Price.Name = "numericUpDown" + number;
            Price.Size = new System.Drawing.Size(120, 22);
            Price.TabIndex = number;
            Price.Maximum = 1000000000000000000;

            QueueLenght.Location = new System.Drawing.Point(x + 200,y);
            QueueLenght.Maximum = cashDesk.MaxQueueLenght;
            QueueLenght.Name = "progressBar" + number;
            QueueLenght.Size = new System.Drawing.Size(100, 20);
            QueueLenght.TabIndex = number;
            QueueLenght.Value = 1;

            LeaveCustomersCount.AutoSize = true;
            LeaveCustomersCount.Location = new System.Drawing.Point(x + 350, y);
            LeaveCustomersCount.Name = "label2" + number;
            LeaveCustomersCount.Size = new System.Drawing.Size(46, 17);
            LeaveCustomersCount.TabIndex = number;
            LeaveCustomersCount.Text = "";


            cashDesk.CheckClosed += CashDesk_CheckClosed;

        }

        private void CashDesk_CheckClosed(object sender, Check e)
        {
            
            Price.Invoke((Action)delegate 
            { 
                Price.Value += e.Price;
                QueueLenght.Value = cashDesk.Count;
                LeaveCustomersCount.Text = cashDesk.ExitCustomer.ToString();
            });

        }
    }
}
