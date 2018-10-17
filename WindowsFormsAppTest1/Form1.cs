using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsAppTest1
{
    public partial class Form1 : Form
    {
        int Num = 0;
        int month = 0;
        string M = "";
        SqlConnectionStringBuilder scsb;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 這行程式碼會將資料載入 'deliciousHouseDataSet2.temp' 資料表。您可以視需要進行移動或移除。
            this.tempTableAdapter1.Fill(this.deliciousHouseDataSet2.temp);
            // TODO: 這行程式碼會將資料載入 'deliciousHouseDataSet1.temp' 資料表。您可以視需要進行移動或移除。
            this.tempTableAdapter.Fill(this.deliciousHouseDataSet1.temp);

            // TODO: 這行程式碼會將資料載入 'deliciousHouseDataSet.Orderdetail' 資料表。您可以視需要進行移動或移除。
            this.orderdetailTableAdapter.Fill(this.deliciousHouseDataSet.Orderdetail);
            // TODO: 這行程式碼會將資料載入 'deliciousHouseDataSet.Menu' 資料表。您可以視需要進行移動或移除。
            this.menuTableAdapter.Fill(this.deliciousHouseDataSet.Menu);
            // TODO: 這行程式碼會將資料載入 'deliciousHouseDataSet.Customers' 資料表。您可以視需要進行移動或移除。
            this.customersTableAdapter.Fill(this.deliciousHouseDataSet.Customers);

            scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "DeliciousHouse";
            scsb.IntegratedSecurity = true;

            btnConfirm.Enabled = false;
            btn加入.Enabled = false;
            btn移除.Enabled = false;

            if (tbC_Name.Text == "")
            {
                btnC_save.Enabled = false;
            }
            if ((tbP_Name.Text == "") || (tbP_Price.Text == ""))
            {
                btnP_save.Enabled = false;
            }
            rbtnNoPay.Checked = true;
            rbtnNoTake.Checked = true;

        }

        private void btnC_add_Click(object sender, EventArgs e)
        {
            tbC_ID.Text = "";
            tbC_Name.Text = "";
            tbC_Phone.Text = "";
            tbC_Address.Text = "";
            dtpC_Bday.Value = DateTime.Now;
            tbC_Level.Text = "";
        }

        private void btnC_revise_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(tbC_ID.Text, out intID);
            if (intID > 0)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "update Customers set C_Name=@NewName,C_Phone=@NewPhone,C_Address=@NewAddress,C_Bday=@NewBirth,C_Level=@NewLevel where C_ID=@SearchID";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchID", intID);
                cmd.Parameters.AddWithValue("@NewName", tbC_Name.Text);
                cmd.Parameters.AddWithValue("@NewPhone", tbC_Phone.Text);
                cmd.Parameters.AddWithValue("@NewAddress", tbC_Address.Text);
                cmd.Parameters.AddWithValue("@NewBirth", (DateTime)dtpC_Bday.Value);
                cmd.Parameters.AddWithValue("@NewLevel", tbC_Level.Text);
                int rows = cmd.ExecuteNonQuery();
                con.Close();

            }

        }

        private void btnC_delete_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(tbC_ID.Text, out intID);

            if (intID > 0)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "delete from Customers where C_id = @SearchC_ID";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchC_ID", intID);

                int rows = cmd.ExecuteNonQuery();
                con.Close();

                tbC_ID.Text = "";
                tbC_Name.Text = "";
                tbC_Phone.Text = "";
                tbC_Address.Text = "";
                dtpC_Bday.Value = DateTime.Now;
                tbC_Level.Text = "";
                btnC_search_Click(null, null);
            }
            else
            {
                MessageBox.Show("無此ID");
            }
        }

        private void customersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.customersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.deliciousHouseDataSet);

        }

        private void btnP_add_Click(object sender, EventArgs e)
        {
            tbP_ID.Text = "";
            tbP_Name.Text = "";
            tbP_Price.Text = "";

        }

        private void btnP_revise_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(tbP_ID.Text, out intID);
            if (intID > 0)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "update Menu set P_Name=@NewName,P_Price=@NewPrice where P_ID=@SearchID";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchID", intID);
                cmd.Parameters.AddWithValue("@NewName", tbP_Name.Text);
                cmd.Parameters.AddWithValue("@NewPrice", tbP_Price.Text);

                int rows = cmd.ExecuteNonQuery();
                con.Close();

            }
        }

        private void btnP_delete_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(tbP_ID.Text, out intID);

            if (intID > 0)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "delete from Menu where P_id = @SearchP_ID";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchP_ID", intID);

                int rows = cmd.ExecuteNonQuery();
                con.Close();

                tbP_ID.Text = "";
                tbP_Name.Text = "";
                tbP_Price.Text = "";
                btnP_Search_Click(null, null);


            }
            else
            {
                MessageBox.Show("無此產品");
            }
        }

        private void btnC_save_Click(object sender, EventArgs e)
        {
            if (tbC_Name.Text != "")
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "if not exists (select * from Customers where C_Name=@C_Name and C_Phone=@C_Phone and C_Address=@C_Address) insert into Customers values(@C_Name,@C_Phone,@C_Address,@NewBirth,@NewLevel)";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@C_Name", tbC_Name.Text);
                cmd.Parameters.AddWithValue("@C_Phone", tbC_Phone.Text);
                cmd.Parameters.AddWithValue("@C_Address", tbC_Address.Text);
                cmd.Parameters.AddWithValue("@NewBirth", (DateTime)dtpC_Bday.Value);
                cmd.Parameters.AddWithValue("@NewLevel", tbC_Level.Text);
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                tbC_Name.Text = "";
                tbC_ID.Text = "";
                tbC_Phone.Text = "";
                tbC_Address.Text = "";
                tbC_Level.Text = "";
                dtpC_Bday.Value = DateTime.Now;

                btnC_search_Click(null, null);
            }
            else
            {
                MessageBox.Show("請輸入姓名");
            }

        }

        private void btnP_save_Click(object sender, EventArgs e)
        {
            if (tbP_Name.Text != "")
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "if not exists (select * from Menu where P_Name=@P_Name) insert into Menu values(@P_Name,@P_Price)";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@P_Name", tbP_Name.Text);
                cmd.Parameters.AddWithValue("@P_Price", tbP_Price.Text);
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                tbP_ID.Text = "";
                tbP_Name.Text = "";
                tbP_Price.Text = "";
                btnP_Search_Click(null, null);

            }
            else
            {
                MessageBox.Show("請輸入產品名稱");
            }
        }

        private void btnplus_Click(object sender, EventArgs e)
        {
            Num += 1;
            tbQty.Text = Num.ToString();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            Num -= 1;
            if (Num < 0)
            {
                Num = 0;
            }
            tbQty.Text = Num.ToString();

        }

        private void tbQty_TextChanged(object sender, EventArgs e)
        {
            if (tbQty.Text != "")
            {
                bool ifNum = Int32.TryParse(tbQty.Text, out Num);
                if ((ifNum == true) && (Num >= 0))
                {

                }
                else
                {
                    Num = 0;
                    tbQty.Text = "0";
                }
            }
            else
            {
                Num = 0;
            }
        }

        private void btn移除_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = " delete from temp where P_Name=@PName";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@PName", lboxOrder.Text);
            int rows = cmd.ExecuteNonQuery();

            string strSQL2 = "select t.P_Name,m.P_Price,t.Qty,m.P_Price*t.Qty as SubTotal from temp as t inner join Menu as m on t.P_Name=m.P_Name";
            string strSQL3 = "select sum(m.P_Price*t.Qty) as Sub from temp as t inner join Menu as m on t.P_Name=m.P_Name";
            SqlCommand cmd2 = new SqlCommand(strSQL2, con);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            string strMsg = "訂單號:  " + tbOrderNum.Text + "     客戶:" + cboxCustomer.Text + "\n***********************************\n";
            int i = 0;
            string strMsg2 = "";
            string strMsg3 = "";
            while (reader2.Read() == true)
            {
                i += 1;
                strMsg += string.Format("{0}  *  {1}   =   {2} 元\n", reader2["P_Name"], reader2["Qty"], reader2["SubTotal"]);
            }
            reader2.Close();
            SqlCommand cmd3 = new SqlCommand(strSQL3, con);
            SqlDataReader reader3 = cmd3.ExecuteReader();
            while (reader3.Read() == true)
            {
                strMsg2 = string.Format("{0}", reader3["sub"]);

            }
            if (strMsg2 != "")
            {
                double a = Convert.ToDouble(strMsg2);
                double b = 0.0;
                bool ifb = double.TryParse(tbDiscount.Text, out b);
                if (ifb == true)
                {
                    if ((b >= 0.0) && (b <= 10.0))
                    {
                        strMsg3 = (a * b / 10).ToString();
                        lbOrderMessage.Text = strMsg + "***********************************\n 折扣:" + tbDiscount.Text + "折       " + "總計: " + strMsg3 + "  元";
                    }
                    else
                    {
                        b = 10.0;
                        //tbDiscount.Text = "10.0";
                        strMsg3 = (a * b / 10).ToString();
                        lbOrderMessage.Text = strMsg + "***********************************\n 折扣:" + " 10.0 折       " + "總計: " + strMsg3 + "  元";
                    }

                }
                else
                {
                    b = 10.0;
                    //tbDiscount.Text = "10.0";
                    strMsg3 = (a * b / 10).ToString();
                    lbOrderMessage.Text = strMsg + "***********************************\n 折扣:" + " 10.0 折       " + "總計: " + strMsg3 + "  元";
                }
            }
            else
            {
                lbOrderMessage.Text = "訂單號:  " + tbOrderNum.Text + "     客戶:" + cboxCustomer.Text + "\n***********************************\n";
            }


            reader3.Close();
            con.Close();
            this.tempBindingSource.EndEdit();
            this.tempTableAdapter.Update(this.deliciousHouseDataSet1.temp);
            this.tempTableAdapter.Fill(this.deliciousHouseDataSet1.temp);
            this.tempBindingSource.MoveLast();

        }

        private void btn加入_Click(object sender, EventArgs e)
        {
            if(cboxCustomer.Text == "")
            {
                this.customersTableAdapter.Fill(this.deliciousHouseDataSet.Customers);
            }
            
            cboxCustomer.Enabled = false;
            dtbShipday.Enabled = false;
            if (tbQty.Text != "")
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "if not exists (select * from temp where P_Name=@P_Name) insert into temp (O_ID,P_Name,Qty,C_Name,O_Date) values(@O_ID,@P_Name,@Qty,@C_Name,@O_Date)";
                string strSQL2 = "select t.P_Name,m.P_Price,t.Qty,m.P_Price*t.Qty as SubTotal from temp as t inner join Menu as m on t.P_Name=m.P_Name";
                string strSQL3 = "select sum(m.P_Price*t.Qty) as Sub from temp as t inner join Menu as m on t.P_Name=m.P_Name";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@O_ID", tbOrderNum.Text);
                cmd.Parameters.AddWithValue("@P_Name", lboxMenu.Text);
                cmd.Parameters.AddWithValue("@C_Name", cboxCustomer.Text);
                cmd.Parameters.AddWithValue("@Qty", tbQty.Text);
                cmd.Parameters.AddWithValue("@O_Date", (DateTime)dtbShipday.Value);
                int rows = cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand(strSQL2, con);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                string strMsg = "訂單號:  " + tbOrderNum.Text + "     客戶:" + cboxCustomer.Text + "\n***********************************\n";
                int i = 0;
                string strMsg2 = "";
                string strMsg3 = "";
                while (reader2.Read() == true)
                {
                    i += 1;
                    strMsg += string.Format("{0}  *  {1}   =   {2} 元\n", reader2["P_Name"], reader2["Qty"], reader2["SubTotal"]);
                }
                reader2.Close();
                SqlCommand cmd3 = new SqlCommand(strSQL3, con);
                SqlDataReader reader3 = cmd3.ExecuteReader();
                while (reader3.Read() == true)
                {
                    strMsg2 = string.Format("{0}", reader3["sub"]);

                }
                double a = Convert.ToDouble(strMsg2);
                double b = 0.0;
                bool ifb = double.TryParse(tbDiscount.Text, out b);
                if (ifb == true)
                {
                    if ((b >= 0.0) && (b <= 10.0))
                    {
                        strMsg3 = (a * b / 10).ToString();
                        lbOrderMessage.Text = strMsg + "***********************************\n 折扣:" + tbDiscount.Text + "折       " + "總計: " + strMsg3 + "  元";
                    }
                    else
                    {
                        b = 10.0;
                        //tbDiscount.Text = "10.0";
                        strMsg3 = (a * b / 10).ToString();
                        lbOrderMessage.Text = strMsg + "***********************************\n 折扣:" + " 10.0 折       " + "總計: " + strMsg3 + "  元";
                    }

                }
                else
                {
                    b = 10.0;
                    //tbDiscount.Text = "10.0";
                    strMsg3 = (a * b / 10).ToString();
                    lbOrderMessage.Text = strMsg + "***********************************\n 折扣:" + " 10.0 折       " + "總計: " + strMsg3 + "  元";
                }

                reader3.Close();
                con.Close();

                this.tempBindingSource.EndEdit();
                this.tempTableAdapter.Update(this.deliciousHouseDataSet1.temp);
                this.tempTableAdapter.Fill(this.deliciousHouseDataSet1.temp);
                this.tempBindingSource.MoveLast();
            }



        }

        private void btnC_search_Click(object sender, EventArgs e)
        {

            lboxCSearch.Items.Clear();

            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = "select *from Customers where C_Name like @C_Name and C_Phone like @C_Phone and C_Address like @C_Address";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@C_Name", "%" + tbC_Name.Text + "%");
            cmd.Parameters.AddWithValue("@C_Phone", "%" + tbC_Phone.Text + "%");
            cmd.Parameters.AddWithValue("@C_Address", "%" + tbC_Address.Text + "%");
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lboxCSearch.Items.Add(reader["C_Name"]);
            }
            reader.Close();
            con.Close();
        }

        private void lboxCSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSearchName = lboxCSearch.SelectedItem.ToString();
            if (strSearchName != "")
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select *from customers where C_Name like @SearchName";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchName", "%" + strSearchName + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    tbC_ID.Text = string.Format("{0}", reader["C_ID"]);//中括號指定的為欄位名稱
                    tbC_Name.Text = string.Format("{0}", reader["C_Name"]);
                    tbC_Phone.Text = string.Format("{0}", reader["C_Phone"]);
                    tbC_Address.Text = string.Format("{0}", reader["C_Address"]);
                    tbC_Level.Text = string.Format("{0}", reader["C_Level"]);
                    dtpC_Bday.Value = (DateTime)reader["C_Bday"];//時間需要用Datetime隱藏轉換


                }
                else
                {
                    MessageBox.Show("查無此人");
                    tbC_ID.Text = "";
                    tbC_Name.Text = "";
                    tbC_Phone.Text = "";
                    tbC_Address.Text = "";
                    tbC_Level.Text = "";
                    dtpC_Bday.Value = DateTime.Now;

                }
                reader.Close();
                con.Close();
            }
        }

        private void btnP_Search_Click(object sender, EventArgs e)
        {
            lboxPSearch.Items.Clear();

            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = "select *from Menu where P_Name like @SearchName";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@SearchName", "%" + tbP_Name.Text + "%");
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lboxPSearch.Items.Add(reader["P_Name"]);
            }
            reader.Close();
            con.Close();
        }

        private void lboxPSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSearchName = lboxPSearch.SelectedItem.ToString();
            if (strSearchName != "")
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select *from Menu where P_Name like @SearchName";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchName", "%" + strSearchName + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    tbP_ID.Text = string.Format("{0}", reader["P_ID"]);//中括號指定的為欄位名稱
                    tbP_Name.Text = string.Format("{0}", reader["P_Name"]);
                    tbP_Price.Text = string.Format("{0}", reader["P_Price"]);
                }
                else
                {
                    MessageBox.Show("無此產品");
                    tbP_ID.Text = "";
                    tbP_Name.Text = "";
                    tbP_Price.Text = "";

                }
                reader.Close();
                con.Close();

            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            double a = 0.0;
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = "insert into temp2(O_ID,C_Name,O_Date,O_Take,O_Pay,O_Discount) values(@O_ID,@C_Name,@O_Date,@cboxTake,@cboxPay,@O_Discount) insert into orders select t.O_ID,c.C_ID,c.C_Name,O_Date,c.C_Phone,t.O_Pay,t.O_Take,t.O_Total,O_Discount from temp2 as t inner join Customers as c on t.C_Name=c.C_Name insert into orderdetail select t.O_ID,c.C_ID,c.C_Name,t.O_Date,m.P_ID,m.P_Name,t.Qty,m.P_Price,t.Qty*m.P_Price as SubTotal from Customers as c inner join temp as t on c.C_Name=t.C_Name inner join Menu as M on t.P_Name=m.P_Name delete from temp delete from temp2 update orders set O_Total = (select sum(SubTotal) from orderdetail where O_ID =@O_ID)*O_Discount/10 where O_ID=@O_ID";
            //string strSQL2 = "insert into orderdetail select t.O_ID,c.C_ID,c.C_Name,t.O_Date,m.P_ID,m.P_Name,t.Qty,m.P_Price,t.Qty*m.P_Price as SubTotal from Customers as c inner join temp as t on c.C_Name=t.C_Name inner join Menu as M on t.P_Name=m.P_Name";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@O_ID", tbOrderNum.Text);
            cmd.Parameters.AddWithValue("@C_Name", cboxCustomer.Text);
            cmd.Parameters.AddWithValue("@O_Date", (DateTime)dtbShipday.Value);
            cmd.Parameters.AddWithValue("@cboxTake", cboxTake.Checked);
            cmd.Parameters.AddWithValue("@cboxPay", cboxPay.Checked);
            bool ifDC = double.TryParse(tbDiscount.Text, out a);
            if (ifDC == true)
            {
                if ((a >= 0.0) && (a <= 10.0))
                {
                    cmd.Parameters.AddWithValue("@O_Discount", tbDiscount.Text);
                }
                else
                {
                    tbDiscount.Text = "10.0";
                    cmd.Parameters.AddWithValue("@O_Discount", tbDiscount.Text);
                }
            }
            else
            {
                tbDiscount.Text = "10.0";
                cmd.Parameters.AddWithValue("@O_Discount", tbDiscount.Text);
            }

            int rows = cmd.ExecuteNonQuery();
            con.Close();
            btnConfirm.Enabled = false;
            btn加入.Enabled = false;
            btn移除.Enabled = false;
            tbOrderNum.Text = "";
            tbQty.Text = "";
            lbOrderMessage.Text = "";
            btnCreateOrder.Enabled = true;
            cboxCustomer.Enabled = true;
            dtbShipday.Enabled = true;
            tbDiscount.Text = "";
            cboxPay.Checked = false;
            cboxTake.Checked = false;
            this.customersTableAdapter.Fill(this.deliciousHouseDataSet.Customers);
            this.tempBindingSource.EndEdit();
            this.tempTableAdapter.Update(this.deliciousHouseDataSet1.temp);
            this.tempTableAdapter.Fill(this.deliciousHouseDataSet1.temp);
            this.tempBindingSource.MoveLast();

        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            this.customersTableAdapter.Fill(this.deliciousHouseDataSet.Customers);
            this.menuTableAdapter.Fill(this.deliciousHouseDataSet.Menu);
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = "select dbo.GetNewOrderNo() as num";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read() == true)
            {
                tbOrderNum.Text = string.Format("{0}", reader["num"]);
            }
            reader.Close();
            con.Close();
            btnConfirm.Enabled = true;
            btn加入.Enabled = true;
            btn移除.Enabled = true;
            btnCreateOrder.Enabled = false;
            this.customersTableAdapter.Fill(this.deliciousHouseDataSet.Customers);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = "delete from temp";

            SqlCommand cmd = new SqlCommand(strSQL, con);
            int rows = cmd.ExecuteNonQuery();
            con.Close();
            btnConfirm.Enabled = false;
            btn加入.Enabled = false;
            btn移除.Enabled = false;
            tbOrderNum.Text = "";
            tbQty.Text = "";
            lbOrderMessage.Text = "";
            btnCreateOrder.Enabled = true;
            cboxCustomer.Enabled = true;
            dtbShipday.Enabled = true;
            tbDiscount.Text = "";
            cboxPay.Checked = false;
            cboxTake.Checked = false;

            this.customersTableAdapter.Fill(this.deliciousHouseDataSet.Customers);
            this.tempBindingSource.EndEdit();
            this.tempTableAdapter.Update(this.deliciousHouseDataSet1.temp);
            this.tempTableAdapter.Fill(this.deliciousHouseDataSet1.temp);
            this.tempBindingSource.MoveLast();
        }

        private void btnO_Search_Click(object sender, EventArgs e)
        {
            lboxOSearch.Items.Clear();
            string strSearchName = tbSpuerSearch.Text;
            if ((rbtnNoPay.Checked == true) && (rbtnNoTake.Checked == true))
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select *from orders where (O_ID like @O_ID or C_Name like @C_Name or C_Phone like @C_Phone) and O_Pay = 0 and O_Take = 0";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@O_ID", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Name", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Phone", "%" + strSearchName + "%");
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lboxOSearch.Items.Add(reader["O_ID"]);
                }
                reader.Close();
                con.Close();
            }
            else if ((rbtnPay.Checked == false) && (rbtnTake.Checked == true))
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select *from orders where (O_ID like @O_ID or C_Name like @C_Name or C_Phone like @C_Phone) and O_Pay = 0 and O_Take = 1";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@O_ID", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Name", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Phone", "%" + strSearchName + "%");
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lboxOSearch.Items.Add(reader["O_ID"]);
                }
                reader.Close();
                con.Close();
            }
            else if ((rbtnPay.Checked == true) && (rbtnTake.Checked == false))
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select *from orders where (O_ID like @O_ID or C_Name like @C_Name or C_Phone like @C_Phone) and O_Pay = 1 and O_Take = 0";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@O_ID", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Name", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Phone", "%" + strSearchName + "%");
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lboxOSearch.Items.Add(reader["O_ID"]);
                }
                reader.Close();
                con.Close();
            }
            else
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select *from orders where (O_ID like @O_ID or C_Name like @C_Name or C_Phone like @C_Phone) and O_Pay = 1 and O_Take = 1";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@O_ID", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Name", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Phone", "%" + strSearchName + "%");
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lboxOSearch.Items.Add(reader["O_ID"]);
                }
                reader.Close();
                con.Close();
            }

        }

        private void lboxOSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSearchName = lboxOSearch.SelectedItem.ToString();
            if (strSearchName != "")
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select *from orders where O_ID like @O_ID or C_Name like @C_Name or C_Phone like @C_Phone";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@O_ID", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Name", "%" + strSearchName + "%");
                cmd.Parameters.AddWithValue("@C_Phone", "%" + strSearchName + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    tbONumber.Text = string.Format("{0}", reader["O_ID"]);
                    tbOCName.Text = string.Format("{0}", reader["C_Name"]);
                    tbOCPhone.Text = string.Format("{0}", reader["C_Phone"]);
                    dtpOSday.Value = (DateTime)reader["O_Date"];
                    cboxOTake.Checked = (bool)reader["O_Take"];
                    cboxOPay.Checked = (bool)reader["O_Pay"];
                    tbOTotal.Text = string.Format("{0}", reader["O_Total"]);
                    tbODiscount.Text = string.Format("{0}", reader["O_Discount"]);

                }
                else
                {
                    MessageBox.Show("無此訂單");
                    tbONumber.Text = "";
                    tbOCName.Text = "";
                    tbOCPhone.Text = "";
                }
                reader.Close();
                con.Close();
            }
        }

        private void tbONumber_TextChanged(object sender, EventArgs e)
        {
            string strSearchName = tbONumber.Text;
            if (strSearchName != "")
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select P_Name as 產品名稱,P_Price as 單價,Qty as 數量,SubTotal as 小計 from orderdetail where O_ID=@O_ID";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@O_ID", tbONumber.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);
                dgvOrderDetail.DataSource = ds.Tables[0];

                con.Close();
            }
        }

        private void btnOrderDelete_Click(object sender, EventArgs e)
        {
            if (tbONumber.Text != "")
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "delete from orders where O_ID = @O_ID delete from orderdetail where O_ID = @O_ID";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@O_ID", tbONumber.Text);

                int rows = cmd.ExecuteNonQuery();
                con.Close();

                tbONumber.Text = "";
                tbOCName.Text = "";
                tbOCPhone.Text = "";
                tbOTotal.Text = "";
                dtpOSday.Value = DateTime.Now;

            }

        }

        private void tbC_Name_TextChanged(object sender, EventArgs e)
        {
            if (tbC_Name.Text != "")
            {
                btnC_save.Enabled = true;
                btnC_revise.Enabled = true;
            }
            else
            {
                btnC_save.Enabled = false;
                btnC_revise.Enabled = false;
            }
        }

        private void tbP_Name_TextChanged(object sender, EventArgs e)
        {
            if ((tbP_Name.Text != "") && (tbP_Price.Text != ""))
            {
                btnP_save.Enabled = true;
                btnP_revise.Enabled = true;
            }
            else
            {
                btnP_save.Enabled = false;
                btnP_revise.Enabled = false;
            }
        }

        private void tbP_Price_TextChanged(object sender, EventArgs e)
        {
            if ((tbP_Price.Text != "") && (tbP_Name.Text != ""))
            {
                btnP_save.Enabled = true;
                btnP_revise.Enabled = true;
            }
            else
            {
                btnP_save.Enabled = false;
                btnP_revise.Enabled = false;
            }
        }

        private void btnOrderUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = "update orders set O_Date=@O_Date,O_Pay=@O_Pay,O_Take=@O_Take where O_ID=@O_ID";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@O_ID",tbONumber.Text);
            cmd.Parameters.AddWithValue("@O_Date", (DateTime)dtpOSday.Value);
            cmd.Parameters.AddWithValue("@O_Pay", (bool)cboxOPay.Checked);
            cmd.Parameters.AddWithValue("@O_Take",(bool)cboxOTake.Checked);
            int rows = cmd.ExecuteNonQuery();
            con.Close();
        }

        private void btnMRevenue_Click(object sender, EventArgs e)
        {
            Revenue();
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = "select sum(SubTotal) as Revenue,count(distinct O_ID) as OrderCount from orderdetail where month(O_Date)=@month";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@month", M);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read() == true)
            {
                lbRevenue.Text = string.Format("{0:C0}", reader["Revenue"]);
                lbOrderCount.Text = string.Format("{0}", reader["OrderCount"]);
            }
            reader.Close();
            string strSQL2 = "select sum(d.SubTotal) as AccountYes from orderdetail as d inner join orders as o on d.O_ID= o.O_ID where month(d.O_Date)=@month and o.O_Pay=1";
            SqlCommand cmd2 = new SqlCommand(strSQL2, con);
            cmd2.Parameters.AddWithValue("@month", M);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            if (reader2.Read() == true)
            {                
                lbAccountsYes.Text = string.Format("{0:C0}", reader2["AccountYes"]);

            }
            reader2.Close();
            string strSQL3 = "select sum(d.SubTotal) as AccountNot from orderdetail as d inner join orders as o on d.O_ID= o.O_ID where month(d.O_Date)=@month and o.O_Pay=0";
            SqlCommand cmd3 = new SqlCommand(strSQL3, con);
            cmd3.Parameters.AddWithValue("@month", M);
            SqlDataReader reader3 = cmd3.ExecuteReader();
            if (reader3.Read() == true)
            {
                lbAccountNot.Text = string.Format("{0:C0}", reader3["AccountNot"]);
            }
            reader3.Close();
            con.Close();
        }

        private void btnRank_Click(object sender, EventArgs e)
        {
            Revenue();
            lboxRank.Items.Clear();
            lbContribution.Text = "";
            lbSalesVolume.Text = "";
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = "select P_Name,O_Date,sum(SubTotal) as Contribution,sum(Qty) SalesVolume from orderdetail group by P_Name,O_Date having month(O_Date)=@month order by 3 desc";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@month", M);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lboxRank.Items.Add(reader["P_Name"]);
            }
            reader.Close();
            con.Close();
        }

        private void lboxRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Revenue();
            string strSearchName = lboxRank.SelectedItem.ToString();
            if (strSearchName != "")
            {
                
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select P_Name,O_Date,sum(SubTotal) as Contribution,sum(Qty) SalesVolume from orderdetail group by P_Name,O_Date having P_Name like @P_Name and month(O_Date)=@month order by 3 desc";
                
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@P_Name","%"+ strSearchName+"%");
                cmd.Parameters.AddWithValue("@month",M);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    lbContribution.Text = string.Format("{0:C0}", reader["Contribution"]);
                    lbSalesVolume.Text = string.Format("{0}", reader["SalesVolume"]);
                }
                reader.Close();
                con.Close();
            }
        }
        void Revenue()
        {

            
            if (cboxMonth.SelectedIndex==0)
            {
                month = 1;
            }
            if (cboxMonth.SelectedIndex == 1)
            {
                month = 2;
            }
            if (cboxMonth.SelectedIndex == 2)
            {
                month = 3;
            }
            if (cboxMonth.SelectedIndex == 3)
            {
                month = 4;
            }
            if (cboxMonth.SelectedIndex == 4)
            {
                month = 5;
            }
            if (cboxMonth.SelectedIndex == 5)
            {
                month = 6;
            }
            if (cboxMonth.SelectedIndex == 6)
            {
                month = 7;
            }
            if (cboxMonth.SelectedIndex == 7)
            {
                month = 8;
            }
            if (cboxMonth.SelectedIndex == 8)
            {
                month = 9;
            }
            if (cboxMonth.SelectedIndex == 9)
            {
                month = 10;
            }
            if (cboxMonth.SelectedIndex == 10)
            {
                month = 11;
            }
            if (cboxMonth.SelectedIndex == 11)
            {
                month = 12;
            }
            switch (month)
            {
                case 1:
                    M = "1";
                    break;
                case 2:
                    M = "2";
                    break;
                case 3:
                    M = "3";
                    break;
                case 4:
                    M = "4";
                    break;
                case 5:
                    M = "5";
                    break;
                case 6:
                    M = "6";
                    break;
                case 7:
                    M = "7";
                    break;
                case 8:
                    M = "8";
                    break;
                case 9:
                    M = "9";
                    break;
                case 10:
                    M = "10";
                    break;
                case 11:
                    M = "11";
                    break;
                case 12:
                    M = "12";
                    break;

            }


        }

    }
}
