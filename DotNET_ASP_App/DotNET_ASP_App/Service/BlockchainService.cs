namespace DotNET_ASP_App.Service;

using System.Numerics;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

public class BlockchainService
{
    private static string infuraUrl = "https://holesky.infura.io/v3/4ff78b295fdb48a49a6f6a225b1914a8";
    private static string privateKey = "71fd4d3c69518e25465130f69b398142199b3cf54d2ce4d616bd5d6e236285aa";
    private static string contractAddress = "0x953c5dEE0De97746ef280602a427373c4C7cABA1";
    private static string abi;
    private string[] addresses = ["0xc1c1204a97aCe7Eb15F283Ac41a4C9E9ab9f896E","0x6E0B70214A25317B0C2eD06Cc5047f4652cdB1B5",
        "0x94E7762eaB719dF8a4bF5EA78aD891326eDd141c", "0xeD1957f74ec64461dF4683F5Ce1aCB094604fB70", "0x2666db05ce7C4214716B59005915044c4788DF78",
    "0xEdbA58bb204A3C0fb25938E27E9e1B4ef8f4aa3b", "0x95EEBB86fC8C039791651d32F7937c9957Edd66E", "0x2cf8406B62427469c79930817e5DF42262C14AEB",
    "0x1F8E950320fd9C72cA88CE3e474dDA1a6928084f", "0x3Bc53dBaf870D5B4575F9DF40A5f2001d4BDbB79", "0xE2384b7ee91aCfF93cBAf7975271594B012EcE7e",
    "0x686F554B8C9f7fDdb52c716b3D45Bdd4ac4e162A", "0xFdb2056188041196349859F216AcF039E88f00EC", "0x3F11e2E89a543b1195dCfEA2CbCc550455d331E1",
    "0x192Ce67c99C29b4ab6377dF5ab7CA13d4840cd4E", "0xb284C643F3Fc1290983adFC4a58B00831569D834", "0x650844a08f68D99C66E478e9AEB3d0be3bC934a4"];
    private Web3 web3;
    private int decimals = -1;

    public BlockchainService()
    {
        var account = new Account(privateKey);
        web3 = new Web3(account, infuraUrl);

        abi = @"[{""inputs"":[],""stateMutability"":""nonpayable"",""type"":""constructor""},
        {""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""owner"",""type"":""address""},
        {""indexed"":true,""internalType"":""address"",""name"":""spender"",""type"":""address""},
        {""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""Approval"",""type"":""event""},
        {""anonymous"":false,""inputs"":[{""indexed"":true,""internalType"":""address"",""name"":""from"",""type"":""address""},
        {""indexed"":true,""internalType"":""address"",""name"":""to"",""type"":""address""},
        {""indexed"":false,""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""},
        {""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""},
        {""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""allowance"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},
        {""inputs"":[{""internalType"":""address"",""name"":""spender"",""type"":""address""},
        {""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},
        {""inputs"":[{""internalType"":""address"",""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},
        {""inputs"":[],""name"":""decimals"",""outputs"":[{""internalType"":""uint8"",""name"":"""",""type"":""uint8""}],""stateMutability"":""view"",""type"":""function""},
        {""inputs"":[],""name"":""name"",""outputs"":[{""internalType"":""string"",""name"":"""",""type"":""string""}],""stateMutability"":""view"",""type"":""function""},
        {""inputs"":[],""name"":""symbol"",""outputs"":[{""internalType"":""string"",""name"":"""",""type"":""string""}],""stateMutability"":""view"",""type"":""function""},
        {""inputs"":[],""name"":""totalSupply"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},
        {""inputs"":[{""internalType"":""address"",""name"":""_to"",""type"":""address""},
        {""internalType"":""uint256"",""name"":""_value"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""},
        {""inputs"":[{""internalType"":""address"",""name"":""sender"",""type"":""address""},
        {""internalType"":""address"",""name"":""recipient"",""type"":""address""},
        {""internalType"":""uint256"",""name"":""amount"",""type"":""uint256""}],""name"":""transferFrom"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""}]
        ";
    }
    
    public async Task<decimal> GetBalance(int sensorId)
    {   
        var contract = web3.Eth.GetContract(abi, contractAddress);
        var balanceOfFunction = contract.GetFunction("balanceOf");
        var balance = await balanceOfFunction.CallAsync<BigInteger>(addresses[sensorId + 1]);
        return Web3.Convert.FromWei(balance);
    }

    public async Task<int> GetDecimals()
    {
        if (decimals == -1)
        {
            var contract = web3.Eth.GetContract(abi, contractAddress);
            var decimalsFunction = contract.GetFunction("decimals");
            return await decimalsFunction.CallAsync<int>();
        }
        return decimals;
    }
    
    public async Task RewardSensor(int sensorId)
    {
        try
        {
            if (sensorId < 0 || sensorId > 15) throw new ArgumentOutOfRangeException(nameof(sensorId), "Invalid sensor ID.");
        
            var senderAddress = addresses[0];
            var rewardedSensorAddress = addresses[sensorId + 1];
            var contract = web3.Eth.GetContract(abi, contractAddress);
            
            var decimals = await GetDecimals();
        
            var tokenAmount = Web3.Convert.ToWei(1, decimals);
            
            var transferFunction = contract.GetFunction("transfer");
            
            var gas = await transferFunction.EstimateGasAsync(senderAddress, null, null, rewardedSensorAddress, tokenAmount);

            var transactionReceipt = await transferFunction.SendTransactionAndWaitForReceiptAsync(
                senderAddress,
                gas,
                null,
                null,
                rewardedSensorAddress,
                tokenAmount
            );
            
            Console.WriteLine($"Transaction successful with receipt: {transactionReceipt}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
        
    }
}