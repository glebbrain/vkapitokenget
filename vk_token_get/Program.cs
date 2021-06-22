using Microsoft.VisualBasic;
using System;
using System.Text;
using VkNet;
using VkNet.Enums.Filters;

namespace vk_token_get
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<3 || !Information.IsNumeric(args[0]))
            {


                Console.WriteLine("VkTokenGet v1.0");
                Console.WriteLine("----------------------------------");
                Console.WriteLine("author: https://www.youtube.com/glebbrainofficial");
                Console.WriteLine("git: ");
                Console.WriteLine("NetFramework v5.0");
                Console.WriteLine("VkNet v1.59");
                Console.WriteLine("git: https://github.com/vknet/vk");
                Console.WriteLine("docs: https://vknet.github.io/vk/authorize/");
                Console.WriteLine("----------------------------------");
                Console.WriteLine("Help:");
                Console.WriteLine("vk_token_get.exe AppID Login \"Password\" [json with permissions]");
                Console.WriteLine(" ");
                Console.WriteLine("Example 1 (with default permissions: all|offline):");
                Console.WriteLine("vk_token_get.exe 132456 71231231212 \"sdS30DS7D82uhfsa\"");
                Console.WriteLine(" ");
                Console.WriteLine("Example 2 (with custom permissions like json):");
                Console.WriteLine("vk_token_get.exe 132456 71231231212 \"sdS30DS7D82uhfsa\" {\"permissions\":\"offline,groups,status,market\"}");
                Console.WriteLine("All VK permissions: https://vk.com/dev/permissions");
            }
            else
            {
                // System.ArgumentException: ''windows-1251' is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method. Parameter name: name'
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                VkApi api = new VkApi();
                string token = "";
                try
                {
                    Settings s = Settings.Offline | Settings.All;
                    if (!String.IsNullOrEmpty(args[3].Trim()))
                    {
                        s = Settings.FromJsonString(args[3].Trim());
                    }
                    api.Authorize(new VkNet.Model.ApiAuthParams()
                    {
                        ApplicationId = ulong.Parse(args[0].ToString()),
                        Login = args[1].ToString(),
                        Password = args[2].ToString(),
                        Settings = s
                    });
                }
                catch (Exception ex)
                {
                    // Console.WriteLine(ex.Message);
                    if (ex.Message.ToString().IndexOf("error") <= -1 && ex.Message.ToString().IndexOf("access_token") > -1)
                    {
                        // get access_token from url
                        token = ex.Message.ToString().Split(new string[] { @"%253D" }, StringSplitOptions.RemoveEmptyEntries).GetValue(1).ToString().Split(new string[] { @"%2526" }, StringSplitOptions.RemoveEmptyEntries).GetValue(0).ToString();
                        Console.WriteLine("access_token:"+token);
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("raw url: " + ex.Message.ToString());
                    }
                    else
                    {
                        // {"error":"invalid_request","error_description":"This application has no right to use messages"}
                        Console.WriteLine(ex.Message.ToString());
                    }
                }

            }
        }
    }
}
