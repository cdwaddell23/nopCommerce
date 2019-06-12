using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Campgrounds.Data;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Campgrounds.Services.Messaging;
using Nop.Plugin.Widgets.Campgrounds.Entity;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.Campgrounds.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private const string ContextName = "nop_object_context_campgrounds";

        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<CampgroundMenuPlugin>().As<CampgroundMenuPlugin>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundService>().As<ICampgroundService>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundTemplateService>().As<ICampgroundTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundAttributeTypeParser>().As<ICampgroundAttributeTypeParser>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundAttributeTypeService>().As<ICampgroundAttributeTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundAddressService>().As<ICampgroundAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundHostService>().As<ICampgroundHostService>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundTypeService>().As<ICampgroundTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundTagService>().As<ICampgroundTagService>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundWorkflowMessageService>().As<ICampgroundWorkflowMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundMessageTokenProvider>().As<ICampgroundMessageTokenProvider>().InstancePerLifetimeScope();
            //builder.RegisterType<CampgroundWorkflowMessageService>().As<ICampgroundWorkflowMessageService>().InstancePerLifetimeScope();

            builder.RegisterType<CampgroundModelFactory>().As<ICampgroundModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundRegisterModelFactory>().As<ICampgroundRegisterModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CampgroundWorkContext>().As<ICampgroundWorkContext>().InstancePerLifetimeScope();

            
            //data context
            this.RegisterPluginDataContext<CampgroundsObjectContext>(builder, ContextName);

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Campground>>()
                .As<IRepository<Campground>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundAddress>>()
                .As<IRepository<CampgroundAddress>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundCategory>>()
                .As<IRepository<CampgroundCategory>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundReview>>()
                .As<IRepository<CampgroundReview>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundAttributeType>>()
                .As<IRepository<CampgroundAttributeType>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundAttributeValue>>()
                .As<IRepository<CampgroundAttributeValue>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundAttributeMapping>>()
                .As<IRepository<CampgroundAttributeMapping>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundHost>>()
                .As<IRepository<CampgroundHost>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<RelatedCampground>>()
                .As<IRepository<RelatedCampground>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CrossSellCampground>>()
                .As<IRepository<CrossSellCampground>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundAttributeCombination>>()
                .As<IRepository<CampgroundAttributeCombination>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundType>>()
                .As<IRepository<CampgroundType>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundTag>>()
                .As<IRepository<CampgroundTag>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<CampgroundPicture>>()
                .As<IRepository<CampgroundPicture>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<PredefinedCampgroundAttributeValue>>()
                .As<IRepository<PredefinedCampgroundAttributeValue>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 1; }
        }
    }
}