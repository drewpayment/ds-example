using System.Data.Entity.ModelConfiguration;

namespace Dominion.Core.EF.Configuration.User
{
    /// <summary>
    /// Configure the Entity to DB mapping schema for the dbo.UserType Table
    /// </summary>
    internal class UserConfiguration : EntityTypeConfiguration<Domain.Entities.User.User>
    {
        /// <summary>
        /// Configure the Entity to DB mapping schema for the dbo.User Table
        /// </summary>
        internal UserConfiguration()
        {
            this.ToTable("User");

            this.Property(x => x.UserId)
                .HasColumnName("UserID");

            this.Property(x => x.AuthUserId)
                .HasColumnName("AuthUserId");

            this.Property(x => x.FirstName)
                .HasMaxLength(25)
                .IsRequired();
            
            this.Property(x => x.LastName)
                .HasMaxLength(25)
                .IsRequired();

            this.Property(x => x.UserName)
                .HasColumnName("Username")
                .HasMaxLength(15)
                .IsRequired();
            
            this.Property(x => x.PasswordHash)
                .HasColumnName("Password")
                .HasMaxLength(60)
                .IsRequired();
            
            this.Property(x => x.EmailAddress)
                .HasMaxLength(50);

            this.Property(x => x.SecretQuestionId)
                .HasColumnName("SecretQuestionID");
            
            this.Property(x => x.SecretQuestionAnswer)
                .HasColumnName("SecretAnswer")
                .HasMaxLength(35)
                .IsRequired();

            this.Property(x => x.UserTypeId)
                .HasColumnName("UserTypeID");

            this.Property(x => x.EmployeeId)
                .HasColumnName("EmployeeID");
            
            this.HasOptional(x => x.Employee).WithMany(x => x.UserAccounts).HasForeignKey(x => x.EmployeeId);

            this.Property(x => x.ViewEmployeePayTypes)
                .HasColumnName("ViewEmployees")
                .HasColumnType("tinyint");
            
            this.Property(x => x.ViewEmployeeRateTypes)
                .HasColumnName("ViewRates")
                .HasColumnType("tinyint");

            this.Property(x => x.IsSecurityEnabled)
                .HasColumnName("SecuritySettings");

            this.Property(x => x.IsPasswordEnabled)
                .HasColumnName("IsEnabled");
            
            this.Property(x => x.IsEmployeeSelfServiceViewOnly)
                .HasColumnName("ViewOnly");
            
            this.Property(x => x.IsEmployeeSelfServiceOnly)
                .HasColumnName("EmployeeSelfServiceOnly");
            
            this.Property(x => x.IsReportingOnly)
                .HasColumnName("ReportingOnly");
            
            this.Property(x => x.IsPayrollAccessBlocked)
                .HasColumnName("BlockPayrollAccess");
            
            this.Property(x => x.IsTimeclockEnabled)
                .HasColumnName("Timeclock");
            
            this.Property(x => x.IsHrBlocked)
                .HasColumnName("BlockHR");
            this.Property(x => x.IsEmployeeAccessOnly)
                .HasColumnName("EmployeeOnly");
            
            this.Property(x => x.IsApplicantTrackingAdmin)
                .HasColumnName("ApplicantAdmin");
            
            this.Property(x => x.IsEditGlEnabled)
                .HasColumnName("EditGL");

            this.Property(x => x.MustChangePassword)
                .HasColumnName("MustChangePassword");
            
            this.Property(x => x.IsUserDisabled)
                .HasColumnName("UserIsDisabled");
            
            this.Property(x => x.IsWageTaxHistoryEditable)
                .HasColumnName("UserCanEditWageTaxHistory");
            
            this.Property(x => x.IsAllowedToAddSystemAdmin)
                .HasColumnName("CanAddSystemAdmins");

            this.Property(x => x.TempEnableFromDate)
                .HasColumnName("TemporaryEnableFromDate");
            
            this.Property(x => x.TempEnableToDate)
                .HasColumnName("TemporaryEnableToDate");

            this.Property(x => x.LastModifiedByUserId)
                .HasColumnName("ModifiedBy");
            
            this.HasOptional(x => x.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(x => x.LastModifiedByUserId);

            this.Property(x => x.CanViewTaxPackets)
                .HasColumnName("ViewTaxPackets");

            Property(x => x.TimeclockAppOnly)
                .HasColumnName("TimeclockAppOnly");

            Property(x => x.IsBillingAdmin)
                .HasColumnName("IsBillingAdmin").IsRequired();

            this.Property(x => x.IsArAdmin)
                .HasColumnName("ARAdmin")
                .IsRequired();
                
            HasOptional(x => x.Permissions).WithRequired(x => x.User);

            HasOptional(x => x.UserPin).WithRequired(x => x.User);

            HasOptional(x => x.Session).WithRequired(x => x.User);
            
        }

        // UserConfiguration()
    } // class UserConfiguration
}
