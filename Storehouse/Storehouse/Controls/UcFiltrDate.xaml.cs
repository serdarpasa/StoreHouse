using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Storehouse.Controls
{    
    public partial class UcFiltrDate
    {
        public UcFiltrDate()
        {
            InitializeComponent();
            rbtn_tm.IsChecked = true;            
        }

        public event EventHandler SelectedDateChanged;

        private DateTime? _dateBegin;
        private DateTime? _dateEnd;
      
      
        public DateTime? DateBegin
        {
            get
            {
                var dt = new DateTime();
                if (_dateBegin != null)
                {
                    dt = dt.AddDays(_dateBegin.Value.Day - 1);
                    dt = dt.AddMonths(_dateBegin.Value.Month - 1);
                    dt = dt.AddYears(_dateBegin.Value.Year - 1);
                    return dt;
                }
                else return null;
                
            }
            set
            {
                if (value!=_dateBegin)
                {
                    _dateBegin = value;
                    dtp_begin.SelectedDate = _dateBegin;                  
                }
            }
        }

        public string FiltrPeriod
        {
            get
            {
                var rez = string.Empty;
                #region месяц
                if (((rbtn_tm.IsChecked == true) || (rbtn_pm.IsChecked == true))&& (dtp_begin.SelectedDate!=null))
                {
                    switch (dtp_begin.SelectedDate.Value.Month)
                    {
                        case 1:
                            {
                                rez = "январь";
                                break;
                            }
                        case 2:
                            {
                                rez = "февраль";
                                break;
                            }
                        case 3:
                            {
                                rez = "март";
                                break;
                            }
                        case 4:
                            {
                                rez = "апрель";
                                break;
                            }
                        case 5:
                            {
                                rez = "май";
                                break;
                            }
                        case 6:
                            {
                                rez = "июнь";
                                break;
                            }
                        case 7:
                            {
                                rez = "июль";
                                break;
                            }
                        case 8:
                            {
                                rez = "август";
                                break;
                            }
                        case 9:
                            {
                                rez = "сентябрь";
                                break;
                            }
                        case 10:
                            {
                                rez = "октябрь";
                                break;
                            }
                        case 11:
                            {
                                rez = "ноябрь";
                                break;
                            }
                        case 12:
                            {
                                rez = "декабрь";
                                break;
                            }
                    }
                    rez = "за " + rez + " "+dtp_begin.SelectedDate.Value.Year.ToString() + "г";
                }
                #endregion

                #region год

                if (((rbtn_tg.IsChecked == true)||(rbtn_pg.IsChecked==true))&& (dtp_begin.SelectedDate!=null))
                {
                    rez = "за " + dtp_begin.SelectedDate.Value.Year.ToString() + " г";
                }

                #endregion

                #region квартал

                if (((rbtn_tk.IsChecked == true) || (rbtn_pk.IsChecked == true)) && (dtp_begin.SelectedDate!=null))
                {
                    rez = "за ";
                    switch (dtp_begin.SelectedDate.Value.Month)
                    {
                        case 1:
                        case 2:
                        case 3:
                            {
                                rez += "I квартал " + dtp_begin.SelectedDate.Value.Year.ToString() + " г";
                                break;
                            }
                        case 4:
                        case 5:
                        case 6:
                            {
                                rez += "II квартал " + dtp_begin.SelectedDate.Value.Year.ToString() + " г";
                                break;
                            }
                        case 7:
                        case 8:
                        case 9:
                            {
                                rez += "III квартал " + dtp_begin.SelectedDate.Value.Year.ToString() + " г";
                                break;
                            }
                        case 10:
                        case 11:
                        case 12:
                            {
                                rez += "IV квартал " + dtp_begin.SelectedDate.Value.Year.ToString() + " г";
                                break;
                            }
                    }
                }

                #endregion

                #region другое
                if (rbtn_drugoe.IsChecked==true)
                {
                    rez = "с " + (dtp_begin.SelectedDate != null ? dtp_begin.SelectedDate.Value.ToShortDateString() : " __ ") +
                          " по " + (dtp_end.SelectedDate != null ? dtp_end.SelectedDate.Value.ToShortDateString() : " __ ");
                }

                #endregion

                return rez;
            }
        }

        public DateTime? DateEnd
        {
            get
            {
                var dt = new DateTime();
                if (_dateEnd != null)
                {
                    dt = dt.AddYears(_dateEnd.Value.Year - 1);
                    dt = dt.AddMonths(_dateEnd.Value.Month - 1);
                    dt = dt.AddDays(_dateEnd.Value.Day - 1);
                   
                    
                    dt = dt.AddHours(22);
                    dt = dt.AddMinutes(58);
                    return dt;
                }
                else return null;
            }
            set
            {
                if (value != _dateEnd)
                {
                    _dateEnd = value;
                    dtp_end.SelectedDate = _dateEnd;                   
                }
            }
        }

        #region ФИЛЬТР ПО ДАТЕ

        private void rbtn_drugoe_Checked(object sender, RoutedEventArgs e)
        {
            lb_drugoe.Foreground = new SolidColorBrush(Colors.Red);
            rbtn_drugoe.BorderBrush = new SolidColorBrush(Colors.Red);
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);
           
        }

        private void rbtn_drugoe_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_drugoe.Foreground = new SolidColorBrush(Colors.Black);
            rbtn_drugoe.BorderBrush = new SolidColorBrush(Colors.Silver);
        }

        private void rbtn_tm_Checked(object sender, RoutedEventArgs e)
        {
            lb_tekuschiy.Foreground = new SolidColorBrush(Colors.Red);
            lb_mesyc.Foreground = new SolidColorBrush(Colors.Red);
            var sd = DateTime.Now;
            
            var d1 = new DateTime(sd.Year,sd.Month,1);

            var d2 = d1.AddMonths(1).AddDays(-1);

            dtp_begin.SelectedDate = d1;

            dtp_end.SelectedDate = d2;
           
            rbtn_tm.BorderBrush = new SolidColorBrush(Colors.Red);
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);
        }

        private void rbtn_tm_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_tekuschiy.Foreground = new SolidColorBrush(Colors.Black);
            lb_mesyc.Foreground = new SolidColorBrush(Colors.Black);
            rbtn_tm.BorderBrush = new SolidColorBrush(Colors.Silver);
        }

        private void rbtn_tg_Checked(object sender, RoutedEventArgs e)
        {
            lb_tekuschiy.Foreground = new SolidColorBrush(Colors.Red);
            lb_god.Foreground = new SolidColorBrush(Colors.Red);

            var sd = DateTime.Now;

            var d1 = new DateTime(sd.Year,1,1);          
        
            var d2 = d1.AddMonths(11).AddDays(30);

                       
            dtp_begin.SelectedDate = d1;
         
            dtp_end.SelectedDate = d2;
            rbtn_tg.BorderBrush = new SolidColorBrush(Colors.Red);
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);
        }

        private void rbtn_tg_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_tekuschiy.Foreground = new SolidColorBrush(Colors.Black);
            lb_god.Foreground = new SolidColorBrush(Colors.Black);
            rbtn_tg.BorderBrush = new SolidColorBrush(Colors.Silver);
        }

        private void rbtn_pm_Checked(object sender, RoutedEventArgs e)
        {
            lb_pred.Foreground = new SolidColorBrush(Colors.Red);
            lb_mesyc.Foreground = new SolidColorBrush(Colors.Red);
            var sd = DateTime.Now;
            var month = sd.Month;
            var year = sd.Year;

            int prevMonth;
            if (month < 2)
            {
                prevMonth = 12;
                year--;

            }
            else
               prevMonth = month-1;


            var d1 = new DateTime(year,prevMonth,1);

            var d2 = d1;           
            d2 = d2.AddMonths(1).AddDays(-1);

            dtp_begin.SelectedDate = d1;

            dtp_end.SelectedDate = d2;
          
            rbtn_pm.BorderBrush = new SolidColorBrush(Colors.Red);
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);
           
        }

        private void rbtn_pm_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_pred.Foreground = new SolidColorBrush(Colors.Black);
            lb_mesyc.Foreground = new SolidColorBrush(Colors.Black);
            rbtn_pm.BorderBrush = new SolidColorBrush(Colors.Silver);
        }

        private void rbtn_pg_Checked(object sender, RoutedEventArgs e)
        {
            lb_pred.Foreground = new SolidColorBrush(Colors.Red);
            lb_god.Foreground = new SolidColorBrush(Colors.Red);

            var sd = DateTime.Now;
        
            var d1 = new DateTime(sd.Year-1,1,1);
          

            var d2 = d1.AddMonths(11).AddDays(30);
          
            dtp_begin.SelectedDate = d1;
           
            dtp_end.SelectedDate = d2;
            rbtn_pg.BorderBrush = new SolidColorBrush(Colors.Red);
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);
        }

        private void rbtn_pg_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_pred.Foreground = new SolidColorBrush(Colors.Black);
            lb_god.Foreground = new SolidColorBrush(Colors.Black);
            rbtn_pg.BorderBrush = new SolidColorBrush(Colors.Silver);
        }

        private void rbtn_tk_Checked(object sender, RoutedEventArgs e)
        {
            lb_tekuschiy.Foreground = new SolidColorBrush(Colors.Red);
            lb_kvart.Foreground = new SolidColorBrush(Colors.Red);
            rbtn_tk.BorderBrush = new SolidColorBrush(Colors.Red);

            var sd = DateTime.Now;
            var month = sd.Month;
            var year = sd.Year;

            int tekkv ;
            if (month <= 3) tekkv = 0;
            else
                if (month <= 6) tekkv = 1;
                else
                    if (month <= 9) tekkv = 2;
                    else
                        tekkv = 3;
          
            var d1 = new DateTime();

            d1 = d1.AddMonths(3 * tekkv);


            d1 = d1.AddYears(year-1);
            var d2 = d1;

            d2 = d2.AddMonths(3);
            d2 = d2.AddDays(-1);
           
            dtp_begin.SelectedDate = d1;
            
            dtp_end.SelectedDate = d2;
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);
                 
        }

        private void rbtn_tk_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_tekuschiy.Foreground = new SolidColorBrush(Colors.Black);
            lb_kvart.Foreground = new SolidColorBrush(Colors.Black);
            rbtn_tk.BorderBrush = new SolidColorBrush(Colors.Silver);
        }

        private void rbtn_pk_Checked(object sender, RoutedEventArgs e)
        {
            lb_pred.Foreground = new SolidColorBrush(Colors.Red);
            lb_kvart.Foreground = new SolidColorBrush(Colors.Red);
            rbtn_pk.BorderBrush = new SolidColorBrush(Colors.Red);
            var sd = DateTime.Now;
            var month = sd.Month;
            var year = sd.Year;

            int tekkv;
            if (month <= 3) tekkv = 0;
            else
                if (month <= 6) tekkv = 1;
                else
                    if (month <= 9) tekkv = 2;
                    else
                        tekkv = 3;

            if (tekkv == 0)
            {
                year--;
                tekkv = 3;
            }
            else
                tekkv--;

            var d1 = new DateTime();

            d1 = d1.AddMonths(3 * tekkv);


            d1 = d1.AddYears(year - 1);
            var d2 = d1;

            d2 = d2.AddMonths(3);
            d2 = d2.AddDays(-1);
           
            dtp_begin.SelectedDate = d1;
            
            dtp_end.SelectedDate = d2;
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);

        }

        private void rbtn_pk_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_pred.Foreground = new SolidColorBrush(Colors.Black);
            lb_kvart.Foreground = new SolidColorBrush(Colors.Black);
            rbtn_pk.BorderBrush = new SolidColorBrush(Colors.Silver);
        }
        #endregion

        private void dtp_begin_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateBegin = dtp_begin.SelectedDate;           
        }

        private void dtp_end_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateEnd = dtp_end.SelectedDate;          
        }
        private void dtp_CalendarClosed(object sender, RoutedEventArgs e)
        {
            rbtn_drugoe.IsChecked = true;
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);
        }

        private void dtp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            rbtn_drugoe.IsChecked = true;
            if (SelectedDateChanged != null)
                SelectedDateChanged(this, EventArgs.Empty);
        }

      
       

    }
        


    
}
