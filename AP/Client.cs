using System;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace UNBEATAP.AP;

public class Client
{
    public bool Connected = false;

    public ArchipelagoSession Session;

    private string ip;
    private int port;
    private string slot;
    private string password;
    private bool deathLink;


    public Client(string ip, int port, string slot, string password, bool deathLink)
    {
        this.ip = ip;
        this.port = port;
        this.slot = slot;
        this.password = password;
        this.deathLink = deathLink;

        Plugin.Logger.LogInfo($"Creating session with server {ip}:{port}");
        Session = ArchipelagoSessionFactory.CreateSession(ip, port);
    }


    private void HandleReceiveItem(string name)
    {
        if(name.StartsWith(DifficultyController.SongNamePrefix))
        {
            DifficultyController.AddProgressiveSong(name);
            return;
        }
        if(CharacterController.CharNames.Contains(name))
        {
            CharacterController.AddCharacter(name);
            return;
        }
    }


    private void GetExistingItems()
    {
        foreach(ItemInfo item in Session.Items.AllItemsReceived)
        {
            HandleReceiveItem(item.ItemName);
        }
    }


    private void GetStoredItems()
    {
        
    }


    public async Task ConnectAndGetData()
    {
        Plugin.Logger.LogInfo($"Connecting to {ip}:{port} with game {Plugin.GameName} as {slot}");
        LoginResult result;
        try
        {
            await Session.ConnectAsync();
            result = await Session.LoginAsync(
                Plugin.GameName,
                slot,
                ItemsHandlingFlags.AllItems,
                null,
                deathLink ? ["DeathLink"] : null,
                null,
                string.IsNullOrEmpty(password) ? null : password,
                true
            );
        }
        catch(Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
        }

        if(!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            string message = $"Failed to connect to {ip}:{port} as {slot}:";
            foreach(string error in failure.Errors)
            {
                message += $"\n    - {error}";
            }
            foreach(ConnectionRefusedError error in failure.ErrorCodes)
            {
                message += $"\n    - {error}";
            }

            Plugin.Logger.LogError(message);
            Connected = false;
            return;
        }
        
        Connected = true;

        GetStoredItems();
        GetExistingItems();
    }
}