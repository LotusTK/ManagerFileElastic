﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerFile.ElasticModel
{
    class FileDAO
    {
        private ElasticSetting connect;
        public FileDAO()
        {
            connect = new ElasticSetting();
        }

        public List<File> SearchAll()
        {
            return connect.client.Search<File>().Documents.ToList();
        }

        public List<File> SearchByField(string field, string query)
        {
            List<File> response = null;

            field = field.ToLower();

            switch (field)
            {
                case "name":
                    response = connect.client.SearchAsync<File>(ele => ele
                                    .Query(qry => qry
                                        .Bool(b => b
                                            .Must(m => m
                                                .Match(mc => mc
                                                    .Field(file => file.Name)
                                                    .Query(query)))))).Result.Documents.ToList();
                    return response;

                case "content":
                    response = connect.client.SearchAsync<File>(ele => ele
                                    .Query(qry => qry
                                        .Bool(b => b
                                            .Must(m => m
                                                .Match(mc => mc
                                                    .Field(file => file.Content)
                                                    .Query(query)))))).Result.Documents.ToList();
                    return response;

                case "extension":
                    response = connect.client.SearchAsync<File>(ele => ele
                                    .Query(qry => qry
                                        .Bool(b => b
                                            .Must(m => m
                                                .Match(mc => mc
                                                    .Field(file => file.Extension)
                                                    .Query(query)))))).Result.Documents.ToList();
                    return response;
            }

            return null;
        }

        public bool CheckResponse(Nest.IResponse response)
        {
            if ((response.ApiCall.HttpStatusCode == 201 || response.ApiCall.HttpStatusCode == 200) && response.ApiCall.Success == true)
            {
                return true;
            }
            return false;
        }

        //add new
        public bool Create(File file)
        {
            file.Id = Guid.NewGuid().ToString();
            var response = connect.client.Index<File>(file, i => i
                       .Index("manager_files")
                       .Id(file.Id)
                       .Refresh(Elasticsearch.Net.Refresh.True));
            return CheckResponse(response);
        }

        //
        public async Task<bool> Delete(string id)
        {
            var response = await connect.client.DeleteAsync<File>(id, i => i
                .Index("manager_files")
                .Refresh(Elasticsearch.Net.Refresh.True));
            var abc = 1;
            return CheckResponse(response);
        }

        //update
        public async Task<bool> Edit(string id, File file)
        {
            var response = await connect.client.UpdateAsync<File>(file, i => i
                       .Index("manager_files")
                       .Doc(file)
                       .Refresh(Elasticsearch.Net.Refresh.True));
            var abc = 2;
            return CheckResponse(response);
        }

        //search all
        public async Task<Nest.ISearchResponse<File>> Find(string SearchString)
        {
            var response = await connect.client.SearchAsync<File>(e => e
                .Index("manager_files")
                //.Size(1)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Name)
                        .Query(SearchString)
                    )
                )
            );

            return response;
        }
    }
}
