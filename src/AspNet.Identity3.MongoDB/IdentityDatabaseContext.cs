﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace AspNet.Identity3.MongoDB
{
	public class IdentityDatabaseContext : IdentityDatabaseContext<IdentityUser, IdentityRole, string>
	{
	}

	public class IdentityDatabaseContext<TUser, TRole, TKey>
		where TRole : IdentityRole<TKey>
		where TUser : IdentityUser<TKey>
		where TKey : IEquatable<TKey>
	{
		public bool CreateCollectionIndexes { get; set; } = false;
		public string ConnectionString { get; set; }

		public string DatabaseName { get; set; } = "AspNetIdentity";
		public string UsersCollectionName { get; set; } = "AspNetUsers";
		public string RolesCollectionName { get; set; } = "AspNetRoles";
		public MongoCollectionSettings CollectionSettings { get; set; } = new MongoCollectionSettings { WriteConcern = WriteConcern.WMajority };


		private IMongoClient _client;
		public virtual IMongoClient Client
		{
			get
			{
				if (_client == null)
				{
					if (_database != null)
					{
						_client = _database.Client;
					}

					if (string.IsNullOrWhiteSpace(ConnectionString))
					{
						throw new NullReferenceException($"The parameter '{nameof(ConnectionString)}' in '{typeof (IdentityDatabaseContext<TUser, TRole, TKey>).FullName}' is null and must be set before calling '{nameof(Client)}'. This is usually configured as part of Startup.cs");
					}
					_client = new MongoClient(ConnectionString);
				}
				return _client;
			}
			set { _client = value; }
		}


		private IMongoDatabase _database;
		public virtual IMongoDatabase Database
		{
			get
			{
				if (_database == null)
				{
					if (string.IsNullOrWhiteSpace(DatabaseName))
					{
						throw new NullReferenceException($"The parameter '{nameof(DatabaseName)}' in '{typeof(IdentityDatabaseContext<TUser, TRole, TKey>).FullName}' is null and must be set before calling '{nameof(Database)}'. This is usually configured as part of Startup.cs");
					}
					_database = Client.GetDatabase(DatabaseName);
				}
				return _database;
			}
			set { _database = value; }
		}



		/// <summary>
		/// Initiates the user collection. If <see cref="CreateCollectionIndexes"/> == true, will call <see cref="CreateUserCollection"/>
		/// </summary>
		public virtual IMongoCollection<TUser> Users
		{
			get
			{
				if (_users == null)
				{
					if (string.IsNullOrWhiteSpace(UsersCollectionName))
					{
						throw new NullReferenceException($"The parameter '{nameof(UsersCollectionName)}' in '{typeof (IdentityDatabaseContext<TUser, TRole, TKey>).FullName}' is null and must be set before calling '{nameof(Users)}'. This is usually configured as part of Startup.cs");
					}
					_users = Database.GetCollection<TUser>(UsersCollectionName, CollectionSettings);
				}
				return _users;
			}
			set { _users = value; }
		}
		private IMongoCollection<TUser> _users;


		/// <summary>
		/// Initiates the role collection. If <see cref="CreateCollectionIndexes"/> == true, will call <see cref="CreateRoleCollection"/>
		/// </summary>
		public virtual IMongoCollection<TRole> Roles
		{
			get
			{
				if (_roles == null)
				{
					if (string.IsNullOrWhiteSpace(RolesCollectionName))
					{
						throw new NullReferenceException($"The parameter '{nameof(RolesCollectionName)}' in '{typeof(IdentityDatabaseContext<TUser, TRole, TKey>).FullName}' is null and must be set before calling '{nameof(Roles)}'. This is usually configured as part of Startup.cs");
					}
					_roles = Database.GetCollection<TRole>(RolesCollectionName, CollectionSettings);
				}
				return _roles;
			}
			set { _roles = value; }
		}
		private IMongoCollection<TRole> _roles;



		/// <summary>
		/// Ensures the user collection is instantiated, and has the standard indexes applied. Called when <see cref="Users"/> is called if <see cref="CreateCollectionIndexes"/> == true.
		/// </summary>
		/// <remarks>
		/// Indexes Created:
		/// Users: Id, NormalizedUserName, NormalizedEmail, LoginProvider, Roles.NormalizedName, Roles.Claims.ClaimType, Claims.ClaimType
		/// </remarks>
		public virtual void CreateUserCollection()
		{
			// TODO: change so create collections and indexes on appropriate fields 
			
			//if (Users == null)
			//{
			//	var taskList = new List<Task>
			//	{
			//		Database.CreateCollectionAsync(UsersCollectionName),
			//		Database.CreateCollectionAsync(RolesCollectionName)
			//	};

			//	Task.WaitAll(taskList.ToArray());
			//}
		}



		/// <summary>
		/// Ensures the role collection is instantiated, and has the standard indexes applied. Called when <see cref="Roles"/> is called if <see cref="CreateCollectionIndexes"/> == true.
		/// </summary>
		/// <remarks>
		/// Indexes Created:
		/// Roles: NormalizedName
		/// </remarks>
		public virtual void CreateRoleCollection()
		{
			// TODO: change so create collections and indexes on appropriate fields 

			//if (Users == null)
			//{
			//	var taskList = new List<Task>
			//	{
			//		Database.CreateCollectionAsync(UsersCollectionName),
			//		Database.CreateCollectionAsync(RolesCollectionName)
			//	};

			//	Task.WaitAll(taskList.ToArray());
			//}
		}

		/// <summary>
		/// WARNING: Permanently deletes user collection, including all indexes and data.
		/// </summary>
		public virtual void DeleteUserCollectioAsyncn()
		{
			Database.DropCollectionAsync(UsersCollectionName).Wait();
		}

		/// <summary>
		/// WARNING: Permanently deletes role collection, including all indexes and data.
		/// </summary>
		public virtual void DeleteRoleCollectionAsync()
		{
			Database.DropCollectionAsync(RolesCollectionName).Wait();
		}
	}
}