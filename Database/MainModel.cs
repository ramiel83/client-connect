using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Database
{
    public class MainModel : DbContext
    {
        // Your context has been configured to use a 'MainModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Database.MainModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MainModel' 
        // connection string in the application configuration file.
        public MainModel()
            : base("name=MainModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Switch> SwitchSet { get; set; }
        public virtual DbSet<PbxConnection> PbxConnectionSet { get; set; }
        public virtual DbSet<KolanConnection> KolanConnectionSet { get; set; }
        public virtual DbSet<TelnetConnection> TelnetConnectionSet { get; set; }
        public virtual DbSet<CheckPointVpnConnection> CheckPointVpnConnectionSet { get; set; }
        public virtual DbSet<User> UserSet { get; set; }
        public virtual DbSet<File> FileSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Switch>().HasKey(x => x.Id);
            modelBuilder.Entity<Switch>().Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Switch>().HasOptional(x => x.PbxConnection).WithRequired(v => v.Switch);
            modelBuilder.Entity<Switch>().HasOptional(x => x.KolanConnection).WithRequired(v => v.Switch);
            modelBuilder.Entity<Switch>().HasOptional(x => x.TelnetConnection).WithRequired(v => v.Switch);
            modelBuilder.Entity<Switch>().HasOptional(x => x.CheckPointVpnConnection).WithRequired(v => v.Switch);

            modelBuilder.Entity<PbxConnection>().HasKey(x => x.SwitchId);
            modelBuilder.Entity<PbxConnection>().Property(x => x.SwitchId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<KolanConnection>().HasKey(x => x.SwitchId);
            modelBuilder.Entity<KolanConnection>().Property(x => x.SwitchId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<TelnetConnection>().HasKey(x => x.SwitchId);
            modelBuilder.Entity<TelnetConnection>().Property(x => x.SwitchId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<CheckPointVpnConnection>().HasKey(x => x.SwitchId);
            modelBuilder.Entity<CheckPointVpnConnection>().Property(x => x.SwitchId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Switch
    {
        public int Id { get; set; }

        public string CrmNum { get; set; }
        public string Comments { get; set; }

        [Required] public string Name { get; set; }

        public string SwRelease { get; set; }
        public string MachineType { get; set; }
        public virtual PbxConnection PbxConnection { get; set; }
        public virtual KolanConnection KolanConnection { get; set; }
        public virtual TelnetConnection TelnetConnection { get; set; }
        public virtual CheckPointVpnConnection CheckPointVpnConnection { get; set; }
        public virtual List<File> Files { get; set; }
    }

    public class PbxConnection
    {
        public int SwitchId { get; set; }

        [Required] public string DialNum { get; set; }

        public string LoginName { get; set; }

        [Required] public string LoginPassword { get; set; }

        public string DebugPassword { get; set; }

        [Required] public int BaudRate { get; set; }

        [Required] public string ParDataStop { get; set; }

        public virtual Switch Switch { get; set; }
    }

    public class KolanConnection
    {
        public int SwitchId { get; set; }

        [Required] public string DialNum { get; set; }

        [Required] public int BaudRate { get; set; }

        [Required] public string ParDataStop { get; set; }

        public virtual Switch Switch { get; set; }
    }

    public class TelnetConnection
    {
        public int SwitchId { get; set; }

        [Required] public string IpAddress { get; set; }

        [Required] public string UsernameSs { get; set; }

        [Required] public string PasswordSs { get; set; }

        [Required] public string UsernameCs { get; set; }

        [Required] public string PasswordCs { get; set; }

        public virtual Switch Switch { get; set; }
    }

    public class CheckPointVpnConnection
    {
        public int SwitchId { get; set; }
        public virtual Switch Switch { get; set; }

        [Required] public string Site { get; set; }

        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }

        public string IpAddress { get; set; }
        public short? Port { get; set; }
    }

    public class User
    {
        [Key] public int Id { get; set; }

        [Required] public string Username { get; set; }

        [Required] public string PasswordHash { get; set; }

        [Required] public AccessLevel AccessLevel { get; set; }
    }

    public class File
    {
        [Key] public int Id { get; set; }

        [Required] public int SwitchId { get; set; }

        [Required] public DateTime DateTime { get; set; }

        [Required] public byte[] Content { get; set; }

        [Required] public string Name { get; set; }

        public virtual Switch Switch { get; set; }
    }
}