namespace SBT.Apps.Erp.Blazor.Server
{
    using DevExpress.DataAccess.ConnectionParameters;
    using DevExpress.DataAccess.Wizard.Services;
    using DevExpress.DataAccess.Web;
    // ...

    public class MyCustomValidator : ICustomQueryValidator
    {
        public bool Validate(DataConnectionParametersBase connectionParameters, string sql, ref string message)
        {
            string[] noValid = { "insert", "update", "delete", "merge", "truncate", "create", "alter", "drop" };
            // Add your custom validation logic here.
            // Return true if the query is valid; otherwise, return false.
            foreach (string _word in noValid) 
            { 
                if (sql.ToLower().Contains(_word))
                {
                    message = $@"Sentencia SQL con {_word} no es permitida";
                    return false;
                }
            }
            return true;
        }
    }

    public class CustomSqlDataSourceWizardCustomizationService : ISqlDataSourceWizardCustomizationService
    {
        public ICustomQueryValidator CustomQueryValidator
        {
            get { return new MyCustomValidator(); }
        }

        public bool IsCustomSqlDisabled
        {
            get { return false; }
        }
    }
}
