using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Agresso.Interface.CommonExtension;
using Agresso.Interface.TopGenExtension;

namespace HLS_SjekkKonto
{
    [TopGen("TPO022", "*", "*", "HLS_KontoSjekk v1.0 - TPO022")]
    public class TPO022 : IProjectTopGen
    {
        private IForm _form;
        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnCallingAction += _form_OnCallingAction;
            logging("Tpo022 init");
            // _form.OnValidatedField += _form_OnValidatedField;
        }

        private void _form_OnCallingAction(object sender, ActionEventArgs e)
        {
            if ((e.Action == "TASKM:AP" || e.Action == "TASK:DONE") && !AccountValid())
            {
                e.Cancel = true;
                logging("Action");

            }
        }

        //private void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
        //{
        //  if (e.TableName == "acrtrans" && (e.FieldName == "account" || e.FieldName == "dim_1") && Common.UpdatePinTaxCodeFromSetup(e.Row["client"].ToString()))
        //{
        //     SetTaxCode(e.Row);
        //}
        //}

        private bool AccountValid()
        {
            ISection section = _form.GetSection("invoice_header");
            DataRow selectedDataRow = section.GetSelectedDataRow();
            bool flag = true;

            logging("AccountValid");
            foreach (DataRow row in _form.Data.Tables["acrtrans"].Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["trans_type"].ToString() == "GL" && row["wf_elementtype"].ToString() != string.Empty && (row["wf_task_action"].ToString() == "AP" || row["wf_task_action"].ToString() == "") && (row["wf_task_action"].ToString() == "AP" || (row["client"].ToString() == selectedDataRow["client"].ToString() && row["voucher_no"].ToString() == selectedDataRow["voucher_no"].ToString())))
                {
                    int rightAccount = 0;
                    IStatement sqlgetRightAccount = CurrentContext.Database.CreateStatement();
                    sqlgetRightAccount.Append("select rel_value from aglrelvalue where client = @client  and att_value = @apar_id and attribute_id = 'A5' and rel_attr_id = 'A0'");
                    sqlgetRightAccount["client"] = row["client"].ToString();
                    sqlgetRightAccount["apar_id"] = row["apar_id"].ToString();

                    if (CurrentContext.Database.ReadValue(sqlgetRightAccount, ref rightAccount))
                    {
                        if (Int32.Parse(row["account"].ToString()) != rightAccount)
                        {

                            StringBuilder tekst = new StringBuilder();
                            tekst.Append("Konto er blitt endret til: ");
                            tekst.Append(rightAccount);
                            flag = false;
                            row.SetColumnError("account", tekst.ToString());
                            row["account"] = rightAccount;

                        }
                    }
                }
            }
            return flag;
        }

        void logging(string text)
        {
            CurrentContext.Message.Display(text);
        }
    }




  

[TopGen("TFI004", "*", "*", "HLS_KontoSjekk v1.0 - TFI004")]
public class TFI004 : IProjectTopGen
{
    private IForm _form;
    public void Initialize(IForm form)
    {
        _form = form;
        _form.OnCallingAction += _form_OnCallingAction;
        // _form.OnValidatedField += _form_OnValidatedField;
    }

    private void _form_OnCallingAction(object sender, ActionEventArgs e)
    {
        if ((e.Action == "TASKM:AP" || e.Action == "TASK:DONE") && !AccountValid())
        {
            e.Cancel = true;
        }
    }

    //private void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
    //{
    //  if (e.TableName == "acrtrans" && (e.FieldName == "account" || e.FieldName == "dim_1") && Common.UpdatePinTaxCodeFromSetup(e.Row["client"].ToString()))
    //{
    //     SetTaxCode(e.Row);
    //}
    //}

    private bool AccountValid()
    {
        ISection section = _form.GetSection("invoice_header");
        DataRow selectedDataRow = section.GetSelectedDataRow();
        bool flag = true;
        foreach (DataRow row in _form.Data.Tables["acrtrans"].Rows)
        {
            if (row.RowState != DataRowState.Deleted && row["trans_type"].ToString() == "GL" && row["wf_elementtype"].ToString() != string.Empty && (row["wf_task_action"].ToString() == "AP" || row["wf_task_action"].ToString() == "") && (row["wf_task_action"].ToString() == "AP" || (row["client"].ToString() == selectedDataRow["client"].ToString() && row["voucher_no"].ToString() == selectedDataRow["voucher_no"].ToString())))
            {
                int rightAccount = 0;
                IStatement sqlgetRightAccount = CurrentContext.Database.CreateStatement();
                sqlgetRightAccount.Append("select rel_value from aglrelvalue where client = @client  and att_value = @apar_id and attribute_id = 'A5' and rel_attr_id = 'A0'");
                sqlgetRightAccount["client"] = row["client"].ToString();
                sqlgetRightAccount["apar_id"] = row["apar_id"].ToString();

                if (CurrentContext.Database.ReadValue(sqlgetRightAccount, ref rightAccount))
                {
                    if (Int32.Parse(row["account"].ToString()) != rightAccount)
                    {

                            StringBuilder tekst = new StringBuilder();
                            tekst.Append("Konto er blitt endret til: ");
                            tekst.Append(rightAccount);
                            flag = false;
                            row.SetColumnError("account", tekst.ToString());
                            row["account"] = rightAccount;
                      
                    }
                }
            
            }
        }
        return flag;
    }
    void logging(string text)
    {
        CurrentContext.Message.Display(text);
    }

}

        [TopGen("TPO002", "*", "*", "HLS_KontoSjekk v1.0 - TPO002")]
        public class TPO002 : IProjectTopGen
    {
        private IForm _form;
        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnSaving += _form_OnSaving;
            _form.OnCallingAction += _form_OnCallingAction;


            logging("TPO002");
            // _form.OnValidatedField += _form_OnValidatedField;
        }

        private void _form_OnSaving(object sender, SaveEventArgs e)
        {
            if (!AccountValid())
            {
                e.Cancel = true;
            }
        }

        private void _form_OnCallingAction(object sender, ActionEventArgs e)
        {
            if ((e.Action == "TASKM:AP" || e.Action == "TASK:DONE") && !AccountValid())
            {
                e.Cancel = true;
                logging("Action");

            }
        }

        //private void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
        //{
        //  if (e.TableName == "acrtrans" && (e.FieldName == "account" || e.FieldName == "dim_1") && Common.UpdatePinTaxCodeFromSetup(e.Row["client"].ToString()))
        //{
        //     SetTaxCode(e.Row);
        //}
        //}

        private bool AccountValid()
        {
            ISection section = _form.GetSection("invoice_header");
            DataRow selectedDataRow = section.GetSelectedDataRow();
            bool flag = true;

            logging("AccountValid");
            foreach (DataRow row in _form.Data.Tables["acrtransdetail"].Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["trans_type"].ToString() == "GL" && row["wf_elementtype"].ToString() != string.Empty && (row["wf_task_action"].ToString() == "AP" || row["wf_task_action"].ToString() == "") && (row["wf_task_action"].ToString() == "AP" || (row["client"].ToString() == selectedDataRow["client"].ToString() && row["voucher_no"].ToString() == selectedDataRow["voucher_no"].ToString())))
                {
                    int rightAccount = 0;
                    IStatement sqlgetRightAccount = CurrentContext.Database.CreateStatement();
                    sqlgetRightAccount.Append("select rel_value from aglrelvalue where client = @client  and att_value = @apar_id and attribute_id = 'A5' and rel_attr_id = 'A0'");
                    sqlgetRightAccount["client"] = row["client"].ToString();
                    sqlgetRightAccount["apar_id"] = row["apar_id"].ToString();

                    if (CurrentContext.Database.ReadValue(sqlgetRightAccount, ref rightAccount))
                    {
                        if (Int32.Parse(row["account"].ToString()) != rightAccount)
                        {
                            StringBuilder tekst = new StringBuilder();
                            tekst.Append("Konto er endret til : ");
                            tekst.Append(rightAccount);
                            flag = false;
                            row.SetColumnError("account", tekst.ToString());
                            row["account"] = rightAccount;
                        }
                    }
                 
                }
            }



            return flag;
        }

    void logging(string text)
    {
        CurrentContext.Message.Display(text);
    }
            
        }

[TopGen("TPO010", "*", "*", "HLS_KontoSjekk v1.0 - TPO010")]
public class TPO010 : IProjectTopGen
{
    private IForm _form;
    public void Initialize(IForm form)
    {
        _form = form;
        _form.OnCallingAction += _form_OnCallingAction;
        logging("TPO010");
        // _form.OnValidatedField += _form_OnValidatedField;
    }

    private void _form_OnCallingAction(object sender, ActionEventArgs e)
    {
        if ((e.Action == "TASKM:AP" || e.Action == "TASK:DONE") && !AccountValid())
        {
            e.Cancel = true;
            logging("Action");

        }
    }

    //private void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
    //{
    //  if (e.TableName == "acrtrans" && (e.FieldName == "account" || e.FieldName == "dim_1") && Common.UpdatePinTaxCodeFromSetup(e.Row["client"].ToString()))
    //{
    //     SetTaxCode(e.Row);
    //}
    //}

    private bool AccountValid()
    {
        ISection section = _form.GetSection("req_header");
        DataRow selectedDataRow = section.GetSelectedDataRow();
        bool flag = true;

        logging("AccountValid");
        foreach (DataRow row in _form.Data.Tables["acrtransdtail"].Rows)
        {
            if (row.RowState != DataRowState.Deleted && row["trans_type"].ToString() == "GL" && row["wf_elementtype"].ToString() != string.Empty && (row["wf_task_action"].ToString() == "AP" || row["wf_task_action"].ToString() == "") && (row["wf_task_action"].ToString() == "AP" || (row["client"].ToString() == selectedDataRow["client"].ToString() && row["voucher_no"].ToString() == selectedDataRow["voucher_no"].ToString())))
            {
                int rightAccount = 0;
                IStatement sqlgetRightAccount = CurrentContext.Database.CreateStatement();
                sqlgetRightAccount.Append("select rel_value from aglrelvalue where client = @client  and att_value = @apar_id and attribute_id = 'A5' and rel_attr_id = 'A0'");
                sqlgetRightAccount["client"] = row["client"].ToString();
                sqlgetRightAccount["apar_id"] = row["apar_id"].ToString();

                if (CurrentContext.Database.ReadValue(sqlgetRightAccount, ref rightAccount))
                {
                        if (Int32.Parse(row["account"].ToString()) != rightAccount)
                        {

                            flag = false;
                            StringBuilder tekst = new StringBuilder();
                            tekst.Append("Konto er endret til : ");
                            tekst.Append(rightAccount);
                            row["account"] = rightAccount;
                        }
           
                }
              
            }
        }
        return flag;
    }
    void logging(string text)
    {
        CurrentContext.Message.Display(text);
    }

}
}

