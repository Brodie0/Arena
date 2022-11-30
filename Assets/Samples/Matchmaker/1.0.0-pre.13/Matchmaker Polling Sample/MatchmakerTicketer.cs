using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using StatusOptions = Unity.Services.Matchmaker.Models.MultiplayAssignment.StatusOptions;

public class MatchmakerTicketer : MonoBehaviour
{
    public Text AuthIdText;
    public Text TicketIdText;
    public Text InfoPaneText;
    public Button AuthButton;
    public Button FindMatchButton;
    public Text FindMatchButtonText;
    public string QueueName = "MainQueue";
    private string ticketId = "";
    private bool searching = false;
    private bool shouldPoll = false;
    private bool currentlyPolling = false;

    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void OnSignIn() 
    {
        Debug.Log("OnSignIn");
                        var imagens = string.Join(", ", typeof(TicketStatusResponse).GetProperties().ToList().Select(prop => prop.PropertyType + prop.Name));
                           Debug.Log(imagens); 
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        AuthIdText.text = AuthenticationService.Instance.PlayerId;
        Debug.Log(AuthenticationService.Instance.AccessToken);
        AuthButton.interactable = false;
        FindMatchButton.interactable = true;
    }

    public async void OnFindMatch() 
    {
        Debug.Log("OnFindMatch");
        ClearInfoPane();

        try
        {
            // Check toggle
            if (!searching)
            {
                if (ticketId.Length > 0)
                {
                    LogToInfoViewAndConsole($"A Matchmaking ticket is already active for this client!");
                    return;
                }

                FindMatchButtonText.text = "Quit Queue";
                searching = true;

                await StartSearch();
            }
            else
            {
                if (ticketId.Length == 0)
                {
                    LogToInfoViewAndConsole("Cannot delete ticket as no ticket is currently active for this client!");
                    return;
                }

                await StopSearch();
                FindMatchButtonText.text = "Find Match";
                searching = false;
            }
        }
        catch (Exception e)
        {
            LogToInfoViewAndConsole(e.Message);
        }
    }

    private async Task StartSearch() 
    {
        var attributes = new Dictionary<string, object>();
        var players = new List<Player>
        { 
            new Player(AuthenticationService.Instance.PlayerId, new Dictionary<string, object>{ {"skill", 455.6} }), 
        };

        // Set options for matchmaking
        var options = new CreateTicketOptions(QueueName, attributes);

        // Create ticket
        var ticketResponse = await MatchmakerService.Instance.CreateTicketAsync(players, options);
        TicketIdText.text = ticketId = ticketResponse.Id;
        LogToInfoViewAndConsole($"Ticket '{ticketResponse.Id}' created!");

        // Poll ticket status
        shouldPoll = true;
        await PollTicketStatus(); 
    }

    private async Task StopSearch()
    {
        // Stop active poll thread
        shouldPoll = false;

        // Wait until the PollTicketStatus is done polling before attempting to delete a ticket
        while (currentlyPolling)
        {
            await Task.Delay(200);
        }
        
        // Delete ticket
        await MatchmakerService.Instance.DeleteTicketAsync(ticketId);
        ClearInfoPane();
        LogToInfoViewAndConsole("Ticket deleted!");
        TicketIdText.text = "N/A";
        ticketId = "";
    }

    private async Task PollTicketStatus()
    {
        Debug.Log("PollTicketStatus");

        string waitingMessage = "Finding match...";
        string preMessagePaneText = Environment.NewLine + InfoPaneText.text;
        InfoPaneText.text = waitingMessage + preMessagePaneText;
        
        TicketStatusResponse response = null;
        MultiplayAssignment assignment = null;
        
        bool gotAssignment = false;
        currentlyPolling = true;
        
        // Initial delay so we don't poll as soon as we create the ticket
        await Task.Delay(2000);
        
        while (!gotAssignment && shouldPoll)
        {
            waitingMessage += ".";
            InfoPaneText.text = waitingMessage + preMessagePaneText;
            
            // Get ticket status
            response = await MatchmakerService.Instance.GetTicketAsync(ticketId);

            // InfoPaneText.text += @$"
            //     Ticket response: 
            //     status: {response.Status}
            //     type: {response.Type}
            //     error message: {response.Message}
            //     server IP: {response.Ip}
            //     server port: {response.Port}";


                
            if (response.Type == typeof(MultiplayAssignment)) 
            {
                assignment = response.Value as MultiplayAssignment;
                InfoPaneText.text += 
@$"
status: {assignment.Status}
type: {response.Type}
error message: {assignment.Message}
server IP: {assignment.Ip}
server port: {assignment.Port}";
            }

            if (assignment == null)
            {
                var message = $"GetTicketStatus returned a type that was not a {nameof(MultiplayAssignment)}. This operation is not supported.";
                throw new InvalidOperationException(message);
            }

            switch (assignment.Status) 
            {
                case StatusOptions.Found:
                    gotAssignment = true;
                    break;
                case StatusOptions.InProgress:
                    // Do nothing
                    break;
                case StatusOptions.Failed:
                    ClearInfoPane();
                    LogToInfoViewAndConsole("Failed to get ticket status. See logged exception for more details.");
                    throw new MatchmakerServiceException(MatchmakerExceptionReason.Unknown, assignment.Message);
                case StatusOptions.Timeout:
                    gotAssignment = true;
                    ClearInfoPane();
                    LogToInfoViewAndConsole("Failed to get ticket status. Ticket timed out.");
                    break;
                default:
                    throw new InvalidOperationException("Assignment status was a value other than 'In Progress', 'Found', 'Timeout' or 'Failed'! " +
                        $"Mismatch between Matchmaker SDK expected responses and service API values! Status value: '{assignment.Status}'");
            }

            // Wait for 2 seconds before polling again
            await Task.Delay(2000);
        }

        if (assignment != null)
        {
            string jsonOutput = JsonConvert.SerializeObject(assignment, Formatting.Indented);
            LogToInfoViewAndConsole(jsonOutput);
        }
        
        currentlyPolling = false;
    }
    
    private void LogToInfoViewAndConsole(string output) 
    {
        Debug.Log(output);
        InfoPaneText.text = output + Environment.NewLine + InfoPaneText.text;
    }

    private void ClearInfoPane() 
    {
        InfoPaneText.text = "";
    }
}
