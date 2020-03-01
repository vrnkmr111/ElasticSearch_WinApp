using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch_WinApp
{
    public class ElasticConn
    {
        public static ElasticClient EsClient()
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            StaticConnectionPool connectionPool;

            //Connection string for Elasticsearch
            /*connectionSettings = new ConnectionSettings(new Uri("http://localhost Jump :9200/")); //local PC
            elasticClient = new ElasticClient(connectionSettings);*/

            //Multiple node for fail over (cluster addresses)
            var nodes = new Uri[]
            {
        new Uri("http://localhost:9200/"),
                //new Uri("Add server 2 address")   //Add cluster addresses here
                //new Uri("Add server 3 address")
            };

            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool);
            elasticClient = new ElasticClient(connectionSettings);

            return elasticClient;
        }
    }
}
