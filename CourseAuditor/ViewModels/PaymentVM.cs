﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.ViewModels.Dialogs;
using CourseAuditor.Views;
using System.Data.Entity;

namespace CourseAuditor.ViewModels
{
    public class PaymentVM: BaseVM, IDialogRequestClose
    {

        private Student _SelectedStudent;
        public Student SelectedStudent
        {
            get
            {
                return _SelectedStudent;
            }
            set
            {
                _SelectedStudent = value;
                OnPropertyChanged("SelectedStudent");
                LoadStudentInfo();
            }
        }

        private double _Sum;
        public double Sum
        {
            get
            {
                return _Sum;
            }
            set
            {
                _Sum = Convert.ToDouble(value);
                OnPropertyChanged("Sum");
                CalculateActualSum();
            }
        }

        void CalculateActualSum()
        {
            if (Discount > 1) Discount = 1;
            if (Discount < 0) Discount = 0;

            if (Discount == 1) ActualSum = 0;
            else ActualSum = Math.Round(Sum / (1 - Discount), 2);
        }

        private double _ActualSum;
        public double ActualSum
        {
            get
            {
                return _ActualSum;
            }
            set
            {
                try
                {
                    _ActualSum = Convert.ToDouble(value);
                }
                catch
                {
                    _ActualSum = 0;
                }
                OnPropertyChanged("ActualSum");
            }
        }

        private PaymentType _Type;
        public PaymentType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                OnPropertyChanged("Type");
            }
        }

        private double _Discount;
        public double Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                _Discount = Convert.ToDouble(value);
                OnPropertyChanged("Discount");
                CalculateActualSum();
            }
        }

        private string _StudentInfo;
        public string StudentInfo
        {
            get
            {
                return _StudentInfo;
            }
            set
            {
                _StudentInfo = value;
                OnPropertyChanged("StudentInfo");
            }
        }

        private void AddPayment()
        {
            CalculateActualSum();
            SelectedStudent.Balance += Sum;
            using (var _context = new ApplicationContext())
            {
                var payment = new Payment();
                payment.Discount = Discount;
                payment.Sum = Sum;
                payment.Student_ID = SelectedStudent.ID;
                payment.Date = DateTime.Now;
                payment.Type = Type;
                _context.Payments.Add(payment);
                var student = _context.Students.First(x => x.ID == SelectedStudent.ID);
                _context.Entry(student).CurrentValues.SetValues(SelectedStudent);
                _context.SaveChanges();
                SelectedStudent = student;
                EventsManager.RaiseObjectChangedEvent(payment, ChangeType.Added);
            }
            
        }

   

        private ICommand _AddPaymentCommand;
        public ICommand AddPaymentCommand =>
            _AddPaymentCommand ??
            (_AddPaymentCommand = new RelayCommand(
                (obj) =>
                {
                    AddPayment();
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                },
                (obj) =>
                {
                    return true;
                }
        ));

        private ICommand _ChangeDiscount;

        public ICommand ChangeDiscount =>
            _ChangeDiscount ??
            (_ChangeDiscount = new RelayCommand(
                (obj) =>
                {
                    if (obj != null)
                        try
                        {
                            int type = Convert.ToInt32(obj);
                            switch (type)
                            {
                                case 0:
                                    Type = PaymentType.OneTime;
                                    Discount = 0;
                                    break;
                                case 1:
                                    Type = PaymentType.Month;
                                    Discount = 0.1;
                                    break;
                                case 2:
                                    Type = PaymentType.Module;
                                    Discount = 0.2;
                                    break;
                                default:
                                    Type = PaymentType.Arbitraty;
                                    break;
                            }
                        }
                        catch
                        {
                            Type = PaymentType.Arbitraty;
                            Discount = 0;
                        }
                }
                ));

        void LoadStudentInfo()
        {
            StudentInfo = $"Студент: {SelectedStudent.Person.FullName}.\n" +
                $"Курс: {SelectedStudent.Module.Group.Course.Name}.\n" +
                $"Группа: {SelectedStudent.Module.Group.Title}.\n" +
                $"Модуль: {SelectedStudent.Module.Number}.\n" +
                $"Текущий баланс: {SelectedStudent.Balance}";
        }

        public PaymentVM(Student selectedStudent)
        {
            Type = PaymentType.OneTime;
            using (var _context = new ApplicationContext())
            {
                SelectedStudent = _context.Students
                    .Include(x => x.Person)
                    .Include(x => x.Module.Group.Course)
                    .FirstOrDefault(x => x.ID == selectedStudent.ID);
            }
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
