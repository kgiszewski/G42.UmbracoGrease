using System;
using System.Collections;
using System.Collections.Generic;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42AppSettings.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.G42AppSettings.Repositories
{
    internal class G42AppSettingRepository
    {
        internal static G42AppSetting Get(PetaPocoUnitOfWork unitOfWork, string key)
        {
            return unitOfWork.Database.SingleOrDefault<G42AppSetting>(@"
                WHERE [key] = @0
            ", key);
        }

        internal static G42AppSetting Save(PetaPocoUnitOfWork unitOfWork, G42AppSetting appSetting)
        {
            appSetting.UpdatedOn = DateTime.UtcNow;

            unitOfWork.Database.Save(appSetting);

            return appSetting;
        }

        internal static IEnumerable<G42AppSetting> GetErrorReportingConfigs(PetaPocoUnitOfWork unitOfWork)
        {
            //TODO: use constant
            return unitOfWork.Database.Fetch<G42AppSetting>(@"
                WHERE [key] LIKE 'errorReporting:%'
            ");
        }

        internal static IEnumerable<G42AppSetting> Get404trackerConfigs(PetaPocoUnitOfWork unitOfWork)
        {
            //TODO: use constant
            return unitOfWork.Database.Fetch<G42AppSetting>(@"
                WHERE [key] LIKE '_404tracker:%'
            ");
        }

        internal static void CreateTable(PetaPocoUnitOfWork unitOfWork)
        {
            if (!unitOfWork.Database.TableExist("G42GreaseAppSettings"))
            {
                LogHelper.Info<G42AppSetting>("Creating table.");

                unitOfWork.Database.Execute(@"
                    CREATE TABLE [dbo].[G42GreaseAppSettings](
	                    [id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [key] [nvarchar](150) NOT NULL,
	                    [value] [nvarchar](150) NOT NULL,
	                    [updatedOn] [datetime] NOT NULL,
                     CONSTRAINT [PK_GreaseAppSettings] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                    ) 
                ");
            }
            else
            {
                LogHelper.Info<G42AppSetting>("Table exists.");
            }
        }
    }
}