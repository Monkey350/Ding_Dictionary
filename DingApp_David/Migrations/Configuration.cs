using DingApp_David.Models;

namespace DingApp_David.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DingApp_David.Models.DingDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DingApp_David.Models.DingDb context)
        {
            //  This method will be called after migrating to the latest version.

            context.Words.AddOrUpdate(r => r.definitions, 
                new WordModel { word = "kek", definitions = "1) The Korean version of LOL. \r\n2) One of the primary foundational principles of the nation of Kekistan."}
                );
        }
    }
}
