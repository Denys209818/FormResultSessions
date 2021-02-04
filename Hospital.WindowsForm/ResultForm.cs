using Hospital.DAL;
using Hospital.WindowsForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Hospital.WindowsForm
{
    public partial class ResultForm : Form
    {
        private MyContext context { get; set; } = new MyContext();
        private int rightAnswer = 0;
        private int wrongAnswer = 0;
        public ResultForm(QuestionAnswerModelResult[] result)
        {
            InitializeComponent();
            for (int i = 0; i < result.Length; i++)
            {
                if(result[i].IsTrue)
                {
                    rightAnswer++;
                }
                else
                {
                    wrongAnswer++;
                }
            }
            lblCountQuestions.Text = $"Всього пройдено запитань: {result.Length}";
            lblRightAnswers.Text = $"Кількість правильних відповідей: {rightAnswer}";
            lblWrongAnswers.Text = $"Кількість неправильних відповідей: {wrongAnswer}";
            int mark = (rightAnswer * 100) / result.Length;
            lblMark.Text = $"Оцінка за проходження тесту: {mark}";


            if (UserAccount.user != null) 
            {
                //  Створює і додає в БД нову сесію
                Session session = new Session 
                {
                    Begin = UserAccount.Begin,
                    End = DateTime.Now,
                    UserId = UserAccount.user.Id,
                    Marks = mark,
                    
                };
                context.Sessions.Add(session);
                context.SaveChanges();
                //  Записує у БД відповіді, на які користувач відповів
                for (int i = 0; i < result.Length; i++) 
                {
                    context.Results.Add(
                        new Result 
                        {
                        AnswerId = result[i].Id,
                        SessionId = session.Id
                        }
                        );
                }
                context.SaveChanges();

            }
        }

        
    }
}
