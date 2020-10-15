using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerFile.ElasticModel
{
    class ElasticSetting
    {
        /// Elastic settings
        private ConnectionSettings settings;

        /// Current instantiated client
        public ElasticClient client { get; set; }

        /// Constructor
        public ElasticSetting()
        {
            var node = new Uri("http://localhost:9200");
            settings = new ConnectionSettings(node)
                .DefaultMappingFor<File>(i => i
                    .IndexName("manager_files")
                    .IdProperty(p => p.Id))
                .EnableDebugMode()
                .PrettyJson()
                .RequestTimeout(TimeSpan.FromMinutes(2));


            client = new ElasticClient(settings);
        }
    }
}
