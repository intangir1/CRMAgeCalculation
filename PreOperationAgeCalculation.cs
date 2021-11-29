using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginContact
{
    public class PreOperationAgeCalculation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
			IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

			if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity && ((Entity)context.InputParameters["Target"]).LogicalName.Equals(Contact.EntityLogicalName))
			{
				Contact contact = ((Entity)context.InputParameters["Target"]).ToEntity<Contact>();
				var birthDate = contact.BirthDate;
				if (birthDate != null)
				{
					string birthDateString = birthDate.ToString();
					var dateBirthday = Convert.ToDateTime(birthDateString);
					var today = DateTime.Now;

					int months = today.Month - dateBirthday.Month;
					int years = today.Year - dateBirthday.Year;
					if (today.Day < dateBirthday.Day)
					{
						months--;
					}
					if (months < 0)
					{
						years--;
						months += 12;
					}

					float age = years;
					age += (months / 100f);
					contact.mtx_AgeFltp = age;
				}
				else
				{
					contact.mtx_AgeFltp = 0;
				}
			}
		}
    }
}
