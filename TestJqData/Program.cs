﻿using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace TestJqData
{

    class GetTokenBody
    {
        public string method { get; set; }
        public string mob { get; set; }
        public string pwd { get; set; }
    }


    class BaseBody
    {

        public string method { get; set; }
        public string token { get; set; }
    }
    class GetSecurityBody : BaseBody
    {

        public string code { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            QuerySecurityInfo();
            Console.WriteLine("Hello World!");
        }

        public static string QueryInfo(HttpClient client, object body)
        {

            const string url = "https://dataapi.joinquant.com/apis";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };


            var content = JsonSerializer.Serialize(body, options);

            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            //POST请求并等待结果
            var result = client.PostAsync(url, bodyContent).Result;


            return result.Content.ReadAsStringAsync().Result;
        }

        static void QuerySecurityInfo()
        {
            using (var client = new HttpClient())
            {
                //需要添加System.Web.Extensions
                //生成JSON请求信息



                object body = new GetTokenBody()
                {
                    method = "get_token",
                    mob = "18080802572", //mob是申请JQData时所填写的手机号
                    pwd = "dragon00A" //Password为聚宽官网登录密码，新申请用户默认为手机号后6位
                };




                //读取返回的TOKEN
                string token = QueryInfo(client, body);

                body = new GetSecurityBody
                {
                    method = "get_security_info",
                    token = token, //token
                    code = "502050.XSHG" //代码
                };
                //code,display_name,name,start_date,end_date,type,parent
                //502050.XSHG,上证50B,SZ50B,2015-04-27,2200-01-01,fjb,502048.XSHG
                var info = QueryInfo(client, body);
                Console.WriteLine("Result\n" + info);


                //查询剩余条数。


                body = new BaseBody
                {
                    method = "get_query_count",
                    token = token, //token
                };

                info = QueryInfo(client, body);
                Console.WriteLine("Result\n" + info);

            }


        }

    }
}
