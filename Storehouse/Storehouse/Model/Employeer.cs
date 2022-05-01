namespace Storehouse
{
    public partial class Employeer
    {
        public string FIO_Short
        {
            get
            {
                return LastName + " "+
                    (!string.IsNullOrEmpty(FirstName) ? (FirstName[0].ToString().ToUpper()+". ") : "")+
                    (!string.IsNullOrEmpty(MiddleName) ? (MiddleName[0].ToString().ToUpper()+".") : "");
            }
        }
        public string FIO_Full
        {
            get
            {
                return LastName + " " + FirstName+" " + MiddleName;
            }
        }

        public string RoleName
        {
            get
            {
                var role = string.Empty;
                switch (Role)
                {
                    case 0:
                    {
                        role = "МОЛ";
                        break;
                    }
                    case 1:
                    {
                        role = "Руководитель";
                        break;
                    }
                    case 2:
                    {
                        role = "Сотрудник";
                        break;
                    }
                }
                return role;
            }
        }
    }
}
