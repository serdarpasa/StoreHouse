using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Storehouse.reportViewer
{  
    public partial class wrTransaction
    {
      
        public List<Model.TransactionItems> Lr = null;
        public Transaction CurrentTransaction;
        public wrTransaction()
        {   
            try
            {
                InitializeComponent();
                _reportViewer.Load += ReportViewer_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0} \n {1} ", ex.Message, ex.StackTrace));
            }
        }

       

        private bool _isReportViewerLoaded;

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                var reportDS = new Microsoft.Reporting.WinForms.ReportDataSource
                {
                    Name = "Ds",
                    Value = Lr
                };




                _reportViewer.LocalReport.DataSources.Add(reportDS);


            _reportViewer.LocalReport.ReportEmbeddedResource = "Storehouse.reportViewer.templates.rTransaction.rdlc";

          

            var type = string.Empty;
            var employeers = string.Empty;
                switch(CurrentTransaction.Type)
                {
                    case 1:
                        {
                            type = "ПОСТУПЛЕНИЕ";
                            employeers = CurrentTransaction.Employeer.FIO_Short;
                            break;
                        }
                    case 0:
                        {
                            type = "ПЕРЕМЕЩЕНИЕ";
                            employeers = "от кого: " + CurrentTransaction.User.FIO_Short +
                                         "\nкому: " + CurrentTransaction.Employeer.FIO_Short;
                            break;
                        }
                    case -1:
                        {
                            type = "СПИСАНИЕ";
                            employeers = CurrentTransaction.Employeer.FIO_Short;
                            break;
                        }
                };
                ReportParameter[] parameters = new ReportParameter[3];
            
                parameters[0] = new ReportParameter("Type", type);
                parameters[1] = new ReportParameter("Date", CurrentTransaction.Date!=null ? CurrentTransaction.Date.Value.ToShortDateString():"");            
                parameters[2] = new ReportParameter("Employeers", employeers);
          



            _reportViewer.LocalReport.SetParameters(parameters);

                _reportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }
    }
}
