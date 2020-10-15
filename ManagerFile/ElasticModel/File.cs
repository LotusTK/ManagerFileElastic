using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerFile.ElasticModel
{
    [ElasticsearchType(RelationName = "file_info")]
    class File
    {
        public File() { }
        public File(string id, string path, string name, string content, DateTime dateCreate, string extension)
        {
            Id = id;
            Path = path;
            Name = name;
            Content = content;
            DateCreate = dateCreate;
            Extension = extension;
        }

        [Text(Name = "id")]
        public string Id { get; set; }
        [Text(Name = "path")]
        public string Path { get; set; }
        [Text(Name = "name_file")]
        public string Name { get; set; }
        [Text(Name = "content")]
        public string Content { get; set; }
        [Date(Format = "MMddyyyy")]
        public DateTime DateCreate { get; set; }
        [Text(Name = "extension")]
        public string Extension { get; set; }
    }
}
