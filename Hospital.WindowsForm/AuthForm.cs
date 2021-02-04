using Hospital.DAL;
using Hospital.WindowsForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hospital.WindowsForm
{
    public partial class AuthForm : Form
    {
        private MyContext context { get; set; }
        public AuthForm(MyContext context)
        {
            this.context = context;
            InitializeComponent();
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var query = this.context.Users.AsQueryable();
            try
            {
                if (!string.IsNullOrEmpty(this.txtLogin.Text))
                {
                    var el = query.Where(x => x.Login == this.txtLogin.Text).FirstOrDefault();
                    if (el != null)
                    {
                        if (el.Password == this.txtPassword.Text)
                        {
                            MessageBox.Show("Вхід схвалено!");
                            UserAccount.user = el;
                            this.Close();
                        }
                        else
                        {
                            throw new Exception("Неправильний пароль!");
                        }
                    }
                    else 
                    {
                        throw new Exception("Аккаунта не знайдено!");
                    }
                }
            } 
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
