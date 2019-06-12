using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using Nop.Core;
using Nop.Data;
using Nop.Data.Mapping;

namespace Nop.Plugin.Campgrounds.Data
{
    /// <summary>
    /// Object context
    /// </summary>
    public class CampgroundsObjectContext : DbContext, IDbContext
    {
        #region Ctor

        public CampgroundsObjectContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether proxy creation setting is enabled (used in EF)
        /// </summary>
        public virtual bool ProxyCreationEnabled
        {
            get { return this.Configuration.ProxyCreationEnabled; }
            set { this.Configuration.ProxyCreationEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto detect changes setting is enabled (used in EF)
        /// </summary>
        public virtual bool AutoDetectChangesEnabled
        {
            get { return this.Configuration.AutoDetectChangesEnabled; }
            set { this.Configuration.AutoDetectChangesEnabled = value; }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Add entity to the configuration of the model for a derived context before it is locked down
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //disable EdmMetadata generation
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //dynamically load all configuration
            //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(NopEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new CampgroundMap());
            //modelBuilder.Configurations.Add(new CampgroundAddressMap());
            //modelBuilder.Configurations.Add(new CampgroundReviewMap());
            //modelBuilder.Configurations.Add(new CampgroundReviewHelpfulnessMap());
            //modelBuilder.Configurations.Add(new CampgroundCategoryMap());
            //modelBuilder.Configurations.Add(new CampgroundAttributeTypeMap());
            //modelBuilder.Configurations.Add(new CampgroundAttributeValueMap());
            //modelBuilder.Configurations.Add(new CampgroundAttributeMappingMap());
            //modelBuilder.Configurations.Add(new CampgroundAttributeTypeCombinationMap());
            //modelBuilder.Configurations.Add(new CampgroundPictureMap());
            //modelBuilder.Configurations.Add(new CampgroundSpecificationAttributeMap());
            //modelBuilder.Configurations.Add(new CampgroundTagMap());
            //modelBuilder.Configurations.Add(new CampgroundTypeMap());
            //modelBuilder.Configurations.Add(new CampgroundTemplateMap());
            //modelBuilder.Configurations.Add(new PredefinedCampgroundAttributeValueMap());
            //modelBuilder.Configurations.Add(new RelatedCampgroundMap());
            //modelBuilder.Configurations.Add(new SeasonalPriceMap());

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a data definition language script that creates schema objects
        /// </summary>
        /// <returns>A DDL script</returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Returns a System.Data.Entity.DbSet`1 instance for access to entities of the given type in the context and the underlying store
        /// </summary>
        /// <typeparam name="TEntity">The type entity for which a set should be returned</typeparam>
        /// <returns>A set for the given entity type</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Install object context
        /// </summary>
        public void Install()
        {
            //create the table
            Database.ExecuteSqlCommand(CreateDatabaseScript());
            SaveChanges();
        }

        /// <summary>
        /// Uninstall object context
        /// </summary>
        public void Uninstall()
        {
            try
            {
                // DROP Tables via migrator. we just pass 0 to tell migrator to reset to original version
                //var migrator = new DbMigrator(new Configuration());
                //migrator.Update("0");

                //drop the table
                //this.DropPluginTable(this.GetTableName<Campground>());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (var i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = SqlQuery<TEntity>(commandText, parameters).ToList();

            return result;
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            var results = Database.SqlQuery<TElement>(sql, parameters);
            return results;
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        /// <summary>
        /// Detach an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        #endregion

    }
}