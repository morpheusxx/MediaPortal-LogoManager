using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ChannelManager.EF
{
	public class ObservableListSource<T> : ObservableCollection<T>, IListSource, ICollection<T> where T : class
	{
		private IBindingList _bindingList;
		bool IListSource.ContainsListCollection { get { return false; } }

		IList IListSource.GetList()
		{
			// ToBindingList is defined in the EntityFramework.dll, System.Data.Entity namespace. 
			return _bindingList ?? (_bindingList = this.ToBindingList());
		}
	}

	public partial class RepositoryContext : DbContext 
	{
		string connectionStringName;

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		  Database.SetInitializer<RepositoryContext>(
			new MigrateDatabaseToLatestVersion<RepositoryContext, Migrations.Configuration>(connectionStringName)
		  );
		}

		public RepositoryContext () 
		{
			SetDefaultConfiguration();
	    }

	    public RepositoryContext (string ConnectionStringName) : base(ConnectionStringName) 
		{
			this.connectionStringName = ConnectionStringName;
			SetDefaultConfiguration();
	    }

		protected virtual void SetDefaultConfiguration()
		{
			this.Configuration.LazyLoadingEnabled = false;
			this.Configuration.ProxyCreationEnabled = false;
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ValidateOnSaveEnabled = true;
		}

		public virtual DbSet<Repository> Repositorys { get; set; }		
		public virtual DbSet<User> Users { get; set; }		
		public virtual DbSet<Role> Roles { get; set; }		
		public virtual DbSet<Logo> Logos { get; set; }		
		public virtual DbSet<Channel> Channels { get; set; }		
		public virtual DbSet<Alias> Aliases { get; set; }		
		public virtual DbSet<Suggestion> Suggestions { get; set; }		
		public virtual DbSet<Message> Messages { get; set; }		
		public virtual DbSet<Provider> Providers { get; set; }	
	}

	public partial class Repository 
	{	
		[Key]
		public virtual Guid Id { get; set; }
		[InverseProperty("Repository")]
		public virtual ObservableCollection<User> Users { get; set; }
		[InverseProperty("Repository")]
		public virtual ObservableCollection<Channel> Channels { get; set; }
		[InverseProperty("Repository")]
		public virtual ObservableCollection<Role> Roles { get; set; }
		[InverseProperty("Repository")]
		public virtual ObservableCollection<Logo> Logos { get; set; }
		[InverseProperty("Repository")]
		public virtual ObservableCollection<Suggestion> Suggestions { get; set; }
		[InverseProperty("Repository")]
		public virtual ObservableCollection<Provider> Providers { get; set; }
		
		public Repository ()
		{
			Users = new ObservableCollection<User>();
			Channels = new ObservableCollection<Channel>();
			Roles = new ObservableCollection<Role>();
			Logos = new ObservableCollection<Logo>();
			Suggestions = new ObservableCollection<Suggestion>();
			Providers = new ObservableCollection<Provider>();
			
		}
	}

	public partial class User 
	{	
		
		
		public virtual String Login { get; set; }
		
		
		public virtual String Password { get; set; }
		
		
		public virtual String Email { get; set; }
		
		
		public virtual String Info { get; set; }
		[Key]
		public virtual Guid Id { get; set; }
		[InverseProperty("Users")]
		public virtual ObservableCollection<Role> Roles { get; set; }
		[InverseProperty("Creator")]
		public virtual ObservableCollection<Logo> Logos { get; set; }
		[InverseProperty("User")]
		public virtual ObservableCollection<Suggestion> Suggestions { get; set; }
		[InverseProperty("User")]
		public virtual ObservableCollection<Message> Messages { get; set; }
		
		[InverseProperty("Users")]
		public virtual Repository Repository { get; set; }
		
		public User ()
		{
			Roles = new ObservableCollection<Role>();
			Logos = new ObservableCollection<Logo>();
			Suggestions = new ObservableCollection<Suggestion>();
			Messages = new ObservableCollection<Message>();
			
		}
	}

	public partial class Role 
	{	
		
		
		public virtual String Name { get; set; }
		[Key]
		public virtual Guid Id { get; set; }
		[InverseProperty("Roles")]
		public virtual ObservableCollection<User> Users { get; set; }		
		[InverseProperty("Roles")]
		public virtual Repository Repository { get; set; }
		
		public Role ()
		{
			Users = new ObservableCollection<User>();
			
		}
	}

	public partial class Logo 
	{	
		
		
		public virtual String Name { get; set; }
		
		
		public virtual DateTime LastModified { get; set; }
		
		
		public virtual String Origin { get; set; }
		[Key]
		public virtual Guid Id { get; set; }
		[InverseProperty("Logos")]
		public virtual ObservableCollection<Channel> Channels { get; set; }
		
		[InverseProperty("Logos")]
		public virtual User Creator { get; set; }
		
		[InverseProperty("Logos")]
		public virtual Suggestion Suggestion { get; set; }
		
		[InverseProperty("Logos")]
		public virtual Repository Repository { get; set; }
		
		public Logo ()
		{
			Channels = new ObservableCollection<Channel>();
			
		}
	}

	public partial class Channel 
	{	
		
		
		public virtual String Name { get; set; }
		
		
		public virtual String Website { get; set; }
		
		
		public virtual String RegionCode { get; set; }
		
		
		public virtual Byte Type { get; set; }
		
		
		public virtual String Description { get; set; }
		[Key]
		public virtual Guid Id { get; set; }
		[InverseProperty("Channel")]
		public virtual ObservableCollection<Alias> Aliases { get; set; }
		[InverseProperty("Channels")]
		public virtual ObservableCollection<Logo> Logos { get; set; }		
		[InverseProperty("Channels")]
		public virtual Suggestion Suggestion { get; set; }
		
		[InverseProperty("Channels")]
		public virtual Repository Repository { get; set; }
		
		public Channel ()
		{
			Aliases = new ObservableCollection<Alias>();
			Logos = new ObservableCollection<Logo>();
			
		}
	}

	public partial class Alias 
	{	
		
		
		public virtual String Name { get; set; }
		
		
		public virtual DateTime Created { get; set; }
		[Key]
		public virtual Guid Id { get; set; }
		[InverseProperty("Aliases")]
		public virtual ObservableCollection<Provider> Providers { get; set; }
		
		[InverseProperty("Aliases")]
		public virtual Channel Channel { get; set; }
		
		[InverseProperty("Aliases")]
		public virtual Suggestion Suggestion { get; set; }
		
		public Alias ()
		{
			Providers = new ObservableCollection<Provider>();
			
		}
	}

	public partial class Suggestion 
	{	
		
		
		public virtual Byte State { get; set; }
		
		
		public virtual DateTime Created { get; set; }
		
		
		public virtual DateTime LastModified { get; set; }
		[Key]
		public virtual Guid Id { get; set; }
		[InverseProperty("Suggestion")]
		public virtual ObservableCollection<Message> Messages { get; set; }
		[InverseProperty("Suggestion")]
		public virtual ObservableCollection<Alias> Aliases { get; set; }
		[InverseProperty("Suggestion")]
		public virtual ObservableCollection<Logo> Logos { get; set; }
		[InverseProperty("Suggestion")]
		public virtual ObservableCollection<Channel> Channels { get; set; }
		
		[InverseProperty("Suggestions")]
		public virtual User User { get; set; }
		
		[InverseProperty("Suggestions")]
		public virtual Repository Repository { get; set; }
		
		public Suggestion ()
		{
			Messages = new ObservableCollection<Message>();
			Aliases = new ObservableCollection<Alias>();
			Logos = new ObservableCollection<Logo>();
			Channels = new ObservableCollection<Channel>();
			
		}
	}

	public partial class Message 
	{	
		
		
		public virtual DateTime Created { get; set; }
		
		
		public virtual String Text { get; set; }
		[Key]
		public virtual Guid Id { get; set; }
		
		[InverseProperty("Messages")]
		public virtual User User { get; set; }
		
		[InverseProperty("Messages")]
		public virtual Suggestion Suggestion { get; set; }
		
		public Message ()
		{
			
		}
	}

	public partial class Provider 
	{	
		
		
		public virtual String Name { get; set; }
		[Key]
		public virtual Guid Id { get; set; }
		[InverseProperty("Providers")]
		public virtual ObservableCollection<Alias> Aliases { get; set; }		
		[InverseProperty("Providers")]
		public virtual Repository Repository { get; set; }
		
		public Provider ()
		{
			Aliases = new ObservableCollection<Alias>();
			
		}
	}
}
