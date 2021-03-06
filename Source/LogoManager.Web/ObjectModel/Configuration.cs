namespace ChannelManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EF.RepositoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EF.RepositoryContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var repo = context.Repositorys.FirstOrDefault();
            if (repo == null)
            {
                repo = context.Repositorys.Create();
                repo.Id = Guid.NewGuid();
                context.Repositorys.Add(repo);

                var role = context.Roles.Create();
                role.Id = Guid.NewGuid();
                role.Name = "Administrator";
                repo.Roles.Add(role);

                role = context.Roles.Create();
                role.Id = Guid.NewGuid();
                role.Name = "Maintainer";
                repo.Roles.Add(role);

                context.SaveChanges();
            }
        }
    }
}
