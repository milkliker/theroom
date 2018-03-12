using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Nethereum.Web3.Accounts;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using EternalLove2.Models;
using Newtonsoft.Json.Linq;

namespace EternalLove2.Services
{
    public class OathService
    {
        private static string privateKey = "0xf8228d7a9d94daed37cb1caab37df26e214ab2f053f13618d1339dbb8c7e8126";
        private static string senderAddress = "0x308f39397b9b2e539d8edb7216f07e98e8ec9e47";
        private static string web3Url = "https://ropsten.infura.io/e9QpOCiUys5aErEiEbZi";
        private static string abi = @"[ {  ""constant"": true,  ""inputs"": [],  ""name"": ""fee"",  ""outputs"": [   {    ""name"": """",    ""type"": ""uint256""   }  ],  ""payable"": false,  ""stateMutability"": ""view"",  ""type"": ""function"" }, {  ""anonymous"": false,  ""inputs"": [   {    ""indexed"": false,    ""name"": ""from"",    ""type"": ""address""   },   {    ""indexed"": false,    ""name"": ""to"",    ""type"": ""address""   }  ],  ""name"": ""OwnerChanged"",  ""type"": ""event"" }, {  ""anonymous"": false,  ""inputs"": [   {    ""indexed"": false,    ""name"": ""_content"",    ""type"": ""string""   }  ],  ""name"": ""Wrote"",  ""type"": ""event"" }, {  ""constant"": false,  ""inputs"": [   {    ""name"": ""_fee"",    ""type"": ""uint256""   }  ],  ""name"": ""changeFee"",  ""outputs"": [],  ""payable"": false,  ""stateMutability"": ""nonpayable"",  ""type"": ""function"" }, {  ""constant"": false,  ""inputs"": [   {    ""name"": ""_newOwner"",    ""type"": ""address""   }  ],  ""name"": ""changeOwner"",  ""outputs"": [],  ""payable"": false,  ""stateMutability"": ""nonpayable"",  ""type"": ""function"" }, {  ""constant"": false,  ""inputs"": [],  ""name"": ""withdraw"",  ""outputs"": [],  ""payable"": false,  ""stateMutability"": ""nonpayable"",  ""type"": ""function"" }, {  ""constant"": false,  ""inputs"": [   {    ""name"": ""_content"",    ""type"": ""string""   }  ],  ""name"": ""write"",  ""outputs"": [],  ""payable"": true,  ""stateMutability"": ""payable"",  ""type"": ""function"" }]";
        private static string contractAddress = "0x30f708a49a5870e0634b28126478e034924ec3bd";

        private Web3 web3;


        public OathService()
        {
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            web3 = new Web3(account, web3Url);

        }

        private T Sync<T>(Task<T> call)
        {
            return Task.Run(() => call).Result;
        }

        public async Task<string> Write(string content)
        {
            var contract = web3.Eth.GetContract(abi, contractAddress);
            var function = contract.GetFunction("write");

            HexBigInteger gasLimit = new HexBigInteger(1000000);
            HexBigInteger gasPrice = new HexBigInteger(40);
            HexBigInteger value = new HexBigInteger(0);

            var ret = await function.SendTransactionAsync(senderAddress, gasLimit, gasPrice, value, content).ConfigureAwait(false);
            return ret;
        }

        public async Task<Oath> GetOath(string txHash)
        {
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash).ConfigureAwait(false);
            Oath oath = new Models.Oath();
            if (receipt != null)
            {
                oath.BlockHash = receipt.BlockHash;
                oath.BlockHeight = (int)receipt.BlockNumber.Value;
                HexUTF8String str = HexUTF8String.CreateFromHex(receipt.Logs.First.Value<string>("data").Substring(130));
                oath.OathContent = str.Value.Replace("\0", "");
                oath.TxHash = receipt.TransactionHash;
            }
            else
            {
                oath.TxHash = txHash;
            }

            return oath;
        }
    }
}