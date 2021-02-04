using Hospital.DAL;
using Hospital.WindowsForm.Models;
using Microsoft.EntityFrameworkCore;
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
    public partial class ResultTestForm : Form
    {
        /// <summary>
        ///     Стартова позиція GroupBox
        /// </summary>
        private int StartPos { get; set; } = 60;
        /// <summary>
        ///     На стільки зміщуються блоки GroupBox по осі y
        /// </summary>
        private int Dy { get; set; } = 100;
        /// <summary>
        ///     Номер активної сесії (сесія результат якой на даний момент відображається)
        /// </summary>
        private int currentSession = 1;
        private IQueryable<Session> sessions { get; set; }
        public ResultTestForm(IQueryable<Session> sessions)
        {
            this.sessions = sessions;
            InitializeComponent();
            Generate(GetSessionResult());
        }
        /// <summary>
        ///     Метод, який заповнює данними форму (заповнює лейбли, за жопомогою іншого методу формує GroupBox)
        ///     Також формує колекцію результатів
        /// </summary>
        /// <param name="session">Приймає параметром сесію</param>
        private void Generate(Session session)
        {

            /// Очищення групбоксів
            var groups = this.Controls.OfType<GroupBox>();
            
            foreach (var item in groups.Reverse()) 
            {
                this.Controls.Remove(item);
            }
            this.StartPos = 60;

            /// Додавання часу який витрачений на тест і запис його в Label
            TimeSpan begin = session.Begin.TimeOfDay;
            TimeSpan end = session.End.TimeOfDay;

            string begTime = begin.Hours.ToString() + ":" + begin.Minutes.ToString() + ":" + begin.Seconds.ToString();
            string endTime = end.Hours.ToString() + ":" + end.Minutes.ToString() + ":" + end.Seconds.ToString();

            this.lblBegin.Text = "Початок:" + begTime;
            this.lblEnd.Text = "Кінець:" + endTime;


            /// Формування блоків
            var blocks = session.Results.Select(x => new SessionQuestion
            {
                QuestionText = x.Answer.Question.Text,
                QuestionAnswer = x.Answer.Text,
                IsTrue = x.Answer.IsTrue
            });

            foreach (var item in blocks)
            {
                CreateGroupBox(item);
            }

            this.lblCountSession.Text = "Спроба: " + this.currentSession;

        }
        /// <summary>
        ///     Метод який повертає сесію користувача під номером currentSession
        /// </summary>
        /// <returns>Повертає сесію</returns>
        private Session GetSessionResult()
        {
            Session session = sessions.Select(x => new Session
            {
                Id = x.Id, 
                Begin = x.Begin,
                End = x.End,
                Marks = x.Marks,
                Results = x.Results.Select(y => new Result { 
                    Id = y.Id,
                    AnswerId = y.AnswerId,
                    SessionId = y.SessionId,
                    Session = y.Session,
                    Answer = new Answer 
                    {
                     Id = y.Answer.Id,
                     Question = y.Answer.Question,
                     QuestionId = y.Answer.QuestionId,
                     IsTrue = y.Answer.IsTrue,
                     Text = y.Answer.Text
                    },
                }).ToList(),
                User = x.User,
                UserId = x.UserId
            }).ToList()[this.currentSession-1];
            return session;
        }
        /// <summary>
        ///     Метод, який формує GroupBox
        /// </summary>
        /// <param name="question">Приймає модель даних потрібний для GroupBox</param>
        public void CreateGroupBox(SessionQuestion question) 
        {
            GroupBox gbResult = new GroupBox();
            Label lblAnswer = new Label();
            // 
            // lblAnswer
            // 
            lblAnswer.BackColor = Color.FromArgb(100, question.IsTrue ? Color.Green : Color.Red);
            lblAnswer.Location = new System.Drawing.Point(7, 37);
            lblAnswer.Name = "lblAnswer";
            lblAnswer.Size = new System.Drawing.Size(368, 30);
            lblAnswer.Text = question.QuestionAnswer;
            // 
            // gbResult
            // 
            gbResult.Location = new System.Drawing.Point(210, this.StartPos);
            gbResult.Name = "gbResult";
            gbResult.Size = new System.Drawing.Size(381, 75);
            gbResult.TabStop = false;
            gbResult.Text = question.QuestionText;
            gbResult.Controls.Add(lblAnswer);

            this.Controls.Add(gbResult);

            StartPos += Dy;

        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.currentSession < sessions.Count())
            {
                this.currentSession++;
                Session sess = GetSessionResult();
                Generate(sess);
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (this.currentSession > 1)
            {
                this.currentSession--;
                Session sess = GetSessionResult();
                Generate(sess);
            }
        }
    }
}
